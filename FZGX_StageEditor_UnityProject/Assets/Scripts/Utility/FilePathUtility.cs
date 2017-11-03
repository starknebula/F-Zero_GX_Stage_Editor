// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Text.RegularExpressions;
namespace System.IO
{
    public static class FilePathUtility
    {
        public static string PathToUnityPath(this string value)
        {
            return value.Replace("\\", "/");
        }
        /// <summary>
        /// Returns the path starting after Assets/
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string UnityAssetPath(string path)
        {
            return Regex.Match(path, "(?<=Assets/).*$").Value;
        }
        public static string AssetsPath => Application.dataPath;
        //public static string ProjectPath => Regex.Match(Application.dataPath, "(?=Assets/).*$").Value;
    }
}