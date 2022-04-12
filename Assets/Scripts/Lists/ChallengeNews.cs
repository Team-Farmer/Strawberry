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
        public int[] reward_challenge;//보상 = [0]메달 / [1]하트
        public int condition_challenge;
        public string Exp_news;//뉴스 내용


        public ChallengeNewsStruct(string Title, int[] reward_challenge,int condition_challenge, string Exp_news)
        {
            this.Title = Title;
            this.reward_challenge = reward_challenge;
            this.condition_challenge = condition_challenge;
            this.Exp_news = Exp_news;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
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
    private RectTransform GaugeContainer_Challenge;
    [SerializeField]
    private RectTransform Gauge_Challenge;




    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite[] BtnImage_Challenge;

    [Header("==========기타==========")]
    [SerializeField]
    private bool isChallenge;



    //추가 된 Prefab 수
    static int Prefabcount = 0;

    //자신이 몇번째 Prefab인지
    int prefabnum;

    GameObject newsExplanation;
    GameObject newsExp;

    GameObject medalText;

    private void Start()
    {
        
        newsExplanation = GameObject.FindGameObjectWithTag("NewsExplanation");
        medalText = GameObject.FindGameObjectWithTag("medalCount");

        medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//메달 현황 텍스트로 띄우기
        InfoUpdate();
    }


    public void InfoUpdate() {

        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 7)
        { Prefabcount =0; }
        prefabnum = Prefabcount;



        //내용표시=======================================================
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;

        
        if (isChallenge == true) //CHALLENGE=============================
        {

            //도전과제 완료(현재 도전과제 게이지==도전과제 달성 수치)
            if (DataController.instance.gameData.challengeGauge[prefabnum] == Info[prefabnum].condition_challenge)
            {                
                Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];//도전과제 버튼 이미지 변경
                bangMark_Challenge.SetActive(true);//느낌표!! 띄우기 (획득 가능한 도전과제 있다)
            }

            //도전과제 완료해서 보상까지 받은 후
            if (DataController.instance.gameData.challengeEnd[prefabnum] == true)
            {
                doneButton_Challenge.SetActive(true);//더이상 버튼 누르지못하게하기 위에 완료 버튼 이미지 추가
                bangMark_Challenge.SetActive(false);//느낌표 지우기
            }

            //도전과제 게이지 달성 조건 숫자
            gaugeConditionText_Challenge.GetComponent<Text>().text = "/"+Info[prefabnum].condition_challenge.ToString();
            
            //도전과제 게이지 증가
            Gauge_Challenge.GetComponent<Image>().fillAmount 
                = (float)DataController.instance.gameData.challengeGauge[prefabnum] / Info[prefabnum].condition_challenge;
            
            //도전과제 게이지 현재 숫자
            gaugeText_Challenge.GetComponent<Text>().text = DataController.instance.gameData.challengeGauge[prefabnum].ToString();
            

        }
        else //NEWS=======================================================
        {
            //뉴스 lock상태이면 가리기
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false)
            { lock_News.SetActive(true); }
            //뉴스 unlock상태이고 구매하지 않았으면
            else if (DataController.instance.gameData.NewsEnd[prefabnum]==false) { unlock_News.SetActive(true); }

            
            //뉴스 숫자
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }



        Prefabcount++;

    }

    //도전과제 달성후 완료 버튼 누를시
    public void ChallengeSuccess() {
        if (DataController.instance.gameData.challengeGauge[prefabnum] == Info[prefabnum].condition_challenge)
        {
            //메달==================================================
            DataController.instance.gameData.medal += Info[prefabnum].reward_challenge[0];//보상 추가
            medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//메달 현황 텍스트로 띄우기
            //하트==================================================
            DataController.instance.gameData.heart += Info[prefabnum].reward_challenge[1];//보상 추가



            DataController.instance.gameData.challengeEnd[prefabnum] = true;//보상받았다는것 표시
            doneButton_Challenge.SetActive(true);//더이상 버튼 누르지못하게하기(위에 완료 버튼 이미지 넣어서)

            bangMark_Challenge.SetActive(false);//느낌표 지우기
        }
    }






    public void UnlockNews() 
    {
        //메달 감소
        DataController.instance.gameData.medal--;
        DataController.instance.gameData.NewsEnd[prefabnum] = true;
        Destroy(unlock_News); 
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
