// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;

public abstract class ImportExportObject : ScriptableObject
{
    [SerializeField]
    [BrowseFolderField]
    protected string resourcePath;

    private string ProjectAssetsFolder => Application.dataPath;

    public abstract void Import();
    public abstract void Export();

    public abstract string filename
    {
        get;
    }

    public BinaryReader OpenBinaryReaderWithFile(string filename)
    {
        return OpenBinaryReaderWithFile(filename, System.Text.Encoding.ASCII);
    }
    public BinaryReader OpenBinaryReaderWithFile(string filePath, System.Text.Encoding encoding)
    {
        //TextAsset file = Resources.Load<TextAsset>(filename);
        string path = Path.Combine(ProjectAssetsFolder, resourcePath, filePath);
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(0), encoding);
            fileStream.CopyTo(binaryReader.BaseStream);
            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            return binaryReader;
        }
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