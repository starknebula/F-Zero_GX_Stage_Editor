// Created by Raphael "Stark" Tetreault 15/02/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 12/09/2016

using UnityEngine;

public class MonochromePalette
{
    // This class's contents were intentionally made virtual so that this
    // could become a base class for monochrome modules to inherit from.

    public virtual int paletteDepth { get { return 2; } }
    public virtual float min { get { return 0.0f; } }
    public virtual float max { get { return 1.0f; } }
    public virtual float alpha { get { return 1.0f; } }

    public Color black { get { return Palette.CreateShade(0, paletteDepth, alpha, min, max); } }
    public Color grey  { get { return Palette.CreateShade(1, paletteDepth, alpha, min, max); } }
    public Color white { get { return Palette.CreateShade(2, paletteDepth, alpha, min, max); } }

    public Color NewShade(int index, int paletteDepth)
    {
        return NewShade(index, paletteDepth, this.alpha, this.min, this.max);
    }
    public Color NewShade(int index, int paletteDepth, float min, float max)
    {
        return NewShade(index, paletteDepth, this.alpha, min, max);
    }
    public Color NewShade(int index, int paletteDepth, float alpha, float min, float max)
    {
        return Palette.CreateShade(index, paletteDepth, alpha, min, max);
    }
}
