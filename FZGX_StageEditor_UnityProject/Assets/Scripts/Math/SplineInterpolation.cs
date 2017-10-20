// Created by Raphael "Stark" Tetreault 01/05/2015
// Copyright © 2016 Raphael Tetreault
// Last updated 18/09/2016

// Resources:
// http://cubic.org/docs/hermite.htm
// https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline
// https://en.wikipedia.org/wiki/File:Cubic_Catmull-Rom_formulation.png

using UnityEngine;

/// <summary>
/// A suite of method for generating splines.
/// </summary>
public static class SplineInterpolation
{
    /// <summary>
    /// Hermite spline basis function.
    /// http://cubic.org/docs/hermite.htm
    /// </summary>
    /// <param name="time">Value range from 0f to 1f.</param>
    private static float[] HermiteBasisFunctions(float time)
    {
        return new float[]
        {
             2 * Mathf.Pow(time, 3) - 3 * Mathf.Pow(time, 2) + 1,
            -2 * Mathf.Pow(time, 3) + 3 * Mathf.Pow(time, 2),
                 Mathf.Pow(time, 3) - 2 * Mathf.Pow(time, 2) + time,
                 Mathf.Pow(time, 3) -     Mathf.Pow(time, 2)
        };
    }
    /// <summary>
    /// Performs either MathX.Wrap or Mathf.Clamp on the passed params.
    /// </summary>
    /// <param name="doWrap">True wraps value, false clamps it.</param>
    /// <param name="value">Value to wrap or clamp.</param>
    /// <param name="length">Length to wrap or clamp at.</param>
    private static int WrapOrClamp(bool doWrap, int value, int length)
    {
        return (doWrap)
            ? MathX.Wrap(value, length)
            : Mathf.Clamp(value, 0, length - 1);
    }
    /// <summary>
    /// Returns either Array.RangeWrap or Array.Range on the passed Vector3s.
    /// </summary>
    /// <param name="doWrap">True wraps value, false clamps it.</param>
    /// <param name="index">The starting index for RangeWrap or Range.</param>
    /// <param name="length">Length to wrap or clamp at.</param>
    /// <param name="vectors">The Vector3s to use.</param>
    private static Vector3[] GetVector3Range(bool doWrap, int index, int length, params Vector3[] vectors)
    {
        return (doWrap)
            ? vectors.RangeWrap<Vector3>(index, length)
            : vectors.Range<Vector3>(index, length);
    }

