using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject wallCube;
    [SerializeField] int multiplyHoleSizeBy;
    [SerializeField] BoxCollider coll;
    [SerializeField] float fadeTime;
    [SerializeField] float maxPlayerDistanceFromHole;
    [SerializeField] float multiplyColliderSizeBy;
    [SerializeField] GameObject cautionWallCube, deleteCube;
    [SerializeField] int minWallWidth, minWallHeight;
    bool hasBeenTouched;
    WallSpawner spawner;
    List<List<bool>> myHole;
    PlayerScoreHandler playerScoreHandler;
    float holeStartingX;
    float holeStartingZ;

    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("WallSpawner").GetComponent<WallSpawner>();
        playerScoreHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScoreHandler>();
    }

    public void Assemble(List<List<bool>> hole)
    {
        myHole = new List<List<bool>>();
        for (int r = 0; r < hole.Count; r++)
        {
            List<bool> row = new List<bool>();
            for (int c = 0; c < hole[0].Count; c++)
            {
                row.Add(hole[r][c]);
            }
            myHole.Add(row);
        }
        int holeHeight = hole.Count;
        int holeWidth = hole[0].Count;
        int wallHeight = Mathf.Max(multiplyHoleSizeBy * Mathf.Max(holeHeight, holeWidth), minWallHeight);
        int wallWidth = Mathf.Max(multiplyHoleSizeBy * Mathf.Max(holeHeight, holeWidth), minWallWidth);
        coll.size = new Vector3(wallWidth * multiplyColliderSizeBy, 1, wallHeight * multiplyColliderSizeBy);
        int holeStartingWidth = Random.Range(1, wallWidth - holeWidth - 1);
        int holeStartingHeight = Random.Range(1, wallHeight - holeHeight - 1);

        float xPosLeft = -wallWidth / 2;
        float zPosUp = wallHeight / 2;

        holeStartingX = xPosLeft + holeStartingWidth;
        holeStartingZ = zPosUp - holeStartingHeight;

        for (int r = 0; r < wallHeight; r++)
        {
            for (int c = 0; c < wallWidth; c++)
            {
                GameObject cube = null;
                int holeWidthIdx = c - holeStartingWidth;
                int holeHeightIdx = r - holeStartingHeight;
                if (holeWidthIdx < 0 || holeHeightIdx < 0 || holeWidthIdx >= holeWidth || holeHeightIdx >= holeHeight)
                {
                    cube = Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube.transform.parent = transform;
                }
                else if (hole[holeHeightIdx][holeWidthIdx] == false)
                {
                    cube = Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube.transform.parent = transform;
                }
                else
                {
                    cube = Instantiate(deleteCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube.transform.parent = transform;
                }

                if (r == 0 || c == 0 || r == wallHeight - 1 || c == wallWidth - 1 && cube != null)
                {
                    GameObject cube2 = Instantiate(cautionWallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube2.transform.localPosition += new Vector3(0f, 1f, 0f);
                    cube2.transform.parent = transform;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTouched)
        {
            PlayerCubeManager playerCubeManager = other.transform.parent.parent.GetComponent<PlayerCubeManager>();
            if (PlayerMatchesWall(playerCubeManager))
            {
                StartCoroutine(FadeOut());
                spawner.SpawnNewWall();
                playerScoreHandler.Score();
            }
            else
            {
                StartCoroutine(FadeOut());
                var playerCubeGrid = playerCubeManager.GetCubeGrid();
                List<List<bool>> playerShape = new List<List<bool>>();
                for (int r = 0; r < playerCubeGrid.Count; r++)
                {
                    List<bool> row = new List<bool>();
                    for (int c = 0; c < playerCubeGrid[0].cubeRow.Count; c++)
                    {
                        row.Add(playerCubeGrid[r].cubeRow[c] != null);
                    }
                    playerShape.Add(row);
                }
                spawner.SpawnNewWall(playerShape);
                PlayerHealth playerHealth = playerCubeManager.gameObject.GetComponent<PlayerHealth>();
                playerHealth.LoseHeart();
            }
            hasBeenTouched = true;
        }
    }

    bool PlayerMatchesWall(PlayerCubeManager playerCubeManager)
    {
        var playerCubeGrid = playerCubeManager.GetCubeGrid();
        if (playerCubeGrid.Count != myHole.Count || playerCubeGrid[0].cubeRow.Count != myHole[0].Count)
        {
            return false;
        }
        for (int r = 0; r < myHole.Count; r++)
        {
            for (int c = 0; c < myHole[0].Count; c++)
            {
                bool playerIsHere = playerCubeGrid[r].cubeRow[c] != null;
                bool holeIsHere = myHole[r][c];
                if (playerIsHere != holeIsHere)
                {
                    return false;
                }
            }
        }
        Vector3 playerTopLeft = playerCubeManager.TopLeftPosition();
        float distance = Vector3.Distance(playerTopLeft, new Vector3(holeStartingX, playerTopLeft.y, holeStartingZ));
        print(distance);
        return distance < maxPlayerDistanceFromHole;
    }
        
    IEnumerator FadeOut()
    {
        MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            foreach (MeshRenderer rend in rends)
            {
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 1f - (t / fadeTime));
            }
            yield return null;
        }
        foreach (MeshRenderer rend in rends)
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 1f);
        }
        Destroy(gameObject);
    }

    void PrintHole()
    {
        string toPrint = "";
        for (int r = 0; r < myHole.Count; r++)
        {
            for (int c = 0; c < myHole[0].Count; c++)
            {
                toPrint += myHole[r][c] ? "*" : "_";
            }
            toPrint += "\n";
        }
        print(toPrint);
    }
}
