using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;    //제목
        public int rewardMedal; //메달 보상
        public int rewardHeart; //하트 보상
        public int[] clearCriterion=new int[100];  //달성 조건
        public int accClearCriterion;

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart,int[] clearCriterion,int accClearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.clearCriterion = clearCriterion;
            this.accClearCriterion = accClearCriterion;
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
    private GameObject achieveCondition; //도전과제 달성 조건 텍스트
    [SerializeField]
    private GameObject nowCondition; //현재 도전과제 달성 수치 텍스트
    [SerializeField]
    private GameObject Button;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge;

    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;


    [Header("==========패널==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;

    [Header("==========Animation==========")]
    public GameObject heartMover;
    public GameObject medalMover;


    //==========prefab num===========
    static int Prefabcount = 0; //추가 된 Prefab 수
    int prefabnum; //자신이 몇번째 Prefab인지


    //==========도전과제 값==========
    private int LevelReal;//이 레벨에 가야지 보상을 다 받은거다.
    private int LevelNow;//현재 이 레벨에 있다.

    private int[] ChallengeValue = new int[6];//누적된 수

    private int ValueNow;//이번 레벨에서의 수(0부터 갱신된)
    private int ValueReal;
    //=======================================================================================================================
    //=======================================================================================================================
    
    private void Start()
    {
        GameManager.instance.ShowMedalText();//현재 메달을 보인다.
        InfoInit();
    }

    private void Update()
    {

        ChallengeValue[0] = DataController.instance.gameData.unlockBerryCnt;
        ChallengeValue[1] = DataController.instance.gameData.totalHarvBerryCnt;
        ChallengeValue[2] = DataController.instance.gameData.accCoin;
        ChallengeValue[3] = DataController.instance.gameData.accHeart;
        ChallengeValue[4] = DataController.instance.gameData.accAttendance;
        ChallengeValue[5] = DataController.instance.gameData.mgPlayCnt;

        LevelNow= DataController.instance.gameData.challengeLevel[prefabnum];//이거는 없앨 수있음

        //보상을 제대로 다 받았다면 이 레벨이여야 한다.
        //LevelReal = ValueReal[prefabnum] / Info[prefabnum].clearCriterion;
        //if (LevelReal % Info[prefabnum].clearCriterion == 0) { LevelReal--; }

        InfoUpdate();
    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() 
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6){ Prefabcount = 0; }
        prefabnum = Prefabcount;
        Prefabcount++;


        //제목표시
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;


        //보상 설정
        Info[prefabnum].rewardMedal = 1; // 1 뱃지
        Info[prefabnum].rewardHeart = DataController.instance.gameData.challengeLevel[prefabnum] * 5;//레벨X5 하트

    }


    private void ChallengeSet() 
    {
        //Level Real정보 획득
        //acc Clear Criterion정보 획득

        //Clear Criterion설정
        //Value Now 설정
        //Value Real 설정


        //수집 기준 설정
        Info[prefabnum].accClearCriterion = 0;
        Info[prefabnum].clearCriterion = new int[100];

        switch (prefabnum)
        {
            case 0: // 딸기 수집
            case 4: // 누적 출석
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion 설정
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 20 * i;


                    
                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion정보 획득
                        LevelReal = i;//Level Real정보 획득
                    }
                }

                break;

            case 1: // 딸기 수확
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion 설정
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 100; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 200; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion정보 획득
                        LevelReal = i;//Level Real정보 획득
                    }
                }
                break;


            case 2: // 누적 코인
                Info[prefabnum].clearCriterion[0] = 1000;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion 설정
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, LevelNow);



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion정보 획득
                        LevelReal = i;//Level Real정보 획득
                    }
                }
                break;


            case 3: // 누적 하트
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion 설정
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, LevelNow);



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion정보 획득
                        LevelReal = i;//Level Real정보 획득
                    }
                }
                break;


            case 5: //미니게임 플레이
                for (int i = 0; i < 100; i++)
                {
                    //Clear Criterion 설정
                    if (i == 0) { Info[prefabnum].clearCriterion[i] = 10; }
                    else if (i == 1) { Info[prefabnum].clearCriterion[i] = 20; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + Info[prefabnum].clearCriterion[i - 2]; }



                    if (ChallengeValue[prefabnum] > Info[prefabnum].accClearCriterion + Info[prefabnum].clearCriterion[i])
                    {
                        Info[prefabnum].accClearCriterion += Info[prefabnum].clearCriterion[i];//acc Clear Criterion정보 획득
                        LevelReal = i;//Level Real정보 획득
                    }
                }
                break;

        }


        if (LevelNow < LevelReal)//아직 보상 다 얻지 못한 레벨이면
        {
            ValueNow = Info[prefabnum].clearCriterion[LevelNow];//현재 레벨의 최대 달성 값이 ValueNow.
        }
        else //보상 최대로 얻은 레벨이면
        {
            ValueNow = ChallengeValue[prefabnum] - Info[prefabnum].accClearCriterion;
        }



    }
    public void InfoUpdate() {

        ChallengeSet();

        //text 정보=========== update에서 뺄것ㄴ
        levelText.GetComponent<Text>().text ="Lv."+ LevelNow.ToString();  //레벨
        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[LevelNow].ToString();   //도전과제 게이지 달성 조건 숫자

        //게이지===============
        if (LevelReal > LevelNow || ValueNow == Info[prefabnum].clearCriterion[LevelNow])
            //아직 보상 다 못받음  ||  도전과제 달성
        {
            //도전과제 게이지 == 가득 찬 상태로
            Gauge.GetComponent<Image>().fillAmount = 1;
            //도전과제 게이지 현재값 == 최대
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString(); ;
            //도전과제 버튼 이미지 == Done
            Button.GetComponent<Image>().sprite = DoneButton;


        }
        else //보상 최대한 다 받음
        {
            //도전과제 게이지 == ValueNow 만큼 증가
            Gauge.GetComponent<Image>().fillAmount = (float)(ValueNow) / Info[prefabnum].clearCriterion[LevelNow];
            //도전과제 게이지 현재값 == ValueNow
            nowCondition.GetComponent<Text>().text = ValueNow.ToString();

        }

        /*
        if (LevelReal <= LevelNow)
        {
            //도전과제 게이지 == 증가
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[LevelNow]) / Info[prefabnum].clearCriterion[LevelNow];
            //도전과제 게이지 현재값 == ValueNow
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion[LevelNow]).ToString();

            //도전과제 달성하면
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion[LevelNow])
            {
                //가득 찬 상태로
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //도전과제 버튼 이미지 Done
            }
        }
        else
        { 
            //가득 찬 상태로
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //도전과제 버튼 이미지 Done
        }
        */
    }
    //==================================================================================================================
    //==================================================================================================================


    //도전과제 달성후 완료 버튼 누를시
    public void ChallengeSuccess() 
    {

        //도전과제 달성했는지 확인
        if (LevelReal > LevelNow || ValueNow == Info[prefabnum].clearCriterion[LevelNow])
            //아직 보상 다 못받음  ||  도전과제 달성
        {
            AudioManager.instance.RewardAudioPlay();
            heartMover.GetComponent<HeartMover>().HeartChMover(120);
            medalMover.GetComponent<HeartMover>().BadgeMover(120);
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //메달 보상 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //하트 보상 획득

            if (LevelNow < 100)
            {
                if (LevelReal == LevelNow) { LevelNow++; }//지금 보상 최대로 받은 상태이면 두 레벨은 같은값
                Button.GetComponent<Image>().sprite = IngButton; //도전과제 버튼 이미지 변경
                DataController.instance.gameData.challengeLevel[prefabnum]++; //LevelReal증가 == 레벨증가
                
            }
        }
    }



}
