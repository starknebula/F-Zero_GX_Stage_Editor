// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
public abstract class ImportExportObject : ScriptableObject
{
    [Header("Save Settings")]
    [SerializeField]
    protected FileMode fileMode = FileMode.Create;
    [SerializeField]
    protected FileAccess fileAccess = FileAccess.Write;
    [SerializeField]
    protected FileShare fileShare = FileShare.None;
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

    // Could use some tidying up
    public string SourcePathFull => Path.Combine(Application.dataPath, sourcePath).PathToUnityPath();
    public string ImportPathFull => Path.Combine(Application.dataPath, Regex.Match(importPath, "(?<=Assets/).*$").Value).PathToUnityPath();
    public string ExportPathFull => Path.Combine(Application.dataPath, Regex.Match(exportPath, "(?<=Assets/).*$").Value).PathToUnityPath();

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



    // TODO
    // OpenBinaryReaderWithFile assumes FileMode.Open

    public BinaryReader OpenBinaryReaderWithFile(string filename)
    {
        return OpenBinaryReaderWithFile(filename, System.Text.Encoding.ASCII);
    }
    public BinaryReader OpenBinaryReaderWithFile(string fileName, System.Text.Encoding encoding)
    {
        string path = Path.Combine(ProjectAssetsFolder, sourcePath, fileName);
        return new BinaryReader(File.Open(path, FileMode.Open));
    }
    public void Save(BinaryWriter binaryWriter, string fileName)
    {
        WriteStreamToFile(binaryWriter, exportPath, fileName, sourceExtension, fileMode, fileAccess, fileShare);
    }
    public void Save(BinaryWriter binaryWriter, string fileName, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
    {
        WriteStreamToFile(binaryWriter, exportPath, fileName, sourceExtension, fileMode, fileAccess, fileShare);
    }
    private void WriteStreamToFile(BinaryWriter binaryWriter, string directory, string fileName, string fileExtension, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
    {
        binaryWriter.Flush();
        string filePath = string.Format("{0}/{1}.{2}", directory, fileName, fileExtension);

        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        using (FileStream file = File.Open(filePath, fileMode, fileAccess, fileShare))
        {
            binaryWriter.Seek(0, SeekOrigin.Begin);
            binaryWriter.BaseStream.CopyTo(file);
            file.Flush();
        }

        if (debugSavePath)
            Debug.Log(filePath);
    }
    public ScriptableObjectType CreateScriptableObject<ScriptableObjectType>(string fileName) where ScriptableObjectType : ScriptableObject
    {
        ScriptableObjectType so = ScriptableObject.CreateInstance(typeof(ScriptableObjectType)) as ScriptableObjectType;
        string filePath = string.Format("{0}/{1}.asset", importPath, fileName);
        AssetDatabase.CreateAsset(so, filePath);
        return so;
    }
    public ScriptableObjectType CreateScriptableObjectFromBinaryStream<ScriptableObjectType>(string fileName, BinaryReader reader) where ScriptableObjectType : ScriptableObject, IBinarySerializable
    {
        ScriptableObjectType so = ScriptableObject.CreateInstance(typeof(ScriptableObjectType)) as ScriptableObjectType;
        string filePath = string.Format("{0}/{1}.asset", importPath, fileName);
        AssetDatabase.CreateAsset(so, filePath);
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);
        so.Deserialize(reader);

        return so;
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

            if (GUILayout.Button(editorTarget.ImportButtonText))
            {
                editorTarget.Import();
                Debug.Log(editorTarget.ImportMessage);
            }
            EditorGUILayout.HelpBox(editorTarget.HelpBoxImport, MessageType.Info);
            EditorGUILayout.Space();

            if (GUILayout.Button(editorTarget.ExportButtonText))
            {
                editorTarget.Export();
                Debug.Log(editorTarget.ExportMessage);
            }
            EditorGUILayout.HelpBox(editorTarget.HelpBoxExport, MessageType.Info);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}
#endif