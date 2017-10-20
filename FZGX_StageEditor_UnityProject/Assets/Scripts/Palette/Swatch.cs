using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swatch
{
    public Swatch(float minSta, float maxSat)
    {

    }

    protected AnimationCurve
        R = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f/6f, 0f), new Keyframe(5f/6f, 1f) }),
        G,
        B;

    protected virtual float offsetR { get { return 0f / 3f; } }
    protected virtual float offsetG { get { return 2f / 3f; } }
    protected virtual float offsetB { get { return 1f / 3f; } }
    protected virtual float alpha { get { return 1f; } }

    public const int paletteDepth = 24;
    protected virtual float minSaturation { get { return 0f; } }
    protected virtual float maxSaturation { get { return 1f; } }

    public virtual Color Red            { get { return GetColorByIndex(0); } }
    public virtual Color RedOrange      { get { return GetColorByIndex(1); } }
    public virtual Color Orange         { get { return GetColorByIndex(2); } }
    public virtual Color YellowOrange   { get { return GetColorByIndex(3); } }
    public virtual Color Yellow         { get { return GetColorByIndex(4); } }
    public virtual Color YellowLime     { get { return GetColorByIndex(5); } }
    public virtual Color Lime           { get { return GetColorByIndex(6); } }
    public virtual Color LimeGreen      { get { return GetColorByIndex(7); } }
    public virtual Color Green          { get { return GetColorByIndex(8); } }
    public virtual Color GreenTeal      { get { return GetColorByIndex(9); } }
    public virtual Color Teal           { get { return GetColorByIndex(10); } }
    public virtual Color CyanTeal       { get { return GetColorByIndex(11); } }
    public virtual Color Cyan           { get { return GetColorByIndex(12); } }
    public virtual Color CyanCobalt     { get { return GetColorByIndex(13); } }
    public virtual Color Cobalt         { get { return GetColorByIndex(14); } }
    public virtual Color CobaltBlue     { get { return GetColorByIndex(15); } }
    public virtual Color Blue           { get { return GetColorByIndex(16); } }
    public virtual Color BlueViolet     { get { return GetColorByIndex(17); } }
    public virtual Color Violet         { get { return GetColorByIndex(18); } }
    public virtual Color MagentaViolet  { get { return GetColorByIndex(19); } }
    public virtual Color Magenta        { get { return GetColorByIndex(20); } }
    public virtual Color MagentaRose    { get { return GetColorByIndex(21); } }
    public virtual Color Rose           { get { return GetColorByIndex(22); } }
    public virtual Color RoseRed        { get { return GetColorByIndex(23); } }

    /// <summary>
    /// Returns the color <paramref name="index"/> from the palette's depth range.
    /// </summary>
    /// <param name="index">The color to select from the palette's depth range.</param>
    public virtual Color GetColorByIndex(int index)
    {
        // Returns color INDEX/depth using this color's params
        return Palette.CreateColor(offsetR, offsetG, offsetB, alpha, index, paletteDepth, minSaturation, maxSaturation);
    }
    /// <summary>
    /// Returns a color from the palette type.
    /// </summary>
    /// <example>
    /// 2 / 24 = orange.
    /// 3 / 24 = yellow orange.
    /// 4 / 24 = yellow.
    /// </example>
    /// <param name="index">The colour to select.</param>
    /// <param name="paletteDepth">The range to select from.</param>
    public virtual Color CreateColor(int index, int paletteDepth)
    {
        return Palette.CreateColor(offsetR, offsetG, offsetB, alpha, index, paletteDepth, minSaturation, maxSaturation);
    }
    // Semantics. This is the same as CreateColor, but makes more sense when thought of in a loop.
    /// <summary>
    /// Returns a color from the palette type.
    /// </summary>
    /// <example>
    /// 2 / 24 = orange.
    /// 3 / 24 = yellow orange.
    /// 4 / 24 = yellow.
    /// </example>
    /// <param name="index">The color to select.</param>
    /// <param name="paletteDepth">The range to select from.</param>
    public virtual Color ColorWheel(int index, int paletteDepth)
    {
        return Palette.CreateColor(offsetR, offsetG, offsetB, alpha, index, paletteDepth, minSaturation, maxSaturation);
    }
}
