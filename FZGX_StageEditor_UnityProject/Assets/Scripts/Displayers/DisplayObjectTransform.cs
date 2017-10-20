using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using GX_Data;
using GX_Data.Object_0x48;
using GX_Data.SplineData;

namespace FzgxData
{
    public class DisplayObjectTransform : MonoBehaviour, IFZGXEditorStageEventReceiver
    {
        // Draw Simple Variables
        public bool drawSimple = true;
        public float drawSimpleScale = 5f;
        public float drawSimpleScaleMax = 20f;
        public Color drawSimpleColor = Palette.white.SetAlpha(.5f);
        public Mesh drawSimpleMesh;

        static GX_Object_0x48 otherData;


        public void StageUnloaded(BinaryReader reader)
        {
            //throw new NotImplementedException();
        }
        public void StageLoaded(BinaryReader reader)
        {
            otherData = new GX_Object_0x48(Stage.Reader);
        }



        void OnDrawGizmos()
        {
            if (otherData == null) return;

            if (drawSimple)
                DrawSimple(otherData);
            else
                DrawComplexeTransform(otherData);
        }

        public void DrawSimple(GX_Object_0x48 gxData)
        {
            Gizmos.color = drawSimpleColor;

            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
            {
                //Gizmos.color = ColorUtility.NormalToColor(obj.transform.rotation * Vector3.forward);
                Gizmos.DrawMesh(drawSimpleMesh, obj.transform.position, obj.transform.rotation, Vector3Utility.Clamp(obj.transform.scale * drawSimpleScale, -drawSimpleScaleMax, drawSimpleScaleMax));
            }

            Gizmos.color = Palette.red_orange;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
                GizmosDrawTransform(obj, Vector3.right, obj.transform.scale.x);

            Gizmos.color = Palette.lime_green;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
                GizmosDrawTransform(obj, Vector3.up, obj.transform.scale.y);

            Gizmos.color = Palette.cobalt;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
                GizmosDrawTransform(obj, Vector3.forward, obj.transform.scale.z);
        }
        public void GizmosDrawTransform(GX_Data.Object_0x48.BaseObject obj, Vector3 direction, float directionScale)
        {
            Gizmos.DrawLine(
                obj.transform.position,
                obj.transform.position + obj.transform.rotation * direction * Mathf.Clamp(directionScale * drawSimpleScale, -drawSimpleScaleMax, drawSimpleScaleMax)
                );
        }

        public void DrawComplexeTransform(GX_Object_0x48 gxData)
        {
            // X
            Handles.color = Palette.red_orange;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
            {
                Handles.DrawWireDisc(obj.transform.position, obj.transform.normalX, 5f);
                Handles.DrawLine(obj.transform.position, obj.transform.position + obj.transform.normalX * 5F);
                //SmartHandleLabel(g.Transform.position, Vector3.zero, "X: " + g.Transform.normalX.magnitude.ToString());
            }

            // Y
            Handles.color = Palette.lime;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
            {
                Handles.DrawWireDisc(obj.transform.position, obj.transform.normalY, 5f);
                Handles.DrawLine(obj.transform.position, obj.transform.position + obj.transform.normalY * 5F);
                //SmartHandleLabel(g.Transform.position, Vector3.down * .2f, "Y: " + g.Transform.normalY.magnitude.ToString());
            }

            // Z
            Handles.color = Palette.cobalt;
            foreach (GX_Data.Object_0x48.BaseObject obj in gxData.obj)
            {
                Handles.DrawWireDisc(obj.transform.position, obj.transform.normalZ, 5f);
                Handles.DrawLine(obj.transform.position, obj.transform.position + obj.transform.normalZ * 5F);
                //SmartHandleLabel(g.Transform.position, Vector3.down*.4f, "Z: " + g.Transform.normalZ.magnitude.ToString());

                SmartHandleLabel(obj.transform.position, Vector3.down * .0f, obj.ObjectName);
                SmartHandleLabel(obj.transform.position, Vector3.down * .2f, "Position: " + obj.transform.position.ToString());
                SmartHandleLabel(obj.transform.position, Vector3.down * .4f, "Rotation: " + obj.transform.rotation.eulerAngles.ToString());
                SmartHandleLabel(obj.transform.position, Vector3.down * .6f, "Scale:    " + obj.transform.scale.ToString());
            }
        }
        public void SmartHandleLabel(Vector3 position, Vector3 offset, string label)
        {
            if (Vector3.Dot(Camera.current.transform.forward, position - Camera.current.transform.position) > 0)
                if (Vector3.Distance(Camera.current.transform.position, position) < 300f)
                    Handles.Label(position + offset * HandleUtility.GetHandleSize(position + offset), label);
        }
    }
}