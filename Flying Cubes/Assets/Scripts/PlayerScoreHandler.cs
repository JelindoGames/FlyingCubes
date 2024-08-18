using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreHandler : MonoBehaviour
{
    [SerializeField] float initialWallScore;
    [SerializeField] float multiplierPerWall;
    [SerializeField] TextMeshPro scoreText;
    public int currentScore = 0;
    int wallsScored = 0;

    void Start()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void Score()
    {
        currentScore += (int)(initialWallScore * Mathf.Pow(multiplierPerWall, wallsScored));
        scoreText.text = "Score: " + currentScore;
        wallsScored += 1;
    }
}
