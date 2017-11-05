// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GameCube.Games.FZeroGX.FileStructures;
//using GameCube.LibGxTexture;

//namespace UnityEditor
//{
//    [CustomEditor(typeof(TexturePaletteLibrary_ScriptableObject))]
//    internal class TexturePaletteLibrary_ScriptableObject_Editor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            var editorTarget = target as TexturePaletteLibrary_ScriptableObject;

//            editorTarget.numDescriptors = (uint)EditorGUILayout.IntField(ObjectNames.NicifyVariableName("numDescriptors"), (int)editorTarget.numDescriptors);
//            EditorGUILayout.Space();

//            for (int i = 0; i < editorTarget.numDescriptors; i++)
//            {
//                //Rect guiWidth = GUILayoutUtility.GetLastRect();

//                //string objectName = ObjectNames.NicifyVariableName((editorTarget.descriptorArray[i].texture == null) ? "null" : editorTarget.descriptorArray[i].texture.name);
//                EditorGUILayout.BeginHorizontal();
//                EditorGUILayout.BeginVertical();
//                string objectName = string.Format("Element {0}", i);
//                EditorGUILayout.LabelField(objectName);//, GUILayout.Width(guiWidth.width * 0.7f));
//                editorTarget.descriptorArray[i].format = (byte)(GxTextureFormat)EditorGUILayout.EnumPopup((GxTextureFormat)editorTarget.descriptorArray[i].format);
//                EditorGUILayout.EndVertical();
//                editorTarget.descriptorArray[i].texture = (Texture2D)EditorGUILayout.ObjectField(string.Empty, editorTarget.descriptorArray[i].texture, typeof(Texture2D), false);//, GUILayout.Width(guiWidth.width * 0.3f));
//                EditorGUILayout.EndHorizontal();
//            }
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GameCube.Games.FZeroGX.FileStructures;
//using GameCube.LibGxTexture;

//namespace UnityEditor
//{
//    [CustomPropertyDrawer(typeof(TexturePaletteLibrary_ScriptableObject.TEXDescriptor))]
//    internal class TexturePaletteLibrary_ScriptableObject_TexDescriptor_Editor : PropertyDrawer
//    {
//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return base.GetPropertyHeight(property, label);
//        }
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            //base.OnGUI(position, property, label);

//            SerializedProperty format = property.FindPropertyRelative("format");
//            SerializedProperty texture = property.FindPropertyRelative("texture");


//        }
//    }
//}