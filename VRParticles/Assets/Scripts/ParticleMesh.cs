using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ParticleMesh : MonoBehaviour {
    public int Resolution;
    public float Size;
    public List<MeshFilter> ParticleMeshFilters;
    private const int UNITY_VERTEX_LIMIT = 65534;

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

    //Generate a mesh for the particles
    public void Generate()
    {
        int numParts = Resolution * Resolution;
        var meshes = new List<Mesh>();

        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();

        for (int i = 0; i<numParts; ++i) {
            //If our next quad will push us over the unity limit
            if (verts.Count >= UNITY_VERTEX_LIMIT - 4)
            {
                //create the mesh and clear the lists
                CreateMesh(verts, uvs, triangles);
            }
            AddQuad(verts, uvs, triangles, i);
        }

        CreateMesh(verts, uvs, triangles);
    }

    //creates a mesh out of verts, uvs and triangles
    //then clears those lists
    public MeshFilter CreateMesh(List<Vector3> verts, List<Vector2> uvs, List<int> triangles)
    {
        var mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        verts.Clear();
        uvs.Clear();
        triangles.Clear();

        GameObject meshObj = new GameObject();
        meshObj.name = "ParticleMesh";
        var renderer = meshObj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial; //copy mesh renderer
        var filter = meshObj.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        meshObj.transform.SetParent(transform, false);

        return filter;
    }

    public void AddQuad(List<Vector3> verts, List<Vector2> uvs, List<int> triangles, int particleIndex)
    {
        int numParticles = (Resolution * Resolution);
        int currentVerts = verts.Count;
        int currentTris = triangles.Count;

        for (int i = 0; i < 4; i++)
        {
            var globalIndex = particleIndex + i;
            float x = (float)particleIndex / numParticles; //x is a per particle/quad identifier
            float y = globalIndex / numParticles; //y is a per vertex identifier
            uvs.Add(new Vector2(x, y));
        }

        verts.Add(new Vector3(-0.5f, 0.5f) * Size); //top left
        verts.Add(new Vector3(0.5f, 0.5f) * Size); //top right
        verts.Add(new Vector3(-0.5f, -0.5f) * Size); //bottom left
        verts.Add(new Vector3(0.5f, -0.5f) * Size); // bottom right

        //first triangle
        triangles.Add(currentVerts);
        triangles.Add(currentVerts + 1);
        triangles.Add(currentVerts + 2);

        //second triangle
        triangles.Add(currentVerts + 3);
        triangles.Add(currentVerts + 2);
        triangles.Add(currentVerts + 1);
    }



}
