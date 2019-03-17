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
    //must be in pourcentage
    public int spawingObjectSliceChance;
    public int lengthSpawningArea;

    private int maxObjectsOnASlice = 7;
    private int nbrPresets = 30;
    private float widthTerrain;
    private enum Obstacle { beer, jumpObs1, jumpObs2, avoidObs, nothing, jumpBeerObs };


    Obstacle[,] array = new Obstacle[,]
    {
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.jumpObs2, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
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
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing}
    };

    ScrollingTerrain terrain;

    void Start()
    {
        terrain = GetComponent<ScrollingTerrain>();
        widthTerrain = texture.GetComponent<Renderer>().bounds.size.z * maxObjectsOnASlice;

        

        StartCoroutine(GenerateProps());
    }


    // Generate props (obstacles and powerups) at regular intervals.
    IEnumerator GenerateProps()
    {
        widthTerrain = texture.GetComponent<Renderer>().bounds.size.z*maxObjectsOnASlice;

        //initiate terrain
        for (int i = 0; i <= lengthSpawningArea; i++)
        {
            GenerateEmptyLine(widthTerrain);

            yield return new WaitForSeconds(1.75f);
        }


        while (true)
        {
            int isSpawingLine = Random.Range(0, 100);

            //spawn obstacles
            if (isSpawingLine <= spawingObjectSliceChance)
            {
                //getting a preset
                int typeOfTerrain = Random.Range(0, nbrPresets - 1);

                for (int i = 0; i < maxObjectsOnASlice; i++)
                {
                    //position of object
                    float offset = (float)(((i) * (widthTerrain / maxObjectsOnASlice)) - (widthTerrain / 2) + (float)(widthTerrain / maxObjectsOnASlice) / 2);

                    if (array[typeOfTerrain, i] == Obstacle.jumpObs1)
                    {
                        GenerateFloor(porthole1, offset);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.jumpObs2)
                    {
                        GameObject instance = Instantiate(porthole2, new Vector3(offset + (float)(texture.GetComponent<Renderer>().bounds.size.x), 0f, -5 +(float)(texture.GetComponent<Renderer>().bounds.size.z)), porthole2.transform.rotation);
                        terrain.AttachProp(instance.transform);
                        i++;
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                    {
                        GenerateFloor(texture, offset);
                        GenerateObstacle(cone, offset);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.beer)
                    {
                        GenerateFloor(texture, offset);
                        GenerateObstacle(powerUp, offset);
                    }

                    else if (array[typeOfTerrain, i] == Obstacle.jumpBeerObs)
                    {
                       
                        GenerateFloor(porthole1, offset);
                        GameObject instance2 = Instantiate(powerUp, new Vector3(offset, 2, -55), Quaternion.identity);
                        terrain.AttachProp(instance2.transform);
                    }
                    else
                    {
                        GenerateFloor(texture, offset);
                    }

                }
                //spawing 2 empty slices to give the player a chance to react in between slices
                yield return new WaitForSeconds(1.75f);
                GenerateEmptyLine(widthTerrain);

                yield return new WaitForSeconds(1.75f);
                GenerateEmptyLine(widthTerrain);

                yield return new WaitForSeconds(1.75f);
            }

            //spawn empty slice
            else
            {
                GenerateEmptyLine(widthTerrain);

                yield return new WaitForSeconds(1.75f);
            }
        }

        void GenerateEmptyLine(float width)
        {

            for (int i = 0; i < maxObjectsOnASlice; i++)
            {
                //position of object
                float offset = (float)(((i) * (width / maxObjectsOnASlice)) - (width / 2) + (float)(width / maxObjectsOnASlice) / 2);

                GameObject instance = Instantiate(texture, new Vector3(offset, 0f, -5), texture.transform.rotation);
                terrain.AttachProp(instance.transform);

            }

        }

        void GenerateObstacle(GameObject gameobject, float offset)
        {
                GameObject instance = Instantiate(gameobject, new Vector3(offset, 0.2f, -5), gameobject.transform.rotation);
                terrain.AttachProp(instance.transform);
            
        }

        void GenerateFloor(GameObject gameobject, float offset)
        {
                GameObject instance = Instantiate(gameobject, new Vector3(offset, 0f, -5), gameobject.transform.rotation);
                terrain.AttachProp(instance.transform);

        }
    }
}
