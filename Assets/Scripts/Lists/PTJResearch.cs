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
        public int[] Prices = new int[25];//가격

        public PrefabStruct(string Name, string Explanation, int Price, int[] Prices, Sprite Picture, Sprite FacePicture, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Prices = Prices;   // 연구 가격은 배열로
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
    public GameObject titleText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;//지금 알바생인가 확인

    [Header("==========select PTJ===========")]
    //PTJ고용중여부 보일 스프라이트
    [SerializeField]
    private Sprite selectPTJSprite;
    [SerializeField]
    private Sprite originalPTJSprite;

    [Header("==========PTJ EXP===========")]
    //PTJ 설명창
    public GameObject PTJExp;


    [Header("==========PTJ Warning Panel===========")]
    public GameObject warningPanel;
    public GameObject noCoinPanel;
    public GameObject warningBlackPanel;
    public GameObject confirmPanel;
    public GameObject FirePanel;
    public Text panelCoinText;
    public Button FireConfirmButton;

    public Sprite FireSprite;
    public Sprite HireSprite;

    //test
    private GameObject warningBlackPanel2;
    private GameObject noCoinPanel2;
    private GameObject panelCoinText2;

    //Prefab별로 숫자 부여
    static int Prefabcount = 0;
    int prefabnum;


    //몇명 고용중인지 확인
    static int employCount = 0;

    //고용중인 알바생 명단
    static List<Sprite> workingList = new List<Sprite>();
    

    //PTJ Exp의 슬라이드 관련
    private GameObject PTJSlider;
    private GameObject PTJSlider10;
    private GameObject PTJToggle;

    //고용횟수
    private int PTJ_NUM_NOW;

    //비 파티클
    private ParticleSystem rainParticle;

    //고용해제 할까요 확인 패널
    private GameObject HireYNPanel;

    // 글로벌 변수
    private Globalvariable globalVar;
    //===================================================================================================
    //===================================================================================================

    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJToggle = PTJExp.transform.GetChild(8).transform.gameObject;//10단위 체크 토글
            PTJSlider = PTJExp.transform.GetChild(9).transform.gameObject;//1단위 슬라이더
            PTJSlider10= PTJExp.transform.GetChild(10).transform.gameObject;//10단위 슬라이더

            //Init Slider =(10단위 슬라이더만 보인다)
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);
            InitSlider();
            
            //고용중이라면 고용중 상태로 보이기
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {  HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]);  }
            FireConfirmButton.onClick.AddListener(BtnListener);
        }
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();


        warningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        warningBlackPanel2 = warningPanel.transform.GetChild(0).gameObject;
        noCoinPanel2 = warningPanel.transform.GetChild(1).gameObject;
        panelCoinText2 = noCoinPanel2.transform.GetChild(2).transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (PTJ == true)
        {
            //PTJNumNow 값이 변경된다면 set get실행된다.
            PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

            //n번고용중인 현황을 보인다.
            PTJExp.transform.GetChild(12).transform.GetComponent<Text>().text
                = "남은 고용 횟수: "+DataController.instance.gameData.PTJNum[prefabnum].ToString() +"회";
        }
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
                //Fire() 과 유사하지만 employment수때문에 중복 시간있을때 변경
                //=================================
                PTJToggle.GetComponent<Toggle>().isOn = false;

                //토글 활성화/n번고용중 정보 비활성화
                PTJToggle.SetActive(true);
                PTJExp.transform.GetChild(12).gameObject.SetActive(false);

                //고용 해제
                DataController.instance.gameData.PTJNum[prefabnum] = 0;

                //고용해제 상태 이미지
                levelNum.GetComponent<Text>().text = "고용 전";
                levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

                //main game에 현황 적용 엉망진창 나중에 손보디
                --employCount;
                workingList.Remove(Info[prefabnum].FacePicture);
                workingList.Add(null);
                GameManager.instance.workingApply(workingList,prefabnum);
                workingList.Remove(null);
                GameManager.instance.workingApply(workingList,prefabnum);
                GameManager.instance.workingCount(employCount);

                GameManager.instance.workingID.Remove(prefabnum);

                //기타 활성화
                PTJExp.transform.GetChild(13).gameObject.SetActive(true);//n회 숨기기
                PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

                PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireSprite;
                PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "고용 하기";

                //슬라이더 초기화
                InitSlider();

            }
        }
        get { return PTJ_NUM_NOW; }
    }
    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 6은 프리팹 숫자와 관련되어 있다!!! 같이 조절 . 변수설정하기

        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6)
        { Prefabcount -= 6; }
        prefabnum = Prefabcount;

        Info[Prefabcount].Prices = new int[25];

        for (int i=0; i<25; i++)
        {
            Info[prefabnum].Prices[i] = 100 * (i+1);
        }

        //타이틀, 설명, 코인값, 레벨, 고용여부 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//제목(이름) 표시
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//그림 표시
        }
        
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation+"\n"+
            ((DataController.instance.gameData.researchLevel[prefabnum]*2) + "% →" + 
            (DataController.instance.gameData.researchLevel[prefabnum]+1)*2 + "%");//설명 텍스트 표시

        //coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";
        if (PTJ == true)//알바
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[Prefabcount].Price); //비용 표시
        else
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[Prefabcount].Prices[DataController.instance.gameData.researchLevel[prefabnum]]); //비용 표시


        if (PTJ == true)//알바
        { levelNum.GetComponent<Text>().text = "고용 전"; }
        else//연구
        { levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString(); }



        Prefabcount++;
    }

    //=============================================================================================================================

    //연구 레벨
    public void clickCoin_Research() {
        AudioManager.instance.Cute1AudioPlay();
        if (DataController.instance.gameData.researchLevel[prefabnum] < 26)//레벨 25로 한계두기
        {
            //해당 금액이 지금 가진 코인보다 적으면
            if (DataController.instance.gameData.coin >= Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]])
            {
                //해당 금액의 코인이 감소
                GameManager.instance.UseCoin(Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();

                if (DataController.instance.gameData.researchLevel[prefabnum] == 25)
                {
                    coinNum.GetComponent<Text>().text = "Max";
                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                        ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%");//설명 텍스트 표시
                }
                else
                {
                    GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);

                    switch (Info[prefabnum].Name)
                    {
                        case "딸기 값이 금값": IncreaseBerryPrice(); break;
                        case "딸기가 쑥쑥": DecreaseBerryGrowTime(); break;
                        case "부르는 게 값": break;
                        case "도와줘요 세스코": DecreaseBugGenerateProb(); break;
                        case "잡초 바이바이": DecreaseWeedGenerateProb(); break;
                        case "시원한 소나기": IncreaseRainDuration(); break;
                    }

                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                    ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%" + "→" +
                    (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");//설명 텍스트 표시
                }
            }
            else
            {
                //재화 부족 경고 패널 등장
                AudioManager.instance.Cute4AudioPlay();
                //panelCoinText2.GetComponent<Text>().text= DataController.instance.gameData.coin.ToString();
                GameManager.instance.ShowCoinText(panelCoinText2.GetComponent<Text>(), DataController.instance.gameData.coin);
                warningBlackPanel2.SetActive(true);
                noCoinPanel2.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        else
        {
            // 연구 이미 맥스임~ 패널 필요 (아님말고)
            levelNum.GetComponent<Text>().text = "Max";
            coinNum.GetComponent<Text>().text = "Max";
            explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%");//설명 텍스트 표시
            //Debug.Log("연구 렙 맥스임");
        }
            

    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();     
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.getEffi();

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.getEffi();     
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.getEffi();
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[5]) * Globalvariable.instance.getEffi();
        var main = rainParticle.main;
        main.duration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
        DataController.instance.gameData.rainDuration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
    }
    //=============================================================================================================================


    //PTJ설명창 띄운다
    public void ActiveExplanation()
    {
        AudioManager.instance.Cute1AudioPlay();
        //창을 띄운다.
        PTJExp.SetActive(true);

        //PICTURE
        PTJExp.transform.GetChild(3).transform.GetComponent<Image>().sprite
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(5).transform.GetComponent<Text>().text
            = Info[prefabnum].Explanation;

        //EmployButton Init
        InitSlider();



        //Slider값 변경될때 마다 -> n번고용, 비용에 반영
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { EmployButtonHire((int)(PTJSlider.GetComponent<Slider>().value)); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { EmployButtonHire((int)(PTJSlider10.GetComponent<Slider>().value)); });

    }

    //고용 버튼 상태 변경
    public void EmployButtonHire(int SliderNum)
    {
        PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireSprite;
        PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "고용 하기";
        if (PTJToggle.GetComponent<Toggle>().isOn == true) 
        { SliderNum *= 10; }//10단위이면 10을 곱해준다.

        //고용중이 아니면
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //"n번 고용"
            PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "회";
            //PRICE 텍스트
            GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), SliderNum * Info[prefabnum].Price);
        }

    }

    //고용 해제 버튼
    private void EmployButtonFire()
    {
        //EmployButton 텍스트를 "고용 해제"로
        PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = FireSprite;
        PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "";
        //PRICE 텍스트를 빈칸으로
        PTJExp.transform.GetChild(7).transform.GetComponent<Text>().text = "";

    }
    //============================================================================================================
    public void HireFire() {
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
                DataController.instance.gameData.PTJFireConfirm = prefabnum; 
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
                DataController.instance.gameData.PTJFireConfirm = prefabnum;
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

        HireInit(ID,num);
    }

    private void HireInit(int ID,int num) 
    {
        //고용중 이미지
        levelNum.GetComponent<Text>().text = "고용 중";
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
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
        levelNum.GetComponent<Text>().text = "고용 전";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //기타 활성화
        PTJExp.transform.GetChild(13).gameObject.SetActive(true);//n회 숨기기
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

        //main game에 현황 적용
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0){  --employCount;}
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        GameManager.instance.workingCount(employCount);

        GameManager.instance.workingID.Remove(prefabnum);

        

        InitSlider();        
    }


    public void InitSlider() 
    {
        //고용중이 아니면
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //10단위
            if (PTJToggle.GetComponent<Toggle>().isOn == true)
            {
                //slider
                PTJSlider10.SetActive(true);
                PTJSlider.SetActive(false);

                PTJSlider10.GetComponent<Slider>().value = 1;

                //Slider Num 텍스트
                PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text = "10회";
                //PRICE 텍스트
                GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);
            }
            //1단위
            else
            {
                //slider
                PTJSlider.SetActive(true);
                PTJSlider10.SetActive(false);

                PTJSlider.GetComponent<Slider>().value = 1;

                //Slider Num텍스트
                PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text = "1회";
                //PRICE 텍스트
                GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);
            }
        }
        //고용중이면
        else
        {
            EmployButtonFire();
        }
    }

    private void FireConfirm() //시원 건드림
    {

        warningBlackPanel.SetActive(true);
        FirePanel.GetComponent<PanelAnimation>().OpenScale();
        

    }

    //해고 yes버튼누르면
    private void BtnListener() //시원 건드림
    {
        
        Fire(DataController.instance.gameData.PTJFireConfirm);
        warningBlackPanel.SetActive(false);
        FirePanel.GetComponent<PanelAnimation>().CloseScale();

        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "해고했어요 ㅠㅠ";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }
}
