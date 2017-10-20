using UnityEngine;
using UnityEditor;
using System.Collections;
/*
[CustomEditor(typeof(SplineReader))]
public class Spline_Editor : Editor
{
    public bool displayNumber;

    public bool enable_SplineData;
    public bool enable_SplinePoint;
    public bool[] enable_SplineSegment = new bool[1+5];

    //DELETE
    bool foldout;

    public override void OnInspectorGUI()
    {
        displayNumber = displayNumber.Button("Display Enumeration");

        base.OnInspectorGUI();
        SplineReader editorTarget = target as SplineReader;

        #region Spline Data
        enable_SplineData = enable_SplineData.Button("SplineData");
        if (enable_SplineData)
        {
            for (int i = 0; i < editorTarget.splineData.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString("D3"));
                EditorGUILayout.IntField((int)editorTarget.splineData[i].TrackSplitType);
                EditorGUILayout.IntField((int)editorTarget.splineData[i].DataPoolOffset1);
                EditorGUILayout.IntField((int)editorTarget.splineData[i].DataPoolOffset2);
                EditorGUILayout.EndHorizontal();
            }
        }
        #endregion

        #region Spline Point
        enable_SplinePoint = enable_SplinePoint.Button("Spline Point");
        if (enable_SplinePoint)
        {
            for (int i = 0; i < editorTarget.splineData.Length; i++)
                GxData.SplinePointField(i.ToString("D3") + " Point", editorTarget.splineData[i].point, ref foldout);
        }
        #endregion


        GXGUI.SplineSegmentsField(editorTarget.uniqueSegments, enable_SplineSegment, displayNumber, true, null);
        


    }
}
*/