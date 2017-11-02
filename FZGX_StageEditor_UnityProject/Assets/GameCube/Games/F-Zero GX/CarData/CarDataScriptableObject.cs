// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [CreateAssetMenu(fileName = "CarData Container", menuName = "FZGX ScriptableObject/CarData Container")]
    public class CarDataScriptableObject : ScriptableObject
    {
        [SerializeField]
        protected List<CarStatsScriptableObject> carStats = new List<CarStatsScriptableObject>();
        public List<CarStatsScriptableObject> CarStats
        {
            get
            {
                return carStats;
            }
            internal set
            {
                carStats = value;
            }
        }

        [SerializeField]
        protected List<CarStatsScriptableObject> customPartStats = new List<CarStatsScriptableObject>();
        public List<CarStatsScriptableObject> CustomPartStats
        {
            get
            {
                return customPartStats;
            }
            internal set
            {
                customPartStats = value;
            }
        }

    }
}