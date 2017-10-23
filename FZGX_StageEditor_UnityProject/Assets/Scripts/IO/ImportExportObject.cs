// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;

public abstract class ImportExportObject : ScriptableObject
{
    [SerializeField]
    [BrowseFolderField("Resources/")]
    protected string resourcePath;

    public abstract void Import();
    public abstract void Export();

    public abstract string filename
    {
        get;
    }

    public BinaryReader GetStreamFromFile(string filename)
    {
        return GetStreamFromFile(filename, System.Text.Encoding.ASCII);
    }
    public BinaryReader GetStreamFromFile(string filename, System.Text.Encoding encoding)
    {
        TextAsset file = Resources.Load<TextAsset>(filename);
        Stream stream = new MemoryStream(file.bytes);
        BinaryReader binaryReader = new BinaryReader(stream, encoding);

        return binaryReader;
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomEditor(typeof(ImportExportObject), true)]
    public class ImportExportObject_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ImportExportObject editorTarget = target as ImportExportObject;


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
#endif