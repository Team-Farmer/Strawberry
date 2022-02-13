using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    [Serializable]
    public struct PrefabStruct
    {
        public string Name,Explanation;
        public int Price, Level;

        public PrefabStruct(string Name,string Explanation, int Price, int Level)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Level = Level;//PTJ이라면 고용여부 의미. 0이 고용안함 1이 고용함
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;

    //Research Info  적용할 것들
    [Header("==========INFO 적용할 대상=========")]
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //gameManager Script
    GameManager gm1;

    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;


    //===================================================================================================
    void Start()
    {
        gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
    }
    void Update()
    {
       
    }

    //===================================================================================================
    
    
    //coin 버튼 -> 연구 레벨, 코인 변화
    public void clickCoin() {

        //레벨이 올라간다.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //해당 금액의 코인이 감소된다.
        gm1.coin -= Info[prefabnum].Price;
        gm1.ShowCoinText(gm1.coin);
        //gm1.CoinText.text = gm1.coin.ToString() + " A";

    }


    //coin 버튼 -> 알바 레벨, 고용 여부
    public void clickCoin_PTJ() 
    {

        if (Info[prefabnum].Level == 0) //고용 중이 아니라면 !!!!!!!!!!!!왜 prefabnum말로 0을 넣어도 되는가
        {
            //해당 금액의 코인이 감소된다.
            gm1.coin -= Info[prefabnum].Price;
            gm1.ShowCoinText(gm1.coin);
            //gm1.CoinText.text = gm1.coin.ToString() + " A";

            Info[prefabnum].Level = 1;
            levelNum.GetComponent<Text>().text = "고용중";

        }
        else //고용중이라면
        {
            Info[prefabnum].Level = 0;
            levelNum.GetComponent<Text>().text = "무직";
        }

    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!주의!!!!!!!!!!!!!7 저 숫자는 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;

        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();

        if (Info[Prefabcount].Level == 0)//level이 0이라면 PTJ과 관련된 것이다.
        {    levelNum.GetComponent<Text>().text = "무직";    }
        else
        {    levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();    }

        Debug.Log(prefabnum);
        Prefabcount++;
    }
}
