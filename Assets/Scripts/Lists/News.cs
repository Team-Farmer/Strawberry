using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    [Serializable]
    public class NewsStruct
    {
        public string Title;//제목
        public string Exp;//뉴스 내용

        public NewsStruct(string Title, string Exp)
        {
            this.Title = Title;
            this.Exp = Exp;
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


    [Header("==========패널==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;



    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    GameObject newsExp;
    GameObject newsContent;

    //=======================================================================================================================
    //=======================================================================================================================
    private void Start()
    {
        newsExp = GameObject.FindGameObjectWithTag("NewsExplanation").transform.GetChild(0).gameObject;
        newsContent = GameObject.FindGameObjectWithTag("NewsContent");

        //메달
        GameManager.instance.ShowMedalText();

        //프리팹들에게 고유 번호 붙이기
        prefabnum = Prefabcount;
        Prefabcount++;

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
        switch (DataController.instance.gameData.newsState[prefabnum])
        {
            case 0://LOCK
                //lock상태는 누를 수 없다.
                break;
            case 1://UNLOCK ABLE
                if (DataController.instance.gameData.medal >= 1)
                {
                    GameManager.instance.UseMedal(1);//메달 소비
                    //unlock상태
                    Unlockable.SetActive(false);
                    DataController.instance.gameData.newsState[prefabnum] = 2;//잠금해제

                    //만약 다음게 있다면
                    if (prefabnum+1 != newsContent.transform.GetChildCount())
                    {
                        DataController.instance.gameData.newsState[prefabnum + 1] = 1;//다음거 잠금 해제 가능하게

                        newsContent.transform.GetChild(prefabnum + 1).//다음거 찾아서
                            transform.GetChild(4).gameObject.SetActive(true);//잠금해제 이미지로 변경
                        newsContent.transform.GetChild(prefabnum + 1).
                            transform.GetChild(3).gameObject.SetActive(false);//잠금이미지 지우기
                    }
                    //안내창
                    /*
                    BP.SetActive(true);
                    confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뉴스가 해금되었어요!";
                    confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                    */

                    //5의 배수이면 딸기를 얻는다.
                    if ((prefabnum + 1) % 5 == 0)
                    { Debug.Log("딸기.."); }
                }
                else
                {
                    /*
                    //메달이 부족할 시
                    warnningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "뱃지가 부족해요!";
                    BP.SetActive(true);
                    warnningPanel.GetComponent<PanelAnimation>().OpenScale();
                    */
                }

                break;
            case 2://UNLOCK
                //설명 창을 띄운다.
                Explantion();
                break;
        }

        InfoUpdate();



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
