// Created by Raphael "Stark" Tetreault /2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCube;
using GameCube.COLI;
using System.IO;

public class DisplayEffectCollision : MonoBehaviour, IFZGXEditorStageEventReceiver
{
    /// <summary>
    /// Colors used to determine the type of collision. Each color is meant to represent the colors
    /// used for the object in GX. Approximations only, some effect are invisible in game.
    /// </summary>
    private static Color[] ColiTypeColors = new Color[]
    {
        // Missing: Mine?, Lava?
        new Color(1.0f, 1.0f, 1.0f), // 1  WHITE - Drive-able
        new Color(1.0f, 0.0f, 1.0f), // 2  MGNTA - Recover
        new Color(0.0f, 1.0f, 0.0f), // 3  GREEN - "Bonk" collision?
        new Color(1.0f, 1.0f, 0.0f), // 4  YLLOW - Boost Pads
        new Color(1.0f, 0.7f, 0.0f), // 5  ORNGE - Jump Pads
        new Color(0.0f, 1.0f, 1.0f), // 6  CYAN  - Ice/Slip
        new Color(0.8f, 0.5f, 0.2f), // 7  BROWN - Dirt
        new Color(0.7f, 0.5f, 1.0f), // 8  BLUE  - Lava/Laser/Damage
        new Color(0.5f, 0.5f, 0.5f), // 9  GREY  - OOB, Out of Bounds Plane
        new Color(1.0f, 0.5f, 0.5f), // 10 GREY-RED - Insta-Kill Collider. In AX, it would kill you but you'd skip/recochet off of it
        new Color(1.0f, 0.4f, 0.0f), // 11 RED-ORANGE - Instant Kill
        new Color(1.0f, 0.2f, 0.0f), // 12 RED-ORANGE - Kill Plane A
        new Color(0.0f, 0.0f, 0.0f), // 13 BLACK - Kill Plane B (duplicate)
        new Color(1.0f, 0.0f, 0.0f), // 14 RED   - Kill Plane B
    };

    [SerializeField]
    public bool showMesh;

    public void StageUnloaded(BinaryReader reader)
    {

    }
    public void StageLoaded(BinaryReader reader)
    {
        coli = new GameCube.COLI.Collision(reader);
    }

    public static GameCube.COLI.Collision coli;

    void OnDrawGizmos()
    {
        if (coli != null)
        {
            for (int i = 0; i < coli.IndexOffsetBlocksTri.Length; i++)
            {
                Color c = ColiTypeColors[i];
                Gizmos.color = c;

                if (coli.IndexOffsetBlocksTri[i] != null)
                {
                    for (int j = 0; j < coli.IndexOffsetBlocksTri[i].Sequences.Length; j++)
                    {
                        if (coli.IndexOffsetBlocksTri[i].Sequences[j] != null)
                            TriQuadDisp(Vector3.zero, coli.IndexOffsetBlocksTri[i].Sequences[j].GetVertices(coli.Tris), c);

                        if (coli.IndexOffsetBlocksQuad[i].Sequences[j] != null)
                            TriQuadDisp(Vector3.zero, coli.IndexOffsetBlocksQuad[i].Sequences[j].GetVertices(coli.Quads), c);
                    }
                }
            }
        }
    }

    public void TriQuadDisp(Vector3 position, TriQuad[] triQuad, Color color)
    {
        Gizmos.color = color;

        Mesh m = new Mesh();
        List<Vector3> verts1 = new List<Vector3>();
        //List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < triQuad.Length; i++)
        {
            Vector3[] verts = triQuad[i].vertices;

            for (int j = 0; j < verts.Length; j++)
                Gizmos.DrawLine(
                    verts[j] + position,
                    verts[(j + 1) % triQuad[i].vertices.Length] + position
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

        if (showMesh)
        {
            m.SetVertices(verts1);
            //m.SetNormals(normals);

            int[] vertConfig = new int[m.vertexCount];
            for (int i = 0; i < m.vertexCount; i++)
                vertConfig[i] = i;
            m.SetTriangles(vertConfig, 0);
            m.RecalculateNormals();

            Gizmos.DrawMesh(m, position);
        }
    }
}