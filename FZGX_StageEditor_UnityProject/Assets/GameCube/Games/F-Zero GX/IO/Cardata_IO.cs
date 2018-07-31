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

namespace GameCube.Games.FZeroGX.IO
{
    [CreateAssetMenu(fileName = "CarData ImportExport", menuName = "FZGX IO/CarData ImportExport")]
    public class Cardata_IO : ImportExportObject
    {
        private readonly int numVehicles = Enum.GetNames(typeof(VehicleName)).Length;
        private readonly int numBodyParts = Enum.GetNames(typeof(CustomBodyPartName)).Length;
        private readonly int numCockpitParts = Enum.GetNames(typeof(CustomCockpitPartName)).Length;
        private readonly int numBoosterParts = Enum.GetNames(typeof(CustomBoosterPartName)).Length;

        private const long MachinePtr = 0x0000;
        private const long CustomPartsPtr = 0x1F04;
        private const string IndexFormat = "00";

        [SerializeField]
        private string fileName = "cardata,lz";
        [SerializeField]
        private CarDataSobj exportData;

        public override string ImportMessage => "CarData imported!";
        public override string ExportMessage => "CarData exported!";
        public override string HelpBoxImport => "Imports all modules from 'fileName' in folder 'sourcePath' into folder 'importPath'";
        public override string HelpBoxExport => "Exports 'exportData' to folder 'exportPath'";

        public override void Import()
        {
            BinaryReaderWriterExtensions.PushEndianess(true);
            using (BinaryReader reader = OpenBinaryReaderWithFile(fileName))
            {
                CarDataSobj container = CreateScriptableObject<CarDataSobj>(this.fileName);
                exportData = container;
                string assetName = string.Empty;
                const string title = "Importing Vehicle Assets";
                float total = numVehicles + numBodyParts + numCockpitParts + numBoosterParts - 1;
                float offset = 0;

                // MACHINES
                reader.BaseStream.Seek(MachinePtr, SeekOrigin.Begin);
                for (int i = 0; i < numVehicles; i++)
                {
                    int displayIndex = i + 1;

                    // AX Machines
                    if (displayIndex > 31)
                        displayIndex -= 1;
                    // Dark Schneider
                    else if (displayIndex == 31)
                        displayIndex = 0;

                    assetName = string.Format("cardata_machine_{0}_{1}", FormatIndex(displayIndex), (VehicleName)displayIndex);
                    container.CarStats.Add(CreateSobjFromBinaryStream<CarStatsSobj>(assetName, reader));
                    var progress = (offset + i) / total;
                    EditorUtility.DisplayProgressBar(title, assetName, progress);
                }
                offset += numVehicles;

                // CUSTOM PARTS
                reader.BaseStream.Seek(CustomPartsPtr, SeekOrigin.Begin);
                for (int i = 0; i < numBodyParts; i++)
                {
                    assetName = string.Format("cardata_body_{0}_{1}", FormatIndex(i + 1), (CustomBodyPartName)i);
                    container.CustomPartStats.Add(CreateSobjFromBinaryStream<CarStatsSobj>(assetName, reader));
                    var progress = (offset + i) / total;
                    EditorUtility.DisplayProgressBar(title, assetName, progress);
                }
                offset += numBodyParts;

                for (int i = 0; i < numCockpitParts; i++)
                {
                    assetName = string.Format("cardata_cockpit_{0}_{1}", FormatIndex(i + 1), (CustomCockpitPartName)i);
                    container.CustomPartStats.Add(CreateSobjFromBinaryStream<CarStatsSobj>(assetName, reader));
                    var progress = (offset + i) / total;
                    EditorUtility.DisplayProgressBar(title, assetName, progress);
                }
                offset += numCockpitParts;

                for (int i = 0; i < numBoosterParts; i++)
                {
                    assetName = string.Format("cardata_booster_{0}_{1}", FormatIndex(i + 1), (CustomBoosterPartName)i);
                    container.CustomPartStats.Add(CreateSobjFromBinaryStream<CarStatsSobj>(assetName, reader));
                    var progress = (offset + i) / total;
                    EditorUtility.DisplayProgressBar(title, assetName, progress);
                }
                offset += numBoosterParts;
                EditorUtility.ClearProgressBar();
            }
            BinaryReaderWriterExtensions.PopEndianess();
        }
        private string FormatIndex(int index)
        {
            return index.ToString(IndexFormat);
        }

        public override void Export()
        {
            string sourceFilePath = Path.Combine(SourcePathFull, fileName).PathToUnityPath();
            string exportFilePath = Path.Combine(ExportPathFull, fileName).PathToUnityPath();

            // Copy original file
            File.Copy(sourceFilePath, exportFilePath, true);

            BinaryReaderWriterExtensions.PushEndianess(true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(exportFilePath, fileMode, fileAccess)))
            {
                writer.BaseStream.Seek(MachinePtr, SeekOrigin.Begin);
                foreach (CarStatsSobj carStat in exportData.CarStats)
                    carStat.Serialize(writer);

                //writer.BaseStream.Seek(CustomPartsPtr, SeekOrigin.Begin);
                writer.BaseStream.Position = CustomPartsPtr;
                foreach (CarStatsSobj customPartStats in exportData.CustomPartStats)
                    customPartStats.Serialize(writer);
            }
            EditorUtility.SetDirty(this);
            BinaryReaderWriterExtensions.PopEndianess();
        }
    }
}