using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJ : MonoBehaviour
{
    public static PTJ instance;

    [Serializable]
    public class PrefabStruct
    {
        public string Name;//알바생 이름
        public Sprite Picture;//사진
        public Sprite FacePicture;//알바생 얼굴 사진
        public string Explanation;//설명
        public int Price;//가격

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture)
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
    public PrefabStruct[] Info;//구조체

    [Header("==========INFO 적용할 대상=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject nameText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject employTF;


    [Header("==========PTJ select Sprite===========")]
    //PTJ고용 여부 보일 스프라이트
    public Sprite selectPTJSprite;
    public Sprite originalPTJSprite;
    [Header("==========PTJ Button Sprite===========")]
    public Sprite FireButtonSprite;
    public Sprite HireButtonSprite;



    //==========Prefab별로 숫자 부여==========
    static int Prefabcount = 0;
    int prefabnum;

    //==========PTJ 창==========
    private GameObject PTJPanel;

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
    //private ParticleSystem rainParticle;
    // 글로벌 변수
    //private Globalvariable globalVar;


    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        instance = this;
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;

    }

    void Start()
    {
        //==========PTJ Explanation Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(prefabnum).GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10단위 체크 토글
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1단위 슬라이더
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10단위 슬라이더

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;

        
        //??============================
        //rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        //globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

    }

    void OnEnable()
    {
        PreafbInfoUpdate();
    }

    private void Update()
    {

        //자신의 고용횟수값 변경 파악
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];
        //알바 고용 상태 반영
        EmployStateApply();
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
                InitSlider();
            }
            else 
            {
                HireNowLock.transform.GetChild(0).transform.GetComponent<Text>().text
                = "남은 고용 횟수: " + DataController.instance.gameData.PTJNum[prefabnum].ToString() + "회";
            }
        }
        get { return PTJ_NUM_NOW; }
    }

    //===================================================================================================
    //===================================================================================================
    public void PreafbInfoUpdate()
    {

        //====프리팹 내용 채우기====
        //====불변====
        //이름
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //알바생 사진
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        facePicture.GetComponent<Image>().preserveAspect = true;
        //설명
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation;
        //비용
        if (DataController.instance.gameData.researchLevelAv < 5) // 연구 레벨이 5레벨 이하라면
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);
        else if (DataController.instance.gameData.researchLevelAv < 10) // 연구 레벨이 10레벨 이하라면
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price * 2);
        else
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price * 4); // 게임매니저 스크립트도 변경
    }

    //PTJ Explanation Panel 띄우기
    public void PTJPanelActive()
    {
        //효과음
        AudioManager.instance.Cute1AudioPlay();

        DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
        //==== 알바 창 ====
        //알바 창을 띄운다.
        PTJPanel.transform.GetChild(0).gameObject.SetActive(true);
        PTJPanel.GetComponent<PanelAnimation>().OpenScale();


        //알바 창 채우기
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        picture.GetComponent<Image>().preserveAspect = true;
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
       
        
    }


    public void EmployStateApply()
    {

        //고용중이 아니다
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //PANEL============================
            //버튼 이미지, 내용 변경
            PTJButton.transform.GetComponent<Image>().sprite = HireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "고용 하기";

            //HireNowLock 숨기기
            HireNowLock.SetActive(false);


            //PREFAB===========================
            PTJBackground.GetComponent<Image>().sprite = originalPTJSprite;
            employTF.GetComponent<Text>().text = "고용전";
            employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);

        }
        //고용중이다
        else
        {
            //PANEL============================
            //버튼 이미지, 내용 변경
            PTJButton.transform.GetComponent<Image>().sprite = FireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "";

            //HireNowLock 보이기
            HireNowLock.SetActive(true);


            //PREFAB===========================
            PTJBackground.GetComponent<Image>().sprite = selectPTJSprite;
            employTF.GetComponent<Text>().text = "고용중";
            employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        }

    }



    //====================================================================================================
    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {
        //10단위이면 10을 곱해준다.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {  value *= 10;  }

        //"n회"
        SliderNum.transform.GetComponent<Text>().text = value.ToString() + "회";

        DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
        DataController.instance.gameData.PTJSelectNum[1] = value;
        //가격
        if (DataController.instance.gameData.researchLevelAv < 5) // 연구 레벨이 5레벨 이하라면
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price);
        else if (DataController.instance.gameData.researchLevelAv < 10) // 연구 레벨이 10레벨 이하라면
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price * 2);
        else // 연구 레벨이 15레벨 이하라면
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price * 4);
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

            DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
            DataController.instance.gameData.PTJSelectNum[1] = 10;
            //n회
            SliderNum.transform.GetComponent<Text>().text = "10회";
            //가격
            if (DataController.instance.gameData.researchLevelAv < 5) // 연구 레벨이 5레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price);
            else if (DataController.instance.gameData.researchLevelAv < 10) // 연구 레벨이 10레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price * 2);
            else // 연구 레벨이 15레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price * 4);

        }
        //1단위
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;


            DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
            DataController.instance.gameData.PTJSelectNum[1] = 1;
            //n회
            SliderNum.transform.GetComponent<Text>().text = "1회";
            //가격
            if (DataController.instance.gameData.researchLevelAv < 5) // 연구 레벨이 5레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price);
            else if (DataController.instance.gameData.researchLevelAv < 10) // 연구 레벨이 10레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price * 2);
            else // 연구 레벨이 15레벨 이하라면
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price * 4);
        }
    }
}
