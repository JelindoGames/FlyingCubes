using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject wallCube;
    [SerializeField] int multiplyHoleSizeBy;
    [SerializeField] BoxCollider coll;
    [SerializeField] float fadeTime;
    bool hasBeenTouched;
    WallSpawner spawner;
    List<List<bool>> myHole;

    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("WallSpawner").GetComponent<WallSpawner>();
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
        int wallHeight = multiplyHoleSizeBy * holeHeight;
        int wallWidth = multiplyHoleSizeBy * holeWidth;
        coll.size = new Vector3(wallWidth, 1, wallHeight);
        int holeStartingWidth = Random.Range(1, wallWidth - holeWidth - 1);
        int holeStartingHeight = Random.Range(1, wallHeight - holeHeight - 1);

        float xPosLeft = -wallWidth / 2;
        float zPosUp = wallHeight / 2;
        for (int r = 0; r < wallHeight; r++)
        {
            for (int c = 0; c < wallWidth; c++)
            {
                int holeWidthIdx = c - holeStartingWidth;
                int holeHeightIdx = r - holeStartingHeight;
                if (holeWidthIdx < 0 || holeHeightIdx < 0 || holeWidthIdx >= holeWidth || holeHeightIdx >= holeHeight)
                {
                    GameObject cube = Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube.transform.parent = transform;
                }
                else if (hole[holeHeightIdx][holeWidthIdx] == false)
                {
                    GameObject cube = Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                    cube.transform.parent = transform;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTouched)
        {
            if (PlayerMatchesWall(other.transform.parent.GetComponent<PlayerCubeManager>()))
            {
                StartCoroutine(FadeOut());
                spawner.SpawnNewWall();
            }
            else
            {
                print("You lost");
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
        return true;
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
