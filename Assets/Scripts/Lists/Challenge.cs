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
        public string Title;//제목
        public int rewardMedal;//메달 보상
        public int rewardHeart;//하트 보상
        public int clearCriterion;//달성 조건

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
    private GameObject achieveCondition;//도전과제 달성 조건 텍스트
    [SerializeField]
    private GameObject nowCondition;//현재 도전과제 달성 수치 텍스트
    [SerializeField]
    private GameObject Button;
    public GameObject bangMark;

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


    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;


    private int realLevel;//이 레벨에 가야지 보상을 다 받은거다.

    private int[] ChallengeValue = new int[6];
    private int[] ChallengeValue_ = new int[6];
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


        //0~29,0
        ChallengeValue_[prefabnum] = ChallengeValue[prefabnum] % 30;
            
            
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
        if (Prefabcount >= 6)
        { Prefabcount = 0; }
        prefabnum = Prefabcount;

        Prefabcount++;

        titleText.GetComponent<Text>().text = Info[prefabnum].Title;//제목표시



        //보상 설정
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

    public void InfoUpdate() {

        titleText.GetComponent<Text>().text = Info[prefabnum].Title + " " + ChallengeValue[prefabnum];//제목+누적 수
        levelText.GetComponent<Text>().text ="Lv."+ (1+DataController.instance.gameData.challengeLevel[prefabnum]).ToString();//레벨
                                                                                                                                //도전과제 게이지 달성 조건 숫자
        achieveCondition.GetComponent<Text>().text
            = "/" + Info[prefabnum].clearCriterion.ToString();

        if (realLevel <= DataController.instance.gameData.challengeLevel[prefabnum])
        {
            //도전과제 게이지 증가
            Gauge.GetComponent<Image>().fillAmount
                = (float)(ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion) / Info[prefabnum].clearCriterion;
            //도전과제 게이지 현재값
            nowCondition.GetComponent<Text>().text
                = (ChallengeValue[prefabnum] % Info[prefabnum].clearCriterion).ToString();

            //도전과제 달성하면
            if (ChallengeValue[prefabnum] / 30 == DataController.instance.gameData.challengeLevel[prefabnum] + 1)
            {
                //가득 찬 상태로
                Gauge.GetComponent<Image>().fillAmount = 1;
                nowCondition.GetComponent<Text>().text = "30";

                Button.GetComponent<Image>().sprite = DoneButton;//도전과제 버튼 이미지 변경
                bangMark.SetActive(true);//느낌표!! 띄우기 (획득 가능한 도전과제 있다)
            }
        }
        else
        { 
            //가득 찬 상태로
            Gauge.GetComponent<Image>().fillAmount = 1;
            nowCondition.GetComponent<Text>().text = "30";

            Button.GetComponent<Image>().sprite = DoneButton;//도전과제 버튼 이미지 변경
            bangMark.SetActive(true);//느낌표!! 띄우기 (획득 가능한 도전과제 있다)
        }

    }
    //==================================================================================================================
    //==================================================================================================================


    //도전과제 달성후 완료 버튼 누를시

    public void ChallengeSuccess() {

        if (ChallengeValue[prefabnum] / 30 == DataController.instance.gameData.challengeLevel[prefabnum] + 1
            || realLevel > DataController.instance.gameData.challengeLevel[prefabnum])
        {
            AudioManager.instance.Cute1AudioPlay();
            //메달 보상 획득
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            //하트 보상 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);

            Button.GetComponent<Image>().sprite = IngButton;//도전과제 버튼 이미지 변경
            bangMark.SetActive(false);//느낌표 지우기

            DataController.instance.gameData.challengeLevel[prefabnum]++;//레벨증가

        }

    }


}
