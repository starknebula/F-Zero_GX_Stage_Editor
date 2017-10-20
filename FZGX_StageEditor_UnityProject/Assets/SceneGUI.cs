//using UnityEditor;
//using UnityEngine;

//public class SceneGUI : EditorWindow
//{
//    [MenuItem("Window/Scene GUI/Enable")]
//    public static void Enable()
//    {
//        SceneView.onSceneGUIDelegate += OnScene;
//        SceneView.onSceneGUIDelegate.Invoke(SceneView.currentDrawingSceneView);
//        Debug.Log("Scene GUI : Enabled");
//    }

//    [MenuItem("Window/Scene GUI/Disable")]
//    public static void Disable()
//    {
//        SceneView.onSceneGUIDelegate -= OnScene;
//        Debug.Log("Scene GUI : Disabled");
//    }

//    [UnityEditor.Callbacks.DidReloadScripts]
//    public static void Reset()
//    {
//        Enable();
//    }

//    private static void OnScene(SceneView sceneview)
//    {
//        try
//        {
//            Handles.BeginGUI();

//            GUI.skin = Resources.Load("DebugGUI") as GUISkin;

//            /*/
//            GUI.skin.box = GUIStyles.GX;
//            GUI.skin.button = GUIStyles.GX;
//            GUI.skin.horizontalScrollbar = GUIStyles.GX;
//            GUI.skin.horizontalScrollbarLeftButton = GUIStyles.GX;
//            GUI.skin.horizontalScrollbarRightButton = GUIStyles.GX;
//            GUI.skin.horizontalScrollbarThumb = GUIStyles.GX;
//            GUI.skin.horizontalSlider = GUIStyles.GX;
//            GUI.skin.horizontalSliderThumb = GUIStyles.GX;
//            GUI.skin.label = GUIStyles.GX;
//            GUI.skin.scrollView = GUIStyles.GX;
//            GUI.skin.textArea = GUIStyles.GX;
//            GUI.skin.textField = GUIStyles.GX;
//            GUI.skin.toggle = GUIStyles.GX;
//            GUI.skin.verticalScrollbar = GUIStyles.GX;
//            GUI.skin.verticalScrollbarDownButton = GUIStyles.GX;
//            GUI.skin.verticalScrollbarThumb = GUIStyles.GX;
//            GUI.skin.verticalScrollbarUpButton = GUIStyles.GX;
//            GUI.skin.verticalSlider = GUIStyles.GX;
//            GUI.skin.verticalSliderThumb = GUIStyles.GX;
//            GUI.skin.window = GUIStyles.GX;
//            //*/

//            //GUI.skin.font = new Font();
//            //GUI.skin.settings

//            GUI.Box(ScreenRect(new Rect(0, 0, 100, 50), Vector3.zero, AlignX.Center, AlignY.Center), "TEST");
//            if (GUI.Button(ScreenRect(new Rect(0, 5, 80, 20), Vector3.zero, AlignX.Center, AlignY.Center), "Button"))
//                Debug.Log("HELLO WORLD");

//            Handles.EndGUI();
//        }
//        catch { }
//    }

//    public static Rect ScreenRect(Rect rect, Vector3 screenPosition, AlignX alignX, AlignY alignY)
//    {
//        // Transform Vector3 to screenspace for Scene View
//        screenPosition = Camera.current.WorldToScreenPoint(screenPosition);

//        rect.x += screenPosition.x;
//        rect.y += Camera.current.pixelHeight - screenPosition.y; // inverts y from top down to bottom up screen space

//        if (alignX == AlignX.Left)
//            rect.x -= rect.width;
//        if (alignX == AlignX.Center)
//            rect.x -= rect.width / 2;

//        if (alignY == AlignY.Top)
//            rect.y -= rect.height;
//        if (alignY == AlignY.Center)
//            rect.y -= rect.height / 2;

//        return rect;
//    }


//    public enum AlignX
//    {
//        Left,
//        Center,
//        Right,
//    }
//    public enum AlignY
//    {
//        Top,
//        Center,
//        Bottom,
//    }
//}