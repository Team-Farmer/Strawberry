using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJManager : MonoBehaviour
{
    //추가 된 Prefab 수
    static int Prefabcount = 0;

    //Research Info  적용할 것들
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //PTJ Info
    string[] PTJName = { "토끼", "곰", "돼지", "고양이", "햄스터", "강아지", "미정" };
    string[] PTJExplanation = { "딸기 자동심기", "딸기 자동 수확", "거래의 달인, 트럭 수익 상승", "비의 주술사, 소나기 내린다", "잡초 자동 뽑기", "벌레 자동 죽이기", "미정" };
    int[] PTJPrice = { 100, 200, 200, 300, 300, 300, 0 };
    int[] PTJLevel = { 3, 1, 1, 1, 1, 1, 0 };




    void Start()
    {
        titleText.GetComponent<Text>().text = PTJName[Prefabcount];
        explanationText.GetComponent<Text>().text = PTJExplanation[Prefabcount];
        coinNum.GetComponent<Text>().text = PTJPrice[Prefabcount].ToString();
        levelNum.GetComponent<Text>().text = PTJLevel[Prefabcount].ToString();
        Prefabcount++;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
