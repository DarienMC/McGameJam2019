using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTerrain : MonoBehaviour
{
    public float scrollingSpeed = -1.0f;
    public int numberOfSlices = 2;
    public GameObject terrainSlice;

    public GameObject upcomingSlice;

    private Player2Movement player2Movement;

    public enum ScrollModificationType { SpeedUp, SlowDown, BackToNormal}
    private float sliceLength;
    
    private float scrollSpeedMultiplier = 1;
    private bool scrollSpeedModified = false;
    private float scrollSpeedModifiedTimer;

    void Start()
    {
        player2Movement = FindObjectOfType<Player2Movement>();

        for (int i = 1; i <= numberOfSlices; ++i)
        {
            InstantiateSlice();
        }
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
            sliceLength = instance.GetComponent<BoxCollider>().bounds.size.z;
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
        prop.transform.position = new Vector3(prop.transform.position.x * upcomingSlice.transform.localScale.x, prop.transform.position.y * upcomingSlice.transform.localScale.y, prop.transform.position.z * upcomingSlice.transform.localScale.z);
        prop.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
        // prop.transform.localScale = new Vector3( prop.transform.localScale.x * upcomingSlice.transform.localScale.x , prop.transform.localScale.y, prop.transform.localScale.z);
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
