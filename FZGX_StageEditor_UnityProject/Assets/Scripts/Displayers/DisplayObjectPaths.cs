// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using FzgxData;
using FzgxData.COLI;


public class DisplayObjectPaths : MonoBehaviour, IFZGXEditorStageEventReceiver
{
    public void StageUnloaded(BinaryReader reader)
    {

    }
    public void StageLoaded(BinaryReader reader)
    {
        ObjectPath = new ObjectPath(reader);
    }

    private static ObjectPath ObjectPath;
    [SerializeField]
    private Color color = Color.white;
    [SerializeField]
    private float gizmoSphereSize;

    private void OnDrawGizmos()
    {
        if (ObjectPath == null) return;

        Gizmos.color = color;
        foreach (ObjectPath.Path path in ObjectPath.Paths)
        {
            Gizmos.DrawWireSphere(path.PositionStart, 1f * gizmoSphereSize);
            Gizmos.DrawWireSphere(path.PositionEnd, 1.5f * gizmoSphereSize);
            Gizmos.DrawLine(path.PositionStart, path.PositionEnd);
        }
    }
}