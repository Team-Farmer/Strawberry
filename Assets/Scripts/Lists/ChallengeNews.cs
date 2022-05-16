using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeNews : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;//제목
        public int rewardMedal;
        public int rewardHeart;
        public int condition_challenge;
        public string Exp_news;//뉴스 내용
        public int clearCriterion;

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart, int condition_challenge, string Exp_news,int clearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.condition_challenge = condition_challenge;
            this.Exp_news = Exp_news;
            this.clearCriterion = clearCriterion;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    public GameObject levelText;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject countText_News;
    [SerializeField]
    private GameObject lock_News;//뉴스 잠금
    [SerializeField]
    private GameObject unlock_News;//뉴스 잠금 해제 가능

    public GameObject doneButton_Challenge;

    [SerializeField]
    private GameObject gaugeConditionText_Challenge;//도전과제 달성 조건 텍스트
    [SerializeField]
    private GameObject gaugeText_Challenge;//현재 도전과제 달성 수치 텍스트
    [SerializeField]
    private GameObject Btn_Challenge;
    public GameObject bangMark_Challenge;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge_Challenge;




    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;

    [Header("==========기타==========")]
    [SerializeField]
    private bool isChallenge;

    [Header("==========패널==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;

    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;



    //이거 2개 하나로 줄이기 가능
    GameObject newsExplanation;
    GameObject newsExp;

    
    private int[] ChallengeValue = new int[6];


    private void Start()
    {
        newsExplanation = GameObject.FindGameObjectWithTag("NewsExplanation");
        GameManager.instance.ShowMedalText();//현재 메달을 보인다.
        InfoInit();
    }

    private void Update()
    {
        if (isChallenge == true)
        {
            ChallengeValue[0] = DataController.instance.gameData.unlockBerryCnt;
            ChallengeValue[1] = DataController.instance.gameData.totalHarvBerryCnt;
            ChallengeValue[2] = DataController.instance.gameData.accCoin;
            ChallengeValue[3] = DataController.instance.gameData.accHeart;
            ChallengeValue[4] = DataController.instance.gameData.accAttendance;
            ChallengeValue[5] = DataController.instance.gameData.mgPlayCnt;

            for (int i = 0; i < ChallengeValue.Length; i++)
            {
                if (ChallengeValue[i] % 30 == 0&& 
                    ChallengeValue[i] == Info[prefabnum].clearCriterion * DataController.instance.gameData.challengeLevel[prefabnum] + 1) 
                { ChallengeValue[i] = 30; }
                else { ChallengeValue[i] %= 30; }
            }
        }
        InfoUpdate();
    }
    //==================================================================================================================
    //==================================================================================================================
    private void InfoInit() {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6)
        { Prefabcount = 0; }
        prefabnum = Prefabcount;

        Prefabcount++;

        titleText.GetComponent<Text>().text = Info[prefabnum].Title;//제목표시

        //보상 설정
        if (isChallenge == true) 
        {
            Info[prefabnum].rewardMedal = 1;
            Info[prefabnum].clearCriterion = 30;// 일단 30으로 통일
            //임시
            switch (prefabnum) 
            {
                case 0: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion; break;
                case 1: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/5; break;
                case 2: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/50; break;
                case 3: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/30; break;
                case 4: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/6; break;
                case 5: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/10; break;
            }
            
        }

        
    }
    public void InfoUpdate() {

        if (isChallenge == true) //CHALLENGE=============================
        {
            levelText.GetComponent<Text>().text ="Lv."+ (1+DataController.instance.gameData.challengeLevel[prefabnum]).ToString();

            //도전과제 게이지 증가
            Gauge_Challenge.GetComponent<Image>().fillAmount = (float)ChallengeValue[prefabnum]/ Info[prefabnum].clearCriterion;
            //도전과제 게이지 달성 조건 숫자
            gaugeConditionText_Challenge.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion.ToString();
            //도전과제 게이지 현재값
            gaugeText_Challenge.GetComponent<Text>().text = ChallengeValue[prefabnum].ToString();

            //도전과제 달성하면
            if (ChallengeValue[prefabnum] 
                == Info[prefabnum].clearCriterion*DataController.instance.gameData.challengeLevel[prefabnum]+1)
            {
                Btn_Challenge.GetComponent<Image>().sprite = DoneButton;//도전과제 버튼 이미지 변경
                bangMark_Challenge.SetActive(true);//느낌표!! 띄우기 (획득 가능한 도전과제 있다)
            }
            
            
            
        }
        else //NEWS=============================
        {
            //뉴스 lock상태이면 가리기
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false)
            { lock_News.SetActive(true); }
            //뉴스 unlock상태이고 구매하지 않았으면
            else if (DataController.instance.gameData.NewsEnd[prefabnum]==false) { unlock_News.SetActive(true); }

            
            //뉴스 숫자
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }

    }
    //==================================================================================================================
    //==================================================================================================================
    //CHALLENGE

    //도전과제 달성후 완료 버튼 누를시

    public void ChallengeSuccess() {
        if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion)
        {
            //메달 보상 획득
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            //하트 보상 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);

            Btn_Challenge.GetComponent<Image>().sprite = IngButton;//도전과제 버튼 이미지 변경
            bangMark_Challenge.SetActive(false);//느낌표 지우기

            DataController.instance.gameData.challengeLevel[prefabnum]++;//레벨증가

        }
    }

    //==================================================================================================================
    //==================================================================================================================
    //NEWS
    public void UnlockNews() 
    {

        if (DataController.instance.gameData.medal >= 1)
        {
            //메달 하나 사용
            GameManager.instance.UseMedal(1);
            DataController.instance.gameData.NewsEnd[prefabnum] = true;
            Destroy(unlock_News);
            BP.SetActive(true);
            confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뉴스가 해금되었어요!";
            confirmPanel.GetComponent<PanelAnimation>().OpenScale();
        }
        else
        {
            //메달이 부족할 시
            warnningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뱃지가 부족해요!";
            BP.SetActive(true);
            warnningPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }


    //뉴스 설명창
    public void Explantion() {

        newsExp = newsExplanation.transform.GetChild(0).transform.gameObject;//newsExp

        newsExp.SetActive(true);
        try
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == true)
            {
                //Explanation 내용을 채운다.
                newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text 
                    = Info[prefabnum].Title;//이름 설정 newsName
                newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                    = Info[prefabnum].Exp_news;//설명 설정 newsTxt 


            }
        }
        catch
        {
            Debug.Log("ChallengeNews 인덱스 오류");
        }
    }


}
