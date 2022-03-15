using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Weed weed;
    public int farmIdx;

    /*public bool isPlant = false;   
    public bool isHarvest = false;
    
    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 12f;  // À§¿¡ ´Ù ¿Å±è*/

    void Awake()
    {
           
    }    
    void Update()
    {
        if (DataController.instance.gameData.berryFieldData[farmIdx].hasWeed || !DataController.instance.gameData.berryFieldData[farmIdx].canGrowWeed) return;

        if (DataController.instance.gameData.berryFieldData[farmIdx].weedTime <= DataController.instance.gameData.berryFieldData[farmIdx].period)
        {
            DataController.instance.gameData.berryFieldData[farmIdx].weedTime += Time.deltaTime;
        }
        else
        {
            weed.GenerateWeed();
            DataController.instance.gameData.berryFieldData[farmIdx].weedTime = 0f;
        }
    }
    
}
