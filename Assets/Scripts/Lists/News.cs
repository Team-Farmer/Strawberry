using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    public static News instance;

    [Serializable]
    public class NewsStruct
    {
        public string Title;//제목
        public string Exp;//뉴스 내용
        public int Price;
        public NewsStruct(string Title, string Exp,int Price)
        {
            this.Title = Title;
            this.Exp = Exp;
            this.Price = Price;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    NewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject TitleText;
    [SerializeField]
    private GameObject CountText;
    [SerializeField]
    private GameObject Lock;//뉴스 잠금
    [SerializeField]
    private GameObject Unlockable;//뉴스 잠금 해제 가능


    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    GameObject newsExp;
    GameObject newsContent;

    //경고 창 패널들
    GameObject WarningPanel;
    GameObject YNPanel;
    GameObject ConfirmPanel;
    GameObject WarningPanelBlack;
    //=======================================================================================================================
    //=======================================================================================================================
    private void Awake()
    {
        instance = this;

        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;
    }
    private void Start()
    {
        newsExp = GameObject.FindGameObjectWithTag("NewsExplanation").transform.GetChild(0).gameObject;
        newsContent = GameObject.FindGameObjectWithTag("NewsContent");
        
        WarningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        WarningPanelBlack= WarningPanel.transform.GetChild(0).gameObject;
        YNPanel = WarningPanel.transform.GetChild(2).gameObject;
        ConfirmPanel= WarningPanel.transform.GetChild(7).gameObject;

        //메달
        GameManager.instance.ShowMedalText();
     
        //제목
        TitleText.GetComponent<Text>().text = Info[prefabnum].Title;
        //뉴스 숫자
        CountText.GetComponent<Text>().text = "0" + (prefabnum + 1);

        //뉴스 상태 업데이트
        InfoUpdate();
    }
    //==================================================================================================================
    //==================================================================================================================
    
    public void InfoUpdate()
    {
        //뉴스 상태에 따라서 lock, unlockable,unlock으로 보이기 
        switch (DataController.instance.gameData.newsState[prefabnum]) 
        {
            case 0://LOCK
                Lock.SetActive(true);
                Unlockable.SetActive(false);
                break;
            case 1://UNLOCK ABLE
                Lock.SetActive(false);
                Unlockable.SetActive(true);
                break;
            case 2://UNLOCK
                Lock.SetActive(false);
                Unlockable.SetActive(false);
                break;
        }

    }


    public void NewsButton()
    {
        AudioManager.instance.Cute1AudioPlay();
        switch (DataController.instance.gameData.newsState[prefabnum])
        {
            case 0://LOCK
                GameManager.instance.NewsPrefabNum = prefabnum;
                YNPanel.GetComponent<PanelAnimation>().OpenScale();
                YNPanel.transform.GetChild(1).GetComponent<Text>().text
                    = "뱃지" + Info[prefabnum].Price + "개를 소모하여\n뉴스를 해금할까요?";
                WarningPanelBlack.SetActive(true);
                break;
            case 1://UNLOCK ABLE
                //처음 누르는 상황
                Explantion();
                DataController.instance.gameData.newsState[prefabnum] = 2;
                break;
            case 2://UNLOCK
                //설명 창을 띄운다.
                Explantion();
                break;
        }

        InfoUpdate();
    }

    //뉴스 해금
    public void NewsUnlock(int ID) //ID==PrefabNum
    {

        if (DataController.instance.gameData.medal >= Info[ID].Price)//메달이 충분하면
        {
            //메달 소비
            GameManager.instance.UseMedal(Info[ID].Price);

            //unlock상태로 변경
            DataController.instance.gameData.newsState[ID] = 1;
            InfoUpdate();
            GameObject thisNews = newsContent.transform.GetChild(ID).gameObject;
            thisNews.transform.GetChild(3).gameObject.SetActive(false);//Lock
            thisNews.transform.GetChild(4).gameObject.SetActive(true);//Lock



            int RandomNum = UnityEngine.Random.Range(1, 101);
            if (RandomNum <= Info[ID].Price * 10) //price*10%확률로 보너스 딸기 획득
            {
                //딸기 획득
                WarningPanelBlack.SetActive(true);
                ConfirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뉴스와 보너스 딸기가 해금되었어요!";
                ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
                GameManager.instance.newsBerry();
            }
            else //뉴스만 해금
            {
                //안내창
                WarningPanelBlack.SetActive(true);
                ConfirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뉴스가 해금되었어요!";
                ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
            }

            YNPanel.SetActive(false);
        }
        else
        {
            //메달이 부족할 시
            YNPanel.GetComponent<PanelAnimation>().CloseScale();
            WarningPanelBlack.SetActive(true);
            ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
            ConfirmPanel.transform.GetChild(0).transform.GetComponent<Text>().text = "메달이 부족해요!";
            
        }
    }

    //뉴스 설명창
    private void Explantion()
    {
        //설명창을 띄운다.
        newsExp.SetActive(true);
        //내용을 채운다.
        newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text
            = Info[prefabnum].Title;//제목
        newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text
            = Info[prefabnum].Exp;//설명
    }

}
