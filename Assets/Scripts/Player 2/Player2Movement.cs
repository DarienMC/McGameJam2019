using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public Transform runner;
    public float relativeSpeed;
    public float maxDistance;
    public float minDistance;

    private bool movingBackwards = false;
    private bool movingForwards = false;
    private DelegateTimer timerCallbackObject;

    private float moveTimer = 0;
    private GManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameObject.FindWithTag("GameController").GetComponent<GManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingBackwards)
        {
            transform.position -= new Vector3(0, 0, relativeSpeed * Time.deltaTime);
            if (transform.position.z - runner.position.z <= minDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, runner.position.z + minDistance);
                if (!gManager.gameEnding)
                {
                    gManager.PlayerDeath();
                }
            }
        }
        else if (movingForwards)
        {
            transform.position += new Vector3(0, 0, relativeSpeed * Time.deltaTime);
            if (transform.position.z - runner.position.z >= maxDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, runner.position.z + maxDistance);
                if (!gManager.gameEnding)
                {
                    gManager.PlayerWin();
                }
            }
        }

        if(movingForwards || movingBackwards)
        {
            moveTimer -= Time.deltaTime;
            if(moveTimer < 0)
            {
                movingBackwards = false;
                movingForwards = false;
                timerCallbackObject.TimerFinishedCallback();
            }
        }
    }

    public void MoveBackwards(float time, DelegateTimer timerCallback)
    {
        movingBackwards = true;
        movingForwards = false;
        moveTimer = time;
        timerCallbackObject = timerCallback;
    }

    public void MoveForwards(float time, DelegateTimer timerCallback)
    {
        movingBackwards = false;
        movingForwards = true;
        moveTimer = time;
        timerCallbackObject = timerCallback;
    }

    public void BackToNormal()
    {
        movingBackwards = false;
        movingForwards = false;
        moveTimer = 0;
    }
}
