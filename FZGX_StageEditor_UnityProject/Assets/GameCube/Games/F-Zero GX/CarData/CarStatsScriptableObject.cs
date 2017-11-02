// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [CreateAssetMenu(fileName = "CarData", menuName = "FZGX ScriptableObject/CarData")]
    public class CarStatsScriptableObject : ScriptableObject, IBinarySerializable2
    {
        public float null1;
        public float weight;
        public float acceleration;
        public float maxSpeed;
        public float grip1;
        public float grip3;
        public float turnTension;
        public float driftAcceleration;
        public float turnMovement;
        public float strafeTurn;
        public float strafe;
        public float turnReaction;
        public float grip2;
        public float boostStrength;
        public float boostDuration;
        public float turnDeceleration;
        public float drag;
        public float body;
        public float driftCamera;
        public float cameraReorientation;
        public float cameraRepositioning;
        public float tilt1;
        public float tilt2;
        public float tilt3;
        public float tilt4;
        public float tilt5;
        public float tilt6;
        public float tilt7;
        public float tilt8;
        public float tilt9;
        public float tilt10;
        public float tilt11;
        public float tilt12;
        public float wallCollision1;
        public float wallCollision2;
        public float wallCollision3;
        public float wallCollision4;
        public float wallCollision5;
        public float wallCollision6;
        public float wallCollision7;
        public float wallCollision8;
        public float wallCollision9;
        public float wallCollision10;
        public float wallCollision11;
        public float wallCollision12;

        public void Deserialize(BinaryReader reader)
        {
            null1 = reader.GetFloat();
            weight = reader.GetFloat();
            acceleration = reader.GetFloat();
            maxSpeed = reader.GetFloat();
            grip1 = reader.GetFloat();
            grip3 = reader.GetFloat();
            turnTension = reader.GetFloat();
            driftAcceleration = reader.GetFloat();
            turnMovement = reader.GetFloat();
            strafeTurn = reader.GetFloat();
            strafe = reader.GetFloat();
            turnReaction = reader.GetFloat();
            grip2 = reader.GetFloat();
            boostStrength = reader.GetFloat();
            boostDuration = reader.GetFloat();
            turnDeceleration = reader.GetFloat();
            drag = reader.GetFloat();
            body = reader.GetFloat();
            driftCamera = reader.GetFloat();
            cameraReorientation = reader.GetFloat();
            cameraRepositioning = reader.GetFloat();
            tilt1 = reader.GetFloat();
            tilt2 = reader.GetFloat();
            tilt3 = reader.GetFloat();
            tilt4 = reader.GetFloat();
            tilt5 = reader.GetFloat();
            tilt6 = reader.GetFloat();
            tilt7 = reader.GetFloat();
            tilt8 = reader.GetFloat();
            tilt9 = reader.GetFloat();
            tilt10 = reader.GetFloat();
            tilt11 = reader.GetFloat();
            tilt12 = reader.GetFloat();
            wallCollision1 = reader.GetFloat();
            wallCollision2 = reader.GetFloat();
            wallCollision3 = reader.GetFloat();
            wallCollision4 = reader.GetFloat();
            wallCollision5 = reader.GetFloat();
            wallCollision6 = reader.GetFloat();
            wallCollision7 = reader.GetFloat();
            wallCollision8 = reader.GetFloat();
            wallCollision9 = reader.GetFloat();
            wallCollision10 = reader.GetFloat();
            wallCollision11 = reader.GetFloat();
            wallCollision12 = reader.GetFloat();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteX(null1);
            writer.WriteX(weight);
            writer.WriteX(acceleration);
            writer.WriteX(maxSpeed);
            writer.WriteX(grip1);
            writer.WriteX(grip3);
            writer.WriteX(turnTension);
            writer.WriteX(driftAcceleration);
            writer.WriteX(turnMovement);
            writer.WriteX(strafeTurn);
            writer.WriteX(strafe);
            writer.WriteX(turnReaction);
            writer.WriteX(grip2);
            writer.WriteX(boostStrength);
            writer.WriteX(boostDuration);
            writer.WriteX(turnDeceleration);
            writer.WriteX(drag);
            writer.WriteX(body);
            writer.WriteX(driftCamera);
            writer.WriteX(cameraReorientation);
            writer.WriteX(cameraRepositioning);
            writer.WriteX(tilt1);
            writer.WriteX(tilt2);
            writer.WriteX(tilt3);
            writer.WriteX(tilt4);
            writer.WriteX(tilt5);
            writer.WriteX(tilt6);
            writer.WriteX(tilt7);
            writer.WriteX(tilt8);
            writer.WriteX(tilt9);
            writer.WriteX(tilt10);
            writer.WriteX(tilt11);
            writer.WriteX(tilt12);
            writer.WriteX(wallCollision1);
            writer.WriteX(wallCollision2);
            writer.WriteX(wallCollision3);
            writer.WriteX(wallCollision4);
            writer.WriteX(wallCollision5);
            writer.WriteX(wallCollision6);
            writer.WriteX(wallCollision7);
            writer.WriteX(wallCollision8);
            writer.WriteX(wallCollision9);
            writer.WriteX(wallCollision10);
            writer.WriteX(wallCollision11);
            writer.WriteX(wallCollision12);
        }
    }
}