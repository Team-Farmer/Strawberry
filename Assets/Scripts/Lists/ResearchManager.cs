using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    //추가 된 Prefab 수
    static int Prefabcount=0;

    //Research Info  적용할 것들
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //Research Info
    //text파일로 저장한 후 TextAsset으로 읽어와도 되긴하다. 근데 일단 수가 적으니까
    string[] ResearchName = { "딸기 가치 상승", "딸기 성장기간 감소", "트럭 수익 상승", "비가 온다!", "잡초 감소", "벌레 감소", "미정" };
    string[] ResearchExplanation = { "딸기의 가치가 5%->10% 높아진다", " 딸기의 성장시간이 5%->10% 감소한다",
            "트럭으로 딸기를 판매할 때 5%->10% 높은 가격으로 판매한다.","소나기가 내릴 확률이 5%->10% 증가한다.",
            "잡초가 생길 확률이 5%->10% 감소한다.","벌레가 생길 확률이 5%->10% 감소한다.","미정"};
    int[] ResearchPrice = { 100, 200, 200, 300, 300, 300, 0 };
    int[] ResearchLevel = { 3, 1, 1, 1, 1, 1, 0 };

    


    void Start()
    {
        
        titleText.GetComponent<Text>().text = ResearchName[Prefabcount];
        explanationText.GetComponent<Text>().text = ResearchExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = ResearchPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = ResearchLevel[Prefabcount].ToString();
        Prefabcount++;

    }

    
    void Update()
    {
        //ListAdd에서 count변수 값을 받아온다.
        //count변수값을 통해 현재 리스트 내용 구성

        //count=0이면 level 배열 0번째 정보를 리스트 level text에 넣는다
        
    }
}
