// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [CreateAssetMenu(fileName = "CarData Container", menuName = "FZGX ScriptableObject/CarData Container")]
    public class CarDataSobj : ScriptableObject
    {
        [SerializeField]
        protected List<CarStatsSobj> carStats = new List<CarStatsSobj>();
        public List<CarStatsSobj> CarStats
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
        protected List<CarStatsSobj> customPartStats = new List<CarStatsSobj>();
        public List<CarStatsSobj> CustomPartStats
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