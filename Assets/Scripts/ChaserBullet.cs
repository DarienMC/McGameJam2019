using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserBullet : MonoBehaviour
{
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        rigidbody.useGravity = true;
        if (collision.gameObject.tag == "Terrain")
        {
            Destroy(gameObject);
        }
    }
}
