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

    private float timePassed = 0.0f;
    private float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.time;
        playerDistance = chaser.transform.position.z - runner.transform.position.z;

        //Win States
        if (playerDistance <= chaser.GetComponent<Player2Movement>().minZDistance) {
            PlayerDeath();
        }

        if (playerDistance > chaser.GetComponent<Player2Movement>().maxZDistance) {
            PlayerWin();
        }

        if (timerLimit <= timePassed) {
            PlayerDeath();
        }

    }

    public void PlayerDeath()
    {
        StartCoroutine(WaitForPlayerDeath());
    }

    public void PlayerWin() {
        SceneManager.LoadScene(0);
    }

    private IEnumerator WaitForPlayerDeath() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
