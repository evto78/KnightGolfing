using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public uint gridSize;
    public uint gridDistance;
    public uint dimensions;
    public Vector3[,,] gridPoints;
        

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Generate();
        }
    }

    void Generate()
    {
        gridPoints = new Vector3[gridSize,gridSize,gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    gridPoints[i, j, k] = new Vector3(i * gridDistance, j * gridDistance, k * gridDistance);
                    Debug.Log(gridPoints[i, j, k]);
                }
            }
        }
    }
}
