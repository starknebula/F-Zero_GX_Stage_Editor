// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;
using UnityEditor;
using System;

namespace GameCube.Games.FZeroGX.IO
{
    [CreateAssetMenu(fileName = "CAM ImportExport", menuName = "FZGX IO/CAM ImportExport")]
    public class CAM_IO : ImportExportObject
    {
        [SerializeField]
        private CameraScriptableObject[] exportData;

        public override string HelpBoxImport => "Imports all assets from \"cam/\" into Unity editable ScriptableObjects at 'Import Path'";
        public override string HelpBoxExport => "Exports all assets in 'Export Data' into packed .bin format used by F-Zero GX.";
        public override string ExportMessage => "Camera Data export successful";
        public override string ImportMessage => "Camera Data import successful";

        #region CONSTANTS
        private const string livecam = "livecam_{0}.{1}";
        private const string livecam_ball = "livecam_ball_{0}.{1}";
        private const string livecam_stage = "livecam_stage_{0}.{1}";
        private const string livecam_stage_demo = "livecam_stage_demo_{0}.{1}";
        private string[] LiveCamStages = new string[]
        {
            livecam_stage,
            livecam_stage_demo,
        };
        private const string livecam_stage_demo_50_end = "livecam_stage_demo_50_end.{0}";
        private const string livecam_stage_50_1st = "livecam_stage_50_1st.{0}";
        private const string livecam_stage_50_2p = "livecam_stage_50_2p.{0}";
        private const string livecam_stage_50_3p = "livecam_stage_50_3p.{0}";
        private const string livecam_stage_50_4p = "livecam_stage_50_4p.{0}";
        private string[] LiveCam50Stages = new string[]
        {
            livecam_stage_demo_50_end,
            livecam_stage_50_1st,
            livecam_stage_50_2p,
            livecam_stage_50_3p,
            livecam_stage_50_4p,
        };
        #endregion

        public override void Import()
        {
            ImportCameraDataAll();
        }
        public override void Export()
        {
            foreach (CameraScriptableObject export in exportData)
            {
                using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
                {
                    export.CameraData.Serialize(writer);
                    Save(writer, export.name);
                    EditorUtility.SetDirty(export);
                }
            }
            EditorUtility.SetDirty(this);
        }

        private void ImportCameraData(string rawFileName, string importAssetPath)
        {
            try
            {
                using (BinaryReader reader = OpenBinaryReaderWithFile(rawFileName))
                {
                    CameraScriptableObject so = ScriptableObject.CreateInstance(typeof(CameraScriptableObject)) as CameraScriptableObject;
                    so.CameraData = new LiveCamStage();
                    so.CameraData.Deserialize(reader);
                    AssetDatabase.CreateAsset(so, importAssetPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
        private void ImportCameraDataAll()
        {
            foreach (string liveCamStageFormat in LiveCamStages)
            {
                for (int i = 0; i < 50; i++)
                {
                    string rawFileName = string.Format(liveCamStageFormat, i, sourceExtension);
                    string importAssetPath = string.Format(importPath + "/" + liveCamStageFormat, i, "asset");

                    ImportCameraData(rawFileName, importAssetPath);
                }
            }

            foreach (string liveCam50Stage in LiveCam50Stages)
            {
                string rawFileName = string.Format(liveCam50Stage, sourceExtension);
                string importAssetPath = string.Format(importPath + "/" + liveCam50Stage, "asset");

                ImportCameraData(rawFileName, importAssetPath);
            }
        }
    }
}