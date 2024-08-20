using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreHandler : MonoBehaviour
{
    [SerializeField] float initialWallScore;
    [SerializeField] float multiplierPerWall;
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] AudioSource success;
    [SerializeField] AudioSource greatSuccess;
    [SerializeField] float timeForVeryGood;
    [SerializeField] TextMeshProUGUI maxScoreText;
    public int currentScore = 0;
    int wallsScored = 0;
    float timeSinceLastScore = 0;

    int highScore = -1;

    void Start()
    {
        scoreText.text = "Score: " + currentScore;
    }

    void Update()
    {
        timeSinceLastScore += Time.deltaTime;
    }

    public void Score()
    {
        if (timeSinceLastScore > timeForVeryGood)
        {
            currentScore += (int)(initialWallScore * Mathf.Pow(multiplierPerWall, wallsScored));
            success.Play();
        }
        else
        {
            currentScore += (int)(initialWallScore * Mathf.Pow(multiplierPerWall, wallsScored) * 2);
            greatSuccess.Play();
        }
        scoreText.text = "Score: " + currentScore.ToString("#,#");
        wallsScored += 1;
        timeSinceLastScore = 0;
    }

    public void GetMaxScore()
    {
        if (highScore == -1 || currentScore > highScore)
        {
            highScore = currentScore;
            maxScoreText.text = currentScore.ToString("#,#");
        }


    }
}
