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

    //State machine
    public enum jumpStatus {runningState, jumpingState };
    public jumpStatus myJumpStatus;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        myJumpStatus = jumpStatus.runningState;
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

        /**STATE MACHINE**/
        if (myJumpStatus == jumpStatus.runningState)
        {
            if (jump)
            {
                Jump();
                myJumpStatus = jumpStatus.jumpingState;
            }
        }

        if (myJumpStatus == jumpStatus.jumpingState)
        {
            if (grounded)
            {
                myJumpStatus = jumpStatus.runningState;
            }
            if (!grounded)
            {
                // if falling...
                if (rb.velocity.y < 0)
                {
                    // ... fall faster like mario
                    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
                }
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
        rb.AddForce(0f, 0f, 0f);
        //rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        rb.velocity += Vector3.up * jumpForce;
        grounded = false;
        audioSource.PlayOneShot(jumpSound);
    }
}
