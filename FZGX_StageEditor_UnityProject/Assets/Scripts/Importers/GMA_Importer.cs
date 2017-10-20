using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FzgxData.FileStructures;

using GameCube.Games.FzeroGX;

namespace FzgxData.ImportExport
{
    public class GMA_Importer : FZGX_ImporterExporter
    {
        #region MEMBERS
        [SerializeField]
        private FzgxStage index;
        [SerializeField]
        private GMA GMA;

        [SerializeField]
        [Range(1, 100)]
        private int depth = 9;

        [SerializeField]
        private int modelSelect;
        [SerializeField]
        private bool showAllModels;

        [SerializeField]
        public int segmentSelect;
        [SerializeField]
        private bool showAllSegments;
        #endregion

        public string filename
        {
            get
            {
                return string.Format("GMA/st{0},lz", ((int)index).ToString("D2"));
            }
        }
        //string.Format("COLI/COLI_COURSE{0},lz", ((int) currentStage).ToString("D2"));

        public override void Export()
        {
            throw new System.NotImplementedException();
        }
        public override void Import()
        {
            GMA = new GMA(GetStreamFromFile(filename));
        }


        private void ReconstructGeometry(GMA gma)
        {
            int index = 0;

            if (gma != null)
            {
                if (gma.Models != null)
                    foreach (GMA.Model model in gma.Models)
                    {
                        index++;

                        if (index == modelSelect | showAllModels)

                            if (model != null)
                            {
                                if (model.VertexStrips != null)
                                {
                                    TempMesh(model);

                                    foreach (GMA.GC_VertexStripCollection vertCollection in model.VertexStrips)
                                    {
                                        for (int i = 0; i < vertCollection.MT_Strips.Length; i++)
                                            for (int j = 0; j < vertCollection.MT_Strips[i].Verts.Length - 2; j++)
                                            {
                                                Debug.DrawLine(
                                                    vertCollection.MT_Strips[i].Verts[j + 0].Position,
                                                    vertCollection.MT_Strips[i].Verts[j + 1].Position,
                                                    Color.white
                                                    //Palette.ColorWheel(i, depth)
                                                    );

                                                Debug.DrawLine(
                                                    vertCollection.MT_Strips[i].Verts[j + 1].Position,
                                                    vertCollection.MT_Strips[i].Verts[j + 2].Position,
                                                    Color.white
                                                    //Palette.ColorWheel(i, depth)
                                                    );

                                                Debug.DrawLine(
                                                    vertCollection.MT_Strips[i].Verts[j + 2].Position,
                                                    vertCollection.MT_Strips[i].Verts[j + 0].Position,
                                                    Color.white
                                                    //Palette.ColorWheel(i, depth)
                                                    );
                                            }

                                        //for (int i = 0; i < vertCollection.MT_TL_Strips.Length; i++)
                                        //    for (int j = 0; j < vertCollection.MT_TL_Strips[i].Verts.Length - 2; j++)
                                        //    {
                                        //        Debug.DrawLine(
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 0].Position,
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 1].Position,
                                        //            Color.white
                                        //            );

                                        //        Debug.DrawLine(
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 1].Position,
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 2].Position,
                                        //            Color.white
                                        //            );

                                        //        Debug.DrawLine(
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 2].Position,
                                        //            vertCollection.MT_TL_Strips[i].Verts[j + 0].Position,
                                        //            Color.white
                                        //            );
                                        //    }
                                    }
                                }
                            }
                    }
            }
        }


        private void OnDrawGizmos()
        {
            if (GMA != null)
                ReconstructGeometry(GMA);
        }

        private void TempMesh(GMA.Model model)
        {
            Mesh m = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            int indexOffset = 0;
            foreach (GMA.GC_VertexStripCollection vertexStripCollection in model.VertexStrips)
            {
                for (int i = 0; i < vertexStripCollection.MT_Strips.Length; i++)
                {
                    //List<Vector3> _normals = new List<Vector3>();
                    List<Vector3> mtVert = new List<Vector3>();
                    List<int> mtIndices = new List<int>();

                    // Get verts
                    if (segmentSelect == i | showAllSegments)
                        for (int j = 0; j < vertexStripCollection.MT_Strips[i].Verts.Length; j++)
                        {
                            //_normals.Add(vertexStripCollection.MT_Strips[i].Verts[j].Normal);
                            mtVert.Add(vertexStripCollection.MT_Strips[i].Verts[j].Position);
                            Debug.DrawLine(vertexStripCollection.MT_Strips[i].Verts[j].Position, vertexStripCollection.MT_Strips[i].Verts[j].Position + Vector3.up, Color.red);
                        }


                    // Get indices
                    for (int verticeIndex = 0; verticeIndex < mtVert.Count - 2; verticeIndex++)
                    {
                        int v1, v2, v3;

                        v1 = verticeIndex + 0 + indexOffset;
                        v2 = verticeIndex + 1 + indexOffset;
                        v3 = verticeIndex + 2 + indexOffset;

                        //tempIndices.AddRange(new int[] { v1, v2, v3, v2, v1, v3 });
                        if ((verticeIndex) % 2 == 0)
                            mtIndices.AddRange(new int[] { v1, v2, v3 });
                        else
                            mtIndices.AddRange(new int[] { v3, v2, v1 });
                    }

                    verts.AddRange(mtVert);
                    indices.AddRange(mtIndices);
                    indexOffset += mtVert.Count;
                }

                for (int i = 0; i < vertexStripCollection.MT_TL_Strips.Length; i++)
                {
                    List<Vector3> mttlVerts = new List<Vector3>();
                    List<int> mttlIndices = new List<int>();

                    // Get verts
                    if (segmentSelect == i | showAllSegments)
                        for (int j = 0; j < vertexStripCollection.MT_TL_Strips[i].Verts.Length; j++)
                        {
                            mttlVerts.Add(vertexStripCollection.MT_TL_Strips[i].Verts[j].Position);
                            //_normals.Add(vertexStripCollection.MT_TL_Strips[i].Verts[j].Normal);
                            //Debug.DrawLine(vertexStripCollection.MT_TL_Strips[i].Verts[j].Position, vertexStripCollection.MT_TL_Strips[i].Verts[j].Position + Vector3.up, Color.yellow);
                        }

                    // Get indices
                    for (int vi = 0; vi < mttlVerts.Count - 2; vi++)
                    {
                        int v1, v2, v3;

                        v1 = vi + 0 + indexOffset;
                        v2 = vi + 1 + indexOffset;
                        v3 = vi + 2 + indexOffset;

                        //tempIndices.AddRange(new int[] { v1, v2, v3, v2, v1, v3 });
                        if (vi % 2 == 0)
                            mttlIndices.AddRange(new int[] { v1, v2, v3 });//, v2, v1, v3 });
                        else
                            mttlIndices.AddRange(new int[] { v2, v1, v3 });
                    }

                    verts.AddRange(mttlVerts);
                    //normals.AddRange(normals);
                    indices.AddRange(mttlIndices);
                    indexOffset += mttlVerts.Count;
                }
                //Debug.LogFormat("{2}::{0}: {1}", model.Name, verts.Count, model.Index);

                //while (verts.Count % 3 > 0)
                //    verts.RemoveAt(0);

                m.SetVertices(verts);
                //m.SetNormals(normals);

                m.SetTriangles(indices.ToArray(), 0);
                m.RecalculateNormals();

                Gizmos.color = Palette.ColorWheel(model.Index, 12).SetAlpha(.6f);
                Gizmos.DrawMesh(m, Vector3.zero);// model.GCMF.Origin);
                Debug.DrawLine(model.GCMF.Origin, model.GCMF.Origin + Vector3.up, Color.red);
            }
        }
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomEditor(typeof(FzgxData.ImportExport.GMA_Importer))]
    internal class GMA_Importer_Editor : FZGX_ImporterExporter_Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FzgxData.ImportExport.GMA_Importer editorTarget = target as FzgxData.ImportExport.GMA_Importer;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                editorTarget.segmentSelect++;
            }
            if (GUILayout.Button("-"))
            {
                editorTarget.segmentSelect--;
            }
            EditorGUILayout.EndHorizontal();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif