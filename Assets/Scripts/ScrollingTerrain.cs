using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public float scrollingSpeed = -1.0f;
    public int numberOfSlices = 2;
    public GameObject terrainSlice;

    private float sliceLength;
    private float scrollSpeedMultiplier = 1;
    private bool scrollSpeedModified = false;
    private float scrollSpeedModifiedTimer;

    void Start()
    {
        for (int i = 1; i <= numberOfSlices; ++i)
        {
            InstantiateSlice();
        }
    }

    private void Update()
    {
        if (scrollSpeedModified)
        {
            scrollSpeedModifiedTimer -= Time.deltaTime;
            if(scrollSpeedModifiedTimer < 0)
            {
                scrollSpeedModified = false;
                scrollSpeedMultiplier = 1;
            }
        }
    }

    void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            // Scrolling
            child.Translate(0, 0, -scrollingSpeed * Time.deltaTime * scrollSpeedMultiplier);
            
            // Check if beyond sight range
            if (child.transform.position.z > sliceLength * (numberOfSlices - 1))
            {
                child.transform.SetParent(null);
                Destroy(child.gameObject);
                InstantiateSlice();
            }
        }
    }

    // Create a new terrain slice behind the camera.
    void InstantiateSlice()
    {
        GameObject instance = Instantiate(terrainSlice);
        if (transform.childCount == 0)
        {
            sliceLength = instance.GetComponent<BoxCollider>().bounds.size.z;
        }
        float offset = sliceLength * (numberOfSlices - transform.childCount - 2);
        instance.transform.Translate(Vector3.forward * offset);
        instance.transform.SetParent(transform);
    }

    public void ModifyScrollSpeed(float speedMultiplier, float time)
    {
        scrollSpeedMultiplier = speedMultiplier;
        scrollSpeedModified = true;
        scrollSpeedModifiedTimer = time;
    }
}
