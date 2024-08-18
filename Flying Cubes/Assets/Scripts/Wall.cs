using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject wallCube;
    [SerializeField] int multiplyHoleSizeBy;
    [SerializeField] BoxCollider coll;

    public void Assemble(List<List<bool>> hole)
    {
        int holeHeight = hole.Count;
        int holeWidth = hole[0].Count;
        int wallHeight = multiplyHoleSizeBy * holeHeight;
        int wallWidth = multiplyHoleSizeBy * holeWidth;
        coll.size = new Vector3(wallWidth, 1, wallHeight);
        int holeStartingWidth = Random.Range(0, wallWidth - holeWidth);
        int holeStartingHeight = Random.Range(0, wallHeight - holeHeight);

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
                    Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                }
                else if (hole[holeHeightIdx][holeWidthIdx] == false)
                {
                    Instantiate(wallCube, new Vector3(xPosLeft + c, transform.position.y, zPosUp - r), Quaternion.identity);
                }
            }
        }
        /*
        string toPrint = "";
        foreach (List<bool> row in hole)
        {
            foreach (bool spot in row)
            {
                toPrint += spot ? "*" : " ";
            }
            toPrint += "\n";
        }
        Debug.Log(toPrint);
        */
    }
}
