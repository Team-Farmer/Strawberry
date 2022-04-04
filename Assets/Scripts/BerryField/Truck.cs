using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Truck : MonoBehaviour
{
    public GameObject MaxPanel;    
    //public int berryCnt = 0; // 옮김
    private Animator anim;   
    
    public const int CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    void Awake()
    {
        anim = GetComponent<Animator>();          
    }
    void FixedUpdate()
    {
        if(CNT_LEVEL_0 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_1)
        {
            if (anim.GetInteger("Truck") == 0) return;
            MaxPanel.SetActive(false);
            SetAnim(0);
        }
        if (CNT_LEVEL_1 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_2)
        {
            if (anim.GetInteger("Truck") == 1) return;
            SetAnim(1);
        }
        if (CNT_LEVEL_2 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_MAX)
        {
            if (anim.GetInteger("Truck") == 2) return;
            SetAnim(2);
        }
        if (DataController.instance.gameData.truckBerryCnt == CNT_LEVEL_MAX)
        {
            if (anim.GetInteger("Truck") == 3) return;
            MaxPanel.SetActive(true);
            SetAnim(3);
        }      
    }    
    void SetAnim(int level)
    {
        anim.SetInteger("Truck", level);
    }
    public void ReceiveCoinNormal()
    {
        //DataController.instance.gameData.coin += DataController.instance.gameData.truckCoin;
        GameManager.instance.GetCoin(DataController.instance.gameData.truckCoin);
        //Debug.Log(DataController.instance.gameData.accCoin);      // 누적 코인 테스트
        DataController.instance.gameData.truckBerryCnt = 0;
        DataController.instance.gameData.truckCoin = 0;
    }
}
