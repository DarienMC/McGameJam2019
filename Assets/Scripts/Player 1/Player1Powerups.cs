using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Powerups : MonoBehaviour, DelegateTimer
{
    public float speedUpMultiplier;
    public float slowDownMultiplier;
    public float speedUpTime;
    public GameObject obstacleHitVFX;

    //Audio Clips
    public AudioClip hitByChaserBulletSound;
    public AudioClip hitByLaserSound;
    public AudioClip hitByObstacleSound;
    public AudioClip hitByConeSound;
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
            Instantiate(obstacleHitVFX, other.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(hitByObstacleSound);
        }

        if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            SlowDown();
            audioSource.PlayOneShot(hitByLaserSound);
            Debug.Log("Player was hit by Laser");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ChaserBullet")
        {
            Destroy(collision.gameObject);
            SlowDown();
            audioSource.PlayOneShot(hitByChaserBulletSound);
            Debug.Log("Player was hit by ChaserBullet");

        }
        else if (collision.transform.tag == "Obstacle")
        {
            Destroy(collision.gameObject);
            SlowDown();
            Instantiate(obstacleHitVFX, collision.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(hitByObstacleSound);
        }

        else if (collision.transform.tag == "Cone")
        {
            Destroy(collision.gameObject);
            SlowDown();
            Instantiate(obstacleHitVFX, collision.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(hitByConeSound);
        }

    }

    public void TimerFinishedCallback()
    {
        poweredUp = false;
        slowedDown = false;
    }
}
