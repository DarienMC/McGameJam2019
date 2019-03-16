using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{

    //Timer elements
    public float timerLimit = 400.0f;

    //References
    public GameObject runner;
    public GameObject chaser;
    public float separationForChaserVictory;
    public float separationForRunnerVictory;

    private float timePassed = 0.0f;
    private float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
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
        StartCoroutine(WaitForPlayerDeath());
    }

    public void PlayerWin() {
        Debug.Log("Runner wins!");
        SceneManager.LoadScene(0);
    }

    private IEnumerator WaitForPlayerDeath() {
        Debug.Log("Chaser wins!");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
