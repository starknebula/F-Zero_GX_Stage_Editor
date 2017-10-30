// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BitLayer
{
    [HideInInspector]
    [SerializeField]
    private uint value;
    public bool[] layers;
    public uint Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            layers = UintToBoolLayer(value);
        }
    }

    private bool[] UintToBoolLayer(uint value)
    {
        bool[] layers = new bool[32];

        for (int i = 0; i < layers.Length; i++)
            layers[i] = ((value >> i) & 1) > 0;

        return layers;
    }

    public BitLayer(uint value)
    {
        this.value = value;
        layers = null;
        layers = UintToBoolLayer(value);
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(BitLayer))]
    public class BitLayer_Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            Rect contentPosition = position;
            contentPosition = EditorGUI.PrefixLabel(position, label);

            SerializedProperty layers = property.FindPropertyRelative("layers");

            contentPosition.width /= 35; // 32 bits + 3 paddings between each 8 bits
            for (int i = 0; i < layers.arraySize; i++)
            {
                layers.GetArrayElementAtIndex(i).boolValue = EditorGUI.Toggle(contentPosition, layers.GetArrayElementAtIndex(i).boolValue);
                contentPosition.x += contentPosition.width;
            }
        }
    }
}
#endif