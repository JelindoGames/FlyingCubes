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
    List<CubeRow> cubeGrid; // Note: Can include null cubes, always same width and height
    List<Vector2Int> placements = new List<Vector2Int>();
    int centerRow = 0;
    int centerCol = 0;
    [SerializeField] float cubeSize;
    [SerializeField] GameObject playerCubePrefab;
    [SerializeField] Transform cubeSpawnTransform;
    [SerializeField] float cubeClickAnimationLength;
    [SerializeField] float cubeClickAnimationScaleFactor;
    [SerializeField] AudioSource buildSound;

    void Start()
    {
        CubeRow cubeRow = new CubeRow();
        cubeRow.cubeRow = new List<GameObject>() { centerCube };
        cubeGrid = new List<CubeRow>() { cubeRow };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Undo();
        }
    }

    void Undo()
    {
        if (placements.Count == 0)
        {
            return;
        }
        Vector2Int relativeToCenter = placements[placements.Count - 1];
        int r = centerRow + relativeToCenter.y;
        int c = centerCol + relativeToCenter.x;
        Destroy(cubeGrid[r].cubeRow[c]);
        cubeGrid[r].cubeRow[c] = null;
        // Check if row empty
        bool clearRow = true;
        for (int i = 0; i < cubeGrid[0].cubeRow.Count; i++)
        {
            if (cubeGrid[r].cubeRow[i] != null)
            {
                clearRow = false;
            }
        }
        bool clearColumn = true;
        for (int i = 0; i < cubeGrid.Count; i++)
        {
            if (cubeGrid[i].cubeRow[c] != null)
            {
                clearColumn = false;
            }
        }
        if (clearRow)
        {
            cubeGrid.RemoveAt(r);
            if (centerRow > r)
            {
                centerRow--;
            }
        }
        if (clearColumn)
        {
            for (int row = 0; row < cubeGrid.Count; row++)
            {
                cubeGrid[row].cubeRow.RemoveAt(c);
            }
            if (centerCol > c)
            {
                centerCol--;
            }
        }
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
        GameObject newCube = Instantiate(playerCubePrefab, cubeSpawnTransform);
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
        placements.Add(new Vector2Int(colsFromCenter, rowsFromCenter));
        newCube.transform.position = centerCube.transform.position + (new Vector3(colsFromCenter, 0, -rowsFromCenter) * cubeSize);
        StartCoroutine(CubeClickAnimation());
        buildSound.Play();
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

    public List<CubeRow> GetCubeGrid()
    {
        return cubeGrid;
    }

    public IEnumerator CubeClickAnimation()
    {
        for (float t = 0; t < cubeClickAnimationLength; t += Time.deltaTime)
        {
            yield return null;
            float factor = Mathf.Lerp(cubeClickAnimationScaleFactor, 1, t / cubeClickAnimationLength);
            cubeSpawnTransform.localScale = new Vector3(1, 1, 1) * factor;
        }
        cubeSpawnTransform.localScale = new Vector3(1, 1, 1);
    }

    public Vector3 TopLeftPosition()
    {
        return transform.position + (new Vector3(-centerCol, 0, centerRow) * cubeSize);
    }

    public float GetWidth()
    {
        return cubeGrid[0].cubeRow.Count * cubeSize;
    }

    public float GetHeight()
    {
        return cubeGrid.Count * cubeSize;
    }
}
