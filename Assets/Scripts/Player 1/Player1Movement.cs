using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player1Movement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip hitSound;

    // Input variables
    private float horizontal;
    private bool jump;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool grounded = false;
    private int lastMovementDirection = 1; // -1 left, 0 none, 1 right

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(horizontal * speed, 0, 0));

        if(jump && grounded)
        {
            Jump();
        }

        if (!grounded)
        {
            // if falling...
            if (rb.velocity.y < 0)
            {
                // ... fall faster like mario
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        if (horizontal != 0)
        {
            if (lastMovementDirection != Mathf.Sign(horizontal))
            {
                lastMovementDirection *= -1;
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9)
        {
            grounded = true;
            audioSource.PlayOneShot(landingSound);
        }
    }

    private void Jump() {
        rb.AddForce(new Vector3(0, jumpForce, 0));
        grounded = false;
        audioSource.PlayOneShot(jumpSound);
    }
}
