using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player1 : MonoBehaviour
{
    public float speed;

    // Input variables
    private float horizontal;

    private Rigidbody rb;
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
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(horizontal * speed, 0, 0));
    }

    private void LateUpdate()
    {
        if (horizontal != 0)
        {
            if (lastMovementDirection != Mathf.Sign(horizontal))
            {
                lastMovementDirection *= -1;
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
    }
}
