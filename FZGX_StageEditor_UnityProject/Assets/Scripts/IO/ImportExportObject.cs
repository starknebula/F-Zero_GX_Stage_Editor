// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;

public abstract class ImportExportObject : ScriptableObject
{
    [Header("Save Settings")]
    [SerializeField]
    protected FileMode fileMode = FileMode.Create;
    [SerializeField]
    protected FileAccess fileAccess = FileAccess.Write;
    [SerializeField]
    protected bool debugSavePath = true;

    [Header("Import Settings")]
    [SerializeField]
    protected string sourceExtension = "bin";
    [SerializeField]
    [BrowseFolderField]
    protected string sourcePath;
    [SerializeField]
    [BrowseFolderField("FZGX_StageEditor_UnityProject/")]
    protected string importPath;

    [Header("Export Settings")]
    [SerializeField]
    [BrowseFolderField("FZGX_StageEditor_UnityProject/")]
    protected string exportPath;


    public virtual string ImportButtonText
    {
        get
        {
            return "Import";
        }
    }
    public virtual string ExportButtonText
    {
        get
        {
            return "Export";
        }
    }
    public abstract string ImportMessage
    {
        get;
    }
    public abstract string ExportMessage
    {
        get;
    }
    public abstract string HelpBoxImport
    {
        get;
    }
    public abstract string HelpBoxExport
    {
        get;
    }
    private string ProjectAssetsFolder => Application.dataPath;

    public abstract void Import();
    public abstract void Export();

    public BinaryReader OpenBinaryReaderWithFile(string filename)
    {
        return OpenBinaryReaderWithFile(filename, System.Text.Encoding.ASCII);
    }
    public BinaryReader OpenBinaryReaderWithFile(string fileName, System.Text.Encoding encoding)
    {
        //TextAsset file = Resources.Load<TextAsset>(filename);
        string path = Path.Combine(ProjectAssetsFolder, sourcePath, fileName);
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(0), encoding);
            fileStream.CopyTo(binaryReader.BaseStream);
            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            return binaryReader;
        }
    }
    public void Save(BinaryWriter binaryWriter, string fileName)
    {
        WriteFile(binaryWriter, exportPath, fileName, sourceExtension, fileMode, fileAccess);
    }
    public void Save(BinaryWriter binaryWriter, string fileName, FileMode fileMode, FileAccess fileAccess)
    {
        WriteFile(binaryWriter, exportPath, fileName, sourceExtension, fileMode, fileAccess);
    }

    private void WriteFile(BinaryWriter binaryWriter, string directory, string fileName, string fileExtension, FileMode fileMode, FileAccess fileAccess)
    {
        binaryWriter.Flush();
        string filePath = string.Format("{0}/{1}.{2}", directory, fileName, fileExtension);

        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        using (FileStream file = new FileStream(filePath, fileMode, fileAccess))
        {
            binaryWriter.Seek(0, SeekOrigin.Begin);
            binaryWriter.BaseStream.CopyTo(file);
            file.Flush();
        }

        if (debugSavePath)
            Debug.Log(filePath);
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
            ImportExportObject editorTarget = target as ImportExportObject;

            //EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(editorTarget.ImportButtonText))
            {
                editorTarget.Import();
                Debug.Log(editorTarget.ImportMessage);
            }
            EditorGUILayout.HelpBox(editorTarget.HelpBoxImport, MessageType.Info);
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(editorTarget.ExportButtonText))
            {
                editorTarget.Export();
                Debug.Log(editorTarget.ExportMessage);
            }
            EditorGUILayout.HelpBox(editorTarget.HelpBoxExport, MessageType.Info);
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}
#endif