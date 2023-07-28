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
        // ScoreText 오브젝트를 찾아서 TextMeshProUGUI 컴포넌트의 text를 업데이트합니다.
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