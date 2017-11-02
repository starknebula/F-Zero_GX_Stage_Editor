// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.IO
{
    public static partial class BinaryReaderWriterExtensions
    {
        ///// Indicates the byte order ("endianness") in which data is written to the stream.
        ///// </summary>
        ///// <remarks>
        ///// This field is set to <c>true</c> by default.
        ///// </remarks>
        //private static bool isLittleEndian = false;
        ///// <summary>
        ///// Indicates the byte order ("endianness") in which data is written to the stream.
        ///// </summary>
        ///// <remarks>
        ///// This field is set to <c>false</c> by default.
        ///// </remarks>
        //public static bool IsLittleEndian
        //{
        //    get
        //    {
        //        return isLittleEndian;
        //    }
        //    set
        //    {
        //        isLittleEndian = value;
        //    }
        //}

        public static void WriteX(this BinaryWriter writer, byte value)
        {
            writer.Write(value);
        }
        public static void WriteX(this BinaryWriter writer, sbyte value)
        {
            writer.Write(value);
        }
        public static void WriteX(this BinaryWriter writer, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian ^ IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }
        public static void WriteX(this BinaryWriter writer, char value)
        {
            throw new NotImplementedException();
        }
        public static void WriteX(this BinaryWriter writer, string value)
        {
            throw new NotImplementedException();
        }
        public static void WriteX(this BinaryWriter writer, decimal value)
        {
            throw new NotImplementedException();
        }
    }
}