using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI inputScore;
    [SerializeField] TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        int attempt = int.Parse((inputScore.text == "" ? "0" : inputScore.text).Replace(",", ""));
        submitScoreEvent.Invoke(inputName.text, attempt);
    }

}
