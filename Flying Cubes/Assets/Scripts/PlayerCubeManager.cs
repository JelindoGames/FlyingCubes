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

    [SerializeField] GameObject centerCube;
    [SerializeField] List<CubeRow> cubeGrid; // Note: Can include null cubes, always same width and height
    int centerRow = 0;
    int centerCol = 0;

    [SerializeField] float cubeSize;
    [SerializeField] GameObject playerCubePrefab;

    void Start()
    {
        CubeRow cubeRow = new CubeRow();
        cubeRow.cubeRow = new List<GameObject>() { centerCube };
        cubeGrid = new List<CubeRow>() { cubeRow };
    }

    public void RequestMerge(RogueCube rogueCube)
    {
        // Step 1: Find which cube the new one is closest to
        float minDistance = float.MaxValue;
        GameObject closestCube = null;
        int closestCubeRow = -1;
        int closestCubeCol = -1;
        for (int r = 0; r < cubeGrid.Count; r++)
        {
            for (int c = 0; c < cubeGrid[r].cubeRow.Count; c++)
            {
                if (cubeGrid[r].cubeRow[c] == null)
                {
                    continue;
                }
                float dist = Vector3.Distance(cubeGrid[r].cubeRow[c].transform.position, rogueCube.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestCube = cubeGrid[r].cubeRow[c];
                    closestCubeCol = c;
                    closestCubeRow = r;
                }
            }
        }
        // Step 2: Find out whether it's above, to the left, etc...
        Vector2 displacement = new Vector2(rogueCube.transform.position.x - closestCube.transform.position.x, rogueCube.transform.position.z - closestCube.transform.position.z);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        int desiredRow, desiredCol;
        if (angle >= 45 && angle < 135) // Above
        {
            desiredRow = closestCubeRow - 1;
            desiredCol = closestCubeCol;
        }
        else if (angle >= 135 || angle < -135) // Left
        {
            desiredRow = closestCubeRow;
            desiredCol = closestCubeCol - 1;
        }
        else if (angle >= -135 && angle < -45) // Below
        {
            desiredRow = closestCubeRow + 1;
            desiredCol = closestCubeCol;
        }
        else // Right
        {
            desiredRow = closestCubeRow;
            desiredCol = closestCubeCol + 1;
        }
        // Step 3: Check if the desired spot is available
        GameObject newCube = Instantiate(playerCubePrefab, transform);
        if (desiredRow == -1)
        {
            AddEmptyTopRow();
            desiredRow = 0;
        }
        else if (desiredRow == cubeGrid.Count)
        {
            AddEmptyBottomRow();
        }
        else if (desiredCol == -1)
        {
            AddEmptyLeftCol();
            desiredCol = 0;
        }
        else if (desiredCol == cubeGrid[desiredRow].cubeRow.Count)
        {
            AddEmptyRightCol();
        }

        if (cubeGrid[desiredRow].cubeRow[desiredCol] == null)
        {
            cubeGrid[desiredRow].cubeRow[desiredCol] = newCube;
        }
        else
        {
            Debug.Log("INVALID");
            Destroy(newCube);
            return;
        }
        int colsFromCenter = desiredCol - centerCol;
        int rowsFromCenter = desiredRow - centerRow;
        newCube.transform.position = centerCube.transform.position + (new Vector3(colsFromCenter, 0, -rowsFromCenter) * cubeSize);
        Destroy(rogueCube.gameObject);
    }

    void AddEmptyTopRow()
    {
        CubeRow newRow = new CubeRow();
        newRow.cubeRow = new List<GameObject>();
        for (int i = 0; i < cubeGrid[0].cubeRow.Count; i++)
        {
            newRow.cubeRow.Add(null);
        }
        cubeGrid.Insert(0, newRow);
        centerRow += 1;
    }

    void AddEmptyBottomRow()
    {
        CubeRow newRow = new CubeRow();
        newRow.cubeRow = new List<GameObject>();
        for (int i = 0; i < cubeGrid[0].cubeRow.Count; i++)
        {
            newRow.cubeRow.Add(null);
        }
        cubeGrid.Add(newRow);
    }

    void AddEmptyLeftCol()
    {
        foreach (CubeRow cubeRow in cubeGrid)
        {
            cubeRow.cubeRow.Insert(0, null);
        }
        centerCol += 1;
    }

    void AddEmptyRightCol()
    {
        foreach (CubeRow cubeRow in cubeGrid)
        {
            cubeRow.cubeRow.Add(null);
        }
    }
}
