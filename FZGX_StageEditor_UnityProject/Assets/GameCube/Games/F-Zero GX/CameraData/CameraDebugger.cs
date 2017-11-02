// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube.Games.FZeroGX.FileStructures;

namespace GameCube.Games.FZeroGX
{
    public class CameraDebugger : MonoBehaviour
    {
        [SerializeField]
        private CameraScriptableObject cam;
        [SerializeField]
        private float radiusStartEnd, radiusLerpPoint;
        [SerializeField]
        private float lineLength;

        void OnDrawGizmos()
        {
            if (cam == null) return;

            int iteration = 0;
            int length = cam.CameraData.cameraPans.Count;
            foreach (CameraPan pan in cam.CameraData.cameraPans)
            {
                Gizmos.color = Palette.ColorWheel(iteration, length);

                Vector3
                    from = pan.interpolateFrom.position,
                    to = pan.interpolateTo.position;
                Quaternion
                    rFrom = pan.interpolateFrom.Rotation,
                    rTo = pan.interpolateTo.Rotation;

                UnityEditor.Handles.Label(from + Vector3.up * 5f, (iteration + 1).ToString());

                Gizmos.DrawSphere(from, radiusStartEnd);
                Gizmos.DrawWireSphere(to, radiusStartEnd);
                Gizmos.DrawLine(from, from + rFrom * Vector3.forward * lineLength);
                Gizmos.DrawLine(to, to + rTo * Vector3.forward * lineLength);

                Vector3 thisFrame = from;
                Quaternion rThisFrame = rFrom;
                for (uint i = 0; i < pan.@params.frameDuration; i++)
                {
                    Vector3 lastFrame = thisFrame;
                    thisFrame = Vector3.Lerp(lastFrame, to, pan.@params.lerpSpeed);

                    Quaternion rLastFrame = rThisFrame;
                    rThisFrame = Quaternion.Slerp(rLastFrame, rTo, pan.@params.lerpSpeed);

                    Gizmos.DrawSphere(thisFrame, radiusLerpPoint);
                    Gizmos.DrawLine(thisFrame, thisFrame + rThisFrame * Vector3.forward * lineLength);
                }

                ++iteration;
            }
        }
    }
}