using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCube : MonoBehaviour
{
    PlayerCubeManager playerCubeManager;
    bool attemptedMerge;

    void Start()
    {
        playerCubeManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //print("Colliding with player");
            if (Input.GetKey(KeyCode.Space) && !attemptedMerge)
            {
                playerCubeManager.RequestMerge(this);
                attemptedMerge = true;
            }
        }
    }
}
