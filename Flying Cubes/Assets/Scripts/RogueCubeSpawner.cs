using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject rogueCube;
    [SerializeField] float maxWaitForNewSpawn;
    GameObject currentCube;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            for (float t = 0; t < maxWaitForNewSpawn; t += Time.deltaTime)
            {
                if (currentCube == null) break;
                yield return null;
            }
            currentCube = Instantiate(rogueCube);
            yield return null;
        }
    }
}