using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneGUITest : MonoBehaviour
{
    public GUISkin skin;
}

[CustomEditor(typeof(SceneGUITest))]
public class SceneGUITest_Editor : Editor
{
    // Has to be selected
    private void OnSceneGUI()
    {
        GameObject[] gobjs = FindObjectsOfType<GameObject>();

        GUI.skin = (target as SceneGUITest).skin;
        foreach (GameObject gobj in gobjs)
        {
            Handles.BeginGUI();
            Vector2 pos = Camera.current.WorldToScreenPoint(gobj.transform.position);
            pos.y = -pos.y + Camera.current.pixelHeight;
            Rect r = new Rect(pos, Vector2.one * 100);
            GUI.Button(r, "Hello, World!");
            Handles.EndGUI();

            //Handles.Button(gobj.transform.position, gobj.transform.rotation, 1f, 1f, Handles.RectangleCap);
        }
    }
}