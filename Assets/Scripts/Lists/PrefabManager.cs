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
        public string Name;
        public Sprite Picture;
        public Sprite FacePicture;
        public string Explanation;
        public int Price;
        public bool isEmployed;


        public PrefabStruct(string Name,string Explanation, int Price, Sprite Picture, Sprite FacePicture, bool isEmployed, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
            this.isEmployed=isEmployed;

            
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;

    //Research Info  적용할 것들
    [Header("==========INFO 적용할 대상=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject titleText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;
    [Header("==========select PTJ===========")]
    [SerializeField]
    private Sprite selectPTJSprite;
    [SerializeField]
    private Sprite originalPTJSprite;


    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    
    //몇명 고용중인지 확인
    static int employCount = 0;

    static List<Sprite> workingList = new List<Sprite>();
    


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

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//레벨 25로 한계두기
        {
            //레벨이 올라간다.
            DataController.instance.gameData.researchLevel[prefabnum]++;
            levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
        
        //해당 금액의 코인이 감소된다.
        //GameManager.instance.coin -= Info[prefabnum].Price;
        DataController.instance.gameData.coin -= Info[prefabnum].Price;
        //GameManager.instance.ShowCoinText(GameManager.instance.coin);
        GameManager.instance.ShowCoinText();
        }
    }


    //coin 버튼 -> 알바 고용 여부
    public void clickCoin_PTJ() 
    {
        if (PTJ == true)//그냥 한번더 확인
        {
            if (employCount < 3)//3명 이하일 때
            {
                if (Info[prefabnum].isEmployed == false) //고용중아니면 고용           
                {   hire();   }
                else //고용중이면 고용해제
                {   fire();   }
            }
            else //3명 이상일 때
            {
                if (Info[prefabnum].isEmployed == true)//고용중이면 고용해제
                {   fire();   }
            }
        }
    }


    private void hire()  //의문점 = 왜 prefabnum말로 0을 넣어도 되는가
    {
        //해당 금액의 코인이 감소된다.
        //GameManager.instance.coin -= Info[prefabnum].Price;
        //GameManager.instance.ShowCoinText(GameManager.instance.coin);
        DataController.instance.gameData.coin -= Info[prefabnum].Price;
        //GameManager.instance.UseCoin(Info[prefabnum].Price); // 함수로 바꿔봄
        GameManager.instance.ShowCoinText();


        Info[prefabnum].isEmployed = true;//고용
        levelNum.GetComponent<Text>().text = "고용 중";//고용중으로 표시
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 글자색 변경
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//배경스프라이트 눌림으로 변경

        workingList.Remove(null);
        workingList.Add(Info[prefabnum].FacePicture);//해당 얼굴 리스트에 추가
        GameManager.instance.workingApply(workingList);//GameManager workingApply에 고용중인 사진 list 보냄


        ++employCount;
        GameManager.instance.workingCount(employCount);
    }
    private void fire() 
    {
        
        Info[prefabnum].isEmployed = false;//0=무직
        levelNum.GetComponent<Text>().text = "고용 전";//고용 전으로 표시
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);//글자색 회색으로
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;//배경 스프라이트 원래대로
       
        --employCount;

        workingList.Remove(Info[prefabnum].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

    }

    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 7은 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6)
        { Prefabcount -= 6; }
        prefabnum = Prefabcount;


        //타이틀, 설명, 코인값, 레벨, 고용여부 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString()+"A";
        
        if (PTJ==true)
        {    levelNum.GetComponent<Text>().text = "고용 전";    }
        else
        {    levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();    }


        
        Prefabcount++;
    }


}
