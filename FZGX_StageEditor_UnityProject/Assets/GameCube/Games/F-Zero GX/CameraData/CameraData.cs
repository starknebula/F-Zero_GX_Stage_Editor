// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures
{
    [Serializable]
    public class LiveCamStage : IBinarySerializable2
    {
        public List<CameraPan> cameraPans;

        public void Deserialize(BinaryReader reader)
        {
            cameraPans = new List<CameraPan>();

            while (!reader.EndOfStream())
            {
                CameraPan pan = new CameraPan();
                pan.Deserialize(reader);
                cameraPans.Add(pan);
            }
        }
        public void Serialize(BinaryWriter writer)
        {
            foreach (CameraPan cPan in cameraPans)
                cPan.Serialize(writer);
        }
    }

    [Serializable]
    public class CameraPan : IBinarySerializable2
    {
        public CameraParams @params;
        public CameraInterpolation interpolateFrom;
        public CameraInterpolation interpolateTo;

        public void Deserialize(BinaryReader reader)
        {
            @params = new CameraParams();
            interpolateFrom = new CameraInterpolation();
            interpolateTo = new CameraInterpolation();

            @params.Deserialize(reader);
            interpolateFrom.Deserialize(reader);
            interpolateTo.Deserialize(reader);
        }
        public void Serialize(BinaryWriter writer)
        {
            @params.Serialize(writer);
            interpolateFrom.Serialize(writer);
            interpolateTo.Serialize(writer);
        }
    }

    [Serializable]
    public class CameraParams : IBinarySerializable2
    {
        [SerializeField]
        internal uint frameDuration;
        [SerializeField]
        internal float lerpSpeed;
        [SerializeField]
        internal uint unk_0x08;

        public void Deserialize(BinaryReader reader)
        {
            frameDuration = reader.GetUInt32();
            lerpSpeed = reader.GetFloat();
            unk_0x08 = reader.GetUInt32();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteX(frameDuration);
            writer.WriteX(lerpSpeed);
            writer.WriteX(unk_0x08);
        }
    }

    [Serializable]
    public class CameraInterpolation : IBinarySerializable2
    {
        public Vector3 position;
        public Vector3 rotation;
        public float fov;
        [EnumFlags]
        public CameraLayers paramFlagsA;
        [EnumFlags]
        public CameraLayers paramFlagsB;

        public Quaternion Rotation
        {
            get
            {
                return Quaternion.Euler(rotation);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            position = reader.GetVector3Position();
            rotation = reader.GetVector3Rotation();
            fov = reader.GetFloat();
            paramFlagsA = (CameraLayers)reader.GetUInt32();
            paramFlagsB = (CameraLayers)reader.GetUInt32();

            //paramsA = new BitLayer(paramFlagsA);
            //paramsB = new BitLayer(paramFlagsB);
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WritePosition(position);
            writer.WriteRotation(rotation);
            writer.WriteX(fov);
            writer.WriteX((uint)paramFlagsA);
            writer.WriteX((uint)paramFlagsB);
        }
    }

    [Flags]
    public enum CameraLayers
    {
        _1 = 1 << 1,
        _2 = 1 << 2,
        _3 = 1 << 3,
        _4 = 1 << 4,
        _5 = 1 << 5,
        _6 = 1 << 6,
        _7 = 1 << 7,
        _8 = 1 << 8,
        _9 = 1 << 9,
        _10 = 1 << 10,
        _11 = 1 << 11,
        _12 = 1 << 12,
        _13 = 1 << 13,
        _14 = 1 << 14,
        _15 = 1 << 15,
        _16 = 1 << 16,
        _17 = 1 << 17,
        _18 = 1 << 18,
        _19 = 1 << 19,
        _20 = 1 << 20,
        _21 = 1 << 21,
        _22 = 1 << 22,
        _23 = 1 << 23,
        _24 = 1 << 24,
        _25 = 1 << 25,
        _26 = 1 << 26,
        _27 = 1 << 27,
        _28 = 1 << 28,
        _29 = 1 << 29,
        _30 = 1 << 30,
        _31 = 1 << 31,
        _32 = 1 << 32,
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameCube.Games.FZeroGX.FileStructures.CameraParams))]
    internal class CameraParams_PropertyDrawer : PropertyDrawer
    {
        private Type Type
        {
            get
            {
                return typeof(GameCube.Games.FZeroGX.FileStructures.CameraParams);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * (Type.GetFields().Length + 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            System.Reflection.FieldInfo[] info = Type.GetFields();
            position.height /= info.Length + 1;

            EditorGUI.LabelField(position, new GUIContent(property.displayName));
            position.y += position.height;

            EditorGUI.indentLevel++;
            foreach (var prop in info)
            {
                SerializedProperty sprop = property.FindPropertyRelative(prop.Name);
                EditorGUI.PropertyField(position, sprop, new GUIContent(sprop.displayName));
                position.y += position.height;
            }
            EditorGUI.indentLevel--;
        }
    }
    [CustomPropertyDrawer(typeof(GameCube.Games.FZeroGX.FileStructures.CameraInterpolation))]
    internal class CameraInterpolation_PropertyDrawer : PropertyDrawer
    {
        private Type Type
        {
            get
            {
                return typeof(GameCube.Games.FZeroGX.FileStructures.CameraInterpolation);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * (Type.GetFields().Length + 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            System.Reflection.FieldInfo[] info = Type.GetFields();
            position.height /= info.Length + 1;

            EditorGUI.LabelField(position, new GUIContent(property.displayName));
            position.y += position.height;

            EditorGUI.indentLevel++;
            foreach (var prop in info)
            {
                SerializedProperty sprop = property.FindPropertyRelative(prop.Name);
                EditorGUI.PropertyField(position, sprop, new GUIContent(sprop.displayName));
                position.y += position.height;
            }
            EditorGUI.indentLevel--;
        }
    }
}
#endif