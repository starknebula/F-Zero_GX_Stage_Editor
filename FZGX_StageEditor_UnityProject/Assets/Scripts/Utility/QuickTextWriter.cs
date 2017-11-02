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
    private static MemoryStream _memStream = new MemoryStream();
    private static BufferedStream _bufStream = new BufferedStream(_memStream);

    private static readonly string DirectorySeperator = new string(Path.DirectorySeparatorChar, 1);
    private const string Tab     = "\t";
    private const string Newline = "\n";
    #endregion

    #region PROPERTIES
    // GET SET
    public static Environment.SpecialFolder SpecialFolder { get; set; } = Environment.SpecialFolder.Personal;
    public static Encoding Encoding                       { get; set; } = Encoding.UTF8;
    public static bool ConsolePrintPathOnSave             { get; set; } = true;
    public static bool ClearStreamAfterSave               { get; set; } = true;
    public static FileMode FileMode                       { get; set; } = FileMode.Create;
    public static FileAccess FileAccess                   { get; set; } = FileAccess.Write;
    // GET
    public static string SpecialDirectory        => Directory.GetParent(Environment.GetFolderPath(SpecialFolder)).FullName;
    public static string PersistantDataDirectory => Application.persistentDataPath;
    #endregion

    #region METHODS
    public static void WriteToBuffer(string value)
    {
        byte[] bValue = Encoding.GetBytes(value);
        _bufStream.Write(bValue, 0, bValue.Length);
    }
    public static void WriteLineToBuffer(string value)
    {
        byte[] bValue = Encoding.GetBytes(value);
        byte[] bNewline = Encoding.GetBytes(Newline);

        _bufStream.Write(bValue, 0, bValue.Length);
        _bufStream.Write(bNewline, 0, bNewline.Length);
    }
    public static void WriteTsvLineToBuffer(params string[] values)
    {
        foreach (string value in values)
        {
            WriteToBuffer(value);
            WriteToBuffer(Tab);
        }
    }
    public static void WriteTsvLineEndToBuffer(params string[] values)
    {
        foreach (string value in values)
        {
            WriteToBuffer(value);
            WriteToBuffer(Tab);
        }
        WriteToBuffer(Newline);
    }

    private static void WriteFile(string directory, string fileName, string fileExtension, FileMode fileMode, FileAccess fileAccess)
    {
        _bufStream.Flush();
        string filePath = string.Format("{0}{1}{2}.{3}", directory, DirectorySeperator, fileName, fileExtension);

        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        using (FileStream file = new FileStream(filePath, fileMode, fileAccess))
        {
            _memStream.Seek(0, SeekOrigin.Begin);
            _memStream.WriteTo(file);

            if (ConsolePrintPathOnSave)
                Debug.LogFormat("File created at {0}", file.Name);
        }

        if (ClearStreamAfterSave)
            ClearStream();
    }
    public static void ClearStream()
    {
        _bufStream.Dispose();
        _memStream.Dispose();
        _memStream = new MemoryStream();
        _bufStream = new BufferedStream(_memStream);
    }
    public static void Save(string fileName, string extension)
    {
        WriteFile(SpecialDirectory, fileName, extension, FileMode, FileAccess);
    }
    public static void Save(string directory, string fileName, string extension)
    {
        WriteFile(directory, fileName, extension, FileMode, FileAccess);
    }
    public static void Save(string directory, string fileName, string extension, FileMode fileMode, FileAccess fileAccess)
    {
        WriteFile(directory, fileName, extension, fileMode, fileAccess);
    }
    #endregion
}