using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] float startingYForWall;
    [SerializeField] float yDistanceForWalls;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] int minNewBlocksPerWall;
    [SerializeField] int maxNewBlocksPerWall;
    public List<List<bool>> currentHole;
    float currentY;

    void Start()
    {
        currentY = startingYForWall;
        currentHole = new List<List<bool>>() { new List<bool>() { true } };
        SpawnNewWall();
    }

    public void SpawnNewWall(List<List<bool>> basedOn = null)
    {
        if (basedOn != null)
        {
            currentHole = basedOn;
            EnlargeHole();
        }
        GameObject wall = Instantiate(wallPrefab, Vector3.up * currentY, Quaternion.identity);
        wall.GetComponent<Wall>().Assemble(currentHole);
        currentY -= yDistanceForWalls;
        EnlargeHole();
    }

    void EnlargeHole()
    {
        int newBlocks = Random.Range(minNewBlocksPerWall, maxNewBlocksPerWall + 1);
        for (int i = 0; i < newBlocks; i++)
        {
            AddBlockToHole();
        }
    }

    void AddBlockToHole()
    {
        bool workInside = Random.Range(0f, 1f) < 0.5f;
        if (workInside)
        {
            bool success = ExpandHoleInside();
            if (success)
            {
                return;
            }
        }
        // If workInside was false, OR couldn't find an available interior block... work outside
        float direction = Random.Range(0f, 1f);
        if (direction < 0.25f)
        {
            ExpandHoleLeft();
        }
        else if (direction < 0.5f)
        {
            ExpandHoleRight();
        }
        else if (direction < 0.75f)
        {
            ExpandHoleUp();
        }
        else
        {
            ExpandHoleDown();
        }
    }

    bool ExpandHoleInside()
    {
        List<Vector2Int> availableSpots = new List<Vector2Int>();
        for (int r = 0; r < currentHole.Count; r++)
        {
            for (int c = 0; c < currentHole[0].Count; c++)
            {
                if (currentHole[r][c] == false && IsAdjacentToHoleBlock(r, c))
                {
                    availableSpots.Add(new Vector2Int(r, c));
                }
            }
        }
        if (availableSpots.Count > 0)
        {
            Vector2Int chosenSpot = availableSpots[Random.Range(0, availableSpots.Count)];
            currentHole[chosenSpot.x][chosenSpot.y] = true;
            return true;
        }
        return false;
    }

    void ExpandHoleLeft()
    {
        for (int r = 0; r < currentHole.Count; r++)
        {
            currentHole[r].Insert(0, false);
        }
        List<int> validRowsForNewHole = new List<int>();
        for (int r = 0; r < currentHole.Count; r++)
        {
            if (currentHole[r][1] == true)
            {
                validRowsForNewHole.Add(r);
            }
        }
        int chosenIdx = Random.Range(0, validRowsForNewHole.Count);
        currentHole[validRowsForNewHole[chosenIdx]][0] = true;
    }

    void ExpandHoleRight()
    {
        for (int r = 0; r < currentHole.Count; r++)
        {
            currentHole[r].Add(false);
        }
        List<int> validRowsForNewHole = new List<int>();
        for (int r = 0; r < currentHole.Count; r++)
        {
            if (currentHole[r][currentHole[r].Count - 2] == true)
            {
                validRowsForNewHole.Add(r);
            }
        }
        int chosenIdx = Random.Range(0, validRowsForNewHole.Count);
        currentHole[validRowsForNewHole[chosenIdx]][currentHole[0].Count - 1] = true;
    }

    void ExpandHoleUp()
    {
        currentHole.Insert(0, new List<bool>());
        for (int c = 0; c < currentHole[1].Count; c++)
        {
            currentHole[0].Add(false);
        }
        List<int> validColsForNewHole = new List<int>();
        for (int c = 0; c < currentHole[1].Count; c++)
        {
            if (currentHole[1][c] == true)
            {
                validColsForNewHole.Add(c);
            }
        }
        int chosenIdx = Random.Range(0, validColsForNewHole.Count);
        currentHole[0][validColsForNewHole[chosenIdx]] = true;
    }

    void ExpandHoleDown()
    {
        currentHole.Add(new List<bool>());
        for (int c = 0; c < currentHole[0].Count; c++)
        {
            currentHole[currentHole.Count - 1].Add(false);
        }
        List<int> validColsForNewHole = new List<int>();
        for (int c = 0; c < currentHole[1].Count; c++)
        {
            if (currentHole[currentHole.Count - 2][c] == true)
            {
                validColsForNewHole.Add(c);
            }
        }
        int chosenIdx = Random.Range(0, validColsForNewHole.Count);
        currentHole[currentHole.Count - 1][validColsForNewHole[chosenIdx]] = true;
    }

    bool IsAdjacentToHoleBlock(int r, int c)
    {
        bool leftIsValid, rightIsValid, topIsValid, bottomIsValid;
        leftIsValid = c >= 1 && currentHole[r][c - 1];
        rightIsValid = c < currentHole[0].Count - 1 && currentHole[r][c + 1];
        topIsValid = r >= 1 && currentHole[r - 1][c];
        bottomIsValid = r < currentHole.Count - 1 && currentHole[r + 1][c];
        return leftIsValid || rightIsValid || topIsValid || bottomIsValid;
    }
}
