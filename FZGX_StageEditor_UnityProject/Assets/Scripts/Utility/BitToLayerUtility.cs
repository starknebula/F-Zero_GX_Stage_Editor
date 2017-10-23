using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class BitToLayerUtility
{
    public static int GetLayer(int value)
    {
        int check;

        for (int i = 0; i < 32; i++)
        {
            check = value >> i;
            check = check & 1;

            if (check == 1)
                return i;
        }

        return 0;
    }
    public static int GetLayer(uint value)
    {
        return GetLayer((int)value);
    }
}