using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralTerrainTexturer : MonoBehaviour
{
    [SerializeField] private int verticesWidth = 11;
    [SerializeField] private int verticesDepth = 11; 
    [SerializeField] private int gridTilesWidth = 11;
    [SerializeField] private int gridTilesDepth = 11;
    [SerializeField] private int dryGrassPatches = 10;
    [SerializeField] private float dryGrassPatchRadius = 1f;
    [SerializeField] private float timeDelay = .05f;
    Vector3[] vertices;
    Mesh mesh;
    Vector2[] uvs;

    public void Initialize(int gridWidth, int gridDepth)
    {
        gridTilesWidth = gridWidth;
        gridTilesDepth = gridDepth;
        verticesWidth = gridTilesWidth + 1;
        verticesDepth = gridTilesDepth + 1;
    }

    public void PaintTexture()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        uvs = new Vector2[vertices.Length];
        // PaintVertices(); // old code is here
        NewPaintVertices();
    }

    private void NewPaintVertices()
    {
        Vector2[] finalizedUvs = new Vector2[verticesWidth * verticesDepth];

        for (int z = 0; z < verticesDepth; z++)
        {
            for (int x = 0; x < verticesWidth; x++)
            {
                // Calculate UV coordinates so they are evenly distributed
                float u = (float)x / (verticesWidth - 1);  // Subtract 1 to ensure the last vertex maps to 1.0
                float v = (float)z / (verticesDepth - 1);  // Subtract 1 to ensure the last vertex maps to 1.0

                finalizedUvs[x + (z * verticesWidth)] = new Vector2(u, v);
            }
        }

        mesh.uv = finalizedUvs;
    }

    //private void NewPaintVertices()
    //{
    //    Vector2[] finalizedUvs = new Vector2[verticesWidth * verticesDepth];

    //    for (int z = 0; z < verticesDepth; z++)
    //    {
    //        for (int x = 0; x < verticesWidth; x++)
    //        {
    //            finalizedUvs[x + (z * verticesWidth)] = new Vector2(x/verticesWidth, z/verticesDepth);
    //        }
    //    }

    //    mesh.uv = finalizedUvs;
    //}

    //private void NewPaintVertices()
    //{
    //    Vector2[] finalizedUvs = new Vector2[verticesWidth * verticesDepth];
    //    int count = 0;

    //    // create the uvs
    //    Vector2[,] UV_Grid = new Vector2[verticesWidth, verticesDepth];

    //    // hook them up
    //    //for(int x = 0; x < UV_Grid.GetLength(0); x++)
    //    //{
    //    //    for(int z = 0; z < UV_Grid.GetLength(1); z++)
    //    //    {
    //    //        finalizedUvs[count] = new Vector2(.75f, .25f);
    //    //        count++;
    //    //    }
    //    //}

    //    for (int z = 0; z < verticesDepth; z += 2)
    //    {
    //        for (int x = 0; x < verticesWidth; x += 2)
    //        {
    //            finalizedUvs[x + (z * verticesWidth)] = new Vector2(0, 0);
    //        }
    //        for (int x = 1; x < verticesWidth; x += 2)
    //        {
    //            finalizedUvs[x + (z * verticesWidth)] = new Vector2(1, 0);
    //        }
    //    }

    //    for (int z = 1; z < verticesDepth; z += 2)
    //    {
    //        for (int x = 0; x < verticesWidth; x += 2)
    //        {
    //            finalizedUvs[x + (z * verticesWidth)] = new Vector2(0, 1);
    //        }
    //        for (int x = 1; x < verticesWidth; x += 2)
    //        {
    //            finalizedUvs[x + (z * verticesWidth)] = new Vector2(1, 1);
    //        }
    //    }

    //    mesh.uv = finalizedUvs;
    //}

    //private void NewPaintVertices()
    //{
    //    int horizontalDivisions = 10;
    //    int verticalDivisions = horizontalDivisions;
    //    if (horizontalDivisions < 1 || verticalDivisions < 1)
    //    {
    //        throw new ArgumentException("Divisions must be greater than 0.");
    //    }

    //    Vector2[] finalizedUvs = new Vector2[verticesWidth * verticesDepth];

    //    for (int z = 0; z < verticesDepth; z++)
    //    {
    //        for (int x = 0; x < verticesWidth; x++)
    //        {
    //            float u = (x % horizontalDivisions) / (float)horizontalDivisions;
    //            float v = (z % verticalDivisions) / (float)verticalDivisions;

    //            if ((x / horizontalDivisions) % 2 == 1)
    //            {
    //                u = 1 - u;
    //            }
    //            if ((z / verticalDivisions) % 2 == 1)
    //            {
    //                v = 1 - v;
    //            }

    //            finalizedUvs[x + z * verticesWidth] = new Vector2(u, v);
    //        }
    //    }

    //    mesh.uv = finalizedUvs;
    //}

    //private void NewPaintVertices()
    //{
    //    int horizontalDivisions = 10; // Number of times the texture repeats horizontally per quad
    //    int verticalDivisions = horizontalDivisions; // Number of times the texture repeats vertically per quad

    //    if (horizontalDivisions < 1 || verticalDivisions < 1)
    //    {
    //        throw new ArgumentException("Divisions must be greater than 0.");
    //    }

    //    Vector2[] finalizedUvs = new Vector2[verticesWidth * verticesDepth];

    //    // Calculate UV coordinates by continuously incrementing them
    //    for (int z = 0; z < verticesDepth; z++)
    //    {
    //        for (int x = 0; x < verticesWidth; x++)
    //        {
    //            // Increment u based on the total count of vertices across and wrap around using the modulo operator
    //            float u = (x / (float)horizontalDivisions) % (verticesWidth / horizontalDivisions);
    //            float v = (z / (float)verticalDivisions) % (verticesDepth / verticalDivisions);

    //            finalizedUvs[x + z * verticesWidth] = new Vector2(u, v);
    //        }
    //    }

    //    mesh.uv = finalizedUvs;
    //}


    private void PaintVertices()
    {
        for(int i = 0;i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(.25f, .75f);
        }

        int[] dryGrassVertices = GenerateRandomNumbers(0, verticesWidth * verticesDepth, dryGrassPatches);
        for(int i = 0; i < dryGrassVertices.Length; i++)
        {
            List<int> dryGrassPatchVertices = GetGrassPatchVerticesFromCenter(dryGrassVertices[i], dryGrassPatchRadius);
            for(int j = 0;j < dryGrassPatchVertices.Count; j++)
            {
                uvs[dryGrassPatchVertices[j]] = new Vector2(.75f, .75f);
            }
        }

        mesh.uv = uvs;
        Debug.Log("done");
    }

    private List<int> GetGrassPatchVerticesFromCenter(int centerVertexIndex, float patchRadius)
    {
        Vector3 centerVertexPosition = vertices[centerVertexIndex];
        List<int> returnVertices = new List<int>();
        for(int i = 0;i < vertices.Length; i++)
        {
            if (Vector3.Distance(vertices[i], centerVertexPosition) <= patchRadius)
                returnVertices.Add(i);
        }
        return returnVertices;
    }

    // Function to generate an array of random numbers
    public int[] GenerateRandomNumbers(int minValue, int maxValue, int count)
    {
        int[] numbers = new int[count];
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            numbers[i] = random.Next(minValue, maxValue + 1);
        }

        return numbers;
    }

}
