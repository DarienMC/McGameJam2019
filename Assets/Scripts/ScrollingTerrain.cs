using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public PropManager propManager;
    public float scrollingSpeed = -1.0f;
    public int numberOfSlices = 100;
    public int bufferedSlices = 20;
    public float sliceLength = 1.8f;
    public GameObject terrainSlice;
    public int obstacleLinesMinDistance = 3;

    private Player2Movement player2Movement;

    public enum ScrollModificationType { SpeedUp, SlowDown, BackToNormal}
    
    private int currentSliceLine = 0;
    private float scrollSpeedMultiplier = 1;
    private bool scrollSpeedModified = false;
    private float scrollSpeedModifiedTimer;
    internal Transform previousSlice = null;
    private Vector3 firstSlicePosition;

    public int buildingSize = 5;
    public GameObject[] buildings;
    private int currentBuildingIndex = 0;

    private bool spawningObstacles = false;

    void Awake()
    {
        propManager = GetComponent<PropManager>();
        player2Movement = FindObjectOfType<Player2Movement>();

        firstSlicePosition = Vector3.forward * ((numberOfSlices - bufferedSlices) * sliceLength);
        for (int i = 1; i <= numberOfSlices; ++i)
        {
            InstantiateSlice();
        }
        spawningObstacles = true;
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
        // Move all the terrain slices
        foreach (Transform child in transform)
        {
            // Scrolling
            child.Translate(0, 0, -scrollingSpeed * Time.deltaTime * scrollSpeedMultiplier);
            
            // Check if beyond sight range
            if (child.transform.position.z > firstSlicePosition.z)
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
        // Generate terrain slice
        GameObject instance = Instantiate(terrainSlice);
        if (previousSlice == null)
        {
            instance.transform.position = firstSlicePosition;
        }
        else
        {
            instance.transform.position = previousSlice.position + Vector3.back * sliceLength;
        }
        instance.transform.SetParent(transform);
        previousSlice = instance.transform;
        
        // Generate obstacles
        if (spawningObstacles)
        {
            currentSliceLine = (currentSliceLine + 1) % obstacleLinesMinDistance;
            if (currentSliceLine == 0)
            {
                propManager.GenerateLine(instance.transform);
            }
        }

        // Generate buildings
        currentBuildingIndex = (currentBuildingIndex + 1) % buildingSize;
        if (currentBuildingIndex == 0)
        {
            GameObject building = buildings[Random.Range(0, buildings.Length)];
            Vector3 buildingPosition = instance.transform.position + building.transform.position + Vector3.right * 12.5f;
            GameObject buildingInstance = Instantiate(building, buildingPosition, building.transform.rotation);
            buildingInstance.transform.SetParent(instance.transform);

            building = buildings[Random.Range(0, buildings.Length)];
            buildingPosition = instance.transform.position + building.transform.position + Vector3.left * 12.5f;
            buildingInstance = Instantiate(building, buildingPosition, building.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
            buildingInstance.transform.SetParent(instance.transform);
        }
        
        propManager.GenerateDecoration(instance.transform);
    }

    // Attach the prop to the terrain so that it moves along with it.
    public void AttachProp(Transform prop)
    {
        prop.SetParent(previousSlice.transform);
    }

    public void ModifyScrollSpeed(float speedMultiplier, float time, ScrollModificationType speedModificationType, DelegateTimer timerCallback)
    {
        scrollSpeedMultiplier = speedMultiplier;
        scrollSpeedModified = true;
        scrollSpeedModifiedTimer = time;

        if (ScrollModificationType.SpeedUp == speedModificationType)
            player2Movement.MoveForwards(time, timerCallback);
        else if(ScrollModificationType.SlowDown == speedModificationType)
        {
            player2Movement.MoveBackwards(time, timerCallback);
        }
        else if(ScrollModificationType.BackToNormal == speedModificationType)
        {
            player2Movement.BackToNormal();
        }
    }
}
