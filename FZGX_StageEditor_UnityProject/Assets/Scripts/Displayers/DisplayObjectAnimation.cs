using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using GX_Data;
using GX_Data.Object_0x48;
using GX_Data.SplineData;
using GameCube;


public class DisplayObjectAnimation : MonoBehaviour, IFZGXEditorStageEventReceiver
{
    [SerializeField]
    private Mesh mesh;

    static GX_Object_0x48 gxDataOther;

    public void StageUnloaded(BinaryReader reader)
    {
        //throw new NotImplementedException();
    }
    public void StageLoaded(BinaryReader reader)
    {
        gxDataOther = new GX_Object_0x48(reader);
        //PrintOutAnimationData();
        //PrintOutAnimationAddresses(); // debug incorrects
        //PrintOutAnimationInfo();
    }


    void OnDrawGizmos()
    {
        if (gxDataOther == null) return;

        int k = 0;
        foreach (GX_Data.Object_0x48.BaseObject obj in gxDataOther.obj)
        {
            SmartHandleLabel(new Vector3(0f, k * 2f, 0f), Vector3.zero, obj.ObjectName);

            for (int i = 0; i < obj.animation.animationEntry.Length; i++)
            {
                Gizmos.color = Palette.ColorWheel(i, 6).SetAlpha(.5f);

                int length = obj.animation.animationEntry[i].Length;
                for (int j = 0; j < length; j++)
                {
                    //if (j < length-1)
                    //Gizmos.DrawLine(obj.animation.animationEntry[i][j].vector, obj.animation.animationEntry[i][j+1].vector);

                    //if (i == 0)
                    if (j == 0)
                    {
                        SmartHandleLabel(new Vector3(i * 2f, k * 2f, j * 2f), Vector3.right / 2f, obj.animation.animationEntry[i][j].isLooping.ToString());

                        if (obj.animation.animationEntry[i][j].isLooping > 2)
                            Debug.LogError("FOUND! " + k + " " + obj.ObjectName);
                    }

                    Gizmos.DrawMesh(mesh, new Vector3(i*2f, k * 2f, j*2f), Quaternion.Euler(obj.animation.animationEntry[i][j].vector));
                    //Gizmos.DrawLine(new Vector3(i * 2f, 0, j * 2f), obj.animation.animationEntry[i][j].vector, 1f);
                }
            }
            k++;
        }
    }

    // COPY PASTED FROM DISPLAYOBJECTTRANSFORM
    public void SmartHandleLabel(Vector3 position, Vector3 offset, string label)
    {
        if (Vector3.Dot(Camera.current.transform.forward, position - Camera.current.transform.position) > 0)
            if (Vector3.Distance(Camera.current.transform.position, position) < 300f)
                Handles.Label(position + offset * HandleUtility.GetHandleSize(position + offset), label);
    }

    public static void PrintOutAnimationData()
    {
        // 6 = all addresses for animation
        for (int address = 0; address < 6; address++)
        {
            // Print out animations
            PrintLog.WriteTsvLineEndToBuffer(
                "Object ID",
                "Name",
                "ID",
                "Time",
                "x",
                "y",
                "z"
                );

            for (int i = 0; i < gxDataOther.obj.Length; i++)
            {
                for (int j = 0; j < gxDataOther.obj[i].animation.animationEntry.Length; j++)
                {
                    // SKIP NULL ENTRIES
                    //if (gxDataOther.obj[i].animation.animationEntry[j] == null)
                    //    return;

                    for (int k = 0; k < gxDataOther.obj[i].animation.animationEntry[j].Length; k++)
                    {
                        if (address == j)
                        {
                            if (gxDataOther.obj[i].animation.animationEntry[j].Length > 0)
                            {
                                PrintLog.WriteTsvLineEndToBuffer(
                                    i.ToString(),
                                    gxDataOther.obj[i].ObjectName,
                                    gxDataOther.obj[i].animation.animationEntry[j][k].isLooping.ToString(),
                                    gxDataOther.obj[i].animation.animationEntry[j][k].time.ToString(),
                                    gxDataOther.obj[i].animation.animationEntry[j][k].vector.x.ToString(),
                                    gxDataOther.obj[i].animation.animationEntry[j][k].vector.y.ToString(),
                                    gxDataOther.obj[i].animation.animationEntry[j][k].vector.z.ToString()
                                    );
                            }
                        }
                    }
                }
            }
            PrintLog.SaveStream(StageManager.currentStage.ToString() + (address + 1).ToString());
        }
    }
    public static void PrintOutAnimationAddresses()
    {
        #region header
        PrintLog.WriteTsvLineEndToBuffer(
            "Object ID",
            "Animation Address",

            "1 address",
            "1 count",

            "2 address",
            "2 count",

            "3 address",
            "3 count",

            "4 address",
            "4 count",

            "5 address",
            "5 count",

            "6 address",
            "6 count"
            );
        #endregion

        for (int i = 0; i < gxDataOther.obj.Length; i++)
        {
            PrintLog.WriteToBuffer(i.ToString() + "\t");
            PrintLog.WriteToBuffer(gxDataOther.obj[i].animationAddress.ToString() + "\t");

            for (int j = 0; j < gxDataOther.obj[i].animation.pointers.Length; j++)
            {
                PrintLog.WriteTsvLineToBuffer(
                    gxDataOther.obj[i].animation.pointers[j].address.ToString(),
                    gxDataOther.obj[i].animation.pointers[j].count.ToString()
                    );
            }
            PrintLog.WriteToBuffer("\n");
        }
        PrintLog.SaveStream(StageManager.currentStage.ToString());
    }
    public static void PrintOutAnimationInfo()
    {
        PrintLog.WriteTsvLineEndToBuffer(
            "Object #",
            "Name",
            "Dat 0x00",
            "Dat 0x04",
            "Layer",
            "1", "2", "3", "4", "5", "6"
            );

        for (int i = 0; i < gxDataOther.obj.Length; i++)
        {
            PrintLog.WriteTsvLineEndToBuffer(
                i.ToString(),
                gxDataOther.obj[i].ObjectName,
                gxDataOther.obj[i].animation.dat_0x00.ToString(),
                gxDataOther.obj[i].animation.dat_0x04.ToString(),
                BitToLayerUtility.GetLayer(gxDataOther.obj[i].animation.layer_16bit >> 16).ToString(),
                (gxDataOther.obj[i].animation.pointers[0].count > 0 ? "x" : ""),
                (gxDataOther.obj[i].animation.pointers[1].count > 0 ? "x" : ""),
                (gxDataOther.obj[i].animation.pointers[2].count > 0 ? "x" : ""),
                (gxDataOther.obj[i].animation.pointers[3].count > 0 ? "x" : ""),
                (gxDataOther.obj[i].animation.pointers[4].count > 0 ? "x" : ""),
                (gxDataOther.obj[i].animation.pointers[5].count > 0 ? "x" : "")
                );
        }
        PrintLog.SaveStream(StageManager.currentStage.ToString());
    }
}