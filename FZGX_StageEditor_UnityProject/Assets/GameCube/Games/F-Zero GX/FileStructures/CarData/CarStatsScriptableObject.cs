// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    // Structure
    // https://github.com/yoshifan/fzerogx-docs/blob/master/addresses/base_machine_stat_blocks.md

    [CreateAssetMenu(fileName = "CarData", menuName = "FZGX ScriptableObject/CarData")]
    public class CarStatsScriptableObject : ScriptableObject, IBinarySerializable
    {
        public float customPartWeight;
        public float machineWeight;
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
        public byte unk1;
        public byte unk2;
        public short unk3;
        public float cameraReorientation;
        public float cameraRepositioning;
        public Vector3 tiltFrontRight;
        public Vector3 tiltFrontLeft;
        public Vector3 tiltBackRight;
        public Vector3 tiltBackLeft;
        public Vector3 wallCollisionFrontRight;
        public Vector3 wallCollisionFrontLeft;
        public Vector3 wallCollisionBackRight;
        public Vector3 wallCollisionBackLeft;

        public void Deserialize(BinaryReader reader)
        {
            customPartWeight = reader.GetFloat();
            machineWeight = reader.GetFloat();
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
            unk1 = reader.GetByte();
            unk2 = reader.GetByte();
            unk3 = reader.GetInt16();
            cameraReorientation = reader.GetFloat();
            cameraRepositioning = reader.GetFloat();
            tiltFrontRight = reader.GetVector3();
            tiltFrontLeft = reader.GetVector3();
            tiltBackRight = reader.GetVector3();
            tiltBackLeft = reader.GetVector3();
            wallCollisionFrontRight = reader.GetVector3();
            wallCollisionFrontLeft = reader.GetVector3();
            wallCollisionBackRight = reader.GetVector3();
            wallCollisionBackLeft = reader.GetVector3();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteX(customPartWeight);
            writer.WriteX(machineWeight);
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
            writer.WriteX(unk1);
            writer.WriteX(unk2);
            writer.WriteX(unk3);
            writer.WriteX(cameraReorientation);
            writer.WriteX(cameraRepositioning);
            writer.WriteX(tiltFrontRight);
            writer.WriteX(tiltFrontLeft);
            writer.WriteX(tiltBackRight);
            writer.WriteX(tiltBackLeft);
            writer.WriteX(wallCollisionFrontRight);
            writer.WriteX(wallCollisionFrontLeft);
            writer.WriteX(wallCollisionBackRight);
            writer.WriteX(wallCollisionBackLeft);
        }
    }
}