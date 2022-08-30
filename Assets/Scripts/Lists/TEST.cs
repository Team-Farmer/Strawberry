using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // 테스트용으로 버튼누르면 딸기 다 개발되는거를 위한 스크립트 입니다.
    private GameObject global;
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global");
    }

    public void Get(int berryClassify) 
    {
        int start=0, end=0;

        switch (berryClassify) 
        {
            case 0: start = 0;end = 64;break;
            case 1: start = 64;end = 128;break;
            case 2: start = 128; end = 192;break;
        }

        for (int i = start; i < end; i++)
        {
            if (global.GetComponent<Globalvariable>().berryListAll[i] != null) 
            {
                if (DataController.instance.gameData.isBerryUnlock[i] == false)
                {
                    DataController.instance.gameData.isBerryUnlock[i] = true;
                    DataController.instance.gameData.unlockBerryCnt++;
                }
            }
        }
    
    }
}
