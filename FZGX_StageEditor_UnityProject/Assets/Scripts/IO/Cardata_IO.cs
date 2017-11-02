// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameCube.Games.FZeroGX.FileStructures;
using GameCube.Games.FZeroGX.Machines;

namespace GameCube.Games.FZeroGX.IO
{
    [CreateAssetMenu(fileName = "CarData ImportExport", menuName = "FZGX IO/CarData ImportExport")]
    public class Cardata_IO : ImportExportObject
    {
        private readonly int numberOfVehicles = Enum.GetNames(typeof(VehicleName)).Length;

        [SerializeField]
        private string fileName = "cardata,lz";
        [SerializeField]
        private CarDataScriptableObject exportData;

        public override string ImportMessage => "CarData imported!";
        public override string ExportMessage => "CarData exported!";
        public override string HelpBoxImport => "Imports all modules from 'fileName' in folder 'sourcePath' into folder 'importPath'";
        public override string HelpBoxExport => "Exports 'exportData' to folder 'exportPath'";

        public override void Import()
        {
            BinaryReaderWriterExtensions.PushEndianess(true);
            using (BinaryReader reader = OpenBinaryReaderWithFile(fileName))
            {
                CarDataScriptableObject container = CreateScriptableObject<CarDataScriptableObject>(fileName);
                exportData = container;

                for (int i = 0; i < numberOfVehicles; i++)
                {
                    int displayIndex = i+1;

                    // AX Machines
                    if (displayIndex > 31)
                        displayIndex -= 1;
                    // Dark Schneider
                    else if (displayIndex == 31)
                        displayIndex = 0;

                    string fileName = string.Format("cardata_{0}_{1}", (displayIndex).ToString(), (VehicleName)displayIndex);
                    container.CarStats.Add(CreateScriptableObjectFromBinaryStream<CarStatsScriptableObject>(fileName, reader));
                }
            }
            BinaryReaderWriterExtensions.PopEndianess();
        }
        public override void Export()
        {
            string sourceFilePath = Path.Combine(SourcePathFull, fileName).PathToSystemPath();
            string exportFilePath = Path.Combine(ExportPathFull, fileName).PathToSystemPath();

            // Copy original file
            if (!File.Exists(exportFilePath))
                File.Copy(sourceFilePath, exportFilePath);

            BinaryReaderWriterExtensions.PushEndianess(true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(exportFilePath, fileMode, fileAccess)))
            {
                writer.BaseStream.Seek(0, SeekOrigin.Begin);

                foreach (CarStatsScriptableObject carStat in exportData.CarStats)
                    carStat.Serialize(writer);

                EditorUtility.SetDirty(exportData);
            }
            EditorUtility.SetDirty(this);
            BinaryReaderWriterExtensions.PopEndianess();
        }
    }
}