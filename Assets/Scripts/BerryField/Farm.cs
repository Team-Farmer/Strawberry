using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Weed weed; // ¾êµµ ¹è¿­·Î ÇØÁà¾ßÇÒµí? ¹ú·¹ °°ÀÌ

    public bool isPlant = false;
    public int farmIdx;
    public bool isHarvest = false;
    
    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 12f;  // À§¿¡ ´Ù ¿Å±è  
    
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
