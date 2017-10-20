using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ColorUtility
{
    public static Color NormalToColor(Vector3 vector3)
    {
        return (new Color(vector3.x, vector3.y, vector3.z, 1f) * .5f) + (Color.white * .5f);
    }
    public static Color Vector3ToColor(Vector3 vector3)
    {
        return new Color(vector3.x, vector3.y, vector3.z, 1f);
    }
}