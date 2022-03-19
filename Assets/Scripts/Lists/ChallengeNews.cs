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
        public string Title;
        public int[] reward;
        public bool isDone_c;
        //public bool isUnlock_n;
        public string Exp_news;
        //public int Gauge_c;


        public ChallengeNewsStruct(string Title, int[] reward, bool isDone_c, bool isUnlock_n, string Exp_news, int Gauge_c)
        {
            this.Title = Title;
            this.reward = reward;
            this.isDone_c = isDone_c;
            //this.isUnlock_n = isUnlock_n;
            this.Exp_news = Exp_news;
            //this.Gauge_c = Gauge_c;
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
    private GameObject lock_News;
    public GameObject doneButton_Challenge;


    [SerializeField]
    private GameObject gaugeText_Challenge;
    [SerializeField]
    private GameObject Btn_Challenge;

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
        
        newsExplanation = GameObject.Find("newsExplanation");//얘네들 find 따로 스크립트로 빼서 할까 고민해볼것
        medalText = GameObject.Find("medalCount");
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
            if (DataController.instance.gameData.challengeGauge[prefabnum] == 30)//도전과제 완료, 30은 변수로 나중에 바꿀것
            {
                medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//메달 현황 텍스트로 띄우기
                Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];
                
            }
            //도전과제 게이지 수치
            Gauge_Challenge.GetComponent<Image>().fillAmount = (float)DataController.instance.gameData.challengeGauge[prefabnum] / 30;
            //도전과제 게이지 수치 문자
            gaugeText_Challenge.GetComponent<Text>().text = DataController.instance.gameData.challengeGauge[prefabnum].ToString();

        }
        else //NEWS=======================================================
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false) 
            { lock_News.SetActive(true); }
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }



        Prefabcount++;

    }

    //도전과제 달성후 완료 버튼 누를시
    public void ChallengeSuccess() {
        if (DataController.instance.gameData.challengeGauge[prefabnum] == 30)
        {
            DataController.instance.gameData.medal += 3;//메달 값 증가
            
            doneButton_Challenge.SetActive(true);//더이상 버튼 누르지못하게하기(위에 완료 버튼 이미지 넣어서)
        }
    }

    public void Explantion() {

        newsExp = newsExplanation.transform.GetChild(0).transform.gameObject;//newsExp

        newsExp.SetActive(true);
        try
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == true)
            {
                //Explanation 내용을 채운다.
                newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = Info[prefabnum].Title;//이름 설정 newsName
                newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text = Info[prefabnum].Exp_news;//설명 설정 newsTxt
                
            }
        }
        catch
        {
            //ExpChildren.SetActive(false);
            Debug.Log("ChallengeNews 인덱스 오류");
        }


    }
}
