using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{

    //Timer elements
    public float timerLimit = 400.0f;

    //References
    public GameObject runner;
    public GameObject chaser;
    public Text timerText;
    public Text winText;
    public Text distanceTrackingText;
    public AudioClip chaserWinClip;
    public AudioClip runnerWinClip;

    private float timePassed = 0.0f;
    private float playerDistance;

    private AudioSource runnerAudioSource;
    private AudioSource chaserAudioSource;

    
    public Animator transitionAnimator;
    public float transitionDuration = 1;
    internal bool gameEnding = false;


    // Start is called before the first frame update
    void Start()
    {
        SetTimerText();
        winText.enabled = false;
        runnerAudioSource = runner.GetComponent<AudioSource>();
        chaserAudioSource = chaser.GetComponent<AudioSource>();
        transitionAnimator.CrossFadeInFixedTime("Transparent", transitionDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(1);
        }

        timePassed += Time.deltaTime;
        SetTimerText();

        SetDistanceTrackingText();

        playerDistance = chaser.transform.position.z - runner.transform.position.z;

        if (!gameEnding)
        {
            if (timerLimit <= timePassed)
            {
                PlayerWin();
            }
        }

    }

    public void PlayerDeath()
    {
        gameEnding = true;
        int x = Random.Range(0, 2);
        Debug.Log("The devil wins!");
        winText.text = "The devil wins!";
        winText.enabled = true;
        chaserAudioSource.PlayOneShot(chaserWinClip);
        FindObjectOfType<ChaserController>().KillPlayer();
        StartCoroutine(Wait());
    }

    public void PlayerWin() {
        gameEnding = true;
        int x = Random.Range(0, 2);
        Debug.Log("The damned wins!");
        winText.text = "The damned Wins!";
        winText.enabled = true;
        runnerAudioSource.PlayOneShot(runnerWinClip);
        StartCoroutine(Wait());
    }

    private void SetTimerText()
    {
        int timeOnTimer = Mathf.FloorToInt(timerLimit - timePassed);
        if (timeOnTimer >= 0) 
        {
            timerText.text = timeOnTimer.ToString() + " s left";
        }
    }

    private void SetDistanceTrackingText()
    {
        distanceTrackingText.text = Mathf.Max(0, Mathf.FloorToInt(playerDistance)).ToString() + " m distance"/* + Mathf.FloorToInt(separationForRunnerVictory - separationForChaserVictory).ToString() + "m"*/;
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(4);
        transitionAnimator.CrossFadeInFixedTime("Opaque", transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
