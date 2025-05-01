using System.Collections;
using UnityEngine;

public class MapGenerationManager : MonoBehaviour
{
    [SerializeField] private TerrainMeshGenerator terrainGenerator;
    [SerializeField] private ProceduralTerrainTexturer textureGenerator;
    [SerializeField] private NavMeshMaker navMeshMaker;
    [SerializeField] private MeshColliderAdder meshColliderAdder;
    [SerializeField] private bool doRunTexturer = false;
    [SerializeField] private ForestGenerator treeMaker;
    private bool hasntRunTexturerYet = true;
    private int gridWidth;
    private int gridDepth;

    private void Start()
    {
        // Start terrain generation
        terrainGenerator.Run();
        // Get dimensions from the terrain generator
        gridWidth = terrainGenerator.GetNumberOfQuadsWide();
        gridDepth = terrainGenerator.GetNumberOfQuadsDeep();
        textureGenerator.Initialize(gridWidth, gridDepth); // make jack add this!
    }

    private void Update()
    {
        // Check if terrain has been generated, texturing has not yet run, and texturing is set to run
        if (terrainGenerator.RanOnce && hasntRunTexturerYet && doRunTexturer)
        {
            hasntRunTexturerYet = false;
            doRunTexturer = false;
            StartCoroutine(SetupTerrain());
        }
    }

    private IEnumerator SetupTerrain()
    {
        // Start the tree population coroutine and wait for it to finish
        yield return StartCoroutine(treeMaker.PopulateGrid(gridWidth, gridDepth));

        // After trees are populated, set the terrain object as static - required for nav mesh
        terrainGenerator.gameObject.isStatic = true;

        // Now create navigation mesh
        navMeshMaker.CreateNavMesh();

        // After nav mesh is created, add a collider and paint texture
        meshColliderAdder.AddCollider();
        textureGenerator.PaintTexture();
    }
}
