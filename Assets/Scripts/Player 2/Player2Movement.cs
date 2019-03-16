using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float relativeSpeed;
    public float maxZDistance;
    public float minZDistance;

    private bool movingBackwards = false;
    private bool movingForwards = false;
    private DelegateTimer timerCallbackObject;

    private float moveTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movingBackwards)
        {
            if(transform.position.z - relativeSpeed >= minZDistance)
                transform.position -= new Vector3(0, 0, relativeSpeed);
        }
        else if (movingForwards)
        {
            if (transform.position.z + relativeSpeed <= maxZDistance)
                transform.position += new Vector3(0, 0, relativeSpeed);
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
