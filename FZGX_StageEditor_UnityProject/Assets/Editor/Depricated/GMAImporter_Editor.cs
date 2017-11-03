// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.ImportExport;

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomEditor(typeof(GMA_Importer))]
    internal class GMA_Importer_Editor : FZGX_ImporterExporter_Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GMA_Importer editorTarget = target as GMA_Importer;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                editorTarget.segmentSelect++;
            }
            if (GUILayout.Button("-"))
            {
                editorTarget.segmentSelect--;
            }
            EditorGUILayout.EndHorizontal();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif