using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] List<GameObject> hearts;

    public void LoseHeart()
    {
        Destroy(hearts[hearts.Count - 1].gameObject);
        hearts.RemoveAt(hearts.Count - 1);
        if (hearts.Count == 0)
        {
            print("You lose");
        }
    }
}
