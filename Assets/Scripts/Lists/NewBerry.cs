using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBerry : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradePrice;
    [SerializeField]
    private GameObject upgradeTimer;

    
    [Header("=====INFO=====")]
    public int[] upgradePrice_;//업그레이드에 필요한 가격 배열
    public int[] upgradeTime_;//업그레이드에 필요한 시간 배열


    private int berryUpgradeLevel=0;

    void Start()
    {
        updateInfo(berryUpgradeLevel);
    }


    void Update()
    {
        
    }

    public void newBerryAdd() 
    {
        //타이머가 0 이라면 
        if (upgradeTime_[0] == 0)
        {
            //새로운 딸기가 추가된다.
            Debug.Log("새로운 딸기!!");

            //금액이 빠져나간다.(지금은 일단 하나로 통일)
            GameManager.instance.coin -= upgradePrice_[0];
            GameManager.instance.ShowCoinText(GameManager.instance.coin);

            //업스레이드 레벨 상승 -> 그 다음 업그레이드 금액이 보인다.
            berryUpgradeLevel++;
            updateInfo(berryUpgradeLevel);

            //타이머가 시작된다.(지금은 일단 10초)
            //버튼을 누르지 못한다.

        }
        else 
        {
            Debug.Log("새로운 딸기를 위해 조금 더 기다리세요");
        
        }

    }

    public void updateInfo(int index) {

        try
        {
            upgradePrice.GetComponent<Text>().text = upgradePrice_[index].ToString();
            upgradeTimer.GetComponent<Text>().text = upgradeTime_[index].ToString();
        }
        catch{
            Debug.Log("다음 레벨 정보 없음");
            //버튼 누르지 못하게 하기
        }
    }
}
