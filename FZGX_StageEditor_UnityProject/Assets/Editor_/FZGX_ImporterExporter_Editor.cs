using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCube.Games.FZeroGX.ImportExport;

namespace UnityEditor
{
    [CustomEditor(typeof(FZGX_ImporterExporter), true)]
    public class FZGX_ImporterExporter_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FZGX_ImporterExporter editorTarget = target as FZGX_ImporterExporter;


            if (GUILayout.Button("Import"))
            {
                editorTarget.Import();
                Debug.Log("Import");
            }
            if (GUILayout.Button("Export"))
            {
                editorTarget.Export();
                Debug.Log("Export");
            }
        }
    }
}