using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Truck : MonoBehaviour
{
    public GameObject MaxPanel;
    public Button normal_receive_btn;
    public Button add_receive_btn;
    public Sprite[] truckSprite;
    //public int berryCnt = 0; // 옮김
    private ArbeitMgr arbeit;
    private int truckSpirteLv;
    public const int CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    void Awake()
    {        
        arbeit = GameObject.FindGameObjectWithTag("Arbeit").GetComponent<ArbeitMgr>();

        //광고보고 3배받기
        add_receive_btn.onClick.AddListener(OnclickAdBtn);
    }
    void FixedUpdate()
    {       
        if (DataController.instance.gameData.truckBerryCnt == CNT_LEVEL_0) // 트럭 누적 딸기 개수가 0개라면
        {
            normal_receive_btn.interactable = false; // 받기 버튼을 비활성화
            add_receive_btn.interactable = false; // 광고 보고 받기 버튼을 비활성화
        }
        else // 트럭 누적 딸기 개수가 1개 이상이라면
        {
            if (!normal_receive_btn.interactable && !normal_receive_btn.interactable) // 버튼이 비활성화 돼있다면
            {
                normal_receive_btn.interactable = true; // 받기 버튼을 활성화
                add_receive_btn.interactable = true; // 광고 보고 받기 버튼을 활성화
            }
        }
        if (CNT_LEVEL_0 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_1)
        {
            if (truckSpirteLv == 0) return;
            truckSpirteLv = 0;
            GetComponent<Image>().sprite = truckSprite[0]; // 트럭 스프라이트를 빈 트럭으로 변경
            MaxPanel.SetActive(false); // MAX 패널 제거
        }
        if (CNT_LEVEL_1 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_2)
        {
            if (truckSpirteLv == 1) return;

            truckSpirteLv = 1;
            GetComponent<Image>().sprite = truckSprite[1];
        }
        if (CNT_LEVEL_2 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_MAX)
        {
            if (truckSpirteLv == 2) return;

            truckSpirteLv = 2;
            GetComponent<Image>().sprite = truckSprite[2];
        }
        if (DataController.instance.gameData.truckBerryCnt == CNT_LEVEL_MAX)
        {
            if (truckSpirteLv == 3) return;

            truckSpirteLv = 3;
            MaxPanel.SetActive(true);
            GetComponent<Image>().sprite = truckSprite[3];
        }
    }
    
    public void ReceiveCoinNormal()
    {
        float coEffi = arbeit.Pigma();
        float totalCoin = (DataController.instance.gameData.truckCoin
            + GameManager.instance.bonusTruckCoin) * coEffi;

        //Debug.Log(totalCoin);
        GameManager.instance.GetCoin((int)totalCoin);

        //Debug.Log("누적 출석 : " + DataController.instance.gameData.accCoin);      // 누적 코인 테스트
        DataController.instance.gameData.truckBerryCnt = 0;
        DataController.instance.gameData.truckCoin = 0;
    }

    //광고
    void OnclickAdBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveCoin3Times;
        RewardAd.instance.OnAdFailed += OnFailedAd;
        RewardAd.instance.ShowAd();
        add_receive_btn.interactable = false; // 광고 보고 받기 버튼을 비활성화
    }

    public void ReceiveCoin3Times()
    {
        float coEffi = arbeit.Pigma();
        float totalCoin = (DataController.instance.gameData.truckCoin
            + GameManager.instance.bonusTruckCoin) * coEffi * 3;

        Debug.Log($"광고보고 3배받기 {totalCoin}");
        GameManager.instance.GetCoin((int)totalCoin);

        DataController.instance.gameData.truckBerryCnt = 0;
        DataController.instance.gameData.truckCoin = 0;

        RewardAd.instance.OnAdComplete -= ReceiveCoin3Times;
        add_receive_btn.interactable = true; // 광고 보고 받기 버튼 활성
        RewardAd.instance.LoadAd();
    }

    void OnFailedAd()
    {
        RewardAd.instance.OnAdComplete -= ReceiveCoin3Times;
        RewardAd.instance.OnAdFailed -= OnFailedAd;
        add_receive_btn.interactable = true; // 광고 보고 받기 버튼 활성
    }
}
