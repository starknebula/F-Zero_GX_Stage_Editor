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
        private uint frameDuration;
        [SerializeField]
        private float lerpSpeed;
        [SerializeField]
        private uint unk_0x08;

        public void Deserialize(BinaryReader reader)
        {
            frameDuration = reader.GetUInt32();
            lerpSpeed = reader.GetFloat();
            unk_0x08 = reader.GetUInt32();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(frameDuration);
            writer.Write(lerpSpeed);
            writer.Write(unk_0x08);
        }
    }

    [Serializable]
    public class CameraInterpolation : IBinarySerializable2
    {
        public Vector3 position;
        public Vector3 rotation;
        public float fov;
        public uint paramFlagsA;
        public uint paramFlagsB;

        public void Deserialize(BinaryReader reader)
        {
            position = reader.GetVector3Position();
            rotation = reader.GetVector3Rotation();
            fov = reader.GetFloat();
            paramFlagsA = reader.GetUInt32();
            paramFlagsB = reader.GetUInt32();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WritePosition(position);
            writer.WriteRotation(position);
            writer.Write(fov);
            writer.Write(paramFlagsA);
            writer.Write(paramFlagsB);
        }
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