using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCube.FileStructures
{
    [Serializable]
    public sealed class GMA : SerializableMember
    {
        public GMA() { }
        public GMA(BinaryReader reader)
        {
            Deserialize(reader, 0);
        }

        #region MEMBERS
        private const uint headerData = 8;

        private uint modelCount;
        private uint headerSize;
        private Model[] models;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// 0x08
        /// </summary>
        public override uint size
        {
            get
            {
                return 0x08;
            }
        }
        public uint NameOffset
        {
            get
            {
                // 8 bytes per model entry, header 8 bytes of data
                return modelCount * 8 + headerData;
            }
        }
        public Model[] Models
        {
            get
            {
                return models;
            }
            internal set
            {
                models = value;
            }
        }
        #endregion
        #region METHODS
        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public override void Deserialize(BinaryReader reader, uint address)
        {
            if (address != 0)
                throw new Exception();

            reader.BaseStream.Seek(address, SeekOrigin.Begin);

            modelCount = reader.GetUInt32();
            headerSize = reader.GetUInt32();

            models = new Model[modelCount];
            for (int i = 0; i < models.Length; i++)
            {
                reader.BaseStream.Seek(headerData + i * 8, SeekOrigin.Begin);
                uint dataAddress = reader.GetUInt32();
                uint nameAddress = reader.GetUInt32();

                if (dataAddress != uint.MaxValue & nameAddress != uint.MaxValue)
                {
                    models[i] = new Model(reader, dataAddress + headerSize, nameAddress + NameOffset, i+1);
                }
            }
        }
        #endregion
        
        [Serializable]
        public sealed class Model
        {
            public Model() { }
            public Model(BinaryReader reader, uint address, uint nameOffset, int index)
            {
                this.index = index;
                this.address = address;
                this.nameOffset = nameOffset;

                // Recover name
                reader.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
                name = GetName(reader);

                //Debug.LogFormat("{0}::{1}", Index, Name);

                // GCMF
                gcmf = new GCMF(reader, address);

                // Observe why null GCMFs occur
                if (gcmf.TextureCount > 0)
                {
                    // MATERIAL DESCRIPTION
                    textureDescriptor = new TextureDescriptor[gcmf.TextureCount];
                    for (int i = 0; i < textureDescriptor.Length; i++)
                        textureDescriptor[i] = new TextureDescriptor(reader);

                    // VERTEX STRIPS
                    // TEMP: NOT INCLUDING MT_TL
                    vertexStrips = new GC_VertexStripCollection[gcmf.Count_MT];
                    if (gcmf.Count_MT > 0)
                        for (int i = 0; i < gcmf.Count_MT; i++)
                            vertexStrips[i] = new GC_VertexStripCollection(reader, gcmf);
                }
            }

            private uint address;
            private uint nameOffset;
            private string name;
            private int index;

            private GCMF gcmf;
            private TextureDescriptor[] textureDescriptor;
            private GC_VertexStripCollection[] vertexStrips;

            public GCMF GCMF
            {
                get
                {
                    return gcmf;
                }
                internal set
                {
                    gcmf = value;
                }
            }
            public TextureDescriptor[] TextureDescriptor
            {
                get
                {
                    return textureDescriptor;
                }
                internal set
                {
                    textureDescriptor = value;
                }
            }
            public GC_VertexStripCollection[] VertexStrips
            {
                get
                {
                    return vertexStrips;
                }
                internal set
                {
                    vertexStrips = value;
                }
            }

            public string Name
            {
                get
                {
                    return name;
                }
            }
            public int Index
            {
                get
                {
                    return index;
                }
            }

            // METHODS
            private string GetName(BinaryReader reader)
            {
                string value = string.Empty;

                char terminatingCharacter = char.MinValue;
                char currentCharacter;

                while ((currentCharacter = reader.GetChar()) != terminatingCharacter)
                {
                    value += currentCharacter;
                }

                return value;
            }
        }

        [Serializable]
        public sealed class GCMF : SerializableMember
        {
            /// <summary>
            /// 0x40
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x40;
                }
            }

            #region CONSTRUCTORS
            public GCMF() { }
            public GCMF(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            // CONST
            public const uint GCMF_TYPE_UInt16 = 0x0099;
            public const uint GCMF_TYPE_Single = 0x0098;
            private const ulong GMA_TEXTURE_INFORMATION = ulong.MaxValue; // FFFF FFFF FFFF FFFF
            private const uint GCMF_MAGIC = 0x47434D46; // Spells out GCMF - GameCube Model Format?

            // 
            private uint gcmf_magic;
            private uint unk_0x04; // only on primitive objects. Values: 00, 01
            private Vector3 origin;
            private float radius;
            private ushort textureCount;    // 
            private ushort materialCount;   // nb_mat
            private ushort tlMaterialCount; // nb_tl_mat: [T]rans[L]ucid?
            private ushort unk_0x1E;        // NULL
            private uint indicesOffset;
            private uint unk_0x24;          // NULL
            private ulong unk_0x28;
            private uint unk_0x30; // NULL?
            private uint unk_0x34; // "
            private uint unk_0x38; // "
            private uint unk_0x3C; // "
            #endregion
            #region PROPERTIES
            public uint TextureCount
            {
                get
                {
                    return textureCount;
                }
            }
            public uint Count_MT
            {
                get
                {
                    return materialCount;
                }
            }
            public uint Count_MT_TL
            {
                get
                {
                    return tlMaterialCount;
                }
            }
            public Vector3 Origin
            {
                get
                {
                    return origin;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                gcmf_magic = reader.GetUInt32();

                // HEADER
                if (gcmf_magic != GCMF_MAGIC)
                    throw new Exception();

                unk_0x04 = reader.GetUInt32();
                origin = reader.GetVector3Position();
                radius = reader.GetFloat();
                textureCount = reader.GetUInt16();
                materialCount = reader.GetUInt16();
                tlMaterialCount = reader.GetUInt16();
                unk_0x1E = reader.GetUInt16();
                indicesOffset = reader.GetUInt32();
                unk_0x24 = reader.GetUInt32();
                unk_0x28 = reader.GetUInt64();

                if (unk_0x28 != GMA_TEXTURE_INFORMATION)
                    //throw new Exception();
                    ;

                unk_0x30 = reader.GetUInt32();
                unk_0x34 = reader.GetUInt32();
                unk_0x38 = reader.GetUInt32();
                unk_0x3C = reader.GetUInt32();
            }
            #endregion
        }
        [Serializable]
        public sealed class TextureDescriptor
        {
            #region CONSTRUCTORS
            public TextureDescriptor() { }
            public TextureDescriptor(BinaryReader reader)
            {
                Deserialize(reader);
            }
            #endregion
            #region MEMBERS
            private const byte CONST_UNK_0x0D = 0xFF;
            private const uint CONST_UNK_0x10 = 0x00000030;


            /*/ 0x00 /*/ private ushort flag; // unk_Flag
            /*/ 0x00 /*/ private byte uvMethod;
            /*/ 0x00 /*/ private byte wrapMode;
            /*/ 0x00 /*/ private ushort tplTextureID;
            /*/ 0x00 /*/ private ushort aniso;
            /*/ 0x00 /*/ private uint unk_0x08; // UNKNOWN TYPE
            /*/ 0x00 /*/ private byte unk_0x0C;
            /*/ 0x00 /*/ private byte unk_0x0D;
            /*/ 0x00 /*/ private ushort index;
            /*/ 0x00 /*/ private uint unk_0x10; // 0x00000030
            /*/ 0x00 /*/ private uint unk_0x14; // NULL?
            /*/ 0x00 /*/ private uint unk_0x18; // NULL?
            /*/ 0x00 /*/ private uint unk_0x1C; // NULL?
            #endregion
            #region METHODS
            public byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public void Deserialize(BinaryReader reader)
            {
                flag = reader.GetUInt16();
                uvMethod = reader.GetByte();
                wrapMode = reader.GetByte();
                tplTextureID = reader.GetUInt16();
                aniso = reader.GetUInt16();
                unk_0x08 = reader.GetUInt32();
                unk_0x0C = reader.GetByte();

                if (unk_0x0C != CONST_UNK_0x0D)
                    //throw new Exception();
                    ;

                unk_0x0D = reader.GetByte();
                index = reader.GetUInt16();
                unk_0x10 = reader.GetUInt32();

                if (unk_0x10 != CONST_UNK_0x10)
                    //throw new Exception();
                    ;

                unk_0x14 = reader.GetUInt32();
                unk_0x18 = reader.GetUInt32();
                unk_0x1C = reader.GetUInt32();
            }
            #endregion

            #region TYPES
            public enum unk1_Materialflags
            {
                _00_default = 0,
                _02_SupportFlag = 0x02,
                _40_Relfective = 0x40,
            }
            public enum unk2_MaterialFlags
            {
                _07_Standard = 0x07,
                _87_I4 = 0x87,
            }
            /// <summary>
            /// GCMF WrapMode for textures. TO BE SAVED AS BYTE.
            /// </summary>
            [Flags]
            public enum WrapFlags
            {
                isStandard1 = 1 << 7,
                isStandard2 = 1 << 8,
                mirrorX = 1 << 3,
                mirrorY = 1 << 5,
                repeatX = 1 << 2,
                repeatY = 1 << 4,
            }
            /// <summary>
            /// Anisotropic filter.
            /// http://libogc.devkitpro.org/gx_8h.html
            /// </summary>
            public enum GXAnisotropy
            {
                GX_ANISO_1        = 0x00,
                GX_ANISO_2        = 0x01,
                GX_ANISO_4        = 0x02,
                GX_MAX_ANISOTROPY = 0x03,
            }
            #endregion
        }
        [Serializable]
        public sealed class GC_MaterialDescriptor
        {
            #region CONSTRUCTORS
            public GC_MaterialDescriptor(BinaryReader reader)
            {
                Deserialize(reader);
            }
            #endregion
            #region MEMBERS
            private const byte CONST_UNK_0x11 = 0xFF;
            private const ulong CONST_UNK_0x20 = ulong.MaxValue; // FFFF FFFF FFFF FFFF

            private ushort unk_0x00;
            private ushort lightingFlag;
            private ulong unk_0x08;
            private uint unk_0x0C;
            /// <summary>
            /// Listed all values (stage files only)
            /// Defninitely a bit-layer
            ///  0 0x00 0000_0000
            ///  8 0x08 0000_1000
            /// 17 0x11 0001_0001
            /// 21 0x15 0001_0011
            /// 25 0x19 0001_0101
            /// 34 0x22 0010_0010
            /// 42 0x2A 0010_1010
            /// </summary>
            private byte unk_0x10; // 42, 0, 17, 34, 25, 8, 21
            private byte unk_0x11;
            private byte mt_VertexFormat; // 1, 2, 3 - another vertex format? For MT, next for MT_TL?
            /// <summary>
            /// vtxfmt: Specifies the vertex attribute format. This format is set using the GXSetVtxAttrFmt function
            /// prior to calling GXBegin. Accepted values are GX_NONE, GX_DIRECT, GX_INDEX8, GX_INDEX16.
            /// 
            /// See in GXBegin - GameCube SDK
            /// </summary>
            private byte mt_tl_VertexFormat;
            private ulong unk_0x14;
            private uint vertexFlags; // http://www.gc-forever.com/forums/viewtopic.php?t=1897#p38749
            private ulong unk_0x20;
            private uint mt_size;
            private uint tl_mt_size;
            private Vector3 UVW;
            private uint unk_0x3C_2;
            private uint unk_0x40;
            private uint unk_0x44; // UV flag?
            private uint unk_0x48; // NULL?
            private uint unk_0x4C; // NULL?
            private uint unk_0x50; // NULL?
            private uint unk_0x54; // NULL?
            private uint unk_0x58; // NULL?
            private uint unk_0x5C; // NULL?

            #endregion
            #region METHODS
            public void Serialize()
            {
                throw new NotImplementedException();
            }
            public void Deserialize(BinaryReader reader)
            {
                // FOOTER
                unk_0x00 = reader.GetUInt16();
                lightingFlag = reader.GetUInt16();
                unk_0x08 = reader.GetUInt64();
                unk_0x0C = reader.GetUInt32();
                unk_0x10 = reader.GetByte();
                unk_0x11 = reader.GetByte();

                if (unk_0x11 != CONST_UNK_0x11)
                    throw new Exception();
                    //;// Debug.LogFormat("0x11: {0}", unk_0x11);

                mt_VertexFormat = reader.GetByte();
                mt_tl_VertexFormat = reader.GetByte();
                unk_0x14 = reader.GetUInt64();
                vertexFlags = reader.GetUInt32();
                unk_0x20 = reader.GetUInt64();

                if (unk_0x20 != CONST_UNK_0x20)
                    //throw new Exception();
                    ;// Debug.LogFormat("0x20: {0}", unk_0x20.ToString("X"));

                mt_size = reader.GetUInt32();
                tl_mt_size = reader.GetUInt32();
                UVW = reader.GetVector3Generic(false);
                unk_0x3C_2 = reader.GetUInt32();
                unk_0x40 = reader.GetUInt32();
                unk_0x44 = reader.GetUInt32();
                unk_0x48 = reader.GetUInt32();
                unk_0x4C = reader.GetUInt32();
                unk_0x50 = reader.GetUInt32();
                unk_0x54 = reader.GetUInt32();
                unk_0x58 = reader.GetUInt32();
                unk_0x5C = reader.GetUInt32();

                //Debug.Log(unk_0x10);
            }
            #endregion


            #region PROPERTIES
            public GC_VertexFlags VertexFlags
            {
                get
                {
                    return (GC_VertexFlags)vertexFlags;
                }
            }
            public uint MT_Size
            {
                get
                {
                    return mt_size;
                }
            }
            public uint TL_MT_Size
            {
                get
                {
                    return tl_mt_size;
                }
            }
            #endregion
        }
        [Serializable]
        public sealed class GC_VertexStripCollection
        {
            public GC_VertexStripCollection() { }
            public GC_VertexStripCollection(BinaryReader reader, GCMF gcmf)
            {
                desc = new GC_MaterialDescriptor(reader);
                long baseAddress = reader.BaseStream.Position;
                reader.SkipBytes(1); // Skip 1 byte

                mt_strips = new List<GC_VertexStrip>();
                if (desc.MT_Size > 0)
                {
                    while (true)
                    {
                        byte type = reader.GetByte();
                        if (type == 0x98 | type == 0x99)
                            mt_strips.Add(new GC_VertexStrip(reader, desc.VertexFlags, type));
                        else break;
                    }
                }

                // mt tl reset
                reader.BaseStream.Seek(baseAddress + desc.MT_Size, SeekOrigin.Begin);
                reader.SkipBytes(1); // Skip 1 byte

                mt_tl_strips = new List<GC_VertexStrip>();
                if (desc.TL_MT_Size > 0)
                {
                    while (true)
                    {
                        byte type = reader.GetByte();
                        if (type == 0x98 | type == 0x99)
                            mt_tl_strips.Add(new GC_VertexStrip(reader, desc.VertexFlags, type));
                        else break;
                    }
                }

                // reset after read
                reader.BaseStream.Seek(baseAddress + desc.MT_Size + desc.TL_MT_Size, SeekOrigin.Begin);
            }
            private GC_MaterialDescriptor desc;
            private List<GC_VertexStrip> mt_strips;
            private List<GC_VertexStrip> mt_tl_strips;

            public GC_MaterialDescriptor Desc
            {
                get
                {
                    return desc;
                }
            }
            public GC_VertexStrip[] MT_Strips
            {
                get
                {
                    return mt_strips.ToArray();
                }
            }
            public GC_VertexStrip[] MT_TL_Strips
            {
                get
                {
                    return mt_tl_strips.ToArray();
                }
            }


        }
        [Serializable]
        public sealed class GC_VertexStrip
        {
            #region CONSTRUCTORS
            public GC_VertexStrip(BinaryReader reader, GC_VertexFlags flags, byte type)
            {
                Deserialize(reader, flags, type);
            }
            #endregion
            #region MEMBERS
            private ushort nVerts;
            private byte type;
            private GC_Vertex[] verts;
            #endregion
            #region PROPERTIES
            public byte Type
            {
                get
                {
                    return type;
                }
            }
            public uint Count
            {
                get
                {
                    return nVerts;
                }
            }
            public GC_Vertex[] Verts
            {
                get
                {
                    return verts;
                }
            }
            #endregion
            #region METHODS
            private void Serialize()
            {
                throw new NotImplementedException();
            }
            private void Deserialize(BinaryReader reader, GC_VertexFlags flags, byte type)
            {
                // GXBegin
                // type, vtxfmt, nverts

                this.type = type;
                //type = reader.GetByte();
                nVerts = reader.GetUInt16();

                verts = new GC_Vertex[nVerts];
                for (int i = 0; i < verts.Length; i++)
                    verts[i] = new GC_Vertex(reader, flags, type);
            }
            #endregion
        }
        [Serializable]
        public sealed class GC_Vertex
        {
            #region CONTRUCTORS
            public GC_Vertex(BinaryReader reader, GC_VertexFlags flags, byte type)
            {
                this.type = type;

                if (type == 0x98)
                    Deserialize_0x98(reader, flags);
                else if (type == 0x99)
                    //Deserialize_0x99(reader, flags);
                    ;
                else
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));
            }
            #endregion
            #region MEMBERS
            private uint type;

            private Vector3 position;
            private Vector3 normal;
            private Color32 color0;
            private Color32 color1;
            private Vector2 tex0;
            private Vector2 tex1;
            private Vector2 tex2;
            private Vector2 tex3;
            private Vector3 matrix_1;
            private Vector3 matrix_2;
            private Vector3 matrix_3;
            #endregion
            #region PROPERTIES
            public Vector3 Position
            {
                get
                {
                    return position;
                }
            }
            public Vector3 Normal
            {
                get
                {
                    return normal;
                }
            }
            public Color32 Color0
            {
                get
                {
                    return color0;
                }
            }
            public Color32 Color1
            {
                get
                {
                    return color1;
                }
            }
            public Vector2 Tex0
            {
                get
                {
                    return tex0;
                }
            }
            public Vector2 Tex1
            {
                get
                {
                    return tex1;
                }
            }
            public Vector2 Tex2
            {
                get
                {
                    return tex2;
                }
            }
            public Vector2 Tex3
            {
                get
                {
                    return tex3;
                }
            }
            public Vector3 Matrix_1
            {
                get
                {
                    return matrix_1;
                }
            }
            public Vector3 Matrix_2
            {
                get
                {
                    return matrix_2;
                }
            }
            public Vector3 Matrix_3
            {
                get
                {
                    return matrix_3;
                }
            }

            public uint Type
            {
                get
                {
                    return type;
                }
            }
            #endregion
            #region METHODS
            public static bool BitCompare(GC_VertexFlags flags, GC_VertexFlags compare)
            {
                return ((flags & compare) > 0);
            }
            private void Serialize()
            {
                throw new NotImplementedException();
            }
            private void Deserialize_0x98(BinaryReader reader, GC_VertexFlags flags)
            {
                if (BitCompare(flags, GC_VertexFlags.GX_VA_POS))
                    position = reader.GetVector3Position();
                if (BitCompare(flags, GC_VertexFlags.GX_VA_NRM))
                    normal = reader.GetVector3Normal();

                if (BitCompare(flags, GC_VertexFlags.GX_VA_CLR0))
                    color0 = reader.GetColor32();
                if (BitCompare(flags, GC_VertexFlags.GX_VA_CLR1))
                    color1 = reader.GetColor32();

                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX0))
                    tex0 = reader.GetVector2();
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX1))
                    tex1 = reader.GetVector2();
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX2))
                    tex2 = reader.GetVector2();
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX3))
                    tex3 = reader.GetVector2();

                if (BitCompare(flags, GC_VertexFlags.GX_VA_NBT))
                {
                    matrix_1 = reader.GetVector3Generic(false);
                    matrix_2 = reader.GetVector3Generic(false);
                    matrix_3 = reader.GetVector3Generic(false);
                }
            }
            private void Deserialize_0x99(BinaryReader reader, GC_VertexFlags flags)
            {
                if (BitCompare(flags, GC_VertexFlags.GX_VA_POS))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));
                if (BitCompare(flags, GC_VertexFlags.GX_VA_NRM))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));

                if (BitCompare(flags, GC_VertexFlags.GX_VA_CLR0))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));

                if (BitCompare(flags, GC_VertexFlags.GX_VA_CLR1))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));


                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX0))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX1))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX2))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));
                if (BitCompare(flags, GC_VertexFlags.GX_VA_TEX3))
                    throw new NotImplementedException(reader.BaseStream.Position.ToString("X"));

                if (BitCompare(flags, GC_VertexFlags.GX_VA_NBT))
                {
                    throw new NotImplementedException();
                }
            }
            #endregion
        }


        /// <summary>
        /// http://libogc.devkitpro.org/gx_8h.html
        /// http://www.gc-forever.com/forums/viewtopic.php?t=1897#p38749
        /// More in GameCube%20SDK%201.0/man/gfx/gx/Geometry/GXSetVtxDesc.html
        /// More in GameCube%20SDK%201.0/man/gfx/gx/Geometry/GXNormal.html
        /// </summary>
        public enum GC_VertexFlags
        {
            GX_VA_PTNMTXIDX  = 1 <<  0,
            GX_VA_TEX0MTXIDX = 1 <<  1,
            GX_VA_TEX1MTXIDX = 1 <<  2,
            GX_VA_TEX2MTXIDX = 1 <<  3,
            GX_VA_TEX3MTXIDX = 1 <<  4,
            GX_VA_TEX4MTXIDX = 1 <<  5,
            GX_VA_TEX5MTXIDX = 1 <<  6,
            GX_VA_TEX6MTXIDX = 1 <<  7,
            GX_VA_TEX7MTXIDX = 1 <<  8,
            GX_VA_POS        = 1 <<  9,
            GX_VA_NRM        = 1 << 10,
            GX_VA_CLR0       = 1 << 11,
            GX_VA_CLR1       = 1 << 12,
            GX_VA_TEX0       = 1 << 13,
            GX_VA_TEX1       = 1 << 14,
            GX_VA_TEX2       = 1 << 15,
            GX_VA_TEX3       = 1 << 16,
            GX_VA_TEX4       = 1 << 17,
            GX_VA_TEX5       = 1 << 18,
            GX_VA_TEX6       = 1 << 19,
            GX_VA_TEX7       = 1 << 20,
            GX_POSMTXARRAY   = 1 << 21,
            GX_NRMMTXARRAY   = 1 << 22,
            GX_TEXMTXARRAY   = 1 << 23,
            GX_LIGHTARRAY    = 1 << 24,
            GX_VA_NBT        = 1 << 25, // Normal, Binormal, Tangent
        }
        public enum GC_VertexFlagsIndex
        {
            GX_VA_PTNMTXIDX  =  0,
            GX_VA_TEX0MTXIDX =  1,
            GX_VA_TEX1MTXIDX =  2,
            GX_VA_TEX2MTXIDX =  3,
            GX_VA_TEX3MTXIDX =  4,
            GX_VA_TEX4MTXIDX =  5,
            GX_VA_TEX5MTXIDX =  6,
            GX_VA_TEX6MTXIDX =  7,
            GX_VA_TEX7MTXIDX =  8,
            GX_VA_POS        =  9,
            GX_VA_NRM        = 10,
            GX_VA_CLR0       = 11,
            GX_VA_CLR1       = 12,
            GX_VA_TEX0       = 13,
            GX_VA_TEX1       = 14,
            GX_VA_TEX2       = 15,
            GX_VA_TEX3       = 16,
            GX_VA_TEX4       = 17,
            GX_VA_TEX5       = 18,
            GX_VA_TEX6       = 19,
            GX_VA_TEX7       = 20,
            GX_POSMTXARRAY   = 21,
            GX_NRMMTXARRAY   = 22,
            GX_TEXMTXARRAY   = 23,
            GX_LIGHTARRAY    = 24,
            GX_VA_NBT        = 25,
        }

        public enum GXPrimitive
        {
            GX_TRIANGLE_FAN = 0xA0,
            GX_LINES        = 0xA8,
            GX_QUADS        = 0x80,
            GX_LINE_STRIP   = 0xB0,
            GX_POINTS       = 0xB8,
            GX_TRIANGLES    = 0x90,

            // Relevant to GMA
            GX_TRAINGLE_STRIP = 0x98,
            GX_UINT16_INDEXED_TRIANGLE_STRIP = 0x99,
        }
    }
}