using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Powerups : MonoBehaviour, DelegateTimer
{
    public float speedUpMultiplier;
    public float slowDownMultiplier;
    public float speedUpTime;

    //Audio Clips
    public AudioClip hitByChaserBulletSound;
    public AudioClip hitByObstacleSound;
    public AudioClip powerUpSound;

    private ScrollingTerrain scrollingTerrain;
    private bool poweredUp = false;
    private bool slowedDown = false;
    private AudioSource audioSource;

     
    // Start is called before the first frame update
    void Start()
    {
        scrollingTerrain = FindObjectOfType<ScrollingTerrain>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Test Button"))
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
        if (!slowedDown)
        {
            if (!poweredUp)
            {
                poweredUp = true;
                scrollingTerrain.ModifyScrollSpeed(speedUpMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.SpeedUp, this);
            }
        }
        else
            NormalSpeed();
    }

    void SlowDown()
    {
        if (!poweredUp)
        {
            scrollingTerrain.ModifyScrollSpeed(slowDownMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.SlowDown, this);
            slowedDown = true;
        }
        else
            NormalSpeed();
    }

    void NormalSpeed()
    {
        poweredUp = false;
        slowedDown = false;
        scrollingTerrain.ModifyScrollSpeed(slowDownMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.BackToNormal, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            SpeedUp();
            audioSource.PlayOneShot(powerUpSound);
        }
        else if (other.transform.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            SlowDown();
            audioSource.PlayOneShot(hitByObstacleSound);
        }
     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ChaserBullet")
        {
            audioSource.PlayOneShot(hitByChaserBulletSound);
            Debug.Log("Player was hit by ChaserBullet");
            Destroy(collision.gameObject);
            SlowDown();

        }
    }

    public void TimerFinishedCallback()
    {
        poweredUp = false;
        slowedDown = false;
    }
}
