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
        public int clearCriterion;  //달성 조건

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart,int clearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
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


    static int Prefabcount = 0; //추가 된 Prefab 수
    int prefabnum; //자신이 몇번째 Prefab인지


    private int realLevel;//이 레벨에 가야지 보상을 다 받은거다.

    private int[] ChallengeValue = new int[6];//누적된 수

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

            
            
        //보상을 받았다면 이 레벨이여야 한다.
        realLevel = ChallengeValue[prefabnum] / 30;
        if (realLevel %30== 0) { realLevel--; }

        InfoUpdate();
    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() {
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


        //수집 기준 설정
        switch (prefabnum)
        {
            case 0: // 딸기 수집
                

                break;
            case 1: // 딸기 수확
                
                break;
            case 2: // 누적 코인
                
                break;
            case 3: // 누적 하트
                
                break;
            case 4: // 누적 출석
                
                break;
            case 5: // 미니게임 플레이
                
                break;
        }
    }

    public void InfoUpdate() {
        // titleText.GetComponent<Text>().text = Info[prefabnum].Title + " " + ChallengeValue[prefabnum];//제목+누적 수
        levelText.GetComponent<Text>().text ="Lv."+ DataController.instance.gameData.challengeLevel[prefabnum].ToString();  //레벨

        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion.ToString();   //도전과제 게이지 달성 조건 숫자

        if (realLevel <= DataController.instance.gameData.challengeLevel[prefabnum])
        {
            //도전과제 게이지 증가
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion) / Info[prefabnum].clearCriterion;
            //도전과제 게이지 현재값
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion).ToString();

            //도전과제 달성하면
            if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion)
            {
                //가득 찬 상태로
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion.ToString();

                Button.GetComponent<Image>().sprite = DoneButton; //도전과제 버튼 이미지 변경
            }
        }
        else
        { 
            //가득 찬 상태로
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion.ToString(); ;

            Button.GetComponent<Image>().sprite = DoneButton; //도전과제 버튼 이미지 변경
        }

    }
    //==================================================================================================================
    //==================================================================================================================


    //도전과제 달성후 완료 버튼 누를시
    public void ChallengeSuccess() {

        if (ChallengeValue[prefabnum] / Info[prefabnum].clearCriterion == 
            DataController.instance.gameData.challengeLevel[prefabnum] + 1
            || realLevel > DataController.instance.gameData.challengeLevel[prefabnum])
        {
            AudioManager.instance.RewardAudioPlay();
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //메달 보상 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //하트 보상 획득

            if (DataController.instance.gameData.challengeLevel[prefabnum] < 100)
            {
                Button.GetComponent<Image>().sprite = IngButton; //도전과제 버튼 이미지 변경
                DataController.instance.gameData.challengeLevel[prefabnum]++; //레벨증가
            }

        }

    }


}
