using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures.FMI
{
    [Serializable]
    public class FMI : IBinarySerializable
    {
        private const long m_FirePtr = 0x0044;
        private const long m_AnimPtr = 0x0208;
        private const long m_NamePtr = 0x02A0;

        private const uint max_fire = 0x08;
        private const string warn_fire = "Detect Over {0} Engine Fires! Export {1} -> {2} engine fires";

        [HideInInspector]
        public uint unknown_0x00;
        [HideInInspector]
        public uint unknown_0x01;
        public uint count_anim;
        public uint count_fire;
        [HideInInspector]
        public uint unknown_0x04;
        [HideInInspector]
        public float unknown_0x05;
        [HideInInspector]
        public uint unknown_0x09;
        [HideInInspector]
        public float unknown_0x0A;
        [HideInInspector]
        public uint unknown_0x0E;
        public List<EngineFire> engineFires;
        public List<AnimationObject> animationObjects;

        public void Deserialize(BinaryReader reader) {
            unknown_0x00 = reader.GetByte();
            unknown_0x01 = reader.GetByte();
            count_anim = reader.GetByte();
            count_fire = reader.GetByte();
            unknown_0x04 = reader.GetByte();
            unknown_0x05 = reader.GetFloat();
            unknown_0x09 = reader.GetByte();
            unknown_0x0A = reader.GetFloat();
            unknown_0x0E = reader.GetUInt16();

            //EngineFire
            reader.BaseStream.Seek(m_FirePtr, SeekOrigin.Begin);
            engineFires = new List<EngineFire>();
            for (int i = 0; i < count_fire; i++) {
                EngineFire engineFire = new EngineFire();
                engineFire.Deserialize(reader);
                engineFires.Add(engineFire);
                reader.BaseStream.Seek(reader.BaseStream.Position + 4, SeekOrigin.Begin);
            }

            //AnimationObject
            long animPos = 0x00;
            long namePos = m_NamePtr;
            reader.BaseStream.Seek(m_AnimPtr, SeekOrigin.Begin);
            animationObjects = new List<AnimationObject>();
            for (int i = 0; i < count_anim; i++) {
                AnimationObject animObject = new AnimationObject();
                animObject.Deserialize(reader);

                animPos = reader.BaseStream.Position;
                reader.BaseStream.Seek(namePos, SeekOrigin.Begin);
                string animObjName = GetName(reader, (uint)namePos);
                namePos = reader.BaseStream.Position;
                reader.BaseStream.Seek(animPos, SeekOrigin.Begin);

                animObject.animationObjectName = animObjName;
                animationObjects.Add(animObject);
            }
        }

        public void Serialize(BinaryWriter writer) {
            count_fire = (uint)engineFires.Count;
            count_anim = (uint)animationObjects.Count;
            if (count_fire > max_fire) {
                Debug.LogWarning(String.Format(warn_fire, max_fire, count_fire, max_fire));
                count_fire = max_fire;
            }

            //EngineFire
            writer.BaseStream.Seek(m_FirePtr, SeekOrigin.Begin);
            foreach (EngineFire engineFire in engineFires) {
                engineFire.Serialize(writer);
                writer.BaseStream.Seek(4, SeekOrigin.Current);
            }

            //AnimationObject
            List<string> animObjNames = new List<string>();
            writer.BaseStream.Seek(m_AnimPtr, SeekOrigin.Begin);
            foreach (AnimationObject animObject in animationObjects) {
                animObject.Serialize(writer);
                animObjNames.Add(animObject.animationObjectName);
            }

            //AnimationObjectName
            writer.BaseStream.Seek(m_NamePtr, SeekOrigin.Begin);
            foreach (string animObjName in animObjNames) {
                char[] name = animObjName.ToCharArray();
                writer.Write(name);
                writer.WriteX((byte)0x00);
            }

            //Header
            writer.BaseStream.Seek(0x00, SeekOrigin.Begin);
            writer.WriteX((byte)unknown_0x00);
            writer.WriteX((byte)unknown_0x01);
            writer.WriteX((byte)count_anim);
            writer.WriteX((byte)count_fire);
            writer.WriteX((byte)unknown_0x04);
            writer.WriteX(unknown_0x05);
            writer.WriteX((byte)unknown_0x09);
            writer.WriteX(unknown_0x0A);
            writer.WriteX((byte)unknown_0x0E);
        }

        private string GetName(BinaryReader reader, uint nameAddress) {
            reader.BaseStream.Seek(nameAddress, SeekOrigin.Begin);
            char c;
            string name = "";

            // read char and assign
            while ((c = reader.GetChar()) != (char)0) {
                name += c;
            }
            return name;
        }
    }

    //Engine Fire
    [Serializable]
    public class EngineFire : IBinarySerializable
    {
        private const float color_coef = 255f; // color coefficient use for convert

        public Vector3 position;
        public uint unknown_0x0C;
        public uint unknown_0x10;
        public float scale_1;
        public float scale_2;
        [HeaderAttribute("Engine Color of Normal Acceleration")]
        public Color color_normal;
        [HeaderAttribute("Engine Color of Strong Acceleration")]
        public Color color_strong;

        public void Deserialize(BinaryReader reader) {
            position = new Vector3();
            position.x = reader.GetFloat();
            position.y = reader.GetFloat();
            position.z = reader.GetFloat();
            unknown_0x0C = reader.GetUInt32();
            unknown_0x10 = reader.GetUInt32();
            scale_1 = reader.GetFloat();
            scale_2 = reader.GetFloat();
            float normal_r = reader.GetFloat();
            float normal_g = reader.GetFloat();
            float normal_b = reader.GetFloat();
            color_normal = new Color(normal_r/color_coef, normal_g/color_coef, normal_b/color_coef);
            float strong_r = reader.GetFloat();
            float strong_g = reader.GetFloat();
            float strong_b = reader.GetFloat();
            color_strong = new Color(strong_r/color_coef, strong_g/color_coef, strong_b/color_coef);
        }

        public void Serialize(BinaryWriter writer) {
            writer.WritePosition(position);
            writer.WriteX(unknown_0x0C);
            writer.WriteX(unknown_0x10);
            writer.WriteX(scale_1);
            writer.WriteX(scale_2);
            writer.WriteX(color_normal.r*color_coef);
            writer.WriteX(color_normal.g*color_coef);
            writer.WriteX(color_normal.b*color_coef);
            writer.WriteX(color_strong.r*color_coef);
            writer.WriteX(color_strong.g*color_coef);
            writer.WriteX(color_strong.b*color_coef);
        }
    }

    //Animation Object
    [Serializable]
    public class AnimationObject : IBinarySerializable
    {
        public Vector3 position;
        public uint padding_0x0C;
        public uint animType;
        public string animationObjectName;

        public void Deserialize(BinaryReader reader) {
            position = new Vector3();
            position.x = reader.GetFloat();
            position.y = reader.GetFloat();
            position.z = reader.GetFloat();
            padding_0x0C = reader.GetUInt32();
            animType = reader.GetUInt32();
        }

        public void Serialize(BinaryWriter writer) {
            writer.WritePosition(position);
            writer.WriteX(padding_0x0C);
            writer.WriteX(animType);
        }
    }
}