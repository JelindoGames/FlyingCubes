using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCube : MonoBehaviour
{
    PlayerCubeManager playerCubeManager;
    bool attemptedMerge;
    [SerializeField] float startingDistFromPlayer;
    [SerializeField] float floatingSpeed;
    float angleToPlayer;
    bool touchingPlayer;

    void Start()
    {
        playerCubeManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeManager>();
        float startingAngle = Random.Range(0f, 6.14f);
        transform.position = playerCubeManager.transform.position + (new Vector3(Mathf.Cos(startingAngle), 0, Mathf.Sin(startingAngle)) * startingDistFromPlayer);
        angleToPlayer = startingAngle + Mathf.PI;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, playerCubeManager.transform.position.y, transform.position.z);
        transform.position += new Vector3(Mathf.Cos(angleToPlayer), 0, Mathf.Sin(angleToPlayer)) * floatingSpeed * Time.deltaTime;
        if (touchingPlayer && Input.GetKeyDown(KeyCode.Space) && !attemptedMerge)
        {
            playerCubeManager.RequestMerge(this);
            attemptedMerge = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}
