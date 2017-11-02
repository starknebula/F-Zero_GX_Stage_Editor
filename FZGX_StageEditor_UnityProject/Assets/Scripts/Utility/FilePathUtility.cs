// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.IO
{
    public static class FilePathUtility
    {
        public static string PathToSystemPath(this string value)
        {
//#if UNITY_EDITOR_WIN
//            return value.Replace("/", "\\");
//#elif UNITY_EDITOR_LINUX || UNITY_EDITOR_OSX
        return value.Replace("\\", "/");
//#endif
        }
    }
}