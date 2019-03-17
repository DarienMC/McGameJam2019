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

    public float separationForChaserVictory;
    public float separationForRunnerVictory;

    private float timePassed = 0.0f;
    private float playerDistance;
    private bool gameEnding = false;

    // Start is called before the first frame update
    void Start()
    {
        SetTimerText();
        winText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        SetTimerText();

        SetDistanceTrackingText();

        playerDistance = chaser.transform.position.z - runner.transform.position.z;

        if (!gameEnding)
        {
            //Win States
            if (playerDistance <= separationForChaserVictory)
            {
                Debug.Log("Chaser wins by catching up!");
                PlayerDeath();
            }

            if (playerDistance >= separationForRunnerVictory)
            {
                PlayerWin();
            }

            if (timerLimit <= timePassed)
            {
                Debug.Log("Chaser wins by time!");
                PlayerDeath();
            }
        }

    }

    public void PlayerDeath()
    {
        Debug.Log("Chaser wins!");
        winText.text = "Chaser wins!";
        winText.enabled = true;
        FindObjectOfType<ChaserController>().KillPlayer();
        StartCoroutine(Wait());
        gameEnding = true;
    }

    public void PlayerWin() {
        Debug.Log("Runner wins!");
        winText.text = "Runner Wins!";
        winText.enabled = true;
        StartCoroutine(Wait());
        gameEnding = true;
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
        distanceTrackingText.text = Mathf.Max(0, Mathf.FloorToInt(playerDistance - separationForChaserVictory)).ToString() + " m distance"/* + Mathf.FloorToInt(separationForRunnerVictory - separationForChaserVictory).ToString() + "m"*/;
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
