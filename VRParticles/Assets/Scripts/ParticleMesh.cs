using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ParticleMesh : MonoBehaviour {
    public int Resolution;
    public float Size;

    public Texture2D GenerateNoise()
    {
        Texture2D noise = new Texture2D(Resolution, Resolution);
        Color[] colors = new Color[Resolution*Resolution];
        for(int i=0; i < colors.Length; i++)
        {
            colors[i] = UnityEngine.Random.ColorHSV();
        }
        noise.SetPixels(colors);
        noise.Apply();
        return noise;
    }

    //Generate a mesh for the particlez
    public void Generate()
    {
        int numParts = Resolution * Resolution;
        var mesh = new Mesh();

        var verts = new Vector3[4 * numParts];
        var uvs = new Vector3[4 * numParts];
        var triangles = new int[2 * 3 * numParts];

        for (int i = 0; i < uvs.Length; i++)
        {
            float x = Mathf.Floor(i / 4f) / Resolution;
            float y = i;
            uvs[i] = new Vector2(x,y);
        }

        for (int i = 0; i < verts.Length; i += 4)
        {
            verts[i] = new Vector3(-0.5f, 0.5f) * Size;
            verts[i+1] = new Vector3(0.5f, 0.5f) * Size;
            verts[i+2] = new Vector3(-0.5f, -0.5f) * Size;
            verts[i+3] = new Vector3(0.5f, -0.5f) * Size;
        }

        int currVerts = 0;
        for (int i=0; i<triangles.Length-5; i += 6)
        {
            //first triangle
            triangles[i] = currVerts;
            triangles[i + 1] = currVerts + 1;
            triangles[i + 2] = currVerts + 2;

            //second triangle
            triangles[i + 5] = currVerts + 1;
            triangles[i + 4] = currVerts + 2;
            triangles[i + 3] = currVerts + 3;
            currVerts += 4;
        }

        mesh.SetVertices(verts.ToList());
        mesh.SetUVs(0, uvs.ToList());
        mesh.SetTriangles(triangles.ToList(), 0);

        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void AddQuad(List<Vector3> verts, List<Vector2> uvs, List<int> triangles)
    {
        for (int i = 0; i < uvs.Length; i++)
        {
            float x = Mathf.Floor(i / 4f) / Resolution;
            float y = i;
            uvs[i] = new Vector2(x, y);
        }

        for (int i = 0; i < verts.Length; i += 4)
        {
            verts[i] = new Vector3(-0.5f, 0.5f) * Size;
            verts[i + 1] = new Vector3(0.5f, 0.5f) * Size;
            verts[i + 2] = new Vector3(-0.5f, -0.5f) * Size;
            verts[i + 3] = new Vector3(0.5f, -0.5f) * Size;
        }

        //first triangle
        triangles[i] = currVerts;
        triangles[i + 1] = currVerts + 1;
        triangles[i + 2] = currVerts + 2;

        //second triangle
        triangles[i + 5] = currVerts + 1;
        triangles[i + 4] = currVerts + 2;
        triangles[i + 3] = currVerts + 3;
        currVerts += 4;
    }



}
