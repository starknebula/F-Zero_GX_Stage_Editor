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

    //[SerializeField]
    //private float radiusStartEnd, radiusLerpPoint;
    //[SerializeField]
    //private float lineLength;
    //[SerializeField]
    //Vector3 addedRotation;
    //[SerializeField]
    //int probe;

    public void PlayCameraAnimation()
    {
        if (cam == null)
            throw new System.Exception();

        StartCoroutine(PlayAnim());
    }

    //void OnDrawGizmos()
    //{
    //    int index = 0;
    //    foreach (CameraPan pan in cam.CameraData.cameraPans)
    //    {
    //        if (index == probe)
    //        {
    //            Vector3 lookNormal = -(pan.interpolateTo.position - Vector3.zero).normalized;
    //            Quaternion rotDif = Quaternion.FromToRotation(Vector3.forward, lookNormal);

    //            Gizmos.color = Color.white;
    //            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 3);
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawLine(Vector3.zero, rotDif * Vector3.forward * 2);
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawLine(Vector3.zero, lookNormal);

    //        }
    //        index++;
    //    }
    //}


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

            // Defines the forward axis
            // This is solved with Quaternion.FromToRotation which converts the rotation from froward (+z) to this
            // Arbitrary axis as forward/back (depth)
            Vector3 cameraDepthAxis = (pan.interpolateFrom.position - pan.interpolateTo.position).normalized;
            Quaternion worldToCameraRotation = Quaternion.FromToRotation(Vector3.forward, cameraDepthAxis);
            Quaternion cameraToWorldRotation = Quaternion.FromToRotation(cameraDepthAxis, Vector3.forward);

            Matrix4x4 cameraMatrix = Matrix4x4.LookAt(pan.interpolateFrom.position, pan.interpolateTo.position, worldToCameraRotation * Vector3.up);

            for (uint i = 0; i < frameDuration; i++)
            {
                float percent = (float)i / frameDuration;

                camera.fieldOfView = Mathf.Lerp(pan.interpolateFrom.fov, pan.interpolateTo.fov, percent);
                camera.transform.position = Vector3.Lerp(pan.interpolateFrom.position, pan.interpolateTo.position, percent);

                //Quaternion rotationLerp = Quaternion.Slerp(
                //    Quaternion.Euler(pan.interpolateFrom.rotation),
                //    Quaternion.Euler(pan.interpolateTo.rotation),
                //    percent / 5f);
                Vector3 vRotationLerp = Vector3.Slerp(pan.interpolateFrom.rotation, pan.interpolateTo.rotation, percent);

                // Appears to be somewhat the appropriate way
                //camera.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Quaternion.Euler(vRotationLerp) * cameraForward);

                // Works WELL for SOME angles (low rotation?)
                // The thought was convert from our space into theirs (new z/depth), then back
                //camera.transform.rotation = Quaternion.FromToRotation(Quaternion.Euler(vRotationLerp) * cameraForward, Vector3.forward);

                // From camera (GX) space to world (Unity)
                camera.worldToCameraMatrix = cameraMatrix * transform.worldToLocalMatrix;
                camera.transform.rotation = Quaternion.Euler(vRotationLerp);

                yield return new WaitForEndOfFrame();
            }

            iteration++;
        }


    }
}