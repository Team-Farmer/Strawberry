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

    private GameObject PTJExplanations;

    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    
    //몇명 고용중인지 확인
    static int employCount = 0;
    //고용중인 알바생 명단
    static List<Sprite> workingList = new List<Sprite>();


    private GameObject PTJExplanation;
    private GameObject PTJExp;
    private GameObject PTJSlider;
    //===================================================================================================
    void Start()
    {
        if (PTJ == true)
        {
            PTJExplanations = GameObject.FindGameObjectWithTag("PTJExplanation");
            PTJExplanation = transform.GetChild(prefabnum).gameObject;
            PTJExp = PTJExplanation.transform.GetChild(0).gameObject;
            PTJSlider = PTJExp.transform.GetChild(7).transform.gameObject;
        }
        InfoUpdate();
    }
    //===================================================================================================

    //coin 버튼 -> 연구 레벨, 코인
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


    //coin 버튼 -> 알바 고용
    public void clickCoin_PTJ(int ID, int num) 
    {
        //ID=prefabnum
        Debug.Log("ID="+ID+"  num="+num);
        //DataController.instance.gameData.PTJNum[ID] = num;//해당 ID가 n번 고용되었는지 저장


        //고용중이면 고용해제
        if (Info[ID].isEmployed == true) { fire(ID); }
        //고용중이 아니라면 고용
        else
        {
            if (employCount < 3)//3명 이하일 때
            {
                hire(ID,num);
            }
            else //3명 이상일 때
            {   Debug.Log("이미 3명이 고용중입니다.");   }
        }
    }


    private void hire(int ID,int num)
    {
        //해당 금액의 코인이 감소
        GameManager.instance.UseCoin(Info[ID].Price);

        DataController.instance.gameData.PTJNum[ID] = num;

        //고용상태임을 시각적으로 보이기=======================================
        Info[ID].isEmployed = true;//고용
        levelNum.GetComponent<Text>().text = "고용 중";//고용중으로 표시
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 글자색 변경
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//배경 스프라이트 눌린 이미지로 변경

        //main game에 고용중인 알바생 보이기======================
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);//해당 알바생 얼굴 리스트에 추가
        GameManager.instance.workingApply(workingList);//GameManager workingApply에 고용 list 보냄

        //알바생 숫자=============================================
        ++employCount;//고용중인 알바생 숫자 증가
        GameManager.instance.workingCount(employCount);//알바생 숫자 보여준다

        //ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value);//Explanation창에 고용해제라고 보이기
    }

    private void fire(int ID) 
    {
        DataController.instance.gameData.PTJNum[ID] = 0;
        Info[ID].isEmployed = false;//0=무직
        levelNum.GetComponent<Text>().text = "고용 전";//고용 전으로 표시
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);//글자색 회색으로
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;//배경 스프라이트 원래대로
       
        --employCount;

        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);
        //ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value);

    }
    //=============================================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 6은 프리팹 숫자와 관련되어 있다!!! 같이 조절 . 변수설정하기
        
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

    //=============================================================================================================================
    public void Explanation() {

        PTJExp.SetActive(true);//설명창 띄우기
        try
        {
            //Explanation 내용을 채운다.
            PTJExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Image>().sprite
                = Info[prefabnum].Picture;//얼굴 사진

            PTJExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Name;//이름 텍스트

            PTJExp.transform.GetChild(4).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Explanation;//설명 텍스트 
            
            
            

            ExplanationUpdate(1);//처음에는 1번 고용으로 보임
            PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
                (delegate { ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value); });//슬라이더 값 바뀔때마다


            PTJExp.transform.GetChild(5).transform.GetComponent<Button>().onClick.AddListener
                (delegate { clickCoin_PTJ(prefabnum, (int)PTJSlider.GetComponent<Slider>().value); });//결제 버튼 누름

            //여기까지는 정상적
        }
        catch
        {
            Debug.Log("PTJExplanation 인덱스");
        }
    }



    public void ExplanationUpdate(int value) 
    {
        if (workingList.Contains(Info[prefabnum].FacePicture))//이미 고용중이라면 (고용 리스트에 이미 알바생이 있으면)
        {
            //고용해제 버튼
            PTJExp.transform.GetChild(6).transform.gameObject.GetComponentInChildren<Text>().text
                = "";
            PTJExp.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text
                = "고용해제";
        }
        else//고용 중이 아니라면
        {
            //고용 정보 보이기
            PTJExp.transform.GetChild(6).transform.gameObject.GetComponentInChildren<Text>().text
                = (Info[prefabnum].Price * value).ToString() + "A";//결제할 가격 보이기
            PTJExp.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text
           = value.ToString() + "번 고용";//몇번 고용하는지
        }
    }


}
