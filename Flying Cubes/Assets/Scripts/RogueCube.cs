using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCube : MonoBehaviour
{
    PlayerCubeManager playerCubeManager;
    bool disallowMerge;
    [SerializeField] float startingDistFromPlayer;
    [SerializeField] float floatingSpeed;
    float angleToPlayer;
    bool touchingPlayer;
    float timeUntilNotTouchingPlayer = 0;

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
        if (touchingPlayer && Input.GetKeyDown(KeyCode.Space) && !disallowMerge)
        {
            playerCubeManager.RequestMerge(this);
            disallowMerge = true;
            Invoke("ReallowMerge", 0.2f);
        }
        if (timeUntilNotTouchingPlayer > 0)
        {
            timeUntilNotTouchingPlayer -= Time.deltaTime;
            if (timeUntilNotTouchingPlayer <= 0)
            {
                touchingPlayer = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeUntilNotTouchingPlayer = 0.1f;
            touchingPlayer = true;
        }
    }

    void ReallowMerge()
    {
        disallowMerge = false;
    }

    /*
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
    */
}
