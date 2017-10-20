using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class AnimationCurveExtensions
{
    public static AnimationCurve SetTangentsToLinear(this AnimationCurve animationCurve, out Keyframe[] keys)
    {
        for (int i = 0; i < animationCurve.length; i++)
        {
            int prevPoint = (i - 1 > 0) ? i - 1 : 0;
            int nextPoint = (i + 1 < animationCurve.length - 1) ? i + 1 : animationCurve.length - 1;

            Debug.Log(animationCurve.keys[i].inTangent + " " + TangentAngle(animationCurve.keys[i], animationCurve.keys[prevPoint]));

            animationCurve.keys[i].inTangent = TangentAngle(animationCurve.keys[i], animationCurve.keys[prevPoint]);
            animationCurve.keys[i].outTangent = TangentAngle(animationCurve.keys[i], animationCurve.keys[nextPoint]);
            Debug.Log(animationCurve.keys[i].inTangent + " " + TangentAngle(animationCurve.keys[i], animationCurve.keys[prevPoint]));

        }

        keys = animationCurve.keys;
        return animationCurve;
    }

    public static float TangentAngle(Keyframe from, Keyframe to)
    {
        Vector2 difference = new Vector2(to.time, to.value) - new Vector2(from.time, from.value);
        float angle = Mathf.Atan2(difference.y, difference.x);
        //Debug.Log(angle * Mathf.Rad2Deg);

        return angle;
    }

}