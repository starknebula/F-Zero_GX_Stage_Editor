// Created by Raphael "Stark" Tetreault 15/09/2015
// Copyright © 2016 Raphael Tetreault
// Last updated 12/09/2016

using UnityEngine;

/// <summary>
/// A collection of colors and UnityEngine.Color structure utilities.
/// </summary>
public static class Palette
{
    #region Color Modules
    // SHADE STRUCTS
    public static MonochromePalette Monochrome = new MonochromePalette();

    // COLOR STRUCTS
    // Same colors as this class, but used if you want to pass base type.
    public static BasePalette Generic = new BasePalette();
    public static NeonPalette Neon = new NeonPalette();
	public static DarkPalette Dark = new DarkPalette();
	public static PastelPalette Pastel = new PastelPalette();
    #endregion

    #region Generic Colors & Parameters
    // Full saturation.
    private const float offsetR = 0f / 3f;
    private const float offsetG = 2f / 3f;
    private const float offsetB = 1f / 3f;
    private const float alpha = 1f;

    public const int paletteDepth = 24;
    public const float minSaturation = 0.0f;
    public const float maxSaturation = 1.0f;
    //
    public static Color red            { get { return GetColorByIndex(0); } }
    public static Color red_orange     { get { return GetColorByIndex(1); } }
    public static Color orange         { get { return GetColorByIndex(2); } }
    public static Color yellow_orange  { get { return GetColorByIndex(3); } }
    public static Color yellow         { get { return GetColorByIndex(4); } }
    public static Color yellow_lime    { get { return GetColorByIndex(5); } }
    public static Color lime           { get { return GetColorByIndex(6); } }
    public static Color lime_green     { get { return GetColorByIndex(7); } }
    public static Color green          { get { return GetColorByIndex(8); } }
    public static Color green_teal     { get { return GetColorByIndex(9); } }
    public static Color teal           { get { return GetColorByIndex(10); } }
    public static Color cyan_teal      { get { return GetColorByIndex(11); } }
    public static Color cyan           { get { return GetColorByIndex(12); } }
    public static Color cyan_cobalt    { get { return GetColorByIndex(13); } }
    public static Color cobalt         { get { return GetColorByIndex(14); } }
    public static Color cobalt_blue    { get { return GetColorByIndex(15); } }
    public static Color blue           { get { return GetColorByIndex(16); } }
    public static Color blue_violet    { get { return GetColorByIndex(17); } }
    public static Color violet         { get { return GetColorByIndex(18); } }
    public static Color magenta_violet { get { return GetColorByIndex(19); } }
    public static Color magenta        { get { return GetColorByIndex(20); } }
    public static Color magenta_rose   { get { return GetColorByIndex(21); } }
    public static Color rose           { get { return GetColorByIndex(22); } }
    public static Color rose_red       { get { return GetColorByIndex(23); } }
	//
    public static Color black { get { return CreateShade(0, 2, alpha, minSaturation, maxSaturation); } }
    public static Color grey  { get { return CreateShade(1, 2, alpha, minSaturation, maxSaturation); } }
    public static Color white { get { return CreateShade(2, 2, alpha, minSaturation, maxSaturation); } }
    //
    public static Color transparent {  get { return new Color(0f, 0f, 0f, 0f); } }
    #endregion

    #region Base Methods for this class
    /// <summary>
    /// Returns a value from 0f to 1f to be used as an R/G/B component for a color.
    /// </summary>
    /// <param name="startingPoint">
    /// The starting percentage of the color cycle. 0f is red, 0.33f if green, 0.66f is blue.
    /// </param>
    /// <param name="index">The color to select.</param>
    /// <param name="paletteDepth">The range to select from.</param>
    /// <param name="min">Min R/G/B value.</param>
    /// <param name="max">Max R/G/B value.</param>
    public static float ColorComponentLerp(float startingPoint, float index, float paletteDepth, float min, float max)
    {
        // Determine where this color is based in the cycle (0% to 100%).
        float cyclePercentage = index / paletteDepth; // 0% to 100%
        cyclePercentage += startingPoint; // Add any offset.
        cyclePercentage *= 6f; // Convert to 6 cycle - RYGCBM
        cyclePercentage %= 6f; // Wrap 6 cycle - RYGCBM

        // Determines where in the RGB cycle this color component is.
        int cycle = (int)(cyclePercentage);

        // Return color component value based on point in RGB cycle.
        switch (cycle)
        {
            // R component base cycle.
            // Offset is used to achieve G and B components.

            case 5: //wraps
            case 0:
                return max;
            case 1:
                return Mathf.Lerp(max, min, cyclePercentage % 1f);
            case 2:
            case 3:
                return min;
            case 4:
                return Mathf.Lerp(min, max, cyclePercentage % 1f);

            default:
                Debug.LogError("Palette.ColorComponentLerp failed! Value not between 0 and 5.");
                return 0f;
        }
    }
    /// <summary>
    /// Used to generate colors.
    /// </summary>
    /// <param name="baseR">The offset for R.</param>
    /// <param name="baseG">The offset for G.</param>
    /// <param name="baseB">The offset for B.</param>
    /// <param name="index">The color to select.</param>
    /// <param name="paletteDepth">The range to select from.</param>
    /// <param name="min">Min R/G/B value.</param>
    /// <param name="max">Max R/G/B value.</param>
    public static Color CreateColor(float baseR, float baseG, float baseB, float alpha, int index, int paletteDepth, float min, float max)
    {
        return new Color(
            ColorComponentLerp(baseR, index, paletteDepth, min, max),
            ColorComponentLerp(baseG, index, paletteDepth, min, max),
            ColorComponentLerp(baseB, index, paletteDepth, min, max),
            alpha);
    }
    /// <summary>
    /// Returns the color <paramref name="index"/> from the palette's depth range.
    /// </summary>
    /// <param name="index">The color to select from the palette's depth range.</param>
    private static Color GetColorByIndex(int index)
    {
        // Returns color index/paletteDepth using this color's params
        return CreateColor(offsetR, offsetG, offsetB, alpha, index, paletteDepth, minSaturation, maxSaturation);
    }

