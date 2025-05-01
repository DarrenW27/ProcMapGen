using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMaker : MonoBehaviour
{
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    private NavMeshBuildSettings buildSettings;
    [SerializeField] private bool runAutomatically = false;

    private void Start()
    {
        if (runAutomatically)
            CreateNavMesh();
    }

    public void CreateNavMesh()
    {
        buildSettings = NavMesh.GetSettingsByID(0);
        navMeshData = new NavMeshData();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);

        BuildNavMesh();
    }

    private void BuildNavMesh()
    {
        var sources = new List<NavMeshBuildSource>();
        var markups = new List<NavMeshBuildMarkup>();

        foreach(var root in FindObjectsOfType<Transform>())
        {
            var meshFilter = root.GetComponent<MeshFilter>();
            if (meshFilter)
            {
                var source = new NavMeshBuildSource
                {
                    shape = NavMeshBuildSourceShape.Mesh,
                    sourceObject = meshFilter.sharedMesh,
                    transform = meshFilter.transform.localToWorldMatrix,
                    area = 0
                };
                sources.Add(source);
            }
        }
        NavMeshBuilder.UpdateNavMeshData(navMeshData, buildSettings, sources, new Bounds(transform.position, new Vector3(500, 500, 500)));
    }

    private void OnDestroy()
    {
        navMeshDataInstance.Remove();
    }
}
