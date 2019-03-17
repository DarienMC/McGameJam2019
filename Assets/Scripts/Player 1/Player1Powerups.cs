using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player1Movement))]
public class Player1Powerups : MonoBehaviour, DelegateTimer
{
    public float speedUpMultiplier;
    public float slowDownMultiplier;
    public float speedUpTime;
    public float respawnHeight = 0.1179351f + 0.2f;
    public Animator anim;
    public GameObject obstacleHitVFX;

    //Audio Clips
    public AudioClip hitByChaserBulletSound;
    public AudioClip hitByLaserSound;
    public AudioClip hitByObstacleSound;
    public AudioClip hitByConeSound;
    public AudioClip hitByPotholeSound;
    public AudioClip powerUpSound;

    public GameObject potholeFallEffect;

    private ScrollingTerrain scrollingTerrain;
    private Player1Movement player1Movement;
    private bool poweredUp = false;
    private bool slowedDown = false;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        scrollingTerrain = FindObjectOfType<ScrollingTerrain>();
        audioSource = GetComponent<AudioSource>();
        player1Movement = GetComponent<Player1Movement>();
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
        if (!poweredUp)
        {
            poweredUp = true;
            scrollingTerrain.ModifyScrollSpeed(speedUpMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.SpeedUp, this);
        }
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

    void Respawn()
    {
        transform.position = new Vector3(transform.position.x, respawnHeight, transform.position.z);
        anim.SetTrigger("respawn");
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
            SlowDown();
            audioSource.PlayOneShot(hitByLaserSound);
            Debug.Log("Player was hit by Laser");
        }
        else if (other.transform.tag == "Cone")
        {
            Destroy(other.gameObject);
            SlowDown();
            Instantiate(obstacleHitVFX, other.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(hitByConeSound);
        }
        else if (other.transform.tag == "Pothole")
        {
            if (player1Movement.GetJumpStatus() != Player1Movement.jumpStatus.jumpingState)
            {
                Instantiate(potholeFallEffect, transform.position, Quaternion.identity);
                anim.SetTrigger("potholeFall");
                audioSource.PlayOneShot(hitByPotholeSound);
                SlowDown();
            }
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pothole")
        {
            Respawn();
        }
    }

    public void TimerFinishedCallback()
    {
        poweredUp = false;
        slowedDown = false;
    }
}
