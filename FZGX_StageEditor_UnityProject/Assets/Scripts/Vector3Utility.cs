using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Vector3Utility
{
    public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(vector.x, min.x, max.x),
            Mathf.Clamp(vector.y, min.y, max.y),
            Mathf.Clamp(vector.z, min.z, max.z)
            );
    }
    public static Vector3 Clamp(Vector3 vector, float min, Vector3 max)
    {
        return Clamp(vector, Vector3.one * min, max);
    }
    public static Vector3 Clamp(Vector3 vector, Vector3 min, float max)
    {
        return Clamp(vector, min, Vector3.one * max);
    }
    public static Vector3 Clamp(Vector3 vector, float min, float max)
    {
        return Clamp(vector, Vector3.one * min, Vector3.one * max);
    }
}