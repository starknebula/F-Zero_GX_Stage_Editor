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
    private CameraScriptableObject cam;

    [SerializeField]
    private Vector3 handedness;
    [SerializeField]
    private Space spaceCoord = Space.Self;
    [SerializeField]
    private bool invX, invY, invZ;

    public void PlayCameraAnimation()
    {
        if (cam == null)
            throw new System.Exception();

        StartCoroutine(PlayAnim());
    }



    private IEnumerator PlayAnim()
    {
        Camera camera = Camera.main;

        // We are dealing with a Camera Matrix here
        // Which explains the dumb numbers and inability to just "insert" rotational values
        // http://catlikecoding.com/unity/tutorials/rendering/part-1/

        int iteration = 0;
        int length = cam.CameraData.cameraPans.Count;
        foreach (CameraPan pan in cam.CameraData.cameraPans)
        {
            float lerpSpeed = pan.@params.lerpSpeed;
            int frameDuration = (int)pan.@params.frameDuration;
         
            for (uint i = 0; i < frameDuration; ++i)
            {
                float percent = (float)i / frameDuration;

                // LERP FOV
                camera.fieldOfView = Mathf.Lerp(pan.interpolateFrom.fov, pan.interpolateTo.fov, percent);
                // LERP POS
                camera.transform.position = Vector3.Lerp(pan.interpolateFrom.position, pan.interpolateTo.position, percent);
                Vector3 from = pan.interpolateFrom.rotation;
                from = SwapCoorSystem(from);
                Vector3 to = pan.interpolateTo.rotation;
                to = SwapCoorSystem(to);
                Quaternion qFrom = Quaternion.Euler(from);
                Quaternion qTo = Quaternion.Euler(to);
                Quaternion rotation = Quaternion.Slerp(qFrom, qTo, percent);

                // rotation
                //var flippedRotation = new Vector3(from.x, -from.y, -from.z);
                //var vFrom = SwapCoorSystem(from);
                //var qx = Quaternion.AngleAxis(vFrom.x, Vector3.right);
                //var qy = Quaternion.AngleAxis(vFrom.y, Vector3.up);
                //var qz = Quaternion.AngleAxis(vFrom.z, Vector3.forward);
                //var qFrom = qz * qy * qx; // exact order!

                //var vTo = SwapCoorSystem(to);
                //var qxt = Quaternion.AngleAxis(vTo.x, Vector3.right);
                //var qyt = Quaternion.AngleAxis(vTo.y, Vector3.up);
                //var qzt = Quaternion.AngleAxis(vTo.z, Vector3.forward);
                //var qTo = qz * qy * qx; // exact order!

                //Quaternion rotation = Quaternion.Slerp(qFrom, qTo, percent);

                camera.transform.rotation = rotation;
                camera.transform.Rotate(handedness, spaceCoord); //Swap "handedness" quaternion from gyro.



                yield return new WaitForEndOfFrame();
            }

            iteration++;
        }
    }

    private Vector3 SwapCoorSystem(Vector3 raw)
    {
        float x = raw.y * (invX ? -1f : 1f);
        float y = raw.z * (invY ? -1f : 1f);
        float z = raw.x * (invZ ? -1f : 1f);
        return new Vector3(x, y, z);
    }
}