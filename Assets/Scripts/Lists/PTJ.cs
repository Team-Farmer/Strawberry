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

        public PrefabStruct(string Name, string Explanation, int Price, int[] Prices, Sprite Picture, Sprite FacePicture, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
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

    //현재 선택한 가격
    private int nowSelectPrice;

    //???
    //비 파티클
    private ParticleSystem rainParticle;

    // 글로벌 변수
    private Globalvariable globalVar;
    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        //프리팹들에게 고유 번호 붙이기
        prefabnum = Prefabcount;
        Prefabcount++;


    }
    void Start()
    {
        InfoUpdate();

        //==========PTJ Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10단위 체크 토글
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1단위 슬라이더
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10단위 슬라이더

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;
        //==============================


        /*
        //고용중이라면 고용중 상태로 보이기
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
        { HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]); }
        FireConfirmButton.onClick.AddListener(BtnListener);

        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();


        warningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        warningBlackPanel2 = warningPanel.transform.GetChild(0).gameObject;
        noCoinPanel2 = warningPanel.transform.GetChild(1).gameObject;
        panelCoinText2 = noCoinPanel2.transform.GetChild(2).transform.GetChild(0).gameObject;
        */
    }
    private void Update()
    {
        /*
        //PTJNumNow 값이 변경된다면 set get실행된다.
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

        //n번고용중인 현황을 보인다.
        PTJExp.transform.GetChild(12).transform.GetComponent<Text>().text
            = "남은 고용 횟수: " + DataController.instance.gameData.PTJNum[prefabnum].ToString() + "회";
        */
    }
    /*
    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;

            //변경된 값이 0(즉 알바를 끝냈다면)이면 아래를 실행
            if (PTJ_NUM_NOW == 0)
            {
                //Fire() 과 유사하지만 employment수때문에 중복 시간있을때 변경
                //=================================
                PTJToggle.GetComponent<Toggle>().isOn = false;

                //토글 활성화/n번고용중 정보 비활성화
                PTJToggle.SetActive(true);
                PTJExp.transform.GetChild(12).gameObject.SetActive(false);

                //고용 해제
                DataController.instance.gameData.PTJNum[prefabnum] = 0;

                //고용해제 상태 이미지
                employTF.GetComponent<Text>().text = "고용 전";
                employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

                //main game에 현황 적용 엉망진창 나중에 손보디
                --employCount;
                workingList.Remove(Info[prefabnum].FacePicture);
                workingList.Add(null);
                GameManager.instance.workingApply(workingList, prefabnum);
                workingList.Remove(null);
                GameManager.instance.workingApply(workingList, prefabnum);
                GameManager.instance.workingCount(employCount);

                GameManager.instance.workingID.Remove(prefabnum);

                //기타 활성화
                PTJExp.transform.GetChild(13).gameObject.SetActive(true);//n회 숨기기
                PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

                PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireButtonSprite;
                PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "고용 하기";

                //슬라이더 초기화
                InitSlider();

            }
        }
        get { return PTJ_NUM_NOW; }
    }
    */
    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {

        //====프리팹 내용 채우기====
        //이름
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //알바생 사진
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;

        //설명
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
            ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "% →" +
            (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");

        //비용
        GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);

        //고용여부
        employTF.GetComponent<Text>().text = "고용 전";

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

        //알바 창 채우기
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        name.GetComponent<Text>().text = Info[prefabnum].Name;
        explanation.GetComponent<Text>().text = Info[prefabnum].Explanation;

        //알바 슬라이더
        //슬라이더 초기화
        InitSlider();
        //Slider값 변경 적용 -> n회, 비용 반영
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider.GetComponent<Slider>().value); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider10.GetComponent<Slider>().value); });

        //알바 고용 상태 반영
        EmployStateApply();

    }
    private void EmployStateApply()
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
    public void EmployButtonClick()
    {
        Debug.Log("select=" + DataController.instance.gameData.PTJSelectNum + " price=" + nowSelectPrice);
    }
    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {

        //10단위이면 10을 곱해준다.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        { value *= 10; }

        //고용중이 아니면
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //"n회"
            SliderNum.transform.GetComponent<Text>().text = value.ToString() + "회";

            //가격
            price.GetComponent<Text>().text = (value * Info[prefabnum].Price).ToString();
            //GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price); 도와주세요
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

            //n회
            SliderNum.transform.GetComponent<Text>().text = "10회";
            //가격
            nowSelectPrice = 10 * Info[DataController.instance.gameData.PTJSelectNum].Price;
            price.GetComponent<Text>().text = nowSelectPrice.ToString(); //도와주세요
            
        }
        //1단위
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;

            //n회
            SliderNum.transform.GetComponent<Text>().text = "1회";
            //가격
            nowSelectPrice = Info[DataController.instance.gameData.PTJSelectNum].Price;
            price.GetComponent<Text>().text = nowSelectPrice.ToString();
            //GameManager.instance.ShowCoinText(PTJPanel.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);//도와주세요
        }
    }
    
    
    /*
   
    //============================================================================================================
    public void HireFire()
    {
        AudioManager.instance.Cute1AudioPlay();
        //3명 아래로 고용중이면
        if (employCount < 3)
        {
            //고용중이 아니면 hire
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                if (Info[prefabnum].Price <= DataController.instance.gameData.coin)
                {
                    //HIRE
                    if (PTJToggle.GetComponent<Toggle>().isOn == true)
                    { Hire(prefabnum, (int)(PTJSlider10.GetComponent<Slider>().value) * 10); }
                    else
                    { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); }

                    warningBlackPanel.SetActive(true);
                    confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용되었어요!";
                    confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                }
                else
                { //재화 부족 경고 패널 등장
                    AudioManager.instance.Cute4AudioPlay();
                    GameManager.instance.ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    warningBlackPanel.SetActive(true);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                }
            }
            //이미 고용중이면 fire
            else
            {
                DataController.instance.gameData.PTJSelectNum = prefabnum;
                FireConfirm();
            }
        }
        //이미 3명이상 고용중이면
        else
        {
            //고용중이 아니면 no
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //3명이상 고용중이라는 경고 패널 등장
                warningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용 가능한 알바 수를\n넘어섰어요!";
                warningBlackPanel.SetActive(true);
                warningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //이미 고용중이면 fire
            else
            {
                DataController.instance.gameData.PTJSelectNum = prefabnum;
                FireConfirm();
            }
        }
    }



    private void Hire(int ID, int num)
    {
        //코인사용
        GameManager.instance.UseCoin(Info[ID].Price);
        //n번고용 한다고 저장
        DataController.instance.gameData.PTJNum[ID] = num;

        HireInit(ID, num);
    }

    private void HireInit(int ID, int num)
    {
        //고용중 이미지
        employTF.GetComponent<Text>().text = "고용 중";
        employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;

        //고용중인 동물 수 증가
        ++employCount;
        GameManager.instance.workingCount(employCount);

        //main game
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);
        GameManager.instance.workingApply(workingList, prefabnum);

        GameManager.instance.workingID.Add(prefabnum);

        //슬라이더,토글
        PTJSlider.SetActive(false);
        PTJSlider10.SetActive(false);
        PTJToggle.SetActive(false);

        //기타 숨기기
        PTJExp.transform.GetChild(13).gameObject.SetActive(false);//n회 숨기기
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(false);//코인 이미지 숨기기 이거 왜 안숨겨짐

        //n번고용중임을 표시
        PTJExp.transform.GetChild(12).gameObject.SetActive(true);


        EmployButtonFire();
    }

    private void Fire(int ID)
    {

        PTJToggle.GetComponent<Toggle>().isOn = false;

        //토글 활성화/n번고용중 정보 비활성화
        PTJToggle.SetActive(true);
        PTJExp.transform.GetChild(12).gameObject.SetActive(false);

        //고용 해제
        DataController.instance.gameData.PTJNum[ID] = 0;

        //고용해제 상태 이미지
        employTF.GetComponent<Text>().text = "고용 전";
        employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //기타 활성화
        PTJExp.transform.GetChild(13).gameObject.SetActive(true);//n회 숨기기
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

        //main game에 현황 적용
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0) { --employCount; }
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        GameManager.instance.workingCount(employCount);

        GameManager.instance.workingID.Remove(prefabnum);



        InitSlider();
    }




    private void FireConfirm() //시원 건드림
    {

        warningBlackPanel.SetActive(true);
        FirePanel.GetComponent<PanelAnimation>().OpenScale();


    }

    //해고 yes버튼누르면
    private void BtnListener() //시원 건드림
    {

        Fire(DataController.instance.gameData.PTJSelectNum);
        warningBlackPanel.SetActive(false);
        FirePanel.GetComponent<PanelAnimation>().CloseScale();

        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "해고했어요 ㅠㅠ";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }
    */
}
