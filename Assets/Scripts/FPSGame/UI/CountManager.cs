using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CountManager : MonoBehaviour
{
    public static float timeRemaining = 60.0f;
    [SerializeField] TextMeshProUGUI countDownTextObject;
    [SerializeField] TextMeshProUGUI startCountDownText;
    [SerializeField] GameObject aimImage;
    [SerializeField] GameObject welcomeCanvas;

    private PlayerControllerFPS playerControllerFPS;
    private TargetManager targetManager;
    Result result;

    private void Awake()
    {
        aimImage.SetActive(false);
        playerControllerFPS = GameObject.Find("Player").GetComponent<PlayerControllerFPS>();
        result = GameObject.Find("Canvases/ResultCanvas").GetComponent<Result>();
        targetManager = GetComponent<TargetManager>();
    }
    private void Start()
    {
        Time.timeScale = 0;
        welcomeCanvas.SetActive(true);
        playerControllerFPS.UnLockCursor();
        playerControllerFPS.DisableShooting();

    }
    public static void AddTime(float time)
    {
        timeRemaining += time;

        Debug.Log("Score: " + timeRemaining);
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            // Format minutes and seconds as "00:00"
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            countDownTextObject.text = formattedTime;
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        countDownTextObject.text = "Time's Up!";
    }
    public void GameStartButton()
    {
        result.ActiveJoystick();
        Time.timeScale = 1;
        welcomeCanvas.SetActive(false);
        //playerControllerFPS.LockCursor();
        StartCoroutine(StartCownDown());

        

    }    

    private IEnumerator StartCownDown()
    {
        targetManager.StartGame();
        if (playerControllerFPS != null)
        {
            playerControllerFPS.DisableMovement();
            playerControllerFPS.DisableShooting();
        }

        startCountDownText.text = "3";
        yield return new WaitForSeconds(1f);

        startCountDownText.text = "2";
        yield return new WaitForSeconds(1f);

        startCountDownText.text = "1";
        yield return new WaitForSeconds(1f);

        startCountDownText.fontSize = 100;
        startCountDownText.text = "Go";
        
        yield return new WaitForSeconds(0.5f);
        startCountDownText.text = "";

        aimImage.SetActive(true);


        if (playerControllerFPS != null)
        {
            playerControllerFPS.EnableMovement();
            playerControllerFPS.EnableShooting();
        }
        playerControllerFPS.EnableShooting();
        StartCoroutine(Countdown());


    }
}