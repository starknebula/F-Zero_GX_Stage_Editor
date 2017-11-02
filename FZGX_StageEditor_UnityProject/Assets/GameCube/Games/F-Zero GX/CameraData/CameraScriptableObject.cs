// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "FZGX ScriptableObject/Camera Data")]
    public class CameraScriptableObject : ScriptableObject
    {
        [SerializeField]
        protected LiveCamStage cameraData;
        public LiveCamStage CameraData
        {
            get
            {
                return cameraData;
            }
            internal set
            {
                cameraData = value;
            }
        }
    }
}