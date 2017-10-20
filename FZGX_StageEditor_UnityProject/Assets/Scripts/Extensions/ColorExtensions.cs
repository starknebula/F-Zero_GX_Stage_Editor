// Created by Raphael "Stark" Tetreault 14/12/2015
// Copyright © 2016 Raphael Tetreault
// Last updated --/--/----

namespace UnityEngine
{
    /// <summary>
    /// Extensions for Unity's Color structure.
    /// </summary>
    public static class ColorExtensions
    {
        // Extension methods
        /// <summary>
        /// Set the red component of this Color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="red"></param>
        /// <returns></returns>
        public static Color SetRed(this Color color, float red)
        {
            color.r = red;
            return color;
        }
        /// <summary>
        /// Set the green component of this Color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="green"></param>
        /// <returns></returns>
        public static Color SetGreen(this Color color, float green)
        {
            color.g = green;
            return color;
        }
        /// <summary>
        /// Set the blue component of this Color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static Color SetBlue(this Color color, float blue)
        {
            color.b = blue;
            return color;
        }
        /// <summary>
        /// Set the alpha component of this Color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        /// <summary>
        /// Override this Color by passing in the desired values to change and using null to skip any value.
        /// </summary>
        /// <param name="color">The Color to override</param>
        /// <param name="red">The red value to override. Pass null to skip overriding it.</param>
        /// <param name="green">The green value to override. Pass null to skip overriding it.</param>
        /// <param name="blue">The blue value to override. Pass null to skip overriding it.</param>
        /// <param name="alpha">The alpha value to override. Pass null to skip overriding it.</param>
        /// <returns></returns>
        public static Color SetOverrides(this Color color, float? red, float? green, float? blue, float? alpha)
        {
            Color newColor = color;

            if (red   != null)
                newColor.r = (float)red;

            if (green != null)
                newColor.g = (float)green;

            if (blue  != null)
                newColor.b = (float)blue;
     
            if (alpha != null)
                newColor.a = (float)alpha;

            return newColor;
        }

        public static Color Whitten(this Color color, float percentWhite)
        {
            return color * (1f - percentWhite) + Color.white * percentWhite;
        }
        public static Color Soften(this Color color, float percentGrey)
        {
            return color * (1f - percentGrey) + Color.gray * percentGrey;
        }
        public static Color Blacken(this Color color, float percentBlack)
        {
            return color * (1f - percentBlack) + Color.black * percentBlack;
        }

        public static Color GetOpposite(this Color color)
        {
            Color oppositeColor = color;
            oppositeColor.r = 1f - color.r;
            oppositeColor.g = 1f - color.g;
            oppositeColor.b = 1f - color.b;

            return oppositeColor;
        }
    }
}
