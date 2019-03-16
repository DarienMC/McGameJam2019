using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public float scrollingSpeed = -1.0f;
    public int numberOfSlices = 2;
    public GameObject terrainSlice;

    private float sliceLength;
    public GameObject upcomingSlice;

    void Start()
    {
        for (int i = 1; i <= numberOfSlices; ++i)
        {
            InstantiateSlice();
        }
    }

    void FixedUpdate()
    {
        // Move all the terrain slices
        foreach (Transform child in transform)
        {
            // Scrolling
            child.Translate(0, 0, -scrollingSpeed * Time.deltaTime);
            
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
            sliceLength = instance.GetComponent<Collider>().bounds.size.z;
        }
        float offset = sliceLength * (numberOfSlices - transform.childCount - 2);
        instance.transform.Translate(Vector3.forward * offset);
        instance.transform.SetParent(transform);
        upcomingSlice = instance;
    }

    // Attach the prop to the terrain so that it moves along with it.
    public void AttachProp(Transform prop)
    {
       // Vector3 actualScale = prop.transform.localScale;
        prop.SetParent(upcomingSlice.transform);
        prop.transform.position = new Vector3(prop.transform.position.x * upcomingSlice.transform.localScale.x, prop.transform.position.y, prop.transform.position.z);
       // prop.transform.localScale = new Vector3( prop.transform.localScale.x * upcomingSlice.transform.localScale.x , prop.transform.localScale.y, prop.transform.localScale.z);
    }
}
