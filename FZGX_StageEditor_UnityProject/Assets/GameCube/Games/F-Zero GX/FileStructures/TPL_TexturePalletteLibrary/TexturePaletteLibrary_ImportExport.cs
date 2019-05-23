// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;
using GameCube.Games.FZeroGX;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [CreateAssetMenu(fileName = "Texture Pallet Library (TPL) ImportExport", menuName = "FZGX ImportExport/TPL ImportExport")]
    public class TexturePaletteLibrary_ImportExport : ImportExportObject
    {
        [SerializeField]
        private TexturePaletteLibrary_ScriptableObject[] exportData;

        [SerializeField]
        private string stageTplFormat = "st{0}.tpl,lz";

        public override string HelpBoxImport => "---";
        public override string HelpBoxExport => "---";
        public override string ExportMessage => "Texture Palette Library (TPL) export successful";
        public override string ImportMessage => "Texture Palette Library (TPL) import successful";


        public override void Import()
        {
            BinaryReaderWriterExtensions.PushEndianess(false);

            // Test with stage 1
            string fileName = string.Format(stageTplFormat, (1).ToString("00"));
            using (BinaryReader reader = OpenBinaryReaderWithFile(fileName))
            {
                // Strips the extension
                fileName = Path.GetFileNameWithoutExtension(fileName);
                // Strips the pseudo extension (,lz)
                int indexOfComma = fileName.LastIndexOf(',');
                fileName = fileName.Substring(0, fileName.Length - indexOfComma + 1);

                CreateSobjFromBinaryStream<TexturePaletteLibrary_ScriptableObject>(fileName, reader);
            }


            BinaryReaderWriterExtensions.PopEndianess();
        }
        public override void Export()
        {
            throw new System.NotImplementedException();
        }
    }
}