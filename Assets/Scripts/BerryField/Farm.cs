using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Weed weed;
    public int farmIdx;
          
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
