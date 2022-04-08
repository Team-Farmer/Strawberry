using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rachel: MonoBehaviour
{
    void FixedUpdate()
    {
        for(int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if(!DataController.instance.gameData.berryFieldData[i].isPlant)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // 심는다                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // 체크 변수 갱신
            }
        }
    }
}
