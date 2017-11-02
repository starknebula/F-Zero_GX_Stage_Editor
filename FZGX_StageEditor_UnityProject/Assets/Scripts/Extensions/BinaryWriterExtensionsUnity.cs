// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using GameCube.Games.FZeroGX;

namespace System.IO
{
    public static partial class BinaryReaderWriterExtensions
    {

        public static void WritePosition(this BinaryWriter writer, Vector3 value)
        {
            writer.WriteX(value.x * ((StageManager.doInverseWindingPositionX) ? -1f : 1f));
            writer.WriteX(value.y);
            writer.WriteX(value.z);
        }
        public static void WriteRotation(this BinaryWriter writer, Vector3 value)
        {
            writer.WriteX(value.x * ((StageManager.doInverseWindingRotationX) ? -1f : 1f));
            writer.WriteX(value.y);
            writer.WriteX(value.z);
        }
        public static void WriteScale(this BinaryWriter writer, Vector3 value)
        {
            writer.WriteX(value.x * ((StageManager.doInverseWindingScaleX) ? -1f : 1f));
            writer.WriteX(value.y);
            writer.WriteX(value.z);
        }
        public static void WriteNormal(this BinaryWriter writer, Vector3 value)
        {
            writer.WriteX(value.x * ((StageManager.doInverseWindingNormalX) ? -1f : 1f));
            writer.WriteX(value.y);
            writer.WriteX(value.z);
        }

        public static void WriteX(this BinaryWriter writer, Vector2 value)
        {
            writer.WriteX(value.x);
            writer.WriteX(value.y);
        }
        public static void WriteX(this BinaryWriter writer, Vector3 value)
        {
            writer.WriteX(value.x);
            writer.WriteX(value.y);
            writer.WriteX(value.z);
        }
        public static void WriteX(this BinaryWriter writer, Color32 value)
        {
            writer.WriteX(value.r);
            writer.WriteX(value.g);
            writer.WriteX(value.b);
            writer.WriteX(value.a);
        }
        public static void WriteX(this BinaryWriter writer, Quaternion value)
        {
            writer.WriteX(value.x);
            writer.WriteX(value.y);
            writer.WriteX(value.z);
            writer.WriteX(value.w);
        }
    }
}