using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
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
    private int nbrPresets = 25;
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
        {Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.jumpObs1, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing}
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

    void GenerateObstacle(GameObject gameobject, Vector3 offset)
    {
        GameObject instance = Instantiate(gameobject, gameObject.transform.position + offset, gameobject.transform.rotation);
        terrain.AttachProp(instance.transform);
        if (instance.transform.position.x > 6 || instance.transform.position.x < -6)
        {
            instance.transform.SetParent(null);
            Destroy(instance);
            Debug.Log("Destroyed");
        }
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

            for (int i = 0; i < maxObjectsOnASlice; ++i)
            {
                if (array[typeOfTerrain, i] == Obstacle.jumpObs1)
                {
                    ReplaceFloor(porthole1, i);
                }

                else if (array[typeOfTerrain, i] == Obstacle.jumpObs2 && i % 2 == 1)
                {
                    ReplaceFloorLarge(porthole2, i);
                }

                else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                {
                    GenerateObstacle(cone, slice.GetChild(i).transform.position + Vector3.up * 0.2f);
                }

                else if (array[typeOfTerrain, i] == Obstacle.beer)
                {
                    if (beerDelay >= distanceBetweenBeers)
                    {
                        GenerateObstacle(powerUp, slice.GetChild(i).transform.position + Vector3.up * 0.2f);
                        foundBeer = true;
                        beerDelay = 0;
                    }
                    else
                        this.GenerateObstacle(cone, slice.GetChild(i).transform.position + Vector3.up * 0.2f);
                }

                else if (array[typeOfTerrain, i] == Obstacle.jumpBeerObs)
                {
                    ReplaceFloor(porthole1, i);
                    if (beerDelay >= distanceBetweenBeers)
                    {
                        GameObject instance2 = Instantiate(powerUp, slice.GetChild(i).transform.position + Vector3.up * 2, Quaternion.identity);
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
}
