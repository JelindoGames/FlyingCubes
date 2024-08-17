using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeManager : MonoBehaviour
{
    [System.Serializable]
    public class CubeRow
    {
        public List<GameObject> cubeRow;
    }

    public List<CubeRow> initialCubeGrid; // Note: Can include null cubes
    List<CubeRow> cubeGrid; // Note: Can include null cubes

    void Start()
    {
        cubeGrid = initialCubeGrid;
    }
}
