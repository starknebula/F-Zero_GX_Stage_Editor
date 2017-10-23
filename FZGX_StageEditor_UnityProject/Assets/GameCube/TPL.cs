// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.GX.Enumerated_Types;
using GameCube.GX.Texture;
using System.IO;

namespace GameCube.FileFormats
{
    /// <summary>
    /// (Vanilla) Texture Palette
    /// Port from texPalette.h (GameCube TPL format)
    /// </summary>
    public class TPL
    {
        /// <summary>
        /// Color Lookup Table Header
        /// </summary>
        public struct CLUTHeader
        {
            public ushort numEntries;
            public byte unpacked;
            public byte pad8; // Padding byte

            public GXTlutFmt format;
            public uint prt; // pointer to data
        }

        public struct TEXHeader
        {
            public ushort height;
            public ushort width;

            public uint format;
            public uint data;

            public GXTexWrapMode wrapS;
            public GXTexWrapMode wrapT;

            public GXTexFilter minFilter;
            public GXTexFilter maxFilter;

            public float LODBias;

            public byte edgeLODEnabled;
            public byte minLOD;
            public byte maxLOD;
            public byte unpacked;
        }
    }
}