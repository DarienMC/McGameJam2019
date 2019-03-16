using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject obstacle2;
    public GameObject powerUp;
    public int maxObjectsOnASlice;
    private int nbrPresets = 20;
    //must be in pourcentage
    public int spawingObjectSliceChance;

    private float widthTerrain = 10.0F;
    private enum Obstacle { beer, jumpObs, avoidObs, nothing, jumpBeerObs };


    Obstacle[,] array = new Obstacle[,]
    {
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.beer},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.beer, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.jumpObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.jumpObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},

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
                   
                    

                    if (array[typeOfTerrain, i] == Obstacle.jumpObs)
                    {   
                        GameObject instance = Instantiate(obstacle, new Vector3(offset, -1, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                    {
                        GameObject instance = Instantiate(obstacle2, new Vector3(offset, 0, -20), obstacle2.transform.rotation);
                        //instance.transform.rotation = 
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.beer)
                    {
                        GameObject instance = Instantiate(powerUp, new Vector3(offset, 0.2F, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.jumpBeerObs)
                    {
                        GameObject instance = Instantiate(obstacle, new Vector3(offset, -1, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                        GameObject instance2 = Instantiate(powerUp, new Vector3(offset, 2, -20), Quaternion.identity);
                        terrain.AttachProp(instance2.transform);
                    }

                }
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
