using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject rogueCube;
    [SerializeField] float maxWaitForNewSpawn;
    [SerializeField] PlayerCubeManager playerCubeManager;
    [SerializeField] float startSpawningAfter;
    GameObject currentCube;

    void Start()
    {
        Invoke("StartSpawning", startSpawningAfter);
    }

    void StartSpawning()
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
            currentCube.GetComponent<RogueCube>().Initialize(playerCubeManager.GetWidth(), playerCubeManager.GetHeight());
            yield return null;
        }
    }
}