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
        [SerializeField]
        private string fileName = "cardata,lz";
        [SerializeField]
        private CarDataScriptableObject carDataExport;

        public override string ImportMessage => "CarData imported!";
        public override string ExportMessage => "CarData exported!"; 
        public override string HelpBoxImport => "Imports all modules from 'fileName' into 'importPath'";
        public override string HelpBoxExport => "Exports 'carDataExport' to 'exportPath'";

        public override void Import()
        {
            BinaryReaderExtensions.IsLittleEndian = true;
            using (BinaryReader reader = OpenBinaryReaderWithFile(fileName))
            {
                CarDataScriptableObject container = CreateScriptableObject<CarDataScriptableObject>("CAR_DATA");

                // Get all 41 vehicles
                for (int i = 0; i < 41; i++)
                {
                    string fileName = string.Format("vehicle_stats_{0}", (i + 1).ToString("00"));

                    container.CarStats.Add(
                      CreateScriptableObjectFromBinaryStream<CarStatsScriptableObject>(fileName, reader)
                      );
                }
            }
            BinaryReaderExtensions.IsLittleEndian = false;
        }
        public override void Export()
        {
            BinaryReaderExtensions.IsLittleEndian = true;
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                foreach (CarStatsScriptableObject carStat in carDataExport.CarStats)
                    carStat.Serialize(writer);
                Save(writer, carDataExport.name);
                EditorUtility.SetDirty(carDataExport);
            }
            EditorUtility.SetDirty(this);
            BinaryReaderExtensions.IsLittleEndian = false;
        }
    }
}