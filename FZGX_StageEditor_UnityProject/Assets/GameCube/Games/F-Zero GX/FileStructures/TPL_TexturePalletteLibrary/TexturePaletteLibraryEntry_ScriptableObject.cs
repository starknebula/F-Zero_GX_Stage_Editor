//// Created by Raphael "Stark" Tetreault /2017
//// Copyright (c) 2017 Raphael Tetreault
//// Last updated 

//using System;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GameCube.LibGxTexture;

//namespace GameCube.Games.FZeroGX.FileStructures
//{
//    internal partial class FZeroGXImportExportMenuConstants
//    {
//        internal const string FzgxImportExportMenuTab = "FZGX ImportExport";
//        internal const string TplFileName = "TexturePaletteLibrary TextureDescriptor";
//        internal const string TplMenuName = FzgxImportExportMenuTab + "/" + TplFileName;
//    }

//    [CreateAssetMenu(fileName = FZeroGXImportExportMenuConstants.TplFileName, menuName = FZeroGXImportExportMenuConstants.TplMenuName)]
//    public class TexturePaletteLibraryEntry_ScriptableObject : ScriptableObject
//    {
//        //[HideInInspector]
//        //public ushort pad16;
//        //[HideInInspector]
//        //public byte isNullEntry;
//        //// Only exposed value
//        public GxTextureFormat format;
//        //[HideInInspector]
//        //public uint dataPtr; // Reader Address
//        //[HideInInspector]
//        //public ushort width;
//        //[HideInInspector]
//        //public ushort height;
//        //[HideInInspector]
//        //public ushort powerOf;
//        //[HideInInspector]
//        //public ushort endianness; // 1234 instead of 3412

//        public Texture texture;

//        public void Serialize(BinaryWriter writer)
//        {

//        }
//        public void Deserialize(BinaryReader reader)
//        {
//            reader.SkipBytes(0x03); // Skip 3 bytes
//            format = (GxTextureFormat)reader.GetByte();
//            reader.SkipBytes(0x0C); // Skip 12 bytes

//            //pad16 = reader.GetUInt16();
//            //isNullEntry = reader.GetByte();
//            //format = reader.GetByte();
//            //dataPtr = reader.GetUInt32();
//            //width = reader.GetUInt16();
//            //height = reader.GetUInt16();
//            //powerOf = reader.GetUInt16();
//            //endianness = reader.GetUInt16();
//        }
//    }
//}