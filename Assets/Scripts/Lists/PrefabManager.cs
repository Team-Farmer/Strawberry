using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
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

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;




    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    
    //몇명 고용중인지 확인
    static int employCount = 0;
    



    //===================================================================================================
    void Start()
    {
        
        InfoUpdate();
    }
    void Update()
    {
       
    }

    //===================================================================================================
    
    //coin 버튼 -> 연구 레벨, 코인 변화
    public void clickCoin_Research() {

        //레벨이 올라간다.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //해당 금액의 코인이 감소된다.
        GameManager.instance.coin -= Info[prefabnum].Price;
        GameManager.instance.ShowCoinText(GameManager.instance.coin);
        

    }


    //coin 버튼 -> 알바 고용 여부
    public void clickCoin_PTJ() 
    {
        if (PTJ == true)//그냥 한번더 확인
        {

            if (employCount < 3)//3명 이하면 선택 가능
            {
                if (Info[prefabnum].Level == 0) //고용 중아니면 고용가능            //의문점 = 왜 prefabnum말로 0을 넣어도 되는가
                {
                    //해당 금액의 코인이 감소된다.
                    GameManager.instance.coin -= Info[prefabnum].Price;
                    GameManager.instance.ShowCoinText(GameManager.instance.coin);

                    ++employCount;
                    Info[prefabnum].Level = 1;//1=고용
                    levelNum.GetComponent<Text>().text = "고용";

                }
                else //고용중이라면 무직으로 변경
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=무직
                    levelNum.GetComponent<Text>().text = "무직";

                }
            }
            else 
            {
                if (Info[prefabnum].Level == 1) 
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=무직
                    levelNum.GetComponent<Text>().text = "무직";
                }
                Debug.Log("3명이 넘게 고용하지 못합니다."); 
            }


            //Debug.Log("count="+employCount);
            //Debug.Log("employ=" + Info[prefabnum].Level);
        }
    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 7은 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;


        //타이틀, 설명, 코인값, 레벨, 고용여부 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();
        
        if (PTJ==true)
        {    levelNum.GetComponent<Text>().text = " ";    }
        else
        {    levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();    }


        
        Prefabcount++;
    }
}
