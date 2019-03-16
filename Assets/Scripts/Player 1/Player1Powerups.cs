using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Powerups : MonoBehaviour
{
    public float speedUpMultiplier;
    public float slowDownMultiplier;
    public float speedUpTime;

    private ScrollingTerrain scrollingTerrain;

    // Start is called before the first frame update
    void Start()
    {
        scrollingTerrain = FindObjectOfType<ScrollingTerrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Test Button"))
        {
            SpeedUp();
        }

        if (Input.GetButtonDown("Test Button 2"))
        {
            SlowDown();
        }
    }

    void SpeedUp()
    {
        scrollingTerrain.ModifyScrollSpeed(speedUpMultiplier, speedUpTime, true);
    }

    void SlowDown()
    {
        scrollingTerrain.ModifyScrollSpeed(slowDownMultiplier, speedUpTime, false);
    }
}
