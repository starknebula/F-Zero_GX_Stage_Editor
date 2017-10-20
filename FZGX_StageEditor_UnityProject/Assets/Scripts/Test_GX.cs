using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using GX_Data;
using GX_Data.SplineData;
using FzgxData;

public class Test_GX : MonoBehaviour
{
    static SplineDataClass SplineData;

    [UnityEditor.Callbacks.DidReloadScripts]
    static void Reset()
    {
        //Stage.StageChangedCallback += delegate { SplineDataAction(); };
    }
    static void SplineDataAction()
    {
        SplineData = new SplineDataClass(Stage.Reader);
        Debug.Log("GX_Test");

        Stage.Current.transform.ClearTransformImmediate();
        GizmosEditorData2(Stage.Current.transform);
    }

    /// <summary>
    /// Serializes data from LibraryEntries
    /// </summary>
    public static void CalcLibraryEntries()
    {
        PrintLog.WriteComment(DateTime.Now.ToString() + "\n");
        PrintLog.WriteComment("i:j:k\tid\tSplinePos");

        for (int val = 0; val < 9; val++)
        {
            for (int i = 0; i < SplineData.EditorData.Count; i++)
                for (int j = 0; j < 9; j++)
                    if (j == val)
                        for (int k = 0; k < SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j].Length; k++)
                            PrintLog.WriteToBuffer(string.Format("{0}.{1}.{2}\t{3}\n", i + 1, j + 1, k + 1, SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j][k].ToString()));
            PrintLog.SaveStream((val + 1) + "EditorData_LibraryEntry_");
        }
    }
    public static void GizmosDebugEditorData()
    {
        for (int i = 0; i < SplineData.EditorData.Count; i++)
        {
            // Draw 1st editor data
            Handles.color = Palette.ColorWheel(i, 12);
            Handles.ArrowCap(0, SplineData.EditorData[i].localPosition, SplineData.EditorData[i].localRotation, 100f);
            Handles.DrawWireDisc(SplineData.EditorData[i].localPosition, SplineData.EditorData[i].localRotation * Vector3.forward, 10f);
            HandleLabel(SplineData.EditorData[i].localPosition, i.ToString());
            Gizmos.color = Handles.color = Handles.color.SetAlpha(.5f);
            
            // Recursive editor data

            for (int j = 6; j < 9; j++)
            {
                for (int k = 0; k < SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j].Length; k++)
                {
                    Vector3 pos = new Vector3(-SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j][k].field_1, SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j][k].field_2, SplineData.EditorData[i].LibraryIndexTable.LibraryEntry[j][k].field_3);
                    pos = SplineData.EditorData[i].localRotation * pos + SplineData.EditorData[i].localPosition;
                    Gizmos.DrawSphere(pos, 10f);

                    Handles.DrawDottedLine(SplineData.EditorData[i].localPosition, pos, 2f);
                }
            }
        }
    }
    public static void GizmosEditorData()
    {
        for (int i = 0; i < SplineData.EditorData.Count; i++)
        {
            Gizmos.color = Handles.color = Palette.ColorWheel(i, 12);
            //Handles.color = Palette.white;
            DebugEditorData(SplineData.EditorData[i], (i+1).ToString(), (i+1));
        }
    }
    public static void DebugEditorData(EditorData data, string name, int iteration)
    {
        Handles.ArrowCap(0, data.localPosition, data.localRotation, 100f);
        Handles.DrawWireDisc(data.localPosition, data.localRotation * Vector3.forward, 10f);
        HandleLabel(data.localPosition, name);


        /*/
        for (int j = 6; j < 9; j++)
        {
            for (int k = 0; k < data.LibraryIndexTable.LibraryEntry[j].Length; k++)
            {
                Vector3 libPos = data.GlobalPosition + data.LibraryIndexTable.LibraryEntry[j][k].AsVector3(Stage.doInverseWindingPositionX);
                Gizmos.DrawSphere(libPos, 10f);
                if (data.parent != null)
                    Handles.DrawLine(data.parent.GlobalPosition, libPos);
            }
        }
        //*/

        for (int i = 0; i < data.childEditorDataCount; i++)
            DebugEditorData(data.childEditorData[i], iteration + "_" + (i + 1).ToString(), (i + 1));
    }

    #region Creates hierarchy
    static public void GizmosEditorData2(Transform transform)
    {
        for (int i = 0; i < SplineData.EditorData.Count; i++)
        {
            DebugEditorData2(SplineData.EditorData[i], transform, i);
        }
    }
    static public void DebugEditorData2(EditorData data, Transform _parent, int i)
    {
        GameObject go1 = new GameObject();
        go1.name = (i + 1).ToString();

        go1.transform.SetParent(_parent);
        go1.transform.localPosition = SplineData.EditorData[i].localPosition;
        go1.transform.localRotation = SplineData.EditorData[i].localRotation;
        go1.transform.localScale = SplineData.EditorData[i].localScale;

        for (int j = 6; j < 9; j++)
        {
            for (int k = 0; k < data.LibraryIndexTable.LibraryEntry[j].Length; k++)
            {
                GameObject go2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go2.name = go1.name + "_LibraryEntry_" + (j+1) + "_" + (k+1);
                go2.transform.SetParent(go1.transform);
                go2.transform.localPosition = data.LibraryIndexTable.LibraryEntry[j][k].AsVector3(Stage.doInverseWindingPositionX);
                //go.transform.localRotation = data.LibraryIndexTable.LibraryEntry[j];
                //go.transform.localScale = data.LibraryIndexTable.LibraryEntry[j];
            }
        }

        for (int index = 0; index < data.childEditorDataCount; index++)
        {
            DebugEditorData2(data.childEditorData[index], go1.transform, index);
        }
    }
    #endregion

    static void HandleLabel(Vector3 position, string label)
    {
        if (Vector3.Dot(Camera.current.transform.forward, position - Camera.current.transform.position) > 0)
            Handles.Label(position, label);
    }
    void OnDrawGizmos()
    {
        if (SplineData == null) return;
        ///////////////////////////////
        
        for (int i = 0; i < SplineData.Base.Length; i++)
        {
            Gizmos.color = Handles.color = Palette.ColorWheel(SplineData.Base[i].EditorDataID, SplineData.EditorData.Count);
            Gizmos.color = (SplineData.Base[i].EditorDataID % 2 == 0) ? Gizmos.color : Gizmos.color.Whitten(.75f);
            Gizmos.DrawLine(
                SplineData.Base[i].RuntimeData.startPosition,
                SplineData.Base[i].RuntimeData.endPosition
                );

            //Handles.DrawWireDisc(SplineData.Base[i].RuntimeData.startPosition, SplineData.Base[i].RuntimeData.startTangent, 5f);
        }

        GizmosEditorData();
    }
}