using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using GameCube.Games.FZeroGX;

namespace GX_Data.Object_0x48
{
    /// <summary>
    /// F-Zero GX GameObject with attached components
    /// </summary>
    [Serializable]
    public class GX_Object_0x48 : GX
    {
        public BaseObject[] obj;

        public GX_Object_0x48(BinaryReader reader) : base(reader)
        {
            header_const = new GX_Header_Data(0x48, 0x54, 16 * 4);
            header_pointer = new GX_Pointer(header_const, reader);

            obj = new BaseObject[header_pointer.count];
            for (uint ui = 0; ui < obj.Length; ui++)
            {
                obj[ui] = new BaseObject(reader, header_pointer.address + ui * BaseObject.size);
            }
        }
    }

    [Serializable] // Non Interactive Object
    public struct BaseObject
    {
        public const uint size = 0x40;

        public BaseObject(BinaryReader reader, uint seekAddress)
        {
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            id1 = reader.GetUInt32();
            id2 = reader.GetUInt32();
            lodAndCollisionAddress = reader.GetUInt32();
            position = reader.GetVector3Position();
            unknown0x18 = reader.GetString(4);
            unknown0x1C = reader.GetString(4);
            scale = reader.GetVector3Scale();
            /*/ NULL 32 /*/
            reader.SkipBytes(4);
            animationAddress = reader.GetUInt32();
            unknownAddress0x34 = reader.GetUInt32();
            skeletonAddress = reader.GetUInt32();
            transformAddress = reader.GetUInt32();

            //
            sectionA = new AdditionalObjectInfo_A(reader, lodAndCollisionAddress);
            transform = new ObjectTransform(reader, transformAddress);
            animation = new ObjectAnimation(reader, animationAddress);
        }
        public string ObjectName { get { return sectionA.name; } }

        // This is an object (like a prefab)
        // Using the last index we get to all the instances of it in the scene
        //
        /*/ 0x00 32 /*/ public uint id1; // ID? - Varies A LOT, but seems to be 4 IDs
        /*/ 0x04 32 /*/ public uint id2; // FFFF FFFF or 8000 0000?
        /*/ 0x08 32 /*/ public uint lodAndCollisionAddress; // Offset to the collision for this object - part of 0x64 header
        /*/ 0x0C 32 /*/ public Vector3 position; // Confirmed position x y z
        /*/ 0x18 32 /*/ public string unknown0x18; // ?
        /*/ 0x1C 32 /*/ public string unknown0x1C; // ID?
        /*/ 0x20 32 /*/ public Vector3 scale; // Confirmed scale x y z
        /*/ 0x2C 32 /*/ //NULL 32
        /*/ 0x30 32 /*/ public uint animationAddress;   // Offset 2
        /*/ 0x34 32 /*/ public uint unknownAddress0x34; // Offset 3
        /*/ 0x38 32 /*/ public uint skeletonAddress;    // Offset 4 - SKL models
        /*/ 0x3C 32 /*/ public uint transformAddress;   // Transform: scale, rotation, position


        // Offset 1
        public AdditionalObjectInfo_A sectionA;
        // Offset 2 - Animation
        public ObjectAnimation animation;
        // Offset 5 - Transform
        public ObjectTransform transform;
    }

    /// <summary>
    /// 
    /// Notes: This part is recursive thus it needs to be a class
    /// </summary>
    [Serializable]
    public class ObjectAnimation
    {
        public float dat_0x00;
        public float dat_0x04;
        public uint layer_16bit;
        public GX_Pointer[] pointers;

        // Always [6][X]
        public AnimationEntry[][] animationEntry;


        public ObjectAnimation(BinaryReader reader, uint seekAddress)
        {
            dat_0x00 = 0;
            dat_0x04 = 0;
            layer_16bit = 0;
            pointers = new GX_Pointer[6];
            animationEntry = new AnimationEntry[6][];

            // Prevent false seeks
            if (seekAddress > 0)
            {
                reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

                // HEADER //
                dat_0x00 = reader.GetFloat();
                dat_0x04 = reader.GetFloat();
                reader.SkipBytes(4 * 4);
                layer_16bit = reader.GetUInt32();
                // HEADER //
               
                for (int i = 0; i < 6; i++)
                {
                    reader.SkipBytes(4 * 4);
                    pointers[i].count = reader.GetUInt32();
                    pointers[i].address = reader.GetUInt32();
                    animationEntry[i] = new AnimationEntry[pointers[i].count];
                }

                for (int i = 0; i < 6; i++)
                {
                    reader.BaseStream.Seek(pointers[i].address, SeekOrigin.Begin);
                    for (int j = 0; j < animationEntry[i].Length; j++)
                        animationEntry[i][j] = new AnimationEntry(reader);
                }
            }
            else
            {
                for (int i = 0; i < animationEntry.Length; i++)
                    animationEntry[i] = new AnimationEntry[0];
            }
        }
    }
    [Serializable]
    public struct AnimationEntry
    {
        public uint isLooping; // 1 = no, 2 = yes, 3 = yes!?
        public float time;     // Keyframe time
        public Vector3 vector; // Can be pos, rot, scale?

        public AnimationEntry(BinaryReader reader)
        {
            isLooping = reader.GetUInt32();
            time = reader.GetFloat();
            vector = reader.GetVector3Position();
        }
    }

