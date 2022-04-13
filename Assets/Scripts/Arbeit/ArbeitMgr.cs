using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbeitMgr : MonoBehaviour
{   
    public bool OnRachel;
    public bool OnThomson;
    public bool OnHamsworth;
    public bool OnFubo;

    void FixedUpdate()
    {
        if(OnRachel) Rachel();

        if (OnThomson) Thomson();
         
        if (OnHamsworth) Hamsworth();

        if (OnFubo) Fubo();

    }
    void Rachel()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // 심는다                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // 체크 변수 갱신
            }
        }
    }
    void Thomson()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4] &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug)
            {
                GameManager.instance.Harvest(GameManager.instance.stemList[i]); // 수확한다                                        
            }
        }
    }
    void Hamsworth()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                StartCoroutine(DeleteWeedByHamsworth(GameManager.instance.farmList[i]));
            }
        }
    }
    IEnumerator DeleteWeedByHamsworth(Farm farm)
    {
        yield return new WaitForSeconds(0.75f);
        farm.weed.DeleteWeed();
    }
    void Fubo()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasBug)
            {
                StartCoroutine(DeleteBugByFubo(GameManager.instance.bugList[i]));
            }
        }
    }
    IEnumerator DeleteBugByFubo(Bug bug)
    {
        yield return new WaitForSeconds(0.75f);
        bug.DieBug();
    }
}
