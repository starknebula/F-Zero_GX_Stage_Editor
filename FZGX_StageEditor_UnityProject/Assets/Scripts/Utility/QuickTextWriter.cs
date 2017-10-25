// Created by Raphael "Stark" Tetreault 25/10/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 25/10/2017

using UnityEngine;
using System;
using System.IO;
using System.Text;

/// <summary>
/// Quickly write small text files
/// </summary>
public static class QuickTextWriter
{
    #region MEMBERS
    private static MemoryStream memStream = new MemoryStream();
    private static BufferedStream bufStream = new BufferedStream(memStream);

    private static readonly string directorySeperator = new string(Path.DirectorySeparatorChar, 1);
    private const string tab = "\t";
    private const string newline = "\n";

    private static Environment.SpecialFolder specialFolder = Environment.SpecialFolder.Personal;
    private static Encoding encoding = Encoding.UTF8;
    private static bool consolePrintPathOnSave = true;
    private static bool clearStreamAfterSave = true;
    private static FileMode fileMode = FileMode.Create;
    private static FileAccess fileAccess = FileAccess.Write;
    #endregion

    #region PROPERTIES
    // GET SET
    public static Environment.SpecialFolder SpecialFolder
    {
        get
        {
            return specialFolder;
        }
        set
        {
            specialFolder = value;
        }
    }
    public static Encoding Encoding
    {
        get
        {
            return encoding;
        }
        set
        {
            encoding = value;
        }
    }
    public static bool ConsolePrintPathOnSave
    {
        get
        {
            return consolePrintPathOnSave;
        }
        set
        {
            consolePrintPathOnSave = value;
        }
    }
    public static bool ClearStreamAfterSave
    {
        get
        {
            return clearStreamAfterSave;
        }
        set
        {
            clearStreamAfterSave = value;
        }
    }
    public static FileMode FileMode
    {
        get
        {
            return fileMode;
        }
        set
        {
            fileMode = value;
        }
    }
    public static FileAccess FileAccess
    {
        get
        {
            return fileAccess;
        }
        set
        {
            fileAccess = value;
        }
    }
    // GET
    public static string SpecialDirectory
    {
        get
        {
            return Directory.GetParent(Environment.GetFolderPath(specialFolder)).FullName;
        }
    }
    public static string PersistantDataDirectory
    {
        get
        {
            return Application.persistentDataPath;
        }
    }
    #endregion

    #region METHODS
    public static void WriteToBuffer(string value)
    {
        byte[] b_value = encoding.GetBytes(value);
        bufStream.Write(b_value, 0, b_value.Length);
    }
    public static void WriteLineToBuffer(string value)
    {
        byte[] b_value = encoding.GetBytes(value);
        byte[] b_newline = encoding.GetBytes(newline);

        bufStream.Write(b_value, 0, b_value.Length);
        bufStream.Write(b_newline, 0, b_newline.Length);
    }
    public static void WriteTsvLineToBuffer(params string[] values)
    {
        foreach (string value in values)
        {
            WriteToBuffer(value);
            WriteToBuffer(tab);
        }
    }
    public static void WriteTsvLineEndToBuffer(params string[] values)
    {
        foreach (string value in values)
        {
            WriteToBuffer(value);
            WriteToBuffer(tab);
        }
        WriteToBuffer(newline);
    }

    private static void WriteFile(string directory, string fileName, string fileExtension, FileMode fileMode, FileAccess fileAccess)
    {
        bufStream.Flush();
        string filePath = string.Format("{0}{1}{2}.{3}", directory, directorySeperator, fileName, fileExtension);

        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        using (FileStream file = new FileStream(filePath, fileMode, fileAccess))
        {
            memStream.Seek(0, SeekOrigin.Begin);
            memStream.WriteTo(file);

            if (consolePrintPathOnSave)
                Debug.LogFormat("File created at {0}", file.Name);
        }

        if (clearStreamAfterSave)
            ClearStream();
    }
    public static void ClearStream()
    {
        bufStream.Dispose();
        memStream.Dispose();
        memStream = new MemoryStream();
        bufStream = new BufferedStream(memStream);
    }
    public static void Save(string fileName, string extension)
    {
        WriteFile(SpecialDirectory, fileName, extension, fileMode, fileAccess);
    }
    public static void Save(string directory, string fileName, string extension)
    {
        WriteFile(directory, fileName, extension, fileMode, fileAccess);
    }
    public static void Save(string directory, string fileName, string extension, FileMode fileMode, FileAccess fileAccess)
    {
        WriteFile(directory, fileName, extension, fileMode, fileAccess);
    }
    #endregion
}