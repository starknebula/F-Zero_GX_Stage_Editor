using UnityEngine;
using System.Collections;

namespace UnityEditor
{
    public static class CustomGUI
    {
        public static bool Button(string label, bool toggle)
        {
            GUI.color = (toggle) ? Palette.white : Palette.grey;
            if (GUILayout.Button(label))
                toggle = !toggle;
            GUI.color = Palette.white;

            return toggle;
        }
        public static void Button(ref bool toggle, string label)
        {
            GUI.color = (toggle) ? Palette.white : Palette.grey;
            if (GUILayout.Button(label))
                toggle = !toggle;
            GUI.color = Palette.white;
        }

        public static void SetGUIColor(bool boolean, Color colorTrue, Color colorFalse)
        {
            GUI.color = (boolean) ? colorTrue : colorFalse;
        }
        public static void SetGUIColor(int integer, params Color[] colors)
        {
            
            for (int i = 0; i < colors.Length; i++)
                if (i == integer)
                    GUI.color = colors[i];
        }
        public static void SetGUIColorWhitten(int integer, params Color[] colors)
        {

            for (int i = 0; i < colors.Length; i++)
                if (i == integer)
                    GUI.color = colors[i].Whitten(0.5f);
        }
        public static void ResetGUIColor()
        {
            GUI.color = Color.white;
        }

        public static GUILayoutOption WidthPixel(int pixels)
        {
            return GUILayout.Width(pixels);
        }

        public static GUILayoutOption Width(int percent)
        {
            return GUILayout.Width(Screen.width * (percent / 100f));
        }
        public static GUILayoutOption Width(int percent, int correction)
        {
            return GUILayout.Width(Screen.width * (percent / 100f) - correction);
        }
        public static GUILayoutOption WidthFoldout(int percent)
        {
            return GUILayout.Width(Screen.width * (percent / 100f) - 35 - 20);
        }

        public static GUILayoutOption Width(float percent)
        {
            return GUILayout.Width(Screen.width * percent);
        }
        public static GUILayoutOption Width(float percent, int correction)
        {
            return GUILayout.Width(Screen.width * percent - correction);
        }
        public static GUILayoutOption WidthFoldout(float percent)
        {
            return GUILayout.Width(Screen.width * percent);
        }


        public static void HexField(uint integer, int places)
        {
            EditorGUILayout.TextField(integer.ToString("X" + places.ToString()));
        }
        public static void HexField(string label, uint integer, int places)
        {
            EditorGUILayout.TextField(label, integer.ToString("X" + places.ToString()));
        }

        public static void IntField(string label, int integer)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);
            EditorGUILayout.IntField(integer);
            EditorGUILayout.EndHorizontal();
        }
        public static void UIntField(string label, uint integer)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);
            EditorGUILayout.IntField((int)integer);
            EditorGUILayout.EndHorizontal();
        }

        public static bool IsAnyTrue(bool[] bools)
        {
            for (int i = 0; i < bools.Length; i++)
                if (bools[i])
                    return true;
            // If none true, return false
            return false;
        }
    }
}