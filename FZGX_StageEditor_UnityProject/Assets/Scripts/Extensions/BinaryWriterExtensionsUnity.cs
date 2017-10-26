// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using GameCube.Games.FZeroGX;

public static partial class BinaryWriterExtensions
{

    public static void WritePosition(this BinaryWriter writer, Vector3 value)
    {
        writer.Write(value.x * ((StageManager.doInverseWindingPositionX) ? -1f : 1f));
        writer.Write(value.y);
        writer.Write(value.z);
    }
    public static void WriteRotation(this BinaryWriter writer, Vector3 value)
    {
        writer.Write(value.x * ((StageManager.doInverseWindingRotationX) ? -1f : 1f));
        writer.Write(value.y);
        writer.Write(value.z);
    }
    public static void WriteScale(this BinaryWriter writer, Vector3 value)
    {
        writer.Write(value.x * ((StageManager.doInverseWindingScaleX) ? -1f : 1f));
        writer.Write(value.y);
        writer.Write(value.z);
    }
    public static void WriteNormal(this BinaryWriter writer, Vector3 value)
    {
        writer.Write(value.x * ((StageManager.doInverseWindingNormalX) ? -1f : 1f));
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public static void WriteX(this BinaryWriter writer, Vector2 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
    }
    public static void WriteX(this BinaryWriter writer, Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }
    public static void WriteX(this BinaryWriter writer, Color32 value)
    {
        writer.Write(value.r);
        writer.Write(value.g);
        writer.Write(value.b);
        writer.Write(value.a);
    }
    public static void WriteX(this BinaryWriter writer, Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }
}