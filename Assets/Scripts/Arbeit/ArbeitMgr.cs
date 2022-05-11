using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbeitMgr : MonoBehaviour
{   
    public bool OnRachel;
    public bool OnThomson;
    public bool OnHamsworth;
    public bool OnFubo;

    private float delay;
    void FixedUpdate()
    {
        delay += Time.deltaTime;
        if(delay >= 0.5f)
        {
            delay = 0f;
            Rachel();
            Thomson();
            Fubo();
            Hamsworth();
        }
    }
    void Rachel()//Åä³¢ 0
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[0] > 0)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // ½É´Â´Ù                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // Ã¼Å© º¯¼ö °»½Å
                DataController.instance.gameData.PTJNum[0]--;
                //Debug.Log("·¹ÀÌÃ¿ ³²Àº È½¼ö: " + DataController.instance.gameData.PTJNum[0]);
                break;
            }
        }
    }
    void Thomson()//°õ 1
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4] &&
                !GameManager.instance.farmList[i].isHarvest &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[1] > 0)
            {
                GameManager.instance.Harvest(GameManager.instance.stemList[i]); // ¼öÈ®ÇÑ´Ù
                DataController.instance.gameData.PTJNum[1]--;
                //Debug.Log("Åè½¼ ³²Àº È½¼ö: " + DataController.instance.gameData.PTJNum[1]);
                break;
            }
        }
    }
    public float Pigma() // µÅÁö 2
    {
        if (DataController.instance.gameData.PTJNum[2] > 0)
        {
            float coEffi = Random.Range(0.7f, 1.8f);
            DataController.instance.gameData.PTJNum[2]--;

            Debug.Log(coEffi);
            return coEffi;
        }
        else return 1.0f;
    }
    void Hamsworth()//ÇÜ½ºÅÍ 3
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[3] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasBug = false;
                DataController.instance.gameData.PTJNum[3]--;
                //Debug.Log("Çªº¸ ³²Àº È½¼ö: " + DataController.instance.gameData.PTJNum[4]);
                StartCoroutine(DeleteBugByHamsworth(GameManager.instance.bugList[i]));
                break;
            }
        }
    }
    IEnumerator DeleteBugByHamsworth(Bug bug)
    {
        yield return new WaitForSeconds(0.75f);
        bug.DieBug();
    }

    void Fubo()//°­¾ÆÁö 4
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed &&
                DataController.instance.gameData.PTJNum[4] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasWeed = false;
                DataController.instance.gameData.PTJNum[4]--;
                //Debug.Log("ÇÜ½º¿ö½º ³²Àº È½¼ö: " + DataController.instance.gameData.PTJNum[3]);
                StartCoroutine(DeleteWeedByFubo(GameManager.instance.farmList[i]));
                break;
            }
        }
    }
    IEnumerator DeleteWeedByFubo(Farm farm)
    {
        yield return new WaitForSeconds(0.75f);
        farm.weed.DeleteWeed();
    }
    
    public int lluvia() // °í¾çÀÌ 5
    {
        if (DataController.instance.gameData.PTJNum[5] > 0)
        {
            DataController.instance.gameData.PTJNum[5]--;           
            return 4;
        }
        else
        {           
            return 2;
        }
    }
}
