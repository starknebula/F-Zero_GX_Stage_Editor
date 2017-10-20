using UnityEngine;
using System;
using System.IO;
using System.Text;

public static class PrintLog
{
    private static MemoryStream fileStream = new MemoryStream();
    private static BufferedStream buffer = new BufferedStream(fileStream);
    private static StringBuilder stringBuilder = new StringBuilder();

    private static string PersonalDirectory
    {
        get
        {
            return Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FullName + Path.DirectorySeparatorChar;
        }
    }


    public static void WriteToBuffer(string str)
    {
        buffer.Write(Encoding.UTF8.GetBytes(str), 0, str.Length);
    }
    public static void WriteTsvLineToBuffer(params string[] strings)
    {
        WriteToBuffer(WriteTsvLine(strings));
    }
    public static void WriteTsvLineEndToBuffer(params string[] strings)
    {
        WriteToBuffer(WriteTsvLineEnd(strings));
    }

    public static void SaveAndClearStream(string name, FileMode fileMode)
    {
        buffer.Flush();

        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        using (FileStream file = new FileStream(PersonalDirectory + name + ".tsv", FileMode.Create, FileAccess.Write))
        {
            byte[] byteStream = fileStream.ToArray();
            file.Write(byteStream, 0, byteStream.Length);
            Debug.Log("File created at " + file.Name);
        }

        fileStream = new MemoryStream(); // Clear stream
        buffer = new BufferedStream(fileStream); // Reset buffer
    }
    public static void SaveStream(string name)
    {
        //SaveAndClearStream(name, FileMode.Create);
    }

    public static void WriteComment(string comment)
    {
        WriteToBuffer(comment + "\n");
    }
    public static string WriteTsvLineEnd(params string[] strings)
    {
        stringBuilder = new StringBuilder();

        for (int i = 0; i < strings.Length; i++)
        {
            stringBuilder.Append(strings[i]);

            if (i == strings.LastIndex())
                stringBuilder.Append("\n");
            else
                stringBuilder.Append("\t");
        }

        return stringBuilder.ToString();
    }
    public static string WriteTsvLine(params string[] strings)
    {
        stringBuilder = new StringBuilder();

        for (int i = 0; i < strings.Length; i++)
        {
            stringBuilder.Append(strings[i]);
            stringBuilder.Append("\t");
        }

        return stringBuilder.ToString();
    }
}