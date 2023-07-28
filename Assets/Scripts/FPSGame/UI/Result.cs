using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    //결과창 구성 : 버튼(재도전, 나가기) , 점수, 별, 기록보기
    [SerializeField] GameObject goUI;
    [SerializeField] GameObject restartBtn;
    [SerializeField] GameObject quitBtn;
    [SerializeField] GameObject uiButtonandJoystick;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] filledStarImages;
    [SerializeField] ParticleSystem[] twinklingParticles;
    [SerializeField] TextMeshProUGUI dialogText;

    PlayerControllerFPS playerControllerFPS;

    private bool resultShown = false;

    private void Start()
    {
        playerControllerFPS = GameObject.Find("Player").GetComponent<PlayerControllerFPS>();
        uiButtonandJoystick.SetActive(false);
        deactiveUI();
    }

    public void ActiveJoystick()
    {
        uiButtonandJoystick.SetActive(true);
    }

    private void Update()
    {
        if (CountManager.timeRemaining <= 0 && !resultShown)
        {
            ShowResult();
            playerControllerFPS.UnLockCursor();
            resultShown = true; // 결과창을 표시했으므로 변수를 true로 설정합니다.
        }
    }

    public void deactiveUI()
    {
        goUI.SetActive(false);
        restartBtn.SetActive(false);
        quitBtn.SetActive(false);
    }

    public void activeUI()
    {
        goUI.SetActive(true);
        restartBtn.SetActive(true);
        quitBtn.SetActive(true);
    }
    public void ShowResult()
    {
        if (playerControllerFPS != null)
        {
            playerControllerFPS.DisableMovement();
            playerControllerFPS.DisableShooting();
        }

        activeUI();

        uiButtonandJoystick.SetActive(false);

        scoreText.text = "score: " + ScoreManager.score.ToString();

        int starCount = GetStarCount(ScoreManager.score);

        SetStarImages(starCount);

        UpdateResultText(starCount);
    }

    private int GetStarCount(int score)
    {
        if (score == 0)
        {
            return 0;
        }
        else if (score <= 250)
        {
            return 1;
        }
        else if (score <= 400)
        {
            return 2;
            
        }
        else
        {
            return 3;
            
        }
    }

    private void SetStarImages(int starCount)
    {
        for (int i = 0; i < filledStarImages.Length; i++)
        {
            if (i < starCount)
            {
                filledStarImages[i].gameObject.SetActive(true);
                twinklingParticles[i].gameObject.SetActive(true);
                twinklingParticles[i].Play();
            }
            else
            {
                filledStarImages[i].gameObject.SetActive(false);
                twinklingParticles[i].Stop();
                twinklingParticles[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateResultText(int starCount)
    {
        // Set different messages based on the starCount
        if (starCount == 0)
        {
            dialogText.text += "Effort is essential! Keep trying!";
        }
        else if (starCount == 1)
        {
            dialogText.text += "Well done! You earned your first star!";
        }
        else if (starCount == 2)
        {
            dialogText.text += "Impressive play! You got 2 stars!";
        }
        else if (starCount == 3)
        {
            dialogText.text += "Flawless performance! You earned all 3 stars!";
        }
    }



    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        CountManager.timeRemaining = 30; 
        ScoreManager.score = 0;


    }
    public void OnClickQuit()
    {
        SceneManager.LoadScene("OriginScene");
    }
}
