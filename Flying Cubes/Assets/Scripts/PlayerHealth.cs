using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] List<GameObject> hearts;
    [SerializeField] UnityEvent onLose;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource deathSound;

    public void LoseHeart()
    {
        Destroy(hearts[hearts.Count - 1].gameObject);
        hearts.RemoveAt(hearts.Count - 1);
        hitSound.Play();
        if (hearts.Count == 0)
        {
            onLose.Invoke();
            deathSound.Play();
        }
    }

    void Update()
    {
        if (hearts.Count == 0 && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void KillAllInnerParticles()
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag("InnerParticles");

        foreach (GameObject g in particles)
            Destroy(g);
    }
}
