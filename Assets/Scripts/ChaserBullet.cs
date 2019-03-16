using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserBullet : MonoBehaviour
{
    internal Vector3 velocity;

    private Vector3 acceleration = Physics.gravity;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        velocity += acceleration * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            Destroy(gameObject);
        }
    }
}
