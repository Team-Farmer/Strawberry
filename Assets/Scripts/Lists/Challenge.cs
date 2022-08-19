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
        public int[] clearCriterion;  //달성 조건
        public Sprite challengeImage;

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart,int[] clearCriterion,Sprite challengeImage)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.clearCriterion = clearCriterion;
            this.challengeImage = challengeImage;

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
    [SerializeField]
    private GameObject image;//왼쪽의 설명 그림
    public GameObject medalTxt;
    public GameObject heartTxt;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge;

    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;


    [Header("==========Animation==========")]
    public GameObject heartMover;
    public GameObject medalMover;


    //==========prefab num===========
    static int Prefabcount = 0; //추가 된 Prefab 수
    private int prefabnum; //자신이 몇번째 Prefab인지


    //==========도전과제 값==========
    private int LevelNow;//현재 이 레벨에 있다.

    private int[] ChallengeValue = new int[6];//누적된 수

    private int ValueNow;//이번 레벨에서의 수(0부터 갱신된)

    //=======================================================================================================================
    //=======================================================================================================================
    
    private void Awake()
    {
        GameManager.instance.ShowMedalText();//현재 메달을 보인다.

        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;
 
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
        Info[prefabnum].rewardHeart = (DataController.instance.gameData.challengeLevel[prefabnum]+1) * 5;//레벨X5 하트//이거는 없앨 수있음
        ValueNow = ChallengeValue[prefabnum];

        InfoUpdate();

    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() 
    {

        //제목표시
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;

        //보상 설정
        Info[prefabnum].rewardMedal = 1; // 1 뱃지
        Info[prefabnum].rewardHeart = (DataController.instance.gameData.challengeLevel[prefabnum]+1) * 5;//레벨X5 하트

        //그림 설정
        image.GetComponent<Image>().sprite = Info[prefabnum].challengeImage;


        //수집 기준 설정
        Info[prefabnum].clearCriterion = new int[100];
        switch (prefabnum)
        {
            case 0: // 딸기 수집
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 0; i < 100; i++)
                {
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 20 * i;
                }
                break;

            case 4: // 누적 출석
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 0; i < 100; i++)
                {
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 20 * i;
                }
                break;

            case 1: // 딸기 수확
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 1; i < 100; i++)
                {
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i-1]+100;
                }
                break;

            case 2: // 누적 코인
                Info[prefabnum].clearCriterion[0] = 1000;
                for (int i = 1; i < 100; i++)
                {
                    //Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2,i);
                    if (i <= 10)
                    { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 2; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1]; }
                }
                break;

            case 3: // 누적 하트
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 1; i < 100; i++)
                {
                    //Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] * (int)Mathf.Pow(2, i);
                    if (i <= 10)
                    { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 2; }
                    else { Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1]; }
                }
                break;

            case 5: //미니게임 플레이
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 1; i < 100; i++)
                {
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 10;
                }
                break;

        }
    }






    public void InfoUpdate() {

        //text 정보=========== update에서 뺄것
        levelText.GetComponent<Text>().text ="Lv."+ LevelNow.ToString();  //레벨
        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[LevelNow].ToString();//도전과제 게이지 달성 조건 숫자

        //게이지===============
        if (ValueNow >= Info[prefabnum].clearCriterion[LevelNow])
        {
            //도전과제 게이지 == 가득 찬 상태로
            Gauge.GetComponent<Image>().fillAmount = 1;
            //도전과제 게이지 현재값 == 최대
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString(); ;
            //도전과제 버튼 이미지 == Done
            Button.GetComponent<Image>().sprite = DoneButton;
            //도전과제 보상 보인다.
            medalTxt.SetActive(true);
            heartTxt.SetActive(true);
            medalTxt.GetComponent<Text>().text = "X"+Info[prefabnum].rewardMedal.ToString();
            heartTxt.GetComponent<Text>().text = "X"+Info[prefabnum].rewardHeart.ToString();

        }
        else
        {
            //도전과제 게이지 == ValueNow 만큼 증가
            Gauge.GetComponent<Image>().fillAmount = (float)(ValueNow) / Info[prefabnum].clearCriterion[LevelNow];
            //도전과제 게이지 현재값 == ValueNow
            nowCondition.GetComponent<Text>().text = ValueNow.ToString();
        }

        
    }
    //==================================================================================================================
    //==================================================================================================================


    //도전과제 달성후 완료 버튼 누를시
    public void ChallengeSuccess() 
    {

        //도전과제 달성했는지 확인
        if (ValueNow >= Info[prefabnum].clearCriterion[LevelNow])
        {
            //효과음, 효과 애니메이션
            AudioManager.instance.RewardAudioPlay();
            heartMover.GetComponent<HeartMover>().HeartChMover(120);
            medalMover.GetComponent<HeartMover>().BadgeMover(120);

            //보상획득
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //메달 보상 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //하트 보상 획득
            //Debug.Log(Info[prefabnum].rewardMedal + "   " + Info[prefabnum].rewardHeart);
            //다음 레벨로 이동
            if (LevelNow < 100)
            {
                switch (prefabnum) 
                {
                    case 0:
                        DataController.instance.gameData.unlockBerryCnt-= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 1:
                        DataController.instance.gameData.totalHarvBerryCnt -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 2:
                        DataController.instance.gameData.accCoin -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 3:
                        DataController.instance.gameData.accHeart -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 4:
                        DataController.instance.gameData.accAttendance -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 5:
                        DataController.instance.gameData.mgPlayCnt -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                }

                Button.GetComponent<Image>().sprite = IngButton; //도전과제 버튼 이미지 변경
                medalTxt.SetActive(false);
                heartTxt.SetActive(false);
                DataController.instance.gameData.challengeLevel[prefabnum]++; //LevelNow증가 == 레벨증가

            }
        }
    }



}
