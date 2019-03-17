using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserController : MonoBehaviour
{
    // Input
    public string inputAxis = "Horizontal";
    public string inputFire = "Fire1";
   
    // References
    public Transform player;
    public Canvas canvas;
    public Reticle reticle;
    
    //Audio Clips
    public AudioClip fireChaserBulletSound;
    public AudioClip fireLaserSound;
    public AudioClip killPlayerSound;

    // Aiming parameters
    public float maxAimSpeed = 200;
    public float aimAccelaration = 100;
    public float aimDecelaration = 0.02f;
    public float maxReticlePosition = 400f;

    public GameObject bullet;
    public Vector3 bulletOffset = Vector3.back;
    public Vector3 laserOffset = Vector3.back;
    public float bulletSpeed = 25.0f;
    public float fireDelay = 1.0f;
    public GameObject laser;
    public Animator animator;

    private RectTransform reticleTransform;
    private GManager gManager;
    private Rigidbody rb;
    
    private AudioSource audioSource;
    private float aimSpeed = 0;
    private float nextFire = Mathf.Infinity;
    private float reticleHeight = 0;

    private bool _canFireLaser = false;
    public bool canFireLaser
    {
        get
        {
            return _canFireLaser;
        }
        set
        {
            _canFireLaser = value;
            if (_canFireLaser)
            {
                reticle.EmpowerReticle();
            }
            else
            {
                reticle.ResetReticle();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        reticle = canvas.transform.GetChild(0).GetComponent<Reticle>();
        animator = GetComponent<Animator>();
        gManager = FindObjectOfType<GManager>();
        reticleTransform = reticle.GetComponent<RectTransform>();
        reticleHeight = reticleTransform.anchoredPosition.y;
        audioSource = GetComponent<AudioSource>();
        reticle.Charge(fireDelay * 3);
        nextFire = Time.time + fireDelay * 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            canFireLaser = true;
        }

        MoveReticle();
        if (Input.GetButtonDown(inputFire) && Time.time > nextFire)
        {
            if (canFireLaser)
            {
                FireLaser();
                audioSource.PlayOneShot(fireLaserSound);
            }
            else
            {
                Shoot();
                audioSource.PlayOneShot(fireChaserBulletSound);
            }
        }
    }

    void MoveReticle()
    {
        float x = Input.GetAxis(inputAxis);
        aimSpeed *= (1 - (1 - aimDecelaration) * Time.deltaTime);
        aimSpeed += x * aimAccelaration * Time.deltaTime;
        aimSpeed = Mathf.Min(aimSpeed, maxAimSpeed);
        aimSpeed = Mathf.Max(aimSpeed, -maxAimSpeed);
        
        reticleTransform.anchoredPosition += new Vector2(aimSpeed*Time.deltaTime, reticleHeight);
        float xCoord = reticleTransform.anchoredPosition.x;
        xCoord = Mathf.Min(xCoord, maxReticlePosition);
        xCoord = Mathf.Max(xCoord, -maxReticlePosition);
        reticleTransform.anchoredPosition = new Vector2(xCoord, reticleHeight);
    }

    Vector3 ComputeTargetPosition()
    {
        Vector2 canvasPosition = canvas.GetComponent<RectTransform>().anchoredPosition
            + reticleTransform.anchoredPosition;
        Vector2 pixelPosition = RectTransformUtility.PixelAdjustPoint(canvasPosition, transform, canvas);
        float distance = Vector3.Distance(player.position, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pixelPosition.x, pixelPosition.y, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return new Vector3(
            hit.point.x,
            player.position.y,
            player.position.z
        );
    }

    void Shoot()
    {
        // Find target position
        Vector3 targetPosition = ComputeTargetPosition();

        // Shoot
        GameObject instance = Instantiate(bullet, transform.position + bulletOffset, Quaternion.identity);
        float distanceToTarget = (targetPosition - instance.transform.position).magnitude;
        float time = distanceToTarget / bulletSpeed;
        Vector3 gravityOffset = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        for (float i = 0; i < time; i += Time.fixedDeltaTime)
        {
            velocity += -Physics.gravity * Time.fixedDeltaTime;
            gravityOffset += velocity * Time.fixedDeltaTime;
        }
        Vector3 direction = (targetPosition + gravityOffset - instance.transform.position).normalized;
        instance.GetComponent<ChaserBullet>().velocity = direction * bulletSpeed;
        
        // Fire delay
        nextFire = Time.time + fireDelay;
        reticle.Charge(fireDelay);
    }

    void FireLaser()
    {
        canFireLaser = false;
        nextFire = Time.time + fireDelay;
        Vector3 targetPosition = ComputeTargetPosition();
        GameObject instance = Instantiate(laser, transform.position + laserOffset, Quaternion.identity);
        instance.GetComponent<Laser>().Set(instance.transform.position + laserOffset, targetPosition);
        reticle.Charge(fireDelay);
    }

    public void KillPlayer()
    {
        animator.SetTrigger("killPlayer");
    }

    public void KillPlayerAnimationCallback()
    {
        gManager.PlayerDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PowerUp")
        {
            canFireLaser = true;
        }
    }
}
