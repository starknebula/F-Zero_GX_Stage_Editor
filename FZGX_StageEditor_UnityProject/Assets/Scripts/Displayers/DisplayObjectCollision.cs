using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using GX_Data;
using GX_Data.Object_0x48;
using GX_Data.SplineData;

namespace FzgxData
{
    public class DisplayObjectCollision : MonoBehaviour, IFZGXEditorStageEventReceiver
    {
        static GX_Object_0x48 gxDataOther;

        public void StageUnloaded(BinaryReader reader)
        {
            //throw new NotImplementedException();
        }
        public void StageLoaded(BinaryReader reader)
        {
            gxDataOther = new GX_Object_0x48(Stage.Reader);
            PrintCollision();
        }


        void OnDrawGizmos()
        {
            if (gxDataOther == null) return;

            foreach (GX_Data.Object_0x48.BaseObject obj in gxDataOther.obj)
            {
                if (obj.sectionA.collision.quadCount > 0)
                    TriQuadDisp(obj, obj.sectionA.collision.quads, Palette.orange.SetAlpha(.7f).Whitten(.3f));

                if (obj.sectionA.collision.triCount > 0)
                    TriQuadDisp(obj, obj.sectionA.collision.triangles, Palette.rose_red.SetAlpha(.7f).Whitten(.3f));
            }
        }

        public static void PrintCollision()
        {
            foreach (GX_Data.Object_0x48.BaseObject obj in gxDataOther.obj)
            {
                PrintLog.WriteTsvLineEndToBuffer(
                    obj.sectionA.collision.id.ToString(),
                    obj.sectionA.collision.dat_Tri_0x04.ToString(),
                    obj.sectionA.collision.dat_Quad_0x08.ToString(),
                    obj.sectionA.collision.dat_Tri_0x0C.ToString(),
                    obj.sectionA.collision.dat_Quad_0x10.ToString(),
                    obj.sectionA.collision.triCount.ToString(),
                    obj.sectionA.collision.quadCount.ToString(),
                    obj.sectionA.collision.triAddress.ToString(),
                    obj.sectionA.collision.quadAddress.ToString()
                    );
            }

            PrintLog.SaveStream(string.Format("Collision_{0}_(1)", ((int)Stage.currentStage).ToString("000"), Stage.currentStage));
        }

        public void TriQuadDisp(GX_Data.Object_0x48.BaseObject obj, TriQuad[] triQuad, Color color)
        {
            Mesh m = new Mesh();
            List<Vector3> verts1 = new List<Vector3>();
            //List<Vector3> normals = new List<Vector3>();

            for (int i = 0; i < triQuad.Length; i++)
            {
                Vector3[] verts = triQuad[i].vertices;

                for (int j = 0; j < verts.Length; j++)
                    Debug.DrawLine(
                        verts[j] + obj.transform.position,
                        verts[(j + 1) % triQuad[i].vertices.Length] + obj.transform.position,
                        color
                        );

                // Add vertices to mesh
                if (triQuad[i].vertices.Length == 3)
                    verts1.AddRange(new Vector3[] {
                    triQuad[i].vertices[2],
                    triQuad[i].vertices[1],
                    triQuad[i].vertices[0]
                });
                else
                    verts1.AddRange(new Vector3[] {
                    triQuad[i].vertices[2],
                    triQuad[i].vertices[1],
                    triQuad[i].vertices[0],
                    triQuad[i].vertices[0],
                    triQuad[i].vertices[3],
                    triQuad[i].vertices[2],
                });
            }

            m.SetVertices(verts1);
            //m.SetNormals(normals);

            int[] vertConfig = new int[m.vertexCount];
            for (int i = 0; i < m.vertexCount; i++)
                vertConfig[i] = i;
            m.SetTriangles(vertConfig, 0);
            m.RecalculateNormals();

            Gizmos.color = color;
            Gizmos.DrawMesh(m, obj.transform.position);
        }
    }
}