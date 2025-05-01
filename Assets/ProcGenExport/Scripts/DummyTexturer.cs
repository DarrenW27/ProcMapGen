using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DummyTexturer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private const int width = 10;
    private const int depth = 10;
    private void Start()
    {
        mesh = meshFilter.mesh;
        vertices = mesh.vertices;
        uvs = mesh.uv;

        GridVerticesNavigator.InitializeGridWidthAndDepth(10, 10);

        // switch to primary
        // go through them all
        // detect when we reach the end
        // reset primary
        // switch to secondary list
        // go through them all
        // detect when we reach the end
        // reset secondary
        // switch to tertiary list
        // go through them all

        Vector2[,] newUVs = new Vector2[width, depth];

        Revolver primaryRevolver = new Revolver();
        primaryRevolver.contents = new List<Vector2>
        {
            new Vector2(0, .5f),
            new Vector2(.25f, .5f),
            new Vector2(.5f, .5f)
        };

        Revolver secondaryRevolver = new Revolver();
        secondaryRevolver.contents = new List<Vector2>
        {
            new Vector2(0, .75f),
            new Vector2(.25f, .75f),
            new Vector2(.5f, .75f)
        };

        Revolver tertiaryRevolver = new Revolver();
        tertiaryRevolver.contents = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(.25f, 1),
            new Vector2(.5f, 1)
        };

        // do a loop to hook up primaries
        for (int z = 0;z < depth; z += 3)
        {
            for(int x = 0;x < width; x++)
            {
                newUVs[x,z] = primaryRevolver.GetRoundAndCycleChamber();
            }
            primaryRevolver.GoBackToFirstRound();
        }
        // do a loop to hook up secondaries
        for (int z = 1; z < depth; z += 3)
        {
            for (int x = 0; x < width; x++)
            {
                newUVs[x, z] = secondaryRevolver.GetRoundAndCycleChamber();
            }
            secondaryRevolver.GoBackToFirstRound();
        }
        // do a loop to hook up tertiaries
        for (int z = 2; z < depth; z += 3)
        {
            for (int x = 0; x < width; x++)
            {
                newUVs[x, z] = tertiaryRevolver.GetRoundAndCycleChamber();
            }
            tertiaryRevolver.GoBackToFirstRound();
        }

        int count = 0;

        for (int z = 0; z < depth; z += 3)
        {
            for (int x = 0; x < width; x++)
            {
                uvs[count] = newUVs[x,z];
                count++;
            }
        }

        mesh.uv = uvs;
        meshFilter.mesh = mesh;
    }

    private class Revolver
    {
        public List<Vector2> contents { get; set; }
        private int currentRound = 0;
        public Vector2 GetRoundAndCycleChamber()
        {
            if (currentRound >= contents.Count)
                GoBackToFirstRound();
            Vector2 returnRound = contents[currentRound];
            currentRound++;
            return returnRound;
        }
        public void GoBackToFirstRound()
        {
            currentRound = 0;
        }
    }
}
