// Created by Raphael "Stark" Tetreault 15/02/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 12/09/2016

using UnityEngine;

/// <summary>
/// Base class to be overridden in order to create new palettes for the Palette class.
/// </summary>
public class BasePalette
{
    public virtual float offsetR { get { return 0f / 3f; } }
    public virtual float offsetG { get { return 2f / 3f; } }
    public virtual float offsetB { get { return 1f / 3f; } }
    public virtual float alpha { get { return 1f; } }

    public const int paletteDepth = 24;
    public virtual float minSaturation { get { return 0f; } }
    public virtual float maxSaturation { get { return 1f; } }

    public virtual Color red            { get { return GetColorByIndex(0); } }
    public virtual Color red_orange     { get { return GetColorByIndex(1); } }
    public virtual Color orange         { get { return GetColorByIndex(2); } }
    public virtual Color yellow_orange  { get { return GetColorByIndex(3); } }
    public virtual Color yellow         { get { return GetColorByIndex(4); } }
    public virtual Color yellow_lime    { get { return GetColorByIndex(5); } }
    public virtual Color lime           { get { return GetColorByIndex(6); } }
    public virtual Color lime_green     { get { return GetColorByIndex(7); } }
    public virtual Color green          { get { return GetColorByIndex(8); } }
    public virtual Color green_teal     { get { return GetColorByIndex(9); } }
    public virtual Color teal           { get { return GetColorByIndex(10); } }
    public virtual Color cyan_teal      { get { return GetColorByIndex(11); } }
    public virtual Color cyan           { get { return GetColorByIndex(12); } }
    public virtual Color cyan_cobalt    { get { return GetColorByIndex(13); } }
    public virtual Color cobalt         { get { return GetColorByIndex(14); } }
    public virtual Color cobalt_blue    { get { return GetColorByIndex(15); } }
    public virtual Color blue           { get { return GetColorByIndex(16); } }
    public virtual Color blue_violet    { get { return GetColorByIndex(17); } }
    public virtual Color violet         { get { return GetColorByIndex(18); } }
    public virtual Color magenta_violet { get { return GetColorByIndex(19); } }
    public virtual Color magenta        { get { return GetColorByIndex(20); } }
    public virtual Color magenta_rose   { get { return GetColorByIndex(21); } }
    public virtual Color rose           { get { return GetColorByIndex(22); } }
    public virtual Color rose_red       { get { return GetColorByIndex(23); } }

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