// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class BinaryWriterExtensions
{
    /// Indicates the byte order ("endianness") in which data is written to the stream.
    /// </summary>
    /// <remarks>
    /// This field is set to <c>true</c> by default.
    /// </remarks>
    private static bool isLittleEndian = false;
    /// <summary>
    /// Indicates the byte order ("endianness") in which data is written to the stream.
    /// </summary>
    /// <remarks>
    /// This field is set to <c>false</c> by default.
    /// </remarks>
    public static bool IsLittleEndian
    {
        get
        {
            return isLittleEndian;
        }
        set
        {
            isLittleEndian = value;
        }
    }

    public static void WriteX(this BinaryWriter writer, byte value)
    {
        writer.Write(value);
    }
    public static void WriteX(this BinaryWriter writer, sbyte value)
    {
        writer.Write(value);
    }
}