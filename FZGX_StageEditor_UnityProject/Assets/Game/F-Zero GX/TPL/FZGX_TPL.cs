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
using LibGxTexture;

namespace GameCube.Games.FZeroGX.FileStructures
{
    /// <summary>
    /// F-Zero GX modified version of GameCube TPL (Texture Palette)
    /// </summary>
    [System.Serializable]
    public class TPL : IBinarySerializable
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
        #endregion
        #region METHODS
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

        public bool ReadTexture(BinaryReader reader, int index, out Texture2D tex, string name)
        {
            if (index > numDescriptors)
            {
                tex = null;
                return false;
            }
            else if (index < 0)
                throw new System.IndexOutOfRangeException("Index must be greater than 0!");

            // TODO
            // Load using GameCube.GX.Texture methods
            TEXDescriptor desc = descriptorArray[index];
            try
            {
                GxTextureFormatCodec codec = GxTextureFormatCodec.GetCodec((GxTextureFormat)desc.format);
                Debug.Log(codec.GetType().Name);
                reader.BaseStream.Position = desc.dataPtr;
                byte[] texRaw = reader.GetBytes(codec.CalcTextureSize(desc.width, desc.height));
                byte[] texRGBA = new byte[4 * desc.width * desc.height]; // RGBA (4) * w * h
                Debug.LogFormat("{0}, {1}", desc.width, desc.height);
                codec.DecodeTexture(texRGBA, 0, desc.width, desc.height, desc.width * 4, texRaw, 0, null, 0);

                tex = new Texture2D(desc.width, desc.height);
                for (int y = 0; y < desc.height; y++)
                    for (int x = 0; x < desc.width; x++)
                    {
                        tex.SetPixel(x, y, new Color32(
                            texRGBA[(y * desc.width + x) * 4 + 0],
                            texRGBA[(y * desc.width + x) * 4 + 1],
                            texRGBA[(y * desc.width + x) * 4 + 2],
                            texRGBA[(y * desc.width + x) * 4 + 3]));
                    }

                SaveBytes(string.Format("{0}.png", name), tex.EncodeToPNG());
                return true;
            }
            catch
            {
                tex = null;
                return false;
            }
        }

        #endregion

        #region INTERNAL CLASSES
        /// <summary>
        /// 
        /// Is byte aligned to 32B (GC FIFO)
        /// </summary>
        [System.Serializable]
        public class TEXDescriptor : IBinarySerializable
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
            public byte entryOccupied;
            public byte format;

            public uint dataPtr; // pointer

            // TODO: double check order here (h/w?)
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
                entryOccupied = reader.GetByte();
                format = reader.GetByte();

                dataPtr = reader.GetUInt32();

                height = reader.GetUInt16();
                width = reader.GetUInt16();

                powerOf = reader.GetUInt16();
                endianness = reader.GetUInt16();
            }
            #endregion
        }
        #endregion

        public static string PersistantDataPath(string filename)
        {
            // Because Unity
            string value = string.Format("{0}/{1}", Application.persistentDataPath, filename);

            //#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            //                value = value.Replace('/', '\\');
            //#endif

            return string.Format("{0}/{1}", Application.persistentDataPath, filename);
            //return Path.Combine(Application.persistentDataPath, filename);
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