    /// <summary>
    /// Used to generate monochrome shades.
    /// </summary>
    /// <param name="index">The color to select.</param>
    /// <param name="paletteDepth">The range to select from.</param>
    /// <param name="min">Min RGB value.</param>
    /// <param name="max">Max RGB value.</param>
    public static Color CreateShade(float index, float paletteDepth, float alpha, float min, float max)
    {
        return new Color(
            Mathf.Lerp(min, max, index / paletteDepth),
            Mathf.Lerp(min, max, index / paletteDepth),
            Mathf.Lerp(min, max, index / paletteDepth),
            alpha);
    }
    #endregion

    #region Utility Methods
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
    public static Color ColorWheel(int index, int paletteDepth)
    {
        // These values come from the above set which describe Generic Colors
        return CreateColor(offsetR, offsetG, offsetB, alpha, index, paletteDepth, minSaturation, maxSaturation);
    }

    /// <summary>
    /// Returns the average of all colors.
    /// </summary>
    /// <param name="colors">The colors to mix.</param>
    public static Color Mix(params Color[] colors)
    {
        float count = colors.Length;
        Color newColor = new Color(0f, 0f, 0f, 0f);

        foreach (Color color in colors)
            newColor += color / count;
		
		return newColor;
	}
    /// <summary>
    /// Structure of a Color value paired with a weight. Used for mixing colors non-uniformly.
    /// </summary>
    public struct ColorWithWeight
    {
        public Color color;
        public float weight;

        public ColorWithWeight(Color color, float weight)
        {
            this.color = color;
            this.weight = weight;
        }
    }
    /// <summary>
    /// Returns the aggregate of all colors using their weights.
    /// </summary>
    /// <param name="colorsWithWeight">The colors with their associated weight.</param>
    public static Color Mix(params ColorWithWeight[] colorsWithWeight) {

        float count = colorsWithWeight.Length;
        Color newColor = new Color(0f, 0f, 0f, 0f);

        float totalWeight = 0f;
        foreach (ColorWithWeight cw in colorsWithWeight)
            totalWeight += cw.weight;

        foreach (ColorWithWeight cw in colorsWithWeight)
            newColor += (cw.color * cw.weight) / (count / totalWeight);

		return newColor;
	}

    #region Set Methods (Set Color Components)
    /// <summary>
    /// Set the red component of <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="red">The component value.</param>
    public static Color SetRed(Color color, float red)
    {
        color.r = red;
        return color;
    }
    /// <summary>
    /// Set the blue component of <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="red">The component value.</param>
    public static Color SetGreen(Color color, float green)
    {
        color.g = green;
        return color;
    }
    /// <summary>
    /// Set the green component of <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="red">The component value.</param>
    public static Color SetBlue(Color color, float blue)
    {
        color.b = blue;
        return color;
    }
    /// <summary>
    /// Set the alpha component of <paramref name="color"/>.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="red">The component value.</param>
    public static Color SetAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    /// <summary>
    /// Override a Color by passing in the desired values to change and using null to skip any value.
    /// </summary>
    /// <param name="color">The Color to override.</param>
    /// <param name="red">The red value to override. Pass null to skip overriding it.</param>
    /// <param name="green">The green value to override. Pass null to skip overriding it.</param>
    /// <param name="blue">The blue value to override. Pass null to skip overriding it.</param>
    /// <param name="alpha">The alpha value to override. Pass null to skip overriding it.</param>
    public static Color SetOverride(Color color, float? red, float? green, float? blue, float? alpha)
    {
        Color newColor = color;

        if (red != null)
            newColor.r = (float)red;

        if (green != null)
            newColor.g = (float)green;

        if (blue != null)
            newColor.b = (float)blue;

        if (alpha != null)
            newColor.a = (float)alpha;

        return newColor;
    }
    #endregion
    #endregion
}