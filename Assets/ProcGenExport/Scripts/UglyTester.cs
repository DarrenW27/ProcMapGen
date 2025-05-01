using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UglyTester : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Vector3 coordinate;
    [SerializeField] private float radius;
    private Vector3 oldCoordinate;
    private Mesh mesh;
    private Vector3[] vertices;
    private void Start()
    {
        mesh = meshFilter.mesh;
        vertices = mesh.vertices;
        oldCoordinate = coordinate;

        GridVerticesNavigator.InitializeGridWidthAndDepth(10, 10);

        BumpVertexAtCoordinate(coordinate);

    }


    private void Update()
    {
        if(coordinate != oldCoordinate)
        {
            SinkVertexAtCoordinate(oldCoordinate);
            BumpVertexAtCoordinate(coordinate);
            oldCoordinate = coordinate;
        }
    }

    private void BumpVertexAtCoordinate(Vector3 inputCoordinate)
    {
        int vertexIndex = GridVerticesNavigator.GetIndexOfHorizontallyClosestVertex(inputCoordinate);
        vertices[vertexIndex].y += 10;

        try
        {
            vertices[GridVerticesNavigator.GetIndexOfAdjacentVertexToVertex(vertexIndex, new Vector2Int(0,(int)radius))].y += 10;
        }
        catch { }

        //int v;
        //for(int i = 1;i < radius; i++)
        //{
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(0, -i));
        //        vertices[v].y += 10;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(0, i));
        //        vertices[v].y += 10;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(-i, 0));
        //        vertices[v].y += 10;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(1, 0));
        //        vertices[v].y += 10;
        //    }
        //    catch (Exception) { }
        //}

        mesh.vertices = vertices;
    }

    private void SinkVertexAtCoordinate(Vector3 inputCoordinate)
    {
        int vertexIndex = GridVerticesNavigator.GetIndexOfHorizontallyClosestVertex(inputCoordinate);
        vertices[vertexIndex].y = 0;

        try
        {
            vertices[GridVerticesNavigator.GetIndexOfAdjacentVertexToVertex(vertexIndex, new Vector2Int(0, (int)radius))].y = 0;
        }
        catch {}
        //int v;
        //for (int i = 1; i < radius; i++)
        //{
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(0, -i));
        //        vertices[v].y += 0;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(0, i));
        //        vertices[v].y += 0;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(-i, 0));
        //        vertices[v].y += 0;
        //    }
        //    catch (Exception) { }
        //    try
        //    {
        //        v = GridVerticesNavigator.GetIndexOfAdjacentVertex(vertexIndex, new Vector2Int(1, 0));
        //        vertices[v].y += 0;
        //    }
        //    catch (Exception) { }
        //}
        mesh.vertices = vertices;
    }
    // test out the vertice finder
    // give it a coordinate
}
