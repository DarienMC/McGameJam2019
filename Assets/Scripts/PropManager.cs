using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
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

    ScrollingTerrain terrain;

    void Start()
    {
        terrain = GetComponent<ScrollingTerrain>();
        StartCoroutine(GenerateProps());
    }

    /*void GenerateLine(GameObject child)
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
    }*/

    // Generate props (obstacles and powerups) at regular intervals.
    IEnumerator GenerateProps()
    {
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            GameObject instance = Instantiate(obstacle, new Vector3(0, 2, -20), Quaternion.identity);
            terrain.AttachProp(instance.transform);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
