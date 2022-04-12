using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thomson : MonoBehaviour
{
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= STEM_LEVEL_MAX &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug)
            {
                GameManager.instance.Harvest(GameManager.instance.stemList[i]); // 수확한다                                        
            }
        }
    }
}
