// Created by Raphael "Stark" Tetreault 13/07/2017
// Copyright © 2017 Raphael Tetreault
// Last updated 17/07/2017

using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// Modifies the GUI.Color for this field
    /// </summary>
    public class GUIColor : PropertyAttribute
    {
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
        }

        public GUIColor(float r, float g, float b)
        {
            color = new Color(r, g, b, 1f);
        }
        public GUIColor(float r, float g, float b, float a)
        {
            color = new Color(r, g, b, a);
        }
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GUIColor))]
    public class GUIBackgroundColorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            Color previousColor = GUI.color;
            GUI.color = (attribute as GUIColor).Color;
            base.OnGUI(position);
            GUI.color = previousColor;
        }
    }
}
#endif