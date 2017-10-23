using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;

public static class GUIStyles
{

    public static Font font = Resources.Load(CombinePathParams("Assets", "Resources", "Fonts", "VeraMono")) as Font;
    public static Texture2D square = Resources.Load(CombinePathParams("Assets", "Resources", "Fonts", "Square")) as Texture2D;

    public static GUIStyle GX
    {
        get
        {
            GUIStyle style = new GUIStyle();

            style.name = "GX";

            //
            style.normal = style.onNormal = NewGUIStyleState(square, new Texture2D[] { square }, Palette.black); // Normal
            style.hover = style.onHover = NewGUIStyleState(square, new Texture2D[] { square }, Palette.green); // Mouse
            style.active = style.onActive = NewGUIStyleState(square, new Texture2D[] { square }, Palette.cobalt); // Button
            style.focused = style.onFocused = NewGUIStyleState(square, new Texture2D[] { square }, Palette.rose_red); // Keyboard

            // Image layout
            style.imagePosition = ImagePosition.ImageLeft;

            // Properties
            style = style.EditedFontProperties(font, 12, FontStyle.Normal);
            style = style.EditedTextProperties(TextAnchor.MiddleCenter, TextClipping.Clip, true, true);
            style = style.EditedDimensionProperties(0, 0, false, false);
            style = style.EditedRectOffsetProperties(
                new RectOffset(1, 1, 1, 1), // border
                new RectOffset(5, 5, 5, 5), // margin
                new RectOffset(2, 2, 2, 2), // padding
                new RectOffset(0, 0, 0, 0), // overflow
                new Vector2(0, 0)           // contentOffset
                );

            return style;
        }
    }


    public static string CombinePathParams(params string[] paths)
    {
        StringBuilder sb = new StringBuilder();

        if (paths.Length > 0)
            sb.Append(paths[0]);

        for (int i = 1; i < paths.Length; i++)
            sb.Append(Path.Combine(sb.ToString(), paths[i]));

        return sb.ToString();
    }

    public static GUIStyle EditedFontProperties(this GUIStyle style, Font font, int fontSize, FontStyle fontStyle)
    {
        // Font
        style.font = font;
        style.fontSize = fontSize;
        style.fontStyle = fontStyle;

        return style;
    }
    public static GUIStyle EditedTextProperties(this GUIStyle style, TextAnchor textAnchor, TextClipping textClipping, bool richText, bool wordWrap)
    {
        style.alignment = textAnchor;
        style.clipping = textClipping;
        style.richText = richText;
        style.wordWrap = wordWrap;

        return style;
    }
    public static GUIStyle EditedRectOffsetProperties(this GUIStyle style, RectOffset border, RectOffset margin, RectOffset padding, RectOffset overflow, Vector2 contentOffset)
    {
        style.border = border;
        style.margin = margin;
        style.padding = padding;
        style.overflow = overflow;
        style.contentOffset = contentOffset;

        return style;
    }
    public static GUIStyle EditedDimensionProperties(this GUIStyle style, float fixedWidth, float fixedHeight, bool stretchWidth, bool stretchHeight)
    {
        style.fixedWidth = fixedWidth;
        style.fixedHeight = fixedHeight;
        style.stretchWidth = stretchWidth;
        style.stretchHeight = stretchHeight;

        return style;
    }

    /// <summary>
    /// Returns a new GUIStyleState.
    /// </summary>
    /// <param name="background"></param>
    /// <param name="scaleBackgrounds"></param>
    /// <param name="textColor"></param>
    public static GUIStyleState NewGUIStyleState(Texture2D background, Texture2D[] scaleBackgrounds, Color textColor)
    {
        GUIStyleState styleState = new GUIStyleState();

        styleState.background = background;
        styleState.scaledBackgrounds = scaleBackgrounds;
        styleState.textColor = textColor;

        return styleState;
    }

}