    // HERMITE
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="tangent">The tangents to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="interpolations">The number of interpolations to perform.</param>
    public static Vector3[] HermiteSpline(Vector3[] position, Vector3[] tangent, bool doWrap, int index, int interpolations)
    {
        // Scale used normals to length of distance between points so they behave properly.
        for (int i = index; i <= index + 1; i++)
            tangent[i] *= Mathf.Abs(Vector3.Distance(position[i], position[WrapOrClamp(doWrap, i + 1, position.Length)]));

        // All interpolations using Hermite method
        Vector3[] hermitePoints = new Vector3[interpolations];
        float[] h;
        float time;
        Vector3 p0, p1, t0, t1;

        for (int i = 0; i < interpolations; i++)
        {
            // Calculate Hermite Basis for given time
            time = i / interpolations;
            h = HermiteBasisFunctions(time);

            // Calculate points and tangents
            p0 = h[0] * position[i];
            p1 = h[1] * position[WrapOrClamp(doWrap, i + 1, position.Length)];
            t0 = h[2] * tangent[i];
            t1 = h[3] * tangent[WrapOrClamp(doWrap, i + 1, tangent.Length)];

            hermitePoints[i] = (p0 + p1 + t0 + t1);
        }

        return hermitePoints;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startPosition">The start position to use.</param>
    /// <param name="endPosition">The end position to use.</param>
    /// <param name="startTangent">The start tangent to use.</param>
    /// <param name="nextTangent">The end tangent to use.</param>
    /// <param name="time">The interpolations point to perform, 0f to 1f.</param>
    public static Vector3 HermiteSplineSingle(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 nextTangent, float time)
    {
        // Scale normals to length of distance between points so they behave properly
        float distance = Mathf.Abs(Vector3.Distance(startPosition, endPosition));
        startTangent *= distance;
        nextTangent *= distance;

        // Calculate Hermite Basis for given time
        float[] h = HermiteBasisFunctions(time);

        // Calculate points and tangents
        Vector3 p0 = h[0] * startPosition;
        Vector3 p1 = h[1] * endPosition;
        Vector3 t0 = h[2] * startTangent;
        Vector3 t1 = h[3] * nextTangent;

        return (p0 + p1 + t0 + t1);
    }

    // CATMULL-ROM
    /// <summary>
    /// Enumerates types of Catmull-Rom splines.
    /// </summary>
    public enum CatmullRomAlpha
    {
        Uniform,
        Chordal,
        Centripetal,
    }
    /// <summary>
    /// Returns the float value of the passed <paramref name="alpha"/>.
    /// </summary>
    /// <param name="alpha">The CatmullRomAlpha to interpret.</param>
    public static float CatmullRomAlphaToFloat(CatmullRomAlpha alpha)
    {
        switch (alpha)
        {
            default:
            case CatmullRomAlpha.Centripetal:
                return 0.5f;
            case CatmullRomAlpha.Chordal:
                return 1f;
            case CatmullRomAlpha.Uniform:
                return 0f;
        }
    }

    /// <summary>
    /// Calculates the Catmull-Rom time between <paramref name="pointA"/> and <paramref name="pointB"/>
    /// using the provided <paramref name="alpha"/> value.
    /// </summary>
    /// <param name="pointA">First position.</param>
    /// <param name="pointB">Second position..</param>
    /// <param name="alpha">The Catmull-Rom alpha value.</param>
    private static float CatmullRomTime(Vector3 pointA, Vector3 pointB, float alpha)
    {
        float x = (pointB.x - pointA.x);
        float y = (pointB.y - pointA.y);
        float z = (pointB.z - pointA.z);

        return Mathf.Pow(Mathf.Pow(x * x + y * y + z * z, 0.5f), alpha);
    }
    /// <summary>
    /// Performs one segment of Barry and Goldman's pyramidal formulation.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline"/>
    /// <param name="t">Time (interval point) being calculated</param>
    /// <param name="Ta">Lower time.</param>
    /// <param name="Tb">Higher time.</param>
    /// <param name="Pa">First point.</param>
    /// <param name="Pb">Second point.</param>
    private static Vector3 CatmullRomFormulation(float t, float Ta, float Tb, Vector3 Pa, Vector3 Pb)
    {
        return (Tb - t) / (Tb - Ta) * Pa + (t - Ta) / (Tb - Ta) * Pb;
    }
    /// <summary>
    /// Performs the entire cubic pyramidal formulation.
    /// </summary>
    /// <param name="P">The 4 positions.</param>
    /// <param name="T">The 4 times.</param>
    /// <param name="t">The position to probe along the spline, 0f to 1f.</param>
    private static Vector3 CubicCatmullRomFormulation(Vector3[] P, float[] T, float t)
    {
        // Barry and Goldman's pyramidal formulation
        // https://en.wikipedia.org/wiki/File:Cubic_Catmull-Rom_formulation.png

        Vector3 A1 = CatmullRomFormulation(t, T[0], T[1], P[0], P[1]);
        Vector3 A2 = CatmullRomFormulation(t, T[1], T[2], P[1], P[2]);
        Vector3 A3 = CatmullRomFormulation(t, T[2], T[3], P[2], P[3]);

        Vector3 B1 = CatmullRomFormulation(t, T[0], T[2], A1, A2);
        Vector3 B2 = CatmullRomFormulation(t, T[1], T[3], A2, A3);

        Vector3 C = CatmullRomFormulation(t, T[1], T[2], B1, B2);

        return C;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="alpha">The Catmull-Rom alpha value.</param>
    /// <param name="interpolations">The number of interpolations to perform.</param>
    public static Vector3[] CatmullRom(Vector3[] position, bool doWrap, int index, float alpha, int interpolations)
    {
        // The 4 points P and 4 times T
        // Get index-1: the point before the point we interpolate
        Vector3[] P = GetVector3Range(doWrap, index - 1, 4);

        // "Times", the weighting of each point in non-linear fashion.
        float[] T = new float[4];
        T[0] = 0;
        T[1] = CatmullRomTime(P[0], P[1], alpha);
        T[2] = CatmullRomTime(P[1], P[2], alpha) + T[1];
        T[3] = CatmullRomTime(P[2], P[3], alpha) + T[2];

        //
        Vector3[] catmullRomPoints = new Vector3[interpolations];
        float time;

        for (int i = 0; i < interpolations; i++)
        {
            // Find time for this iteration
            time = Mathf.Lerp(T[1], T[2], (float)i / interpolations);
            // Interpolate a point between T1 and T2 using time
            catmullRomPoints[i] = CubicCatmullRomFormulation(P, T, time);
        }

        return catmullRomPoints;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="alpha">The Catmull-Rom alpha value.</param>
    /// <param name="interpolations">The number of interpolations to perform.</param>
    public static Vector3[] CatmullRom(Vector3[] position, bool doWrap, int index, CatmullRomAlpha alpha, int interpolations)
    {
        return CatmullRom(position, doWrap, index, CatmullRomAlphaToFloat(alpha), interpolations);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="alpha">The Catmull-Rom alpha value.</param>
    /// <param name="time">The interpolations point to perform, 0f to 1f.</param>
    public static Vector3 CatmullRomSingle(Vector3[] position, bool doWrap, int index, float alpha, float time)
    {
        // The 4 points P and 4 times T
        // Get index-1: the point before the point we interpolate
        Vector3[] P = GetVector3Range(doWrap, index - 1, 4);

        // "Times", the weighting of each point in non-linear fashion.
        float[] T = new float[4];
        T[0] = 0;
        T[1] = CatmullRomTime(P[0], P[1], alpha);
        T[2] = CatmullRomTime(P[1], P[2], alpha) + T[1];
        T[3] = CatmullRomTime(P[2], P[3], alpha) + T[2];


        // Interpolate a point between T1 and T2 using time
        time = Mathf.Lerp(T[1], T[2], time);
        return CubicCatmullRomFormulation(P, T, time);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="alpha">The Catmull-Rom alpha value.</param>
    /// <param name="time">The interpolations point to perform, 0f to 1f.</param>
    public static Vector3 CatmullRomSingle(Vector3[] position, bool doWrap, int index, CatmullRomAlpha alpha, float time)
    {
        return CatmullRomSingle(position, doWrap, index, CatmullRomAlphaToFloat(alpha), time);
    }


    // KOCHANEK-BARTELS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="interpolations">The number of interpolations to perform.</param>
    /// <param name="t">Tension.</param>
    /// <param name="b">Bias.</param>
    /// <param name="c">Continuity.</param>
    /// <param name="tangents">Outs the tangents of position[index] and position[index+1].</param>
    public static Vector3[] KochanekBartelsSpline(Vector3[] position, bool doWrap, int index, int interpolations, float t, float b, float c, out Vector3[] tangents)
    {
        // Crunch points and tangents
        Vector3[] P = GetVector3Range(doWrap, index - 1, 4, position);
        Vector3 T1 = ((1 - t) * (1 + b) * (1 + c)) / 2f * (P[1] - P[0]) + ((1 - t) * (1 - b) * (1 - c)) / 2f * (P[2] - P[1]);
        Vector3 T2 = ((1 - t) * (1 + b) * (1 - c)) / 2f * (P[2] - P[1]) + ((1 - t) * (1 - b) * (1 + c)) / 2f * (P[3] - P[2]);

        //
        Vector3[] tbcPosition = new Vector3[interpolations];
        float[] h;
        float time;

        for (int i = 0; i < interpolations; i++)
        {
            time = (float)i / interpolations;
            h = HermiteBasisFunctions(time);
            tbcPosition[i] = (P[1] * h[0]) + (P[2] * h[1]) + (T1 * h[2]) + (T2 * h[3]);
        }

        // OPTIONAL - out tangents
        tangents = new Vector3[] { T1, T2 };
        // Return spline positions
        return tbcPosition;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="interpolations">The number of interpolations to perform.</param>
    /// <param name="t">Tension.</param>
    /// <param name="b">Bias.</param>
    /// <param name="c">Continuity.</param>
    public static Vector3[] KochanekBartelsSpline(Vector3[] points, bool doWrap, int index, int interpolations, float t, float b, float c)
    {
        Vector3[] skipOutParam;
        return KochanekBartelsSpline(points, doWrap, index, interpolations, t, b, c, out skipOutParam);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="time">The interpolations point to perform, 0f to 1f.</param>
    /// <param name="t">Tension.</param>
    /// <param name="b">Bias.</param>
    /// <param name="c">Continuity.</param>
    public static Vector3 KochanekBartelsSplineSingle(Vector3[] position, bool doWrap, int index, float time, float t, float b, float c, out Vector3[] tangents)
    {
        // Crunch points and tangents
        Vector3[] P = GetVector3Range(doWrap, index - 1, 4, position);
        Vector3 T1 = ((1 - t) * (1 + b) * (1 + c)) / 2f * (P[1] - P[0]) + ((1 - t) * (1 - b) * (1 - c)) / 2f * (P[2] - P[1]);
        Vector3 T2 = ((1 - t) * (1 + b) * (1 - c)) / 2f * (P[2] - P[1]) + ((1 - t) * (1 - b) * (1 + c)) / 2f * (P[3] - P[2]);

        float[] h = HermiteBasisFunctions(time);
        Vector3 tbcPosition = (P[1] * h[0]) + (P[2] * h[1]) + (T1 * h[2]) + (T2 * h[3]);

        // OPTIONAL - out tangents
        tangents = new Vector3[] { T1, T2 };
        // Return spline position
        return tbcPosition;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The positions to use.</param>
    /// <param name="doWrap">Wrap the provided positions array, else clamp it.</param>
    /// <param name="index">The index to calculate from.</param>
    /// <param name="time">The interpolations point to perform, 0f to 1f.</param>
    /// <param name="t">Tension.</param>
    /// <param name="b">Bias.</param>
    /// <param name="c">Continuity.</param>
    public static Vector3 KochanekBartelsSplineSingle(Vector3[] position, bool doWrap, int index, float time, float t, float b, float c)
    {
        Vector3[] skipOutParam;
        return KochanekBartelsSplineSingle(position, doWrap, index, time, t, b, c, out skipOutParam);
    }
}