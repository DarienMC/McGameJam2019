using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public GameObject porthole1;
    public GameObject porthole2;
    public GameObject cone;
    public GameObject powerUp;
    public GameObject texture;
    public int maxObjectsOnASlice;
    private int nbrPresets = 20;
    //must be in pourcentage
    public int spawingObjectSliceChance;

    private float widthTerrain = 10.0F;
    private enum Obstacle { beer, jumpObs1, jumpObs2, avoidObs, nothing, jumpBeerObs };


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
        {Obstacle.jumpObs2, Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs},
        {Obstacle.avoidObs, Obstacle.jumpObs1,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpBeerObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpBeerObs},
        {Obstacle.jumpObs2, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpObs1, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.jumpObs1, Obstacle.jumpObs2,  Obstacle.nothing},
        {Obstacle.jumpBeerObs, Obstacle.jumpObs1,  Obstacle.jumpObs2, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.beer, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.beer, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing}
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
        
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            int isSpawingLine = Random.Range(0, 100);
            

            if (isSpawingLine <= spawingObjectSliceChance)
            {
                //getting a preset
                int typeOfTerrain = Random.Range(0, nbrPresets);

                for (int i = 0; i < maxObjectsOnASlice; i++)
                {
                    //position of object
                    float scaledWidth = (widthTerrain * terrain.terrainSlice.transform.localScale.x);
                    float offset = (float)(((i) * (widthTerrain / maxObjectsOnASlice)) - (widthTerrain / 2) + (float)(widthTerrain / maxObjectsOnASlice)/2);
                   
                    

                    if (array[typeOfTerrain, i] == Obstacle.jumpObs1)
                    {   
                        GameObject instance = Instantiate(porthole1, new Vector3(offset, -0.45f, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if(array[typeOfTerrain, i] == Obstacle.jumpObs2)
                    {
                        GameObject instance = Instantiate(porthole2, new Vector3(offset, -0.40f, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                    {
                        GameObject instance = Instantiate(cone, new Vector3(offset, 0, -20), cone.transform.rotation);
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
                        GameObject instance = Instantiate(porthole1, new Vector3(offset, -0.45F, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                        GameObject instance2 = Instantiate(powerUp, new Vector3(offset, 2, -20), Quaternion.identity);
                        terrain.AttachProp(instance2.transform);
                    }
                    else
                    {
                        GameObject instance = Instantiate(texture, new Vector3(offset, -0.50f, -20), Quaternion.identity);
                        terrain.AttachProp(instance.transform);
                    }

                }
                yield return new WaitForSeconds(2.0f);
            }
            
        }
    }
}
