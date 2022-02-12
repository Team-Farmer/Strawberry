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
            this.Level = Level;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;


    //gameManager Script
    GameManager gm1;

    //추가 된 Prefab 수
    static int Prefabcount=0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    //Research Info  적용할 것들=====================================================================
    [Header("==========INFO 적용할 대상=========")]
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;
      


    void Start()
    {
        gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
    }


    void Update()
    {
       
    }


    //coin 버튼을 누르면 실행
    public void clickCoin() {

        //레벨이 올라간다.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //해당 금액의 코인이 감소된다.
        gm1.coin -= Info[prefabnum].Price;
        gm1.CoinText.text = gm1.coin.ToString() + " A";

    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!주의!!!!!!!!!!!!!7 저 숫자는 프리팹 숫자와 관련되어 있다!!!
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;

        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();
        levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();
        //prefabnum = Prefabcount;
        //Prefabcount++;

        
        Prefabcount++;
    }
}
