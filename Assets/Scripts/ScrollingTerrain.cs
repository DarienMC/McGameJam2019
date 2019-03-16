using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public float scrollingSpeed = 1.0f;
    public int numberOfSlices = 2;
    public GameObject terrainSlice;

    private float sliceLength;

    void Start()
    {
        for (int i = 1; i <= numberOfSlices; ++i)
        {
            InstantiateSlice();
        }
    }

    void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            child.Translate(0, 0, -scrollingSpeed * Time.deltaTime);
            if (child.transform.position.z < -sliceLength)
            {
                child.transform.SetParent(null);
                Destroy(child.gameObject);
                InstantiateSlice();
            }
        }
    }

    void InstantiateSlice()
    {
        GameObject instance = Instantiate(terrainSlice);
        if (transform.childCount == 0)
        {
            sliceLength = instance.GetComponent<Collider>().bounds.size.z;
        }
        else
        {
            instance.transform.Translate(Vector3.forward * sliceLength * transform.childCount);
        }
        instance.transform.SetParent(transform);
    }
}
