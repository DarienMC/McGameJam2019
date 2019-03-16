using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject obstacle2;
    public GameObject powerUp;
    public int maxObjectsOnASlice;
    private int nbrPresets = 9;
    //must be in pourcentage
    public int spawingObjectSliceChance;

    private float widthTerrain = 10.0F;

    int[,] array = new int[,]
    {
        {3, 1, 1, 1, 2, 1, 1},
        {0, 1, 0, 0, 1, 1, 1},
        {0, 0, 1, 1, 0, 1, 1},
        {2, 0, 0, 0, 0, 1, 1},
        {0, 2, 3, 0, 0, 1, 1},
        {0, 0, 2, 3, 0, 1, 1},
        {2, 0, 0, 3, 0, 1, 1},
        {0, 3, 0, 0, 0, 1, 1},
        {0, 0, 3, 0, 0, 1, 1},
    };

    ScrollingTerrain terrain;

    void Start()
    {
        terrain = GetComponent<ScrollingTerrain>();
        StartCoroutine(GenerateProps());
    }


    // Generate props (obstacles and powerups) at regular intervals.
    IEnumerator GenerateProps()
    {
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            int typeOfLine = Random.Range(0, 100);
            

            if (typeOfLine < spawingObjectSliceChance)
            {
                //getting a preset
                int typeOfTerrain = Random.Range(0, nbrPresets);

                for (int i = 0; i < maxObjectsOnASlice; i++)
                {
                    //position of object
                    float scaledWidth = (widthTerrain * terrain.terrainSlice.transform.localScale.x);
                    float offset = (float)(((i) * (widthTerrain / maxObjectsOnASlice)) - (widthTerrain / 2) + (float)(widthTerrain / maxObjectsOnASlice)/2);
                    Debug.Log(offset +", " + ((float)(widthTerrain / (maxObjectsOnASlice*2))));
                    

                    if (array[typeOfTerrain, i] == 1)
                    {   
                        GameObject instance = Instantiate(obstacle, new Vector3(offset, 2, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == 2)
                    {
                        GameObject instance = Instantiate(obstacle2, new Vector3(offset, 2, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == 3)
                    {
                        GameObject instance = Instantiate(powerUp, new Vector3(offset, 2, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
