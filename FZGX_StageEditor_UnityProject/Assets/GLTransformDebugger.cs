// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLTransformDebugger : MonoBehaviour
{
    [SerializeField]
    private Material glAxisX, glAxisY, glAxisZ, glAxisWhite;
    private Material[] glAxesMats => new Material[] { glAxisX, glAxisY, glAxisZ };
    private Material[] glWhiteMats => new Material[] { glAxisWhite, glAxisWhite, glAxisWhite };

    [SerializeField]
    private int ringVertices = 16;
    [SerializeField]
    private float ringRadius = 1f;

    [SerializeField]
    private GLWireSphere wireSphere;
    [SerializeField]
    private GLWireSphere defaultSphere;

    void Awake()
    {
        wireSphere.InitRings(ringVertices, ringRadius);
        defaultSphere.InitRings(ringVertices, ringRadius * 1.1f);
    }
    void OnRenderObject()
    {
        defaultSphere.GLRenderRings(glWhiteMats, Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one));
        wireSphere.GLRenderRings(glAxesMats, transform.localToWorldMatrix);
    }

    [System.Serializable]
    public struct GLRing
    {
        public Vector3[] points;

        public void InitRing(int verts, float radius)
        {
            points = GenerateRing(verts, radius);
        }
        public void GLRenderRing(Material glRenderMaterial, Matrix4x4 objectMatrix)
        {
            glRenderMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(objectMatrix);
            GL.Begin(GL.LINE_STRIP);
            foreach (Vector3 point in points)
                GL.Vertex(point);
            GL.End();
            GL.PopMatrix();
        }

        public static Vector3[] GenerateRing(int verts, float radius)
        {
            Vector3[] pts = new Vector3[verts + 1];
            for (int i = 0; i < pts.Length; i++)
            {
                float step = (float)i / verts * Mathf.PI * 2f;
                pts[i] = new Vector3(Mathf.Cos(step), Mathf.Sin(step)) * radius;
            }
            return pts;
        }
    }
    [System.Serializable]
    public struct GLWireSphere
    {
        public GLRing x;
        public GLRing y;
        public GLRing z;
        // To be used for reset function
        private GLRing defaultRing;

        public GLRing[] Axes
        {
            get
            {
                return new GLRing[] { x, y, z };
            }
        }

        public void InitRings(int verts = 32, float radius = 1f)
        {
            defaultRing.InitRing(verts, radius);
            x.points = new Vector3[defaultRing.points.Length];
            y.points = new Vector3[defaultRing.points.Length];
            z.points = new Vector3[defaultRing.points.Length];

            defaultRing.points.CopyTo(x.points, 0);
            defaultRing.points.CopyTo(y.points, 0);
            defaultRing.points.CopyTo(z.points, 0);

            Quaternion xRotation = Quaternion.Euler(0, 90f, 0);
            for (int i = 0; i < x.points.Length; i++)
                x.points[i] = xRotation * x.points[i];

            Quaternion yRotation = Quaternion.Euler(90f, 0, 0);
            for (int i = 0; i < y.points.Length; i++)
                y.points[i] = yRotation * y.points[i];

            // Z is correct
            //Quaternion zRotation = Quaternion.Euler(90f, 0, 0);
            //for (int i = 0; i < z.points.Length; i++)
            //    z.points[i] = zRotation * z.points[i];
        }
        public void GLRenderRings(Material[] glRenderMaterials3, Matrix4x4 objectMatrix)
        {
            /// 3 axes
            for (int i = 0; i < 3; i++)
                Axes[i].GLRenderRing(glRenderMaterials3[i], objectMatrix);
        }
    }
}