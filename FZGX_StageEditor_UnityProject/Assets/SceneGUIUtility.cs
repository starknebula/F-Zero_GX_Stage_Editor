using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SceneGUIToolbar
{
    public SceneGUIToolbar(Camera camera, Vector3 worldPosition, float toolbarWidth, params ToolbarContent[] contents)
    {
        this.camera = camera;
        this.worldPosition = worldPosition;
        this.toolbarWidth = toolbarWidth;

        this.tabs = contents;
    }
    private ToolbarContent[] tabs;

    private int tabIndex;
    private Camera camera;
    private Camera Camera
    {
        get
        {
            return //camera == null
                Camera.current;
                //: camera;
        }
    }
    private Vector3 worldPosition;
    private Vector2 ScreenPosition
    {
        get
        {
            Vector2 position = Camera.WorldToScreenPoint(worldPosition);
            position.y = Camera.pixelHeight - position.y;
            return position;
        }
    }
    private Rect ScreenRect
    {
        get
        {
            Vector2 toolbarSize = new Vector2(toolbarWidth, GUI.skin.font.lineHeight * 2f);
            return new Rect(ScreenPosition - (toolbarSize * 0.5f), toolbarSize);
        }
    }
    private float toolbarWidth;

    // DEBUG
    private int cellHeight = 20;


    public void Render()
    {
        //tabIndex = GUI.Toolbar(ScreenRect, tabIndex, tabs);
        GUI.skin = Resources.Load<GUISkin>("DebugGUI");

        if (tabs == null) return;

        if (Vector3.Distance(Camera.current.transform.position, worldPosition) > 150f) return;

        // Set up rect for tabs
        Rect toolbarRect = ScreenRect;
        float tabWidth = toolbarRect.width / tabs.Length;
        float tabHeight = GUI.skin.font.fontSize * 1.5f;

        //
        for (int i = 0; i < tabs.Length; i++)
        {
            Rect tabRect = new Rect(toolbarRect.x + tabWidth * i, toolbarRect.y, tabWidth, tabHeight);

            GUI.color = (tabIndex == i) ? Color.green : Color.white;
            if (GUI.Button(tabRect, tabs[i].name))
            {
                tabIndex = i;
            }
            GUI.color = Color.white;
        }

        //
        Rect ContentsRect = ScreenRect;
        // Height = font size * X + font size
        float height = GUI.skin.font.fontSize + GUI.skin.font.fontSize;
        float heightOffset = 0f;

        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabIndex == i)
                for (int j = 0; j < tabs[i].contents.Length; j++)
                {
                    Rect contPos = new Rect(ContentsRect.x, ContentsRect.y + tabHeight + heightOffset, ContentsRect.width, height);
                    GUI.Box(contPos, tabs[i].contents[j]);
                    heightOffset += height;
                }
        }
    }
}

[System.Serializable]
public struct ToolbarContent
{
    public ToolbarContent(string name, params string[] contents)
    {
        this.name = name;
        this.contents = contents;
    }

    public string name;
    public string[] contents;
}