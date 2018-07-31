using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCube.Games.FZeroGX.FileStructures;
using GameCube.Games.FZeroGX.ImportExport;

namespace GameCube.Games.FZeroGX.ImportExport
{
    public class GMA_Importer : FZGX_ImporterExporter
    {
        #region MEMBERS
        [SerializeField]
        private FZeroGXStage index;
        [SerializeField]
        private string resourcePath = "FZGX_EN/stage";
        [SerializeField]
        [HideInInspector]
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
                return string.Format("{1}/st{0},lz", ((int)index).ToString("D2"), resourcePath);
            }
        }

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
            if (gma == null) return;
            if (gma.Models == null) return;

            int index = 0;
            foreach (GMA.Model model in gma.Models)
            {
                index++;
                if (model == null) goto LOOP_END;
                if (model.VertexStrips == null) goto LOOP_END;

                // Display if chosen to in inspector
                if (index == modelSelect | showAllModels)
                    continue;
                else
                    goto LOOP_END;

                DebugDisplayMesh(model);

                foreach (GMA.GC_VertexStripCollection vertCollection in model.VertexStrips)
                {
                    // OPAQUE STRIPS
                    for (int i = 0; i < vertCollection.MT_Strips.Length; i++)
                    {
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
                    }

                    // TRANSLUCID STRIPS
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
            LOOP_END:;
            }
        }

        private void OnDrawGizmos()
        {
            if (GMA != null)
                ReconstructGeometry(GMA);
        }

        private void DebugDisplayMesh(GMA.Model model)
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
                    List<Vector3> MT_Normals = new List<Vector3>();
                    List<Vector3> MT_Vert = new List<Vector3>();
                    List<int> MT_Indices = new List<int>();

                    // GET VERTS + NORMALS
                    if (segmentSelect == i | showAllSegments)
                        for (int j = 0; j < vertexStripCollection.MT_Strips[i].Verts.Length; j++)
                        {
                            MT_Normals.Add(vertexStripCollection.MT_Strips[i].Verts[j].Normal);
                            MT_Vert.Add(vertexStripCollection.MT_Strips[i].Verts[j].Position);
                            //Debug.DrawLine(vertexStripCollection.MT_Strips[i].Verts[j].Position, vertexStripCollection.MT_Strips[i].Verts[j].Position + Vector3.up, Color.red);
                        }

                    // GET INDICES
                    for (int verticeIndex = 0; verticeIndex < MT_Vert.Count - 2; verticeIndex++)
                    {
                        int v1, v2, v3;

                        v1 = verticeIndex + 0 + indexOffset;
                        v2 = verticeIndex + 1 + indexOffset;
                        v3 = verticeIndex + 2 + indexOffset;

                        if ((verticeIndex) % 2 == 0)
                            MT_Indices.AddRange(new int[] { v1, v2, v3 });
                        else
                            MT_Indices.AddRange(new int[] { v3, v2, v1 });
                    }

                    // SET VALUES
                    verts.AddRange(MT_Vert);
                    normals.AddRange(MT_Normals);
                    indices.AddRange(MT_Indices);
                    indexOffset += MT_Vert.Count;
                }

                //for (int i = 0; i < vertexStripCollection.MT_TL_Strips.Length; i++)
                //{
                //    List<Vector3> mttlVerts = new List<Vector3>();
                //    List<int> mttlIndices = new List<int>();

                //    // Get verts
                //    if (segmentSelect == i | showAllSegments)
                //        for (int j = 0; j < vertexStripCollection.MT_TL_Strips[i].Verts.Length; j++)
                //        {
                //            mttlVerts.Add(vertexStripCollection.MT_TL_Strips[i].Verts[j].Position);
                //            _normals.Add(vertexStripCollection.MT_TL_Strips[i].Verts[j].Normal);
                //            //Debug.DrawLine(vertexStripCollection.MT_TL_Strips[i].Verts[j].Position, vertexStripCollection.MT_TL_Strips[i].Verts[j].Position + Vector3.up, Color.yellow);
                //        }

                //    // Get indices
                //    for (int vi = 0; vi < mttlVerts.Count - 2; vi++)
                //    {
                //        int v1, v2, v3;

                //        v1 = vi + 0 + indexOffset;
                //        v2 = vi + 1 + indexOffset;
                //        v3 = vi + 2 + indexOffset;

                //        //tempIndices.AddRange(new int[] { v1, v2, v3, v2, v1, v3 });
                //        if (vi % 2 == 0)
                //            mttlIndices.AddRange(new int[] { v1, v2, v3 });//, v2, v1, v3 });
                //        else
                //            mttlIndices.AddRange(new int[] { v2, v1, v3 });
                //    }

                //    verts.AddRange(mttlVerts);
                //    //normals.AddRange(normals);
                //    indices.AddRange(mttlIndices);
                //    indexOffset += mttlVerts.Count;
                //}

                m.SetVertices(verts);
                m.SetNormals(normals);

                m.SetTriangles(indices.ToArray(), 0);
                //m.RecalculateNormals();

                Gizmos.color = Palette.ColorWheel(model.Index, 12).SetAlpha(.6f);
                Gizmos.DrawMesh(m, Vector3.zero);// model.GCMF.Origin);
                Debug.DrawLine(model.GCMF.Origin, model.GCMF.Origin + Vector3.up, Color.red);
            }
        }
    }
}