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
        // 여기에 미니게임 중인지 확인하는 변수 추가 할수도?
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
    void Rachel()//토끼 0
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[0] > 0)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // 심는다                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // 체크 변수 갱신
                DataController.instance.gameData.PTJNum[0]--;
                //Debug.Log("레이첼 남은 횟수: " + DataController.instance.gameData.PTJNum[0]);
                break;
            }
        }
    }
    void Thomson()//곰 1
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4] &&
                !GameManager.instance.farmList[i].isHarvest &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[1] > 0)
            {

                StartCoroutine(HarvestbyThomson(i));
                //Debug.Log("톰슨 남은 횟수: " + DataController.instance.gameData.PTJNum[1]);
                break;
            }
        }
    }
    IEnumerator HarvestbyThomson(int idx)
    {
        yield return new WaitForSeconds(0.25f);

        GameManager.instance.Harvest(GameManager.instance.stemList[idx]); // 수확한다
        DataController.instance.gameData.PTJNum[1]--;
    }  
    void Hamsworth()//햄스터 2
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[2] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasBug = false;
                DataController.instance.gameData.PTJNum[2]--;
                //Debug.Log("푸보 남은 횟수: " + DataController.instance.gameData.PTJNum[4]);
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

    void Fubo()//강아지 3
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed &&
                DataController.instance.gameData.PTJNum[3] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasWeed = false;
                DataController.instance.gameData.PTJNum[3]--;
                //Debug.Log("햄스워스 남은 횟수: " + DataController.instance.gameData.PTJNum[3]);
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
    public float Pigma() // 돼지 4
    {
        if (DataController.instance.gameData.PTJNum[4] > 0)
        {
            float coEffi = Random.Range(0.7f, 1.8f);
            DataController.instance.gameData.PTJNum[4]--;

            Debug.Log(coEffi);
            return coEffi;
        }
        else return 1.0f;
    }
    public int lluvia() // 고양이 5
    {
        if (DataController.instance.gameData.PTJNum[5] > 0)
        {
            DataController.instance.gameData.PTJNum[5]--;           
            return 3;
        }
        else
        {           
            return 2;
        }
    }
}
