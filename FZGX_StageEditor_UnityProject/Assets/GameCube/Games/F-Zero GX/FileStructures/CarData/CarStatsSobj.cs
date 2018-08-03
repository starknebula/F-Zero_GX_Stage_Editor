// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCube.Games.FZeroGX.FileStructures.CarData
{
    // Structure
    // https://github.com/yoshifan/fzerogx-docs/blob/master/addresses/base_machine_stat_blocks.md

    [CreateAssetMenu(fileName = "CarData", menuName = "FZGX ScriptableObject/CarData")]
    public class CarStatsSobj : ScriptableObject, IBinarySerializable
    {
        [HideInInspector]
        public uint unused_0x00;
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
        [EnumFlags]
        [FormerlySerializedAs("Unk1")]
        public UnknownEnumFlags1 unknownEnumFlags_0x48;
        [FormerlySerializedAs("Unk2")]
        [FormerlySerializedAs("unknownEnumFlags_0x49")]
        public byte unknownByte_0x49;
        [HideInInspector]
        public ushort unused_0x4A;
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
            unused_0x00 = reader.GetUInt32();
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
            unknownEnumFlags_0x48 = (UnknownEnumFlags1)reader.GetByte();
            unknownByte_0x49 = reader.GetByte();
            unused_0x4A = reader.GetUInt16();
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
            writer.WriteX(unused_0x00);
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
            writer.WriteX((byte)unknownEnumFlags_0x48);
            writer.WriteX(unknownByte_0x49);
            writer.WriteX(unused_0x4A);
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