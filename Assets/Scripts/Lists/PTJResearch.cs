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

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture, bool exist)
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
    public GameObject PTJwarningPanel;
    public GameObject NoCoinPanel;
    public GameObject PTJBP;
    public Text panelCoinText;


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



    //===================================================================================================
    //===================================================================================================
    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJSlider = PTJExp.transform.GetChild(8).transform.gameObject;//1단위 슬라이더
            PTJSlider10= PTJExp.transform.GetChild(9).transform.gameObject;//10단위 슬라이더
            PTJToggle= PTJExp.transform.GetChild(7).transform.gameObject;//10단위 체크 토글

            //Init Slider =(10단위 슬라이더만 보인다)
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);
            InitSlider();
            
            //고용중이라면 고용중 상태로 보이기
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {  HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]);  }

        }
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


        //타이틀, 설명, 코인값, 레벨, 고용여부 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//제목(이름) 표시
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//그림 표시
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;//설명 텍스트 표시
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";//비용 표시


        if (PTJ == true)//알바
        { levelNum.GetComponent<Text>().text = "고용 전"; }
        else//연구
        { levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString(); }



        Prefabcount++;
    }

    //=============================================================================================================================

    //연구 레벨
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//레벨 25로 한계두기
        {
            switch (Info[prefabnum].Name)
            {
                case "딸기 가치 상승": IncreaseBerryPrice(); break;
                case "딸기 성장기간 감소": DecreaseBerryGrowTime(); break;
                case "트럭 수익 상승": break;
                case "벌레 확률 감소": DecreaseBugGenerateProb(); break;
                case "잡초 확률 감소": DecreaseWeedGenerateProb(); break;
                case "소나기 확률 증":Debug.Log("소나긴속"); break;
            }

            //해당 금액의 코인이 감소
            GameManager.instance.UseCoin(Info[prefabnum].Price);

            //해당 금액이 지금 가진 코인보다 적으면
            if (DataController.instance.gameData.coin >= Info[prefabnum].Price)
            {
                //레벨이 올라간다.
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
            }
            else if (DataController.instance.gameData.coin < Info[prefabnum].Price) //해당 금액이 지금 가진 코인보다 많으면
            {
                //재화 부족 경고 패널 등장
                GameManager.instance.DisableObjColliderAll();
                GameManager.instance.ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                PTJBP.SetActive(true);
                NoCoinPanel.SetActive(true);
                NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * Globalvariable.instance.coeffi;

        for (int i = 0; i < 192; i++)
        {
            if (Globalvariable.instance.berryListAll[i] == null) continue;

            if (i < 64)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.coeffi;

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.coeffi;
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.coeffi;
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[5]) * Globalvariable.instance.coeffi;

    }
    //=============================================================================================================================

    //PTJ설명창 띄운다
    public void ActiveExplanation()
    {
        //창을 띄운다.
        PTJExp.SetActive(true);

        //PICTURE
        PTJExp.transform.GetChild(2).transform.GetComponent<Image>().sprite
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(3).transform.GetComponent<Text>().text
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
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
        //Debug.Log("ID = " + prefabnum + " / TEST = " + DataController.instance.gameData.PTJNum[prefabnum]);
        
        if (PTJToggle.GetComponent<Toggle>().isOn == true) 
        { SliderNum *= 10; }//10단위이면 10을 곱해준다.

        //고용중이 아니면
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //EmployButton 텍스트를 "n번 고용"으로
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "번 고용";
            //PRICE 텍스트
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
               = (SliderNum * Info[prefabnum].Price).ToString();
        }

    }

    //고용 해제 버튼
    private void EmployButtonFire()
    {
        //EmployButton 텍스트를 "고용 해제"로
        PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "고용 해제";
        //PRICE 텍스트를 빈칸으로
        PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = "";

    }
    //============================================================================================================
    public void HireFire() {

        //3명 아래로 고용중이면
        if (employCount < 3)
        {
            //고용중이 아니면 hire
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //HIRE
                if (PTJToggle.GetComponent<Toggle>().isOn == true) 
                { Hire(prefabnum, (int)(PTJSlider10.GetComponent<Slider>().value)*10); }
                else 
                { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); } 
            }
            //이미 고용중이면 fire
            else
            { Fire(prefabnum); }
        }
        //이미 3명이상 고용중이면
        else
        {
            //고용중이 아니면 no
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //3명이상 고용중이라는 경고 패널 등장
                GameManager.instance.DisableObjColliderAll();
                PTJBP.SetActive(true);
                PTJwarningPanel.SetActive(true);
                PTJwarningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //이미 고용중이면 fire
            else
            { Fire(prefabnum); }
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
        GameManager.instance.workingApply(workingList);

        //슬라이더,토글
        PTJSlider.SetActive(false);
        PTJSlider10.SetActive(false);
        PTJToggle.SetActive(false);

        //n번고용중임을 표시
        PTJExp.transform.GetChild(11).gameObject.SetActive(true);
        PTJExp.transform.GetChild(11).transform.GetComponent<Text>().text = num.ToString() + "번 고용중이다.";

        EmployButtonFire();
    }

    private void Fire(int ID)
    {
        FireCat();
        PTJToggle.GetComponent<Toggle>().isOn = false;

        //토글 활성화/n번고용중 정보 비활성화
        PTJToggle.SetActive(true);
        PTJExp.transform.GetChild(11).gameObject.SetActive(false);

        //고용 해제
        DataController.instance.gameData.PTJNum[ID] = 0;

        //고용해제 상태 이미지
        levelNum.GetComponent<Text>().text = "고용 전";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //main game에 현황 적용
        --employCount;
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

        InitSlider();        
    }
    private void FireCat() 
    {
        if (Info[prefabnum].Name == "고양이") 
        {
            //쿨타임 돌기 시작
            GameManager.instance.isCatTime=true;
        }
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

                //EmployButton 텍스트
                PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "10번 고용";
                //PRICE 텍스트
                PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = (Info[prefabnum].Price).ToString();
            }
            //1단위
            else
            {
                //slider
                PTJSlider.SetActive(true);
                PTJSlider10.SetActive(false);

                PTJSlider.GetComponent<Slider>().value = 1;

                //EmployButton 텍스트
                PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "1번 고용";
                //PRICE 텍스트
                PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = (Info[prefabnum].Price).ToString();
            }
        }
        //고용중이면
        else
        {
            EmployButtonFire();
        }

    }

    
    
}
