using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public float scrollingSpeed = -1.0f;
    public int numberOfSlices = 2;
    public GameObject terrainSlice;

    private float sliceLength;

    public GameObject obstacle;
    public GameObject obstacle2;
    public GameObject powerUp;
    int terrainSlots = 3;

    int[,] array = new int[,]
 {
            {1, 0, 0, 0, 0},
            {0, 1, 0, 0, 0},
            {0, 0, 1, 0, 0},
            {2, 0, 0, 0, 0},
            {0, 2, 0, 0, 0},
            {0, 0, 2, 0, 0},
            {2, 0, 0, 0, 0},
            {0, 2, 0, 0, 0},
            {0, 0, 2, 0, 0},
 };

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
        GenerateLine(instance);
    }

    void GenerateLine(GameObject child)
    {
        //if type of line == 0, line is empty, ow add obstacles
        int typeOfLine = Random.Range(0, 2);

        if (typeOfLine == 1)
        {
            //getting a preset
            int typeOfTerrain = Random.Range(0, 9);

            //places blox 2 units from each other from the chosen preset
            for (int i = -1; i < terrainSlots-1; i++)
            {
                if (array[typeOfTerrain, i+1] == 1)
                {
                    GameObject instance = Instantiate(obstacle, new Vector3(0, 2, 0), Quaternion.identity);
                    float offset = sliceLength * (numberOfSlices - transform.childCount - 2);
                    instance.transform.Translate(Vector3.forward);
                    //child.transform.SetAsLastSibling(instance);

                }
                //Instantiate(obstacle, transform.position+new Vector3(i * 2.0F, 1F, 0), Quaternion.identity);
                else if (array[typeOfTerrain, i+1] == 2)
                {
                    GameObject instance = Instantiate(powerUp, new Vector3(0, 2, 0), Quaternion.identity);
                    float offset = sliceLength * (numberOfSlices - transform.childCount - 2);
                    instance.transform.Translate(Vector3.forward);
                }


            }

        }
    }
}
