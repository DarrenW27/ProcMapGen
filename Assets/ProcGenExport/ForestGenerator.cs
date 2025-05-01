using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private List<ForestLayerParameters> forestGenLayers;
    [SerializeField] private float density = .005f;
    private Mesh mesh;

    private Vector3[,] vertices;

    [System.Serializable]
    private class ForestLayerParameters
    {
        public float frequency;
        public float amplitude;
    }

    public IEnumerator PopulateGrid(int width, int depth)
    {
        mesh = meshFilter.mesh;

        vertices = new Vector3[width, depth];
        Vector3[] tempVertices = mesh.vertices;

        int count = 0;

        List<Vector3> treeCoords = new List<Vector3>();

        for(int x = 0;x < vertices.GetLength(0); x++)
        {
            for(int z = 0;z < vertices.GetLength(1); z++)
            {
                vertices[x, z] = tempVertices[count];
                float perlinX = tempVertices[count].x;
                float perlinZ = tempVertices[count].z;

                foreach(ForestLayerParameters para in forestGenLayers)
                {
                    if (Mathf.PerlinNoise(perlinX * para.frequency / width, perlinZ * para.frequency/ depth) > .5f)
                    {
                        treeCoords.Add(tempVertices[count]);
                        //if(count % 15 == 0)
                        //    Instantiate(objectToSpawn, tempVertices[count], Quaternion.identity);
                    }
                }

                if (Mathf.PerlinNoise((perlinX)/width, perlinZ/depth) > .5f)
                {
                    treeCoords.Add(tempVertices[count]);
                    //if(count % 15 == 0)
                    //    Instantiate(objectToSpawn, tempVertices[count], Quaternion.identity);
                }
                count++;
            }
        }

        foreach (Vector3 v in treeCoords)
        {
            // 10% chance to instantiate the object
            if (UnityEngine.Random.value < density)
            {
                Instantiate(objectToSpawn, v, Quaternion.identity);
                yield return new WaitForSeconds(.00001f);
            }
        }
    }
}