    [Serializable]
    public struct ObjectTransform
    {
        public Vector3 normalX;
        public Vector3 normalY;
        public Vector3 normalZ;
        public Vector3 position;

        // We can infer the rest of the transform
        public Vector3 scale;
        public Quaternion rotation;

        public ObjectTransform(BinaryReader reader, uint seekPosition)
        {
            reader.BaseStream.Seek(seekPosition, SeekOrigin.Begin);

            Vector3 position = new Vector3();

            normalX = reader.GetVector3Normal();
            position.x = reader.GetFloat() * ((StageManager.doInverseWindingPositionX) ? -1f : 1f);
            normalY = reader.GetVector3Normal();
            position.y = reader.GetFloat();
            normalZ = reader.GetVector3Normal();
            position.z = reader.GetFloat();

            this.position = position;
            this.scale = new Vector3(normalX.magnitude, normalY.magnitude, normalZ.magnitude);
            rotation = Quaternion.Euler(Vector3.Angle(normalX, Vector3.right), Vector3.Angle(normalY, Vector3.up), Vector3.Angle(normalZ, Vector3.forward));
        }
    }

    public struct AdditionalObjectInfo_A
    {
        public uint LOD_1;
        public uint LOD_2;
        public uint bInfoAddress;
        public uint collisionAddress;

        public AdditionalObjectInfo_B infoB;
        public string name { get { return infoB.name; } }

        public ObjectCollision collision;

        public AdditionalObjectInfo_A(BinaryReader reader, uint seekAddress)
        {
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            LOD_1 = reader.GetUInt32();
            LOD_2 = reader.GetUInt32();
            bInfoAddress = reader.GetUInt32();
            collisionAddress = reader.GetUInt32();

            reader.BaseStream.Seek(bInfoAddress, SeekOrigin.Begin);
            infoB = new AdditionalObjectInfo_B(reader);

            collision = new ObjectCollision(reader, collisionAddress);
        }
    }
    public struct AdditionalObjectInfo_B
    {
        /*/ 0x00 32 /*/ //NULL
        public uint nameAddress;
        /*/ 0x08 32 /*/ //NULL
        public float dat_0x0C; // LOD related?

        public string name { get; private set; }
        private static string GetName(BinaryReader reader, uint nameAddress)
        {
            reader.BaseStream.Seek(nameAddress, SeekOrigin.Begin);
            char c;
            string name = "";

            // read char and assign
            while ((c = reader.GetChar()) != (char)0)
            {
                name += c;
            }

            return name;
        }

        public AdditionalObjectInfo_B(BinaryReader reader)
        {
            reader.SkipBytes(4);
            nameAddress = reader.GetUInt32();
            reader.SkipBytes(4);
            dat_0x0C = reader.GetUInt32();

            name = GetName(reader, nameAddress);
        }
    }
    public class ObjectCollision
    {
        public uint id; // layer

        // Unknowns
        public uint dat_Tri_0x04;
        public uint dat_Quad_0x08;
        public uint dat_Tri_0x0C;
        public uint dat_Quad_0x10;

        public uint triCount;
        public uint quadCount;
        public uint triAddress;
        public uint quadAddress;

        public TriQuad[] triangles;
        public TriQuad[] quads;

        public ObjectCollision(BinaryReader reader, uint seekAddress)
        {
            if (seekAddress > 0)
            {
                reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

                id = reader.GetUInt32();

                dat_Tri_0x04 = reader.GetUInt32();
                dat_Quad_0x08 = reader.GetUInt32();
                dat_Tri_0x0C = reader.GetUInt32();
                dat_Quad_0x10 = reader.GetUInt32();

                triCount = reader.GetUInt32();
                quadCount = reader.GetUInt32();
                triAddress = reader.GetUInt32();
                quadAddress = reader.GetUInt32();

                //Debug.LogFormat("Tri:{0} Quad:{1}", triCount, quadCount);

                triangles = InitializeTriQuad(reader, triAddress, triCount, true);
                quads =     InitializeTriQuad(reader, quadAddress, quadCount, false);
            }
        }

        public TriQuad[] InitializeTriQuad(BinaryReader reader, uint address, uint count, bool isTriangle)
        {
            TriQuad[] triQuad = new TriQuad[count];

            if (count > 0)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);
                triQuad = new TriQuad[count];
                for (int i = 0; i < count; i++)
                    triQuad[i] = new TriQuad(reader, isTriangle);
            }

            return triQuad;
        }
    }
    public struct TriQuad
    {
        public float dat_0x00;
        public Vector3 normal;
        public Vector3[] vertices; // Tri 3, Quad 4
        public Vector3[] dat_vertices; // Tri 3, Quad 4. (unknown data)

        public TriQuad(BinaryReader reader, bool isTriangle)
        {
            dat_0x00 = reader.GetUInt32();
            normal = reader.GetVector3Normal();

            // 3 if triangle, 4 if quad
            int count = isTriangle ? 3 : 4;
            vertices = new Vector3[count];
            dat_vertices = new Vector3[count];

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = reader.GetVector3Position();
            for (int i = 0; i < vertices.Length; i++)
                dat_vertices[i] = reader.GetVector3Position();
        }
    }

}