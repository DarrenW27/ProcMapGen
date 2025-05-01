using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVerticesNavigator : MonoBehaviour
{
    // this code assumes that vertices have a spacing of one between them.
    private static int gridWidth;
    private static int gridDepth;


    public static void InitializeGridWidthAndDepth(int gridNumberOfTilesWide, int gridNumberOfTilesDeep)
    {
        gridWidth = gridNumberOfTilesWide;
        gridDepth = gridNumberOfTilesDeep;
    }

    public static int GetIndexOfHorizontallyClosestVertex(Vector3 horizontalPosition) // height is irrelevant
    {
        // a one tile wide grid is two vertices wide - therefore we add one to get the vertex width, and so on for depth
        int verticesWidth = gridWidth + 1;
        int verticesDepth = gridDepth + 1;

        // we need to round it off slightly to line up with vertices
        horizontalPosition.x = (int)Mathf.Round(horizontalPosition.x);
        horizontalPosition.z = (int)Mathf.Round(horizontalPosition.z);

        // move it if the location is outside the grid
        if (horizontalPosition.x > verticesWidth)
            horizontalPosition.x = verticesWidth - 1; 
        if (horizontalPosition.z > verticesDepth)
            horizontalPosition.z = verticesDepth - 1;
        if (horizontalPosition.x < 0)
            horizontalPosition.x = 0;
        if (horizontalPosition.z < 0)
            horizontalPosition.z = 0;

        // once that is done we can do basic multiplication to get ahold of the vertex index
        int vIndex = (int)(horizontalPosition.x + (horizontalPosition.z * verticesWidth));
        return vIndex;
    }

    // if we had version control I would not be keeping old code chunks like this!

    //public static int GetIndexOfAdjacentVertex(int startingIndex, Vector2Int adjacentDirection)
    //{
    //    // it is a little wasteful to be doing this again, but only slightly
    //    int verticesWidth = gridWidth + 1;
    //    int verticesDepth = gridDepth + 1;

    //    // move up or down
    //    startingIndex += adjacentDirection.y * verticesWidth;

    //    // move left or right
    //    startingIndex += adjacentDirection.x;
    //    int totalVertices = (verticesWidth * verticesDepth) - 1;

    //    if (startingIndex < 0 || startingIndex > totalVertices)
    //    {
    //        throw new System.Exception("Example Exception");
    //    }
    //    else
    //        return startingIndex;
    //}

    public static int GetIndexOfAdjacentVertexToVertex(int startingIndex, Vector2Int adjacentDirection)
    {
        int verticesWidth = gridWidth + 1;
        int verticesDepth = gridDepth + 1;

        // Calculate the original row and column based on the starting index
        int originalRow = startingIndex / verticesWidth;
        int originalColumn = startingIndex % verticesWidth;

        // Calculate the new row and column
        int newRow = originalRow + adjacentDirection.y;
        int newColumn = originalColumn + adjacentDirection.x;

        // Check if the new row or column is out of bounds
        if (newRow < 0 || newRow >= verticesDepth || newColumn < 0 || newColumn >= verticesWidth)
        {
            throw new System.Exception("Index out of bounds: The vertex is on the edge of the grid.");
        }

        // Calculate the new index
        int newStartingIndex = newRow * verticesWidth + newColumn;

        int totalVertices = (verticesWidth * verticesDepth) - 1;

        if (startingIndex < 0 || startingIndex > totalVertices)
        {
            throw new System.Exception("Example Exception");
        }

        return newStartingIndex;
    }

}
