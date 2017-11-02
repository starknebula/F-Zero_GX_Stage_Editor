// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrintLog : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < 10; i++)
            QuickTextWriter.WriteLineToBuffer("Hello, World!");
        QuickTextWriter.Save("test1", "txt");

        for (int i = 0; i < 10; i++)
            QuickTextWriter.WriteLineToBuffer("Hello, World!");
        QuickTextWriter.Save("test2", "txt");
    }
}