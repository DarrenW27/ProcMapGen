using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfQuadsWide = 256;               // Width of the terrain in tiles NOT VERTICES
    [SerializeField] private int numberOfQuadsDeep = 256;               // Depth of the terrain in tiles NOT VERTICES
    [SerializeField] private float buildSpeed = .01f;                   // affects how fast the mesh generates - looks cool
    [SerializeField] private List<TerrainLayerParameters> noiseLayers;  // useful for blending hills with mountains

    public int GetNumberOfQuadsWide() { return numberOfQuadsWide; }
    public int GetNumberOfQuadsDeep() { return numberOfQuadsDeep; }
    public bool RanOnce { get; set; } = false;                          // helps MapGenerationManager know when we've finished.


    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    [System.Serializable]
    private struct TerrainLayerParameters   // terrain layers will be added together
    {
        public float amplitude;             // noise height
        public float frequency;             // noise density
        public bool isSquared;              // should we square the values? leads to higher highs and lower lows
        public float offsetX;               // shift noise along x axis
        public float offsetZ;               // shift along z
        public float lazyYScaler;           // a lazy scaler to use after other calculations if we want to make final tunings to height
        public float bottomCrop;            // will 'sink' noise into the ground, leaving only the peaks of mountains and hills behind

        public TerrainLayerParameters
            (
            float amplitudeArg,
            float frequencyArg,
            bool isSquaredArg,
            float bottomCropArg,
            float offsetXArg,
            float offsetZArg,
            float lazyYScalerArg
            )
        {
            amplitude = amplitudeArg;
            frequency = frequencyArg;
            isSquared = isSquaredArg;
            bottomCrop = bottomCropArg;
            offsetX = offsetXArg;
            offsetZ = offsetZArg;
            lazyYScaler = lazyYScalerArg;
        }
    }

    public void Run()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // meshes by default can only have 65535 vertices. If we want more than that, we need to use this line of code.
        if (numberOfQuadsWide * numberOfQuadsDeep > 65534)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;


        StartCoroutine(CreateMesh());
    }

    // the only reason we're using a coroutine is to make the mesh generation look cooler - this could work as a plain private void function too
    IEnumerator CreateMesh()
    {
        // create the vertices
        vertices = new Vector3[(numberOfQuadsWide + 1) * (numberOfQuadsDeep + 1)]; // add +1 to ensure array always has one item at least

        // position the vertices
        for (int i = 0, z = 0; z <= numberOfQuadsDeep; z++)                // loop through rows
        {
            for (int x = 0; x <= numberOfQuadsWide; x++)                   // loop through collumns
            {
                float y = 0;
                for (int j = 0; j < noiseLayers.Count; j++)                // loop through the noise layers we want to apply
                {                                                          // NOW we can play with a single vertex
                    float newAmplitude = noiseLayers[j].amplitude;
                    float newFrequency = noiseLayers[j].frequency;
                    float offsetX = noiseLayers[j].offsetX;
                    float offsetZ = noiseLayers[j].offsetZ;
                    float lazyYScaler = noiseLayers[j].lazyYScaler;

                    if (noiseLayers[j].isSquared)
                        newAmplitude *= newAmplitude;

                    y += Mathf.PerlinNoise
                        (((x + offsetX) * newFrequency) / numberOfQuadsWide, ((z + offsetZ) * newFrequency) / numberOfQuadsDeep) * newAmplitude;
                        // apply offsets, then frequency, then multiply by amplitude
                        // we divide by width and depth because we need tiny fractions rather than whole numbers to get good perlin noise.

                    // lazy y scaler
                    if (lazyYScaler > 0)
                        y *= noiseLayers[j].lazyYScaler;

                    // crop bottom part of map
                    y -= noiseLayers[j].bottomCrop;

                    // can't recall why I added this, but commenting it out causes terrain to be much curvier than I'd like
                    if (y < 0)
                    {
                        y = 0;
                    }
                }
                vertices[i] = new Vector3(x, y, z); // we have now done ONE vertex.
                i++;                                // onto the next!
            }
        }

        // create triangles - map is generated in quads so the 6 represents the vertices needed to make two triangles
        triangles = new int[numberOfQuadsWide * numberOfQuadsDeep * 6];
        int vert = 0;
        int tris = 0;

        // loop through rows and collumns again to connect vertices and form triangles
        for (int z = 0; z < numberOfQuadsDeep; z++)
        {
            // define two triangles - enough to form a quad
            for (int x = 0; x < numberOfQuadsWide; x++) // loop through collumns
            {
                // quad triangles

                // first triangle
                triangles[tris + 0] = vert + 0;                         // bottom left of quad
                triangles[tris + 1] = vert + numberOfQuadsWide + 1;     // top left of quad
                triangles[tris + 2] = vert + 1;                         // bottom right of quad
                                                                        // these three points form the first triangle

                // second triangle
                triangles[tris + 3] = vert + 1;                         // bottom right
                triangles[tris + 4] = vert + numberOfQuadsWide + 1;     // top left
                triangles[tris + 5] = vert + numberOfQuadsWide + 2;     // top right
                                                                        // these three points form the second triangle

                                                                        // these triangles together form a quad


                // note: adding (numberOfQuadsWide + 1) is done to access vertices in the next row

                // all vertices are positioned from left to right, and bottom to top
                // vert++ moves us over to define the next quad, which is anchored by bottom left corner
                vert++;
                tris += 6;
            }
            vert++;
            yield return new WaitForSeconds(buildSpeed); // this is just done to make it look cooler, rendering mesh one row at a time
            UpdateMesh();
        }
        // move update mesh here if you no longer want cool coroutine rendering - gives better performance
        RanOnce = true;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
