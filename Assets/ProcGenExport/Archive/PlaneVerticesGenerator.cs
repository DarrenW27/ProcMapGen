using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaneVerticesGenerator : MonoBehaviour
{
    // we need variables for the width and height
    // going to generate all of those
    // we will do the bumpiness in a different script

    [SerializeField] private int planeWidth = 256;
    [SerializeField] private int planeLength = 256;
    [SerializeField] private float vertexSpacing = 1;

    private Vector3[,] vertices;

    private void Start()
    {
        vertices = new Vector3[planeWidth, planeLength];

        for (int i = 0;i < planeWidth; i++)
        {
            for(int j = 0;j < planeLength; j++)
            {
                vertices[i, j] = new Vector3(i, 0, j) * vertexSpacing;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //        return;

    //    for(int i = 0;i < planeWidth; i++)
    //    {
    //        for(int j = 0;j < planeLength; j++)
    //        {
    //            Gizmos.DrawSphere(vertices[i, j], .1f);
    //        }
    //    }
    //}
}
