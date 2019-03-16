using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ChaserController : MonoBehaviour
{
    // Input
    public string inputAxis = "Horizontal";
    public string inputFire = "Fire1";
   
    // UI references
    public Canvas canvas;
    public Image reticle;
    
    // Aiming parameters
    public float maxAimSpeed = 200;
    public float aimAccelaration = 100;
    public float aimDecelaration = 0.02f;
    public float maxReticlePosition = 400f;

    public GameObject bullet;
    public Vector3 bulletOffset = Vector3.zero + Vector3.back;
    public float bulletSpeed = 25.0f;
    public float fireDelay = 1.0f;

    public Transform player;

    private GManager gManager;
    private Rigidbody rb;
    private Animator reticleAnimator;
    private Animator animator;
    private float aimSpeed = 0;
    private float reticleHeight = 0;
    private float nextFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        reticleAnimator = canvas.transform.GetChild(0).GetComponent<Animator>();
        animator = GetComponent<Animator>();
        gManager = FindObjectOfType<GManager>();

        reticleHeight = reticle.rectTransform.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveReticle();
        if (Input.GetButtonDown(inputFire))
        {
            Shoot();
        }

        if(player.transform.position.z == transform.position.z)
        {
            KillPlayer();
        }
    }

    void MoveReticle()
    {
        float x = Input.GetAxis(inputAxis);
        aimSpeed *= (1 - (1 - aimDecelaration) * Time.deltaTime);
        aimSpeed += x * aimAccelaration * Time.deltaTime;
        aimSpeed = Mathf.Min(aimSpeed, maxAimSpeed);
        aimSpeed = Mathf.Max(aimSpeed, -maxAimSpeed);
        
        reticle.rectTransform.anchoredPosition += new Vector2(aimSpeed*Time.deltaTime, reticleHeight);
        float xCoord = reticle.rectTransform.anchoredPosition.x;
        xCoord = Mathf.Min(xCoord, maxReticlePosition);
        xCoord = Mathf.Max(xCoord, -maxReticlePosition);
        reticle.rectTransform.anchoredPosition = new Vector2(xCoord, reticleHeight);
    }

    void Shoot()
    {
        if (Time.time < nextFire)
        {
            return;
        }

        // Find target position
        Vector2 canvasPosition = canvas.GetComponent<RectTransform>().anchoredPosition
            + reticle.rectTransform.anchoredPosition;
        Vector2 pixelPosition = RectTransformUtility.PixelAdjustPoint(canvasPosition, transform, canvas);
        float distance = Vector3.Distance(player.position, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pixelPosition.x, pixelPosition.y, 0.0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Vector3 targetPosition = new Vector3(
            hit.point.x,
            player.position.y,
            player.position.z
        );

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
        reticleAnimator.CrossFade("Charging", 0.0f);
        reticleAnimator.speed = 1 / fireDelay;
    }

    void KillPlayer()
    {
        animator.SetTrigger("killPlayer");
        gManager.PlayerDeath();
    }

    public void KillPlayerAnimationCallback()
    {
        gManager.PlayerDeath();
    }
}
