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
   

    public float separationForChaserVictory;
    public float separationForRunnerVictory;

    private float timePassed = 0.0f;
    private float playerDistance;
    private AudioSource runnerAudioSource;
    private AudioSource chaserAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        SetTimerText();
        winText.enabled = false;
        runnerAudioSource = runner.GetComponent<AudioSource>();
        chaserAudioSource = chaser.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        SetTimerText();

        SetDistanceTrackingText();

        playerDistance = chaser.transform.position.z - runner.transform.position.z;

        //Win States
        if (playerDistance <= separationForChaserVictory) {
            Debug.Log("Chaser wins by catching up!");
            PlayerDeath();
        }

        if (playerDistance >= separationForRunnerVictory) {
            PlayerWin();
        }

        if (timerLimit <= timePassed) {
            Debug.Log("Chaser wins by time!");
            PlayerDeath();
        }
        

    }

    public void PlayerDeath()
    {
        int x = Random.Range(0, 2);
        Debug.Log("Chaser wins!");
        winText.text = "Chaser wins!";
        winText.enabled = true;
        chaserAudioSource.PlayOneShot(chaserWinClip);
        StartCoroutine(Wait());
    }

    public void PlayerWin() {
        int x = Random.Range(0, 2);
        Debug.Log("Runner wins!");
        winText.text = "Runner Wins!";
        winText.enabled = true;
        runnerAudioSource.PlayOneShot(runnerWinClip);
        StartCoroutine(Wait());
    }

    private void SetTimerText()
    {
        int timeOnTimer = Mathf.FloorToInt(timerLimit - timePassed);
        if (timeOnTimer >= 0) 
        {
            timerText.text = timeOnTimer.ToString();
        }
    }

    private void SetDistanceTrackingText()
    {
        distanceTrackingText.text = Mathf.FloorToInt(playerDistance - separationForChaserVictory).ToString() + "m out of " + Mathf.FloorToInt(separationForRunnerVictory - separationForChaserVictory).ToString() + "m";
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
