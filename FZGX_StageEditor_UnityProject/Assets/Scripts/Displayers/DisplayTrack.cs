// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using FzgxData;
using FzgxData.COLI;
using FzgxData.COLI.TrackData;

using UnityEditor;

public class DisplayTrack : MonoBehaviour, IFZGXEditorStageEventReceiver
{
    public bool viewAllNodes;
    [Range(0, 9)]
    public int _depth;

    public void StageUnloaded(BinaryReader reader)
    {

    }
    public void StageLoaded(BinaryReader reader)
    {
        spline = new Spline(reader);

        //if (print)
        //{
        //    List<uint> addresses = new List<uint>();

        //    foreach (SplinePoint point in spline.Points)
        //    {
        //        if (!addresses.Contains(point.TrackSegmentAddress))
        //        {
        //            // Vet addresses
        //            addresses.Add(point.TrackSegmentAddress);

        //            PrintLog.WriteTsvLineEndToBuffer(
        //                //point.Segment.LocalRotation
        //                );


        //        }
        //    }
        //}
    }

    private static Spline spline;

    private void OnDrawGizmos()
    {
        if (spline == null) return;

        List<uint> addresses = new List<uint>();

        int i = 0;
        foreach (SplinePoint point in spline.Points)
        {
            if (!addresses.Contains(point.TrackSegmentAddress))
            {
                // Vet addresses
                addresses.Add(point.TrackSegmentAddress);
                RecursiveDrawSegment(point.Segment);
                ++i;
            }

            Color c = Palette.ColorWheel(i + (i % 2 == 0 ? 0 : 6), 12);
            Handles.color = c;
            Handles.DrawLine(point.Node.PositionStart, point.Node.PositionEnd);

            Temp(c, point.Node, point.Segment);
        }
    }

    private void Temp(Color c, SplineNode node, SplineSegment segment)
    {
        int depth = segment.Depth();
        if (depth == _depth | viewAllNodes)
            if (segment.ChildCount <= 0)
            {
                Handles.color = c.Whitten(.3f);
                Handles.DrawDottedLine(node.PositionStart, segment.GlobalPosition(), 2f);
                Handles.color = c.Blacken(.3f);
                Handles.DrawLine(node.PositionEnd, segment.GlobalPosition());
            }

        foreach (SplineSegment child in segment.Children)
            Temp(c, node, child);
    }

    private void RecursiveDrawSegment(SplineSegment segment)
    {
        foreach (SplineSegment child in segment.Children)
            RecursiveDrawSegment(child);

        int depth = segment.Depth();
        Vector3 gPosition = segment.GlobalPosition();
        Vector3 gScale = segment.GlobalPosition();
        Quaternion gRotation = segment.GlobalRotation();


        if (depth == _depth | viewAllNodes)
        {
            Gizmos.color = Palette.ColorWheel(segment.Depth(), 6);
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawWireDisc(gPosition, gRotation * Vector3.forward, 7f - depth);
            UnityEditor.Handles.DrawLine(gPosition, gPosition + segment.GlobalRotation() * (Vector3.forward * (depth + 1) * 5f));

            //UnityEditor.Handles.Label(compareLocalGlobal ? segment.LocalPosition : segment.GlobalPosition(), segment.Depth().ToString());
            UnityEditor.Handles.Label(segment.GlobalPosition() + Vector3.down * segment.Depth() * 10, "L:" + segment.LocalScale.ToString() + " G:" + segment.GlobalScale());
        }
    }
}