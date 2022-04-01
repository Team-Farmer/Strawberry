using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJResearch : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
    {
        public string Name;//알바생 이름, 연구 제목
        public Sprite Picture;//사진
        public Sprite FacePicture;//알바생 얼굴 사진
        public string Explanation;//설명
        public int Price;//가격
        public bool isEmployed;//고용 중 인가


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
    PrefabStruct[] Info;//구조체

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
    public bool PTJ;//지금 알바생인가 확인

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


    private GameObject PTJExplanation;

    //===================================================================================================
    void Start()
    {
        PTJExplanation = GameObject.Find("PTJExplanation");//gameManager로 뺴면 FInd필요없음 근데 너무 음
        InfoUpdate();
    }

    //===================================================================================================
    
    //coin 버튼 -> 연구 레벨, 코인 변화
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//레벨 25로 한계두기
        {
            //레벨이 올라간다.
            DataController.instance.gameData.researchLevel[prefabnum]++;
            levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
        
        //해당 금액의 코인이 감소
        GameManager.instance.UseCoin(Info[prefabnum].Price);
        
        }
    }


    //coin 버튼 -> 알바 고용 여부
    public void clickCoin_PTJ() 
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


    private void hire()  //의문점 = 왜 prefabnum말로 0을 넣어도 되는가
    {
        //해당 금액의 코인이 감소
        GameManager.instance.UseCoin(Info[prefabnum].Price);
        
        Info[prefabnum].isEmployed = true;//고용
        levelNum.GetComponent<Text>().text = "고용 중";//고용중으로 표시
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 글자색 변경
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//배경 스프라이트 눌린 이미지로 변경

        workingList.Remove(null);
        workingList.Add(Info[prefabnum].FacePicture);//해당 알바생 얼굴 리스트에 추가
        GameManager.instance.workingApply(workingList);//GameManager workingApply에 고용 list 보냄


        ++employCount;//고용중인 알바생 숫자 증가
        GameManager.instance.workingCount(employCount);//알바생 숫자 보여준다
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
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//제목(이름) 표시
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//그림 표시
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;//설명 텍스트 표시
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString()+"A";//비용 표시
        

        if (PTJ==true)//알바
        {    levelNum.GetComponent<Text>().text = "고용 전";    }
        else//연구
        {    levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();    }


        
        Prefabcount++;
    }

    public void Explanation() {

        GameObject PTJExp = PTJExplanation.transform.GetChild(0).gameObject;

        PTJExp.SetActive(true);
        try
        {
            //Explanation 내용을 채운다.
            PTJExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Image>().sprite
                = Info[prefabnum].Picture;//얼굴 사진

            PTJExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Name;//이름 텍스트

            PTJExp.transform.GetChild(4).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Explanation;//설명 텍스트 

        }
        catch
        {
            Debug.Log("PTJExplanation 인덱스");
        }


    }

}
