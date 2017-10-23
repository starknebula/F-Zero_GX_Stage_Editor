// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;
using GameCube.Games.FZeroGX;

[CreateAssetMenu(fileName = "TPL ImportExport", menuName = "FZGX IO/TPL ImportExport")]
public class TPL_IO : ImportExportObject
{
    [SerializeField]
    private FZeroGXStage stageIndex;
    [SerializeField]
    private TPL tpl;

    public override string filename
    {
        get
        {
            return string.Format("{1}/st{0},lz", ((int)stageIndex).ToString("D2"), resourcePath);
        }
    }

    public override void Import()
    {
        tpl = new TPL(GetStreamFromFile(filename));
        Export();
    }
    public override void Export()
    {
        if (tpl != null)
        {
            Texture2D tex;
            for (int i = 0; i < tpl.NumDescriptors; i++)
                tpl.ReadTexture(GetStreamFromFile(filename), i, out tex, (i).ToString("X"));
        }
    }
}