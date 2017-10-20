// Created by Raphael "Stark" Tetreault 15/01/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 11/09/2016

using UnityEngine;

/// <summary>
/// An extended collection of math functions and utilities.
/// </summary>
public static class MathX
{
    /// <summary>
    /// Returns an int that is always within the bounds of an array (that is not 0 in length).
    /// This is achieved by underflowing or overflowing the int value based on the array's size.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int Wrap(int index, int maxValue)
    {
        if (maxValue == 0)
        {
            Debug.LogError("MathX.Wrap: maxSize cannot be 0!");
            return 0;
        }

        if (index < 0)
            index = maxValue - (Mathf.Abs(index) % maxValue);

        if (index >= maxValue)
            index = index % maxValue;

        return index;
    }

    /// <summary>
    /// Check to see if <paramref name="value"/> is between <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <remarks>Use ! operator do check if it isn't between values (inclusive).</remarks>
    public static bool IsBetween(float value, float a, float b)
    {
        if (value > a && value < b)
            return true;

        return false;
    }
    /// <summary>
    /// Check to see if <paramref name="value"/> is between (and including) <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <remarks>Use ! operator do check if it isn't between values (exclusive).</remarks>
    public static bool IsBetweenInclusive(float value, float a, float b)
    {
        if (value >= a && value <= b)
            return true;

        return false;
    }

    /// <summary>
    /// Returns Sine of angle theta.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    public static float Sin(float angleInDegrees)
    {
        return Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
    }
    /// <summary>
    /// Returns Sine of angle theta to the power of <paramref name="power"/>.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <param name="power">The power of Sine.</param>
    public static float SinPow(float angleInDegrees, float power)
    {
        return Mathf.Pow(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), power);
    }
    /// <summary>
    /// Returns Cosine of angle theta.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    public static float Cos(float angleInDegrees)
    {
        return Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
    }
    /// <summary>
    /// Returns Cosine of angle theta to the power of <paramref name="power"/>.
    /// </summary>
    /// <param name="angleInDegrees">The angle in degrees.</param>
    /// <param name="power">The power of Cosine.</param>
    public static float CosPow(float angleInDegrees, float power)
    {
        return Mathf.Pow(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), power);
    }

    public static float SinTime(float time)
    {
        return Mathf.Sin(time * Mathf.PI * 2f);
    }
    public static float SinPowTime(float time, float power)
    {
        return Mathf.Pow(Mathf.Sin(time * Mathf.PI * 2f), power);
    }
    public static float CosTime(float time)
    {
        return Mathf.Cos(time * Mathf.PI * 2f);
    }
    public static float CosPowTime(float time, float power)
    {
        return Mathf.Pow(Mathf.Cos(time * Mathf.PI * 2f), power);
    }

    public static float Hypoteneuse(float x, float y)
    {
        return Mathf.Sqrt(x*x + y*y);
    }

    /// <summary>
    /// Rounds value to the nearest multiple of roundValue.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="nearestMultiple"></param>
    /// <returns></returns>
    public static float RoundValue(float value, float nearestMultiple)
    {
        if (nearestMultiple <= 0)
        {
            Debug.LogError("NearestMultiple cannot be less or equal to 0! Returning Negative Infinity.");
            return Mathf.NegativeInfinity;
        }

        float remainder = value % nearestMultiple;
        float quotient = value - remainder;

        if (remainder < (nearestMultiple / 2f))
            value = quotient;
        else
            value = quotient + nearestMultiple;

        return value;
    }

    /// <summary>
    /// Returns the greatest common divisor between 2 integers.
    /// </summary>
    /// <param name="a">Integer A.</param>
    /// <param name="b">Integer B.</param>
    public static int GreatestCommonDivisor(int a, int b)
    {
        return (b == 0) ? a : GreatestCommonDivisor(b, a % b);
    }

    //https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/2D_affine_transformation_matrix.svg/512px-2D_affine_transformation_matrix.svg.png
    public static Vector3 Rotate(Vector3 vector, Vector3 eulers)
    {
        float x = eulers.x;
        float y = eulers.y;
        float z = eulers.z;

        //Debug.Log(vector);

        Matrix4x4 mX = new Matrix4x4();
        mX.SetRow(0, new Vector3(1, 0, 0));
        mX.SetRow(1, new Vector3(0, Cos(x), -Sin(x)));
        mX.SetRow(2, new Vector3(0, Sin(x), Cos(x)));

        Matrix4x4 mY = new Matrix4x4();
        mY.SetRow(0, new Vector3(Cos(y), 0, Sin(y)));
        mY.SetRow(1, new Vector3(0, 1, 0));
        mY.SetRow(2, new Vector3(-Sin(y), 0, Cos(y)));

        Matrix4x4 mZ = new Matrix4x4();
        mZ.SetRow(0, new Vector3(Cos(z), -Sin(z), 0));
        mZ.SetRow(1, new Vector3(Sin(z), Cos(z), 0));
        mZ.SetRow(2, new Vector3(0, 0, 1));

        vector = Matrix4x4Multiply(vector, mX);
        vector = Matrix4x4Multiply(vector, mY);
        vector = Matrix4x4Multiply(vector, mZ);

        return vector;
    }
    public static Vector3 Matrix4x4Multiply(Vector3 vector, Matrix4x4 matrix)
    {
        Debug.Log(matrix);
        Vector3 multipliedVector = new Vector3(
            matrix.m00 * vector.x + matrix.m01 * vector.y + matrix.m02 * vector.z,
            matrix.m10 * vector.x + matrix.m11 * vector.y + matrix.m12 * vector.z,
            matrix.m20 * vector.x + matrix.m21 * vector.y + matrix.m22 * vector.z
        );

        return multipliedVector;
    }
    public static Vector3 Scale(Vector3 vector, Vector3 scale)
    {
        Matrix4x4 mS = new Matrix4x4();
        mS.m00 = scale.x;
        mS.m11 = scale.y;
        mS.m22 = scale.z;

        //Debug.Log(mS);
        vector = Matrix4x4Multiply(vector, mS);
        //Debug.Log(mS);
        return vector;
    }
    /*
    public static Vector3 QuaternionRotationMatrix(Vector3 q, float r)
    {
        //https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation#Quaternion-derived_rotation_matrix
        // i j k r
        // x y z w

        Vector3 R1 = new Vector3( 1f - 2f*q.y*q.y - 2f*q.z*q.z, 2f*(q.x*q.y - q.z*r), 2f*(q.x*q.z - q.y*r) );
        Vector3 R2 = new Vector3( 2f*(q.x*q.y + q.z*r), 1f - 2f*q.x*q.x - 2F*q.z*q.z, 2f*(q.y*q.z - q.x*r) );
        Vector3 R3 = new Vector3( 2f*(q.x*q.z - q.y*r), 2f*(q.y*q.z + q.x*r), 1f - 2f*q.x*q.x - 2f*q.y*q.y );

    }
    */


}