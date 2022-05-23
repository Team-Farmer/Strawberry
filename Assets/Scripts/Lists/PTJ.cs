using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJ : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
    {
        public string Name;//알바생 이름, 연구 제목
        public Sprite Picture;//사진
        public Sprite FacePicture;//알바생 얼굴 사진
        public string Explanation;//설명
        public int Price;//가격
        public int NowSliderNum;//현재 고른 고용 횟수

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture, int NowSliderNum)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
            this.NowSliderNum = NowSliderNum;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;//구조체

    //Research Info  적용할 것들
    [Header("==========INFO 적용할 대상=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject nameText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject employTF;


    [Header("==========PTJ select Sprite===========")]
    //PTJ고용중여부 보일 스프라이트
    public Sprite selectPTJSprite;
    public Sprite originalPTJSprite;
    [Header("==========PTJ Button Sprite===========")]
    public Sprite FireButtonSprite;
    public Sprite HireButtonSprite;



    [Header("==========PTJ Warning Panel===========")]
    public GameObject warningPanel;
    public GameObject warningBlackPanel;
    public GameObject noCoinPanel;
    public Text noCoinPanel_text;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;



    //==========Prefab별로 숫자 부여==========
    static int Prefabcount = 0;
    int prefabnum;

    //==========PTJ 창==========
    private GameObject PTJPanel;
    //==========PTJ Content==========
    private GameObject PTJContent;

    //==========현재 고용 중 인원==========
    static int employCount = 0;

    //==========현재 고용 중 명단==========
    static List<Sprite> workingList = new List<Sprite>();


    //==========PTJ Panel==========
    private GameObject PTJSlider;
    private GameObject PTJSlider10;
    private GameObject PTJToggle;

    private GameObject SliderNum;
    private GameObject price;

    private GameObject PTJButton;

    private GameObject HireNowLock;

    //==========고용 횟수==========
    private int PTJ_NUM_NOW;


    //???
    //비 파티클
    private ParticleSystem rainParticle;
    // 글로벌 변수
    private Globalvariable globalVar;


    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        
    }


    void Start()
    {  
        //프리팹들에게 고유 번호 붙이기
        prefabnum = Prefabcount;
        Prefabcount++;
        //==========PTJ Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(prefabnum).GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10단위 체크 토글
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1단위 슬라이더
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10단위 슬라이더

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;
        //==============================
        PTJContent = GameObject.FindGameObjectWithTag("PTJContent");

        HireYNPanel_yes.onClick.AddListener(BtnListener);

        //??============================
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

        PreafbInfoUpdate();
    }

    private void Update()
    {

        //자신의 고용횟수값 변경 파악
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

    }


    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;

            //변경된 값이 0(즉 알바를 끝냈다면)이면 아래를 실행
            if (PTJ_NUM_NOW == 0)
            {
                Debug.Log(prefabnum + "번 째 알바생 일 종료");
            }
            else 
            {
                HireNowLock.transform.GetChild(0).transform.GetComponent<Text>().text
                = "남은 고용 횟수: " + DataController.instance.gameData.PTJNum[DataController.instance.gameData.PTJSelectNum].ToString() + "회";

            }
            //고용해제 상태 적용
            EmployStateApply_Panel();
            EmployStateApply_Prefab();
        }
        get { return PTJ_NUM_NOW; }
    }

    //===================================================================================================
    //===================================================================================================
    public void PreafbInfoUpdate()
    {

        //====프리팹 내용 채우기====
        //불변====
        //이름
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //알바생 사진
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        //설명
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation;
        //비용
        GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);

        //가변====
        //고용 상태 반영
        EmployStateApply_Panel();
        EmployStateApply_Prefab();

    }

    //PTJPanel 띄우기
    public void PTJPanelActive()
    {
        //효과음
        AudioManager.instance.Cute1AudioPlay();
        //현재 선택 알바생
        DataController.instance.gameData.PTJSelectNum = prefabnum;
        
        
        //==== 알바 창 ====
        //알바 창을 띄운다.
        PTJPanel.SetActive(true);
        PTJPanel.transform.GetChild(0).gameObject.SetActive(true);

        //알바 창 채우기
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        name.GetComponent<Text>().text = Info[prefabnum].Name;
        explanation.GetComponent<Text>().text = Info[prefabnum].Explanation;

        //알바 슬라이더
        InitSlider();//슬라이더 초기화

        //Slider값 변경 적용 -> n회, 비용 반영
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider.GetComponent<Slider>().value); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider10.GetComponent<Slider>().value); });
        PTJToggle.GetComponent<Toggle>().onValueChanged.AddListener
            (delegate { ToggleChange(); });

        //알바 고용 상태 반영
        EmployStateApply_Panel();
        EmployStateApply_Prefab();
    }

    private void EmployStateApply_Panel()
    {

        //고용중이 아니다
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //버튼 이미지, 내용 변경
            PTJButton.transform.GetComponent<Image>().sprite = HireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "고용 하기";

            //HireNowLock 숨기기
            HireNowLock.SetActive(false);
        }
        //고용중이다
        else
        {
            //버튼 이미지, 내용 변경
            PTJButton.transform.GetComponent<Image>().sprite = FireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "";

            //HireNowLock 보이기
            HireNowLock.SetActive(true);
        }

    }

    private void EmployStateApply_Prefab() 
    {

        //고용 중이 아니다
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            PTJBackground.GetComponent<Image>().sprite = originalPTJSprite;
            employTF.GetComponent<Text>().text = "고용전";
            employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        }
        else 
        {
            PTJBackground.GetComponent<Image>().sprite = selectPTJSprite ;
            employTF.GetComponent<Text>().text = "고용중";
            employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        }
        
    }

    public void EmployButtonClick()
    {
        //지금 선택된 알바생
        //int nowPrefabNum = DataController.instance.gameData.PTJSelectNum;

        //효과음
        AudioManager.instance.Cute1AudioPlay();

        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //Hire(prefabnum, Info[prefabnum].NowSliderNum);
            Debug.Log("hire"+prefabnum);
        }
        else 
        {
            //DataController.instance.gameData.PTJNum[prefabnum] = 0;
            Debug.Log("fire"+prefabnum);
        }
        /*
        //=======================고용 or 해고=======================
        //3명 아래로 고용중이면
        if (employCount < 3)
        {
            //고용중이 아니면
            if (DataController.instance.gameData.PTJNum[nowPrefabNum] == 0)
            {
                if (Info[nowPrefabNum].Price <= DataController.instance.gameData.coin)
                {

                    //코인사용
                    //GameManager.instance.UseCoin(Info[ID].Price);

                    //HIRE
                    Hire(nowPrefabNum, Info[nowPrefabNum].NowSliderNum);

                }
                else
                { 
                    //효과음
                    AudioManager.instance.Cute4AudioPlay();
                    //재화 부족 경고 패널
                    GameManager.instance.ShowCoinText(noCoinPanel_text, DataController.instance.gameData.coin);
                    warningBlackPanel.SetActive(true);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();

                }
            }
            //이미 고용중 이면
            else
            {
                //Fire 묻는 창
                warningBlackPanel.SetActive(true);
                HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        //3명이상 고용중이면
        else
        {
            //고용중이 아니면 no
            if (DataController.instance.gameData.PTJNum[nowPrefabNum] == 0)
            {
                //3명이상 고용중이라는 경고 패널 등장
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용 가능한 알바 수를\n넘어섰어요!";
                warningBlackPanel.SetActive(true);
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //이미 고용중이면
            else
            {
                //Fire 묻는 창
                warningBlackPanel.SetActive(true);
                HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        */
        //고용 상태 반영
        EmployStateApply_Panel();
        EmployStateApply_Prefab();
    }
    private void Hire(int ID, int num)
    {
        //고용
        DataController.instance.gameData.PTJNum[ID] = num;

        //고용중인 알바생 수 증가
        ++employCount;
        GameManager.instance.workingCount(employCount);

        
        //main game
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);
        GameManager.instance.workingApply(workingList, ID);
        GameManager.instance.workingID.Add(ID);
        
    }

    private void Fire(int ID)
    {

        //고용 해제
        DataController.instance.gameData.PTJNum[ID] = 0;

        //고용 중인 알바생 수 감소
        --employCount;
        GameManager.instance.workingCount(employCount);

        //main game
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, ID);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, ID);
        GameManager.instance.workingID.Remove(ID);

        InitSlider();
    }

    //해고 yes버튼누르면
    private void BtnListener() //시원 건드림
    {
        //해고하기
        Fire(DataController.instance.gameData.PTJSelectNum);

        //해고 확인 패널 내리기
        warningBlackPanel.SetActive(false);
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();

        //해고 통보 패널 올리기
        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "해고했어요 ㅠㅠ";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }





    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {

        //10단위이면 10을 곱해준다.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {  value *= 10;  }

        //"n회"
        SliderNum.transform.GetComponent<Text>().text = value.ToString() + "회";
        Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = value;
            
        //가격
        price.GetComponent<Text>().text = (value * Info[DataController.instance.gameData.PTJSelectNum].Price).ToString();
        //GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price); 도와주세요

    }
    public void ToggleChange()
    {
        //10단위
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);
            InitSlider();
        }
        else 
        {
            PTJSlider10.SetActive(false);
            PTJSlider.SetActive(true);
            InitSlider();
        }
    }
    public void InitSlider()
    {

        //10단위
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            //slider
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);

            PTJSlider10.GetComponent<Slider>().value = 1;

            Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = 10;

            //n회
            SliderNum.transform.GetComponent<Text>().text = "10회";
            //가격
            price.GetComponent<Text>().text = (10 * Info[DataController.instance.gameData.PTJSelectNum].Price).ToString(); //도와주세요
            
        }
        //1단위
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;

            Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = 1;

            //n회
            SliderNum.transform.GetComponent<Text>().text = "1회";
            //가격
            price.GetComponent<Text>().text = (Info[DataController.instance.gameData.PTJSelectNum].Price).ToString();
            //GameManager.instance.ShowCoinText(PTJPanel.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);//도와주세요

        }
    }

    

}
