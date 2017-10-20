using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using FzgxData;


namespace System.IO
{
    /// <summary>
    /// Defines BinaryReaderExtensions for F-Zero GX Stage Editor
    /// </summary>
    public static partial class BinaryReaderExtensions
    {
        public static Vector3 GetVector3Position(this BinaryReader reader)
        {
            return new Vector3(
                reader.GetFloat() * ((Stage.doInverseWindingPositionX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Vector3 GetVector3Rotation(this BinaryReader reader)
        {
            return new Vector3(
                reader.GetFloat() * ((Stage.doInverseWindingRotationX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Quaternion GetQuaternionRotation(this BinaryReader reader)
        {
            return Quaternion.Euler(
                reader.GetFloat() * ((Stage.doInverseWindingRotationX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Vector3 GetVector3Scale(this BinaryReader reader)
        {
            return new Vector3(
                reader.GetFloat() * ((Stage.doInverseWindingScaleX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Vector3 GetVector3Normal(this BinaryReader reader)
        {
            return new Vector3(
                reader.GetFloat() * ((Stage.doInverseWindingNormalX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Vector3 GetVector3Generic(this BinaryReader reader, bool flipX)
        {
            return new Vector3(
                reader.GetFloat() * ((flipX) ? -1f : 1f),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }

        public static void SkipBytes(this BinaryReader reader, int count)
        {
            reader.ReadBytes(count);
        }

        public static Vector2 GetVector2(this BinaryReader reader)
        {
            return new Vector2(
                reader.GetFloat(),
                reader.GetFloat()
                );
        }
        public static Color32 GetColor32(this BinaryReader reader)
        {
            return new Color32(
                reader.GetByte(),
                reader.GetByte(),
                reader.GetByte(),
                reader.GetByte()
                );
        }
        public static Quaternion GetQuaternion(this BinaryReader reader)
        {
            return new Quaternion(
                reader.GetFloat(),
                reader.GetFloat(),
                reader.GetFloat(),
                reader.GetFloat()
                );
        }

        // SPECIFIC TO GMA
        public static Vector3 GetShortVector3(this BinaryReader reader)
        {
            return new Vector3(
                reader.GetInt16() / 16383.0f,
                reader.GetInt16() / 16383.0f,
                reader.GetInt16() / 16383.0f
                );
        }
        public static Vector3 GetShortVector3(this BinaryReader reader, bool flipX)
        {
            return new Vector3(
                reader.GetInt16() / 16383.0f * ((flipX) ? -1f : 1f),
                reader.GetInt16() / 16383.0f,
                reader.GetInt16() / 16383.0f
                );
        }
        public static Vector2 GetShortVector2(this BinaryReader reader)
        {
            return new Vector2(
                reader.GetInt16() / 16383.0f,
                reader.GetInt16() / 16383.0f
                );
        }
        public static float GetShortFloat(this BinaryReader reader)
        {
            return reader.GetInt16() / 16383.0f;
        }
    }
}