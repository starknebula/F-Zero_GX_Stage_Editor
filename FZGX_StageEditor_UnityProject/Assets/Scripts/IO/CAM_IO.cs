// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;
using UnityEditor;

namespace GameCube.Games.FZeroGX.IO
{
    [CreateAssetMenu(fileName = "CAM ImportExport", menuName = "FZGX IO/CAM ImportExport")]
    public class CAM_IO : ImportExportObject
    {
        //[SerializeField]
        //private FZeroGXStage stageIndex;
        //[SerializeField]
        //private string fileFormat = "livecam_demo{0}.{1}";
        [SerializeField]
        private string extension = "bin";
        [SerializeField]
        [BrowseFolderField("FZGX_StageEditor_UnityProject/")]
        private string exportPath;

        public override string filename
        {
            get
            {
                return "";// string.Format(fileFormat, ((int)stageIndex).ToString("D2"), resourcePath);
            }
        }

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

        public override void Import()
        {
            ImportAllLiveCams();
        }
        public override void Export()
        {
            throw new System.NotImplementedException();
        }



        private void ImportSingleCam(string rawFileName, string exportAssetPath)
        {
            try
            {
                using (BinaryReader reader = OpenBinaryReaderWithFile(rawFileName))
                {
                    CAM so = ScriptableObject.CreateInstance(typeof(CAM)) as CAM;
                    so.CameraData = new LiveCamStage();
                    so.CameraData.Deserialize(reader);
                    AssetDatabase.CreateAsset(so, exportAssetPath);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
        private void ImportAllLiveCams()
        {
            foreach (string liveCamStageFormat in LiveCamStages)
            {
                for (int i = 0; i < 50; i++)
                {
                    string rawFileName = string.Format(liveCamStageFormat, i, extension);
                    string exportAssetPath = string.Format(exportPath + "/" + liveCamStageFormat, i, "asset");

                    ImportSingleCam(rawFileName, exportAssetPath);
                }
            }

            foreach (string liveCam50Stage in LiveCam50Stages)
            {
                string rawFileName = string.Format(liveCam50Stage, extension);
                string exportAssetPath = string.Format(exportPath + "/" + liveCam50Stage, "asset");

                ImportSingleCam(rawFileName, exportAssetPath);
            }
        }

    }
}