using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player1 : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    // Input variables
    private float horizontal;
    private bool jump;

    private Rigidbody rb;

    private bool grounded = false;
    private int lastMovementDirection = 1; // -1 left, 0 none, 1 right

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(horizontal * speed, 0, 0));

        if(jump && grounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            grounded = false;
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
        }
    }
}
