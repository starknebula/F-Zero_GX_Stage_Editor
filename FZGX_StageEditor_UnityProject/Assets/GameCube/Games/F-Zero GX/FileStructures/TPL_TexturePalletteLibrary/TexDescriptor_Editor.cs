// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;
using GameCube.LibGxTexture;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(TexturePaletteLibrary_ScriptableObject.TEXDescriptor))]
    internal class TexDescriptor_Editor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty format = property.FindPropertyRelative("format");
            SerializedProperty texture = property.FindPropertyRelative("texture");

            Rect contentPosition = position;
            string textureName = (texture.objectReferenceValue != null) ? texture.objectReferenceValue.name : "null";
            contentPosition = EditorGUI.PrefixLabel(contentPosition, new GUIContent(string.Format("{0} : {1}", label.text, textureName)));
            contentPosition.width = contentPosition.width / 3f * 2;
            texture.objectReferenceValue = (Texture2D)EditorGUI.ObjectField(contentPosition, texture.objectReferenceValue, typeof(Texture2D), false);
            contentPosition.x += contentPosition.width;
            contentPosition.width /= 2f;
            format.intValue = (int)(GxTextureFormat)EditorGUI.EnumPopup(contentPosition, (GxTextureFormat)format.intValue);
        }
    }
}