using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public GameObject decoration1;
    public GameObject decoration2;
    public GameObject decoration3;

    public ScrollingTerrain terrain;
    public GameObject porthole1;
    public GameObject porthole2;
    public GameObject cone;
    public GameObject powerUp;
    public GameObject texture;
    //must be in percentage
    public float spawingObjectSliceChance;
    public int distanceBetweenBeers;
    public float maxSpawingObjectSliceChance;
    public float deltaSpawningObjectSliceChance;

    private int maxObjectsOnASlice = 7;
    private int nbrPresets = 35;
    private float widthTerrain;
    private enum Obstacle { beer, jumpObs1, jumpObs2, avoidObs, nothing, jumpBeerObs };
    private int beerDelay = 0;

    Obstacle[,] array = new Obstacle[,]
    {
        {Obstacle.nothing, Obstacle.jumpObs2, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.jumpObs2, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.jumpObs2, Obstacle.nothing, Obstacle.jumpObs2, Obstacle.avoidObs,  Obstacle.avoidObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.jumpObs1,  Obstacle.jumpBeerObs},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpBeerObs},
        {Obstacle.nothing, Obstacle.jumpObs2,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.jumpObs2, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpObs1, Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpObs1},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.jumpObs2, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.jumpBeerObs, Obstacle.nothing,  Obstacle.jumpObs2, Obstacle.jumpObs1, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing, Obstacle.jumpObs2,  Obstacle.avoidObs},
        {Obstacle.jumpBeerObs, Obstacle.jumpObs1,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing, Obstacle.beer,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing, Obstacle.jumpObs2, Obstacle.jumpObs1, Obstacle.nothing,  Obstacle.jumpObs1},
        {Obstacle.nothing, Obstacle.jumpObs2,  Obstacle.nothing, Obstacle.jumpObs2, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.jumpObs1, Obstacle.jumpObs2, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.jumpObs1, Obstacle.jumpObs2, Obstacle.avoidObs,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs},
        {Obstacle.jumpObs2, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.avoidObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.nothing, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
    };

    void Start()
    {
        widthTerrain = texture.GetComponent<Renderer>().bounds.size.z * maxObjectsOnASlice;
    }

    private void FixedUpdate()
    {
        if(!(spawingObjectSliceChance + deltaSpawningObjectSliceChance >= maxSpawingObjectSliceChance))
            spawingObjectSliceChance = spawingObjectSliceChance + deltaSpawningObjectSliceChance;
    }

    void GenerateObstacle(GameObject gameobject, Vector3 position)
    {
        GameObject instance = Instantiate(gameobject, gameObject.transform.position + position, gameobject.transform.rotation);
        terrain.AttachProp(instance.transform);
    }

    void ReplaceFloor(GameObject gameObject, int childIndex)
    {
        GameObject replacement = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Transform current = terrain.previousSlice.GetChild(childIndex);
        replacement.transform.position = current.position;
        replacement.transform.parent = current.parent;
        current.SetParent(null);
        Destroy(current.gameObject);
    }

    void ReplaceFloorLarge(GameObject gameObject, int childIndex)
    {
        GameObject replacement = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Transform left = terrain.previousSlice.GetChild(childIndex - 1);
        Transform right = terrain.previousSlice.GetChild(childIndex);
        replacement.transform.position = right.position + new Vector3(0.0f, 0.0f, 1.0f) * terrain.sliceLength;
        replacement.transform.parent = right.parent;
        left.SetParent(null);
        Destroy(left.gameObject);
        right.SetParent(null);
        Destroy(right.gameObject);
    }

    public void GenerateLine(Transform slice)
    {
        
        int isSpawingLine = Random.Range(0, 100);

        //spawn obstacles
        if (isSpawingLine <= spawingObjectSliceChance)
        {
            //getting a preset
            int typeOfTerrain = Random.Range(0, nbrPresets - 1);
            bool foundBeer = false;

            int portHoleGenerated = 0;
            for (int i = 0; i < maxObjectsOnASlice; ++i)
            {
                float offset = -5.4f + 1.8f * i;
                Vector3 position = new Vector3(offset, 0.2f, slice.position.z);

                if (array[typeOfTerrain, i] == Obstacle.jumpObs1)
                {
                    ReplaceFloor(porthole1, i - portHoleGenerated);
                    portHoleGenerated += 1;
                }

                else if (array[typeOfTerrain, i] == Obstacle.jumpObs2 && i % 2 == 1)
                {
                    ReplaceFloorLarge(porthole2, i - portHoleGenerated);
                    portHoleGenerated += 2;
                }

                else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                {
                    GenerateObstacle(cone, position);
                }

                else if (array[typeOfTerrain, i] == Obstacle.beer)
                {
                    if (beerDelay >= distanceBetweenBeers)
                    {
                        GenerateObstacle(powerUp, position);
                        foundBeer = true;
                        beerDelay = 0;
                    }
                    else
                        this.GenerateObstacle(cone, position);
                }

                else if (array[typeOfTerrain, i] == Obstacle.jumpBeerObs)
                {
                    ReplaceFloor(porthole1, i - portHoleGenerated);
                    portHoleGenerated += 1;
                    if (beerDelay >= distanceBetweenBeers)
                    {
                        GameObject instance2 = Instantiate(powerUp, position + Vector3.up * 2, Quaternion.identity);
                        terrain.AttachProp(instance2.transform);
                        foundBeer = true;
                        beerDelay = 0;
                    }
                }
            }
            if (foundBeer == false)
                beerDelay++;
        }
    }

    public void GenerateDecoration(Transform slice)
    {
        int generateSnow = Random.Range(0, 8);
        if (generateSnow < 2)
        {
            int typeOfSnow = Random.Range(0, 4);

            float offset = -5.4f + 1.8f * -1;
            float randomOffset = Random.Range(0.8f, 1.5f);
            Vector3 position = new Vector3(offset - randomOffset, 0.5f, slice.position.z);
            if (typeOfSnow == 0)
            {
                //generate at -1
                GenerateObstacle(decoration1, position);
            }

            if (typeOfSnow == 1)
            {
                //generate at -1
                GenerateObstacle(decoration2, position);
            }
            if (typeOfSnow == 2)
            {
                //generate at -1
                GenerateObstacle(decoration3, position);
            }

        }

        generateSnow = Random.Range(0, 8);
        if (generateSnow < 2)
        {
            int typeOfSnow = Random.Range(0, 3);
            //generate at maxObjectsOnASlice

            float offset = -5.4f + 1.8f * maxObjectsOnASlice;
            float randomOffset = Random.Range(0.8f, 1.5f);
            Vector3 position = new Vector3(offset + randomOffset, 0.5f, slice.position.z);
            if (typeOfSnow == 0)
            {
                //generate at -1
                GenerateObstacle(decoration1, position);
            }

            if (typeOfSnow == 2)
            {
                //generate at -1
                GenerateObstacle(decoration3, position);
            }


        }

    }
}
