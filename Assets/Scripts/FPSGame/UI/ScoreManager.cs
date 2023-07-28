using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;


    private void Start()
    {
        UpdateScoreText();
    }
    public static void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
        UpdateScoreText();
    }

    private static void UpdateScoreText()
    {
        // ScoreText ������Ʈ�� ã�Ƽ� TextMeshProUGUI ������Ʈ�� text�� ������Ʈ�մϴ�.
        GameObject scoreTextObject = GameObject.FindWithTag("ScoreText");
        if (scoreTextObject != null)
        {
            TextMeshProUGUI scoreTextComponent = scoreTextObject.GetComponent<TextMeshProUGUI>();
            if (scoreTextComponent != null)
            {
                scoreTextComponent.text = "Score: " + score;
            }
        }
    }
}