// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    public class CameraData : MonoBehaviour
    {
        public struct CameraParams
        {
            public uint frameDuration;
            public float lerpSpeed;
        }

        public struct CameraInterpolation
        {
            public Vector3 position;
            public Quaternion rotation;
            public float fov;
            public uint paramA;
            public uint paramB;
        }
    }
}