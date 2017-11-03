// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCube.GX.Enumerated_Types;
using GameCube.GX.Texture;
using GameCube;
using GameCube.LibGxTexture;

namespace GameCube.Games.FZeroGX.FileStructures
{
    /// <summary>
    /// F-Zero GX modified version of GameCube TPL (Texture Palette)
    /// </summary>
    [System.Serializable]
    public class TPL : IBinarySerializableDepricated
    {
        #region CONSTRUCTORS
        public TPL() { }
        /// <summary>
        /// Deserialize TPL by passing in BinaryReader with TPL file to parse
        /// </summary>
        /// <param name="reader">The reader housing the TPL file stream to deserialize from</param>
        public TPL(BinaryReader reader)
        {
            Deserialize(reader, 0);
        }
        #endregion

        #region MEMBERS
        private uint numDescriptors;
        private TEXDescriptor[] descriptorArray;
        #endregion

        #region PROPERTIES
        public uint NumDescriptors
        {
            get
            {
                return numDescriptors;
            }
            internal set
            {
                numDescriptors = value;
            }
        }
        public TEXDescriptor[] DescriptorArray
        {
            get
            {
                return descriptorArray;
            }
            internal set
            {
                descriptorArray = value;
            }
        }
        #endregion

        #region METHODS
        // IBinarySerializable
        public byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }
        public void Deserialize(BinaryReader reader, long address)
        {
            reader.BaseStream.Seek(address, SeekOrigin.Begin);

            numDescriptors = reader.GetUInt32();

            descriptorArray = new TEXDescriptor[numDescriptors];
            for (int i = 0; i < descriptorArray.Length; i++)
                descriptorArray[i] = new TEXDescriptor(reader);
        }

        // 
        public bool ReadTextureFromTPL(BinaryReader reader, int index, out Texture2D texture, string name)
        {
            return ReadTextureFromTPL(reader, index, out texture, name, false);
        }
        public bool ReadTextureFromTPL(BinaryReader reader, int index, out Texture2D texture, string name, bool saveToDisk)
        {
            // If index is invalid
            if (index > numDescriptors)
                throw new System.IndexOutOfRangeException(string.Format("Index must be less than or equal to {0}!", numDescriptors));
            else if (index < 0)
                throw new System.IndexOutOfRangeException("Index must be greater than 0!");

            // Load Texture Descriptor at index
            TEXDescriptor desc = descriptorArray[index];

            // Verify if index is valid (some entries can be nulled out)
            // We check for 0 specifically because garbage can be store in the
            // first few entries of the file.
            if (desc.isNullEntry != 0)
            {
                texture = null;
                return false;
            }

            // We use Try as GxTextureFormatCodec.GetCodec() can return an error if the type is invalid
            try
            {
                GxTextureFormatCodec codec = GxTextureFormatCodec.GetCodec((GxTextureFormat)desc.format);
                reader.BaseStream.Position = desc.dataPtr;
                byte[] texRaw = reader.GetBytes(codec.CalcTextureSize(desc.width, desc.height));
                byte[] texRGBA = new byte[4 * desc.width * desc.height]; // RGBA (4 bytes) * w * h
                codec.DecodeTexture(texRGBA, 0, desc.width, desc.height, desc.width * 4, texRaw, 0, null, 0);

                // Reconstruct Texture using Unity's format
                texture = new Texture2D(desc.width, desc.height);
                for (int y = 0; y < desc.height; y++)
                {
                    for (int x = 0; x < desc.width; x++)
                    {
                        // Invert Y because LibGXTexture return array upside-down
                        // ei 'x, (desc.width - y)' instead of 'x, y'
                        texture.SetPixel(x, (desc.width - y), new Color32(
                            texRGBA[(y * desc.width + x) * 4 + 0],
                            texRGBA[(y * desc.width + x) * 4 + 1],
                            texRGBA[(y * desc.width + x) * 4 + 2],
                            texRGBA[(y * desc.width + x) * 4 + 3]));
                    }
                }

                if (saveToDisk)
                {
                    Debug.LogFormat("Saved {0} to path {1}", name, PersistantDataPath(""));
                    SaveBytes(string.Format("{0}.png", name), texture.EncodeToPNG());
                }

                return true;
            }
            catch
            {
                Debug.LogErrorFormat("GxTextureFormatCodec.GetCodec() failed to find format [{0}]", desc.format.ToString("X"));
                texture = null;
                return false;
            }
        }
        public bool WriteTextureToTPL(BinaryWriter writer, int index, Texture2D texture)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region INTERNAL CLASSES
        /// <summary>
        /// 
        /// Is byte aligned to 32B (GC FIFO)
        /// </summary>
        [System.Serializable]
        public class TEXDescriptor : IBinarySerializableDepricated
        {
            #region CONSTRUCTORS
            public TEXDescriptor() { }
            public TEXDescriptor(BinaryReader reader)
            {
                // Skip address seek because it is not necessary, pass in current address
                Deserialize(reader, reader.BaseStream.Position);
            }
            #endregion

            #region MEMBERS
            public ushort pad16;
            public byte isNullEntry;
            public byte format;

            public uint dataPtr; // Reader Address

            public ushort width;
            public ushort height;

            public ushort powerOf;
            public ushort endianness; // 1234 instead of 3412
            #endregion

            #region METHODS
            public byte[] Serialize()
            {
                throw new System.NotImplementedException();
            }
            public void Deserialize(BinaryReader reader, long address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                // Read
                pad16 = reader.GetUInt16();
                isNullEntry = reader.GetByte();
                format = reader.GetByte();

                dataPtr = reader.GetUInt32();

                width = reader.GetUInt16();
                height = reader.GetUInt16();

                powerOf = reader.GetUInt16();
                endianness = reader.GetUInt16();
            }
            #endregion
        }
        #endregion

        // TEMP
        public static string PersistantDataPath(string filename)
        {
            return string.Format("{0}/{1}", Application.persistentDataPath, filename);
        }
        public static bool SaveBytes(string filename, byte[] bytes)
        {
            try
            {
                using (FileStream stream = new FileStream(PersistantDataPath(filename), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    Debug.Log(PersistantDataPath(filename));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}