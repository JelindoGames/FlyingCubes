using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfter : MonoBehaviour
{
    [SerializeField] GameObject toSetActive;
    [SerializeField] float setActiveAfter;

    void Start()
    {
        toSetActive.SetActive(false);
        Invoke("Active", setActiveAfter);
    }

    void Active()
    {
        toSetActive.SetActive(true);
    }
}
