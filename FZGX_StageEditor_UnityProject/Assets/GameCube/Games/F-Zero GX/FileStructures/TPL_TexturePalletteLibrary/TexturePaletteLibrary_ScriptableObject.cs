// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCube.LibGxTexture;

namespace GameCube.Games.FZeroGX.FileStructures
{
    internal partial class FZeroGXImportExportMenuConstants
    {
        internal const string FzgxImportExportMenuTab = "FZGX ImportExport";
        internal const string TplFileName = "TexturePaletteLibrary TextureDescriptor";
        internal const string TplMenuName = FzgxImportExportMenuTab + "/" + TplFileName;
    }

    [CreateAssetMenu(fileName = FZeroGXImportExportMenuConstants.TplFileName, menuName = FZeroGXImportExportMenuConstants.TplMenuName)]
    public class TexturePaletteLibrary_ScriptableObject : ScriptableObject, IBinarySerializable
    {
        // Values public for Editor Script
        [HideInInspector]
        public uint numDescriptors;
        public TEXDescriptor[] descriptorArray;


        #region EDITOR METHODS
#if UNITY_EDITOR
        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
        public void Deserialize(BinaryReader reader)
        {
            numDescriptors = reader.GetUInt32();
            descriptorArray = new TEXDescriptor[numDescriptors];
            for (uint i = 0; i < descriptorArray.Length; i++)
            {
                descriptorArray[i] = new TEXDescriptor();
                descriptorArray[i].Deserialize(reader);
            }

            string filePath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            string destFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)).PathToUnityPath();
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            if (!Directory.Exists(destFilePath))
                AssetDatabase.CreateFolder(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

            for (int i = 0; i < numDescriptors; i++)
                descriptorArray[i].texture = ReadTexture(reader, descriptorArray[i], destFilePath, string.Format("{0}_idx{1}", fileName, i));
        }

        public Texture2D ReadTexture(BinaryReader reader, TEXDescriptor desc, string saveFilePath, string textureName)
        {
            // Verify if index is valid (some entries can be nulled out)
            // We check for 0 specifically because garbage can be store in the
            // first few entries of the file.
            // TPL can have null entries so this is actually correct to the format
            if (desc.isNullEntry != 0)
                return null;

            // We use Try as GxTextureFormatCodec.GetCodec() can return an error if the type is invalid
            try
            {
                GxTextureFormatCodec codec = GxTextureFormatCodec.GetCodec((GxTextureFormat)desc.format);
                reader.BaseStream.Position = desc.dataPtr;
                byte[] texRaw = reader.GetBytes(codec.CalcTextureSize(desc.width, desc.height));
                byte[] texRGBA = new byte[4 * desc.width * desc.height]; // RGBA (4 bytes) * w * h
                codec.DecodeTexture(texRGBA, 0, desc.width, desc.height, desc.width * 4, texRaw, 0, null, 0);

                // Reconstruct Texture using Unity's format
                Texture2D texture = new Texture2D(desc.width, desc.height);
                for (int y = 0; y < desc.height; y++)
                {
                    for (int x = 0; x < desc.width; x++)
                    {
                        // Invert Y because LibGXTexture returns array upside-down?
                        // ei 'x, (desc.width - y)' instead of 'x, y'
                        texture.SetPixel(x, (desc.width - y), new Color32(
                            texRGBA[(y * desc.width + x) * 4 + 0],
                            texRGBA[(y * desc.width + x) * 4 + 1],
                            texRGBA[(y * desc.width + x) * 4 + 2],
                            texRGBA[(y * desc.width + x) * 4 + 3]));
                    }
                }

                string assetPath = string.Format("{0}/tex_{1}.png", saveFilePath, textureName).PathToUnityPath();
                byte[] imageBytes = texture.EncodeToPNG();
                DestroyImmediate(texture);

                using (BinaryWriter writer = new BinaryWriter(File.Create(assetPath, imageBytes.Length)))
                {
                    writer.Write(imageBytes);
                }

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh(ImportAssetOptions.Default);
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
                return tex;
            }
            catch
            {
                Debug.LogErrorFormat("GxTextureFormatCodec.GetCodec() failed to find format [0x{0}]", desc.format.ToString("X2"));
                return null;
            }
        }
        public void WriteTexture(BinaryWriter writer, Texture2D texture, GxTextureFormat format)
        {
            // Decide whether to use name as format or link each texture to a SO
            // Name only is nice because it means no linkage with SO
            // SO is nice because there need not be copies of textures to export

            // The only data needed from TEXDescriptor
            // - format

            throw new System.NotImplementedException();
        }
#endif
        #endregion

        [Serializable]
        public class TEXDescriptor : IBinarySerializable
        {
            public ushort pad16;
            public byte isNullEntry;
            public byte format;
            public uint dataPtr; // Reader Address
            public ushort width;
            public ushort height;
            public ushort powerOf;
            public ushort endianness; // 1234 instead of 3412

            public Texture2D texture;

            #region EDITOR METHODS
#if UNITY_EDITOR
            public void Serialize(BinaryWriter writer)
            {
                throw new System.NotImplementedException();
            }
            public void Deserialize(BinaryReader reader)
            {
                pad16 = reader.GetUInt16();
                isNullEntry = reader.GetByte();
                format = reader.GetByte();
                dataPtr = reader.GetUInt32();
                width = reader.GetUInt16();
                height = reader.GetUInt16();
                powerOf = reader.GetUInt16();
                endianness = reader.GetUInt16();

                // Make ScriptableObject cleaner
                // Make 0x00 I4 appear as empty
                // Prevent garbage data from being mis-interpreted
                if (isNullEntry != 0)
                    format = 0xFF;
            }
#endif
            #endregion
        }


    }
}