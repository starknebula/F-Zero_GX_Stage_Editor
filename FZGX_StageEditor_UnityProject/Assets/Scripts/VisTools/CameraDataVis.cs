// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube;
using GameCube.Games.FZeroGX.FileStructures;

public class CameraDataVis : MonoBehaviour
{
    [SerializeField]
    private CAM cam;
    [SerializeField]
    private float radiusStartEnd, radiusLerpPoint;
    [SerializeField]
    private float lineLength;



    public void PlayCameraAnimation()
    {
        if (cam == null)
            throw new System.Exception();

        StartCoroutine(PlayAnim());
    }



    private IEnumerator PlayAnim()
    {
        int iteration = 0;
        int length = cam.CameraData.cameraPans.Count;
        foreach (CameraPan pan in cam.CameraData.cameraPans)
        {
            float lerpSpeed = pan.@params.lerpSpeed;
            Vector3
                from = pan.interpolateFrom.position,
                to = pan.interpolateTo.position;
            Vector3
                rFrom = pan.interpolateFrom.rotation,
                rTo = pan.interpolateTo.rotation;

            rFrom.y += 180;
            rTo.y += 180;

            float thisFrameFOV = pan.interpolateFrom.fov;
            float lastFramFOV = thisFrameFOV;
            Vector3 thisFramePos = from;
            Vector3 lastFramePos = thisFramePos;
            Vector3 thisFrameRot = rFrom;
            Vector3 lastFrameRot = thisFrameRot;

            // DEBUG
            Color c = Palette.ColorWheel(iteration, length);
            //UnityEditor.Handles.Label(from + Vector3.up * 5f, (iteration + 1).ToString());

            //UnityEditor.Handles.DrawSphere(0, from, Quaternion.identity, radiusStartEnd);
            //UnityEditor.Handles.DrawSphere(0, to, Quaternion.identity, radiusStartEnd);


            //Quaternion lookatVector = Quaternion.FromToRotation(from.normalized, to.normalized);
            Vector3 dir = (to - from).normalized;
            dir = Vector3.one - dir;
            dir /= 5f;

            for (uint i = 0; i < pan.@params.frameDuration; i++)
            {
                lastFramePos = thisFramePos;
                thisFramePos = Vector3.Lerp(lastFramePos, to, lerpSpeed);

                lastFrameRot = thisFrameRot;
                thisFrameRot = Vector3.Slerp(lastFrameRot, rTo, lerpSpeed);

                lastFramFOV = thisFrameFOV;
                thisFrameFOV = Mathf.Lerp(lastFramFOV, thisFrameFOV, lerpSpeed);

                Camera.main.transform.position = thisFramePos;
                //Camera.main.transform.rotation = Quaternion.Euler(thisFrameRot.x * dir.x, thisFrameRot.y * dir.y, thisFrameRot.z * dir.z);
                Vector3 lookPos = Quaternion.Euler(thisFrameRot) * dir;
                Camera.main.transform.LookAt(Camera.main.transform.position + lookPos, Vector3.up);

                Camera.main.fieldOfView = thisFrameFOV;
                yield return new WaitForEndOfFrame();

                Debug.DrawLine(from, from + Quaternion.Euler(rFrom) * Vector3.forward * lineLength, c);
                Debug.DrawLine(to, to + Quaternion.Euler(rTo) * Vector3.forward * lineLength, c);
                Debug.DrawLine(from, to, c);
            }

            iteration++;
        }

        
    }

    private bool CheckLayer(int index, CameraLayers layer)
    {
        return (((int)layer >> index) & 1) > 0;
    }

}

