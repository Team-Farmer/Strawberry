using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public bool isPlant = false;
    public int farmIdx;
    public bool isHarvest = false;
    public Weed weed;

    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 12f;    
    
    void Awake()
    {
           
    }    
    void Update()
    {
        if (hasWeed || !canGrowWeed) return;

        if (weedTime <= period)
        {
            weedTime += Time.deltaTime;
        }
        else
        {
            weed.GenerateWeed();
            weedTime = 0f;
        }
    }
    
}
