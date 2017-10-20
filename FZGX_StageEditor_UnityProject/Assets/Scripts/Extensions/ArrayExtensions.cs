// Created by Raphael "Stark" Tetreault 05/09/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 09/09/2016

using System;

/// <summary>
/// Generic extensions for arrays.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Returns the indexed length of the array.
    /// </summary>
    /// <param name="array">The current array.</param>
    /// <example><code>
    /// int[] i = new int[3];
    /// Console.WriteLine("Usable index is {0}", i.ArrayLength());
    /// 
    /// // Console prints
    /// // Usable index is 2
    /// </code></example>
    public static int LastIndex(this Array array)
    {
        return (array.Length > 0) ? array.Length - 1 : 0;
    }

    /// <summary>
    /// Returns an array subsection from this array, from <paramref name="index"/> 
    /// of size <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="T">The type of this array.</typeparam>
    /// <param name="array">The current array.</param>
    /// <param name="index">The index of the array to start the range at.</param>
    /// <param name="length">How many indices the range returns.</param>
    public static T[] Range<T>(this Array array, int index, int length)
    {
        // Check to see if the desired indices are within range of the array
        if (index + length > array.LastIndex())
            throw new IndexOutOfRangeException(
                "Array.Range() desired is outside of scope! Index + Length [" + (index + length) +
                "] is greater than array length - 1 [" + array.LastIndex() + "]"
                );

        // Initialize array which will be the indexed range desired
        T[] range = new T[length];

        // Set each index to that of the larger array
        for (int i = 0; i < length; i++)
            range[i] = (T)array.GetValue(index + i);

        return range;
    }

    /// <summary>
    /// Returns an array using data from this array, from <paramref name="index"/> 
    /// to index + <paramref name="length"/> where if the value exceeds the length
    /// wraps back to the first entry and continues.
    /// </summary>
    /// <typeparam name="T">The type of this array.</typeparam>
    /// <param name="array">The current array.</param>
    /// <param name="index">The index of the array to start the range at.</param>
    /// <param name="length">How many indices the range returns.</param>
    public static T[] RangeWrap<T>(this Array array, int index, int length)
    {
        // Initialize array which will be the indexed range desired
        T[] range = new T[length];

        // Set each index to that of the larger array
        for (int i = 0; i < length; i++)
            range[i] = (T)array.GetValue(MathX.Wrap(index + i, array.Length));

        return range;
    }

    /// <summary>
    /// Returns a new array created with each index passed in order.
    /// </summary>
    /// <typeparam name="T">The type of this array.</typeparam>
    /// <param name="array">The current array.</param>
    /// <param name="indexes">Which indexes you wish to build the returning array from in the order presented.</param>
    public static T[] Indices<T>(this Array array, params int[] indexes)
    {
        foreach (int index in indexes)
            if (index > array.LastIndex())
                throw new IndexOutOfRangeException(
                    "One or more desired indexes are out of scope for this array! Requested index: [" + index + "]."
                    );

        // Initialize array for each index we wish to copy
        T[] indices = new T[indexes.Length];

        // Set each index to new array IN ORDER
        for (int i = 0; i < indexes.Length; i++)
            indices[i] = (T)array.GetValue(indexes[i]);

        return indices;
    }
}
