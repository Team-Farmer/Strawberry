using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGameManager : MonoBehaviour
{
    public GameObject Store;
    public Sprite[] StoreSprite;
    public GameObject popup;
    public GameObject inside;
    public GameObject dotori;
    public PanelAnimation storeConfirm;
    public PanelAnimation blackPanel;
    public Button UnlockBtn;
    public Button startButton;
    public Text infoText;
    public Text daramSpeechBubbleText;
    public Text dotoriTimer;
    public List<GameObject> miniGameList;
    public static bool isOpen;
    public static bool isTimerOn;

    public static MiniGameManager instance { get; private set; }

    void Awake()
    {
        instance = this;
        if (DataController.instance.gameData.isStoreOpend == true)
            Store.GetComponent<Image>().sprite = StoreSprite[1];
        isTimerOn = false;
    }

    public void EnterStore()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
        {
            inside.SetActive(true);
            dotori.SetActive(true);
            isOpen = true;
            AudioManager.instance.BGMPlay(2);

            //예림-오디오
            GameManager.instance.isMiniGameMode = true;
            AudioManager.instance.PauseAudio("RainSFXSound");
            dotoriTimer.text = DataController.instance.gameData.nextDotoriTime.ToString("mm':'ss");
            DotoriInit();
        }
        else
        {
            Debug.Log(DataController.instance.gameData.isStoreOpend);
            blackPanel.gameObject.SetActive(true);
            blackPanel.Fadein();
            storeConfirm.OpenScale();
            //해금조건 - 연구레벨 15이상, 700A 소모 가능상태
            
            if (DataController.instance.gameData.coin >= 700 && ResearchLevelCheck(15))
            {
                UnlockBtn.interactable = true;
            }
            else
            {
                UnlockBtn.interactable = false;
            }
        }
    }

    public void ClickUnlockStore()
    {
        DataController.instance.gameData.isStoreOpend = true;
        popup.SetActive(false);
        Store.GetComponent<Image>().sprite = StoreSprite[1];
        AudioManager.instance.BGMPlay(2);

        //예림-오디오
        GameManager.instance.isMiniGameMode = true;
        AudioManager.instance.PauseAudio("RainSFXSound");
        dotoriTimer.text = DataController.instance.gameData.nextDotoriTime.ToString("mm':'ss");
        DotoriInit();
    }

    private bool ResearchLevelCheck(int level)
    {
        for (int i = 0; i < DataController.instance.gameData.researchLevel.Length; i++)
        {
            if (DataController.instance.gameData.researchLevel[i] < level)
            { return false; }
        }
        return true;
    }

    public void ExitStore()
    {
        isOpen = false;
        GameManager.instance.isMiniGameMode = false;
        AudioManager.instance.ResumePlayAudio("RainSFXSound");
        DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;
        StopCoroutine(DotoriTimer());
    }

        public void SetInfo(int btnIdx)
    {
        switch (btnIdx)
        {
            case 1:
                infoText.text = "딸기 창고가 정전이 되었어요!\n빨리 딸기들을 분류해서 팔아야 하는데...\n안은 깜깜하고 어둑어둑한 바깥은 비까지 내려요.\n하지만 우리는 하루 이틀 딸기를 수확한 게 아니죠!\n어둠 속에서 최대한 많은 딸기들을 분류해 봐요!";
                daramSpeechBubbleText.text = "사장님,\n이게 어떤 딸기인지\n모르겠어요. 도와주세요!";
                break;
            case 2:
                infoText.text = "잘 익은 딸기들 따서 모아뒀었는데,\n날이 더워서인지 몇몇 딸기들이 무른 것 같아요.\n하지만 우리에겐 굳은 신념이 있죠.\n가장 달콤하고 가장 싱싱한 딸기들을 팔자!\n그러니 눈을 크게 뜨고 상한 딸기들을 골라내봐요!";
                daramSpeechBubbleText.text = "사장님,\n어떤 게 상한 딸기일까요?\n 도와주세요!";
                break;
            case 3:
                infoText.text = "딸기 밭에 폭풍이 지나갔어요!\n밭에 열려있던 딸기들도, 수확해둔 딸기들도\n모두 모두 날아가 버렸어요...\n어라? 근데 날아갔던 딸기들이 하늘에서 떨어지네요? 보고만 있을 때가 아니죠!\n바구니로 멀쩡한 딸기들만 최대한 담아봐요!";
                daramSpeechBubbleText.text = "사장님,\n잔뜩 떨어지는 딸기를\n받아야해요. 도와주세요!";
                break;
            case 4:
                infoText.text = "우리가 정성들여 딸기를 키우고\n열심히 수확했더니 트럭에 딸기들이 가득 찼어요!\n딸기들을 판매하기 위해서는\n우선 딸기들을 종류에 맞게 나누어야 해요.\n비슷하게 생겨도 문제 없죠! 재빠르게 딸기들을 분류해서 알맞게 박스에 담아봐요!";
                daramSpeechBubbleText.text = "사장님,\n트럭에 있는 딸기들을\n분류해야해요. 도와주세요!";
                break;
        }
    }

    public void DotoriInit()
    {

        if (DataController.instance.gameData.dotori < 5) //0초 이상일 때 , 5개보다 적을 때
        {
            if (DataController.instance.gameData.totalDotoriTime.TotalMinutes >= 300) // 최대치 300 고정
                DataController.instance.gameData.totalDotoriTime = TimeSpan.FromMinutes(300);

            TimeSpan gap = DataController.instance.gameData.currentTime // 미니게임 부재중 시간
            - DataController.instance.gameData.lastMinigameExitTime;

            DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;

            double dotoriDiv = Math.Round(gap.TotalSeconds / 3600f); // 충전해야하는 도토리 개수
            double dotoriRem = Math.Round(gap.TotalSeconds % 3600f); // 남은 타이머 갱신 시간

            if (DataController.instance.gameData.totalDotoriTime.TotalSeconds > 0)
            {
                DataController.instance.gameData.totalDotoriTime=DataController.instance.gameData.totalDotoriTime.Subtract(TimeSpan.FromHours((int)dotoriDiv));
                DataController.instance.gameData.totalDotoriTime=DataController.instance.gameData.totalDotoriTime.Subtract(TimeSpan.FromSeconds(dotoriRem));

                if (DataController.instance.gameData.totalDotoriTime.TotalSeconds <= 0)
                    DataController.instance.gameData.totalDotoriTime = TimeSpan.FromSeconds(0);
                else
                {
                    if (DataController.instance.gameData.totalDotoriTime.TotalSeconds < 3600f)
                        DataController.instance.gameData.nextDotoriTime = DataController.instance.gameData.totalDotoriTime;
                    else
                    {
                        TimeSpan gap2 = DataController.instance.gameData.nextDotoriTime.Subtract(TimeSpan.FromSeconds(dotoriRem));
                        if(gap2.TotalSeconds<=0)
                            DataController.instance.gameData.nextDotoriTime = DataController.instance.gameData.nextDotoriTime.Subtract(TimeSpan.FromSeconds(3600f-gap.TotalSeconds));
                        else
                            DataController.instance.gameData.nextDotoriTime = DataController.instance.gameData.nextDotoriTime.Subtract(TimeSpan.FromSeconds(dotoriRem));
                    }
                       
                }
            }

            if (DataController.instance.gameData.dotori + (int)dotoriDiv > 5) // 5개 이상이면
            {
                DataController.instance.gameData.dotori = 5;
                GameManager.instance.invokeDotori();
                DataController.instance.gameData.nextDotoriTime = TimeSpan.FromSeconds(0);
                DataController.instance.gameData.totalDotoriTime = TimeSpan.FromSeconds(0);
                dotoriTimer.text = "00:00";
            }
            else
            {
                DataController.instance.gameData.dotori += (int)dotoriDiv;
                GameManager.instance.invokeDotori();
                dotoriTimer.text = DataController.instance.gameData.nextDotoriTime.ToString("mm':'ss");
                //Debug.Log("다음 도토리 충전까지 : " + DataController.instance.gameData.nextDotoriTime);
                //Debug.Log("총 남은 시간 : " + DataController.instance.gameData.totalDotoriTime);

                if(isTimerOn==false)
                    StartCoroutine(DotoriTimer());
            }
        }
        else
        {
            GameManager.instance.invokeDotori();
            DataController.instance.gameData.nextDotoriTime = TimeSpan.FromSeconds(0);
            DataController.instance.gameData.totalDotoriTime = TimeSpan.FromSeconds(0);
            dotoriTimer.text = "00:00";
        }
    }

    IEnumerator DotoriTimer()
    {
        isTimerOn = true;
        while (DataController.instance.gameData.nextDotoriTime.TotalSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            DataController.instance.gameData.nextDotoriTime=DataController.instance.gameData.nextDotoriTime.Subtract(TimeSpan.FromSeconds(1f));
            DataController.instance.gameData.totalDotoriTime=DataController.instance.gameData.totalDotoriTime.Subtract(TimeSpan.FromSeconds(1f));
            dotoriTimer.text = DataController.instance.gameData.nextDotoriTime.ToString("mm':'ss");
        }


        //남은 총 시간을 보여줘야지 next 도토리 타임은 한번 충전할 때 쓰려고 만든거임
        if (DataController.instance.gameData.totalDotoriTime.TotalSeconds <= 0)
        {
            isTimerOn = false;
            yield return null;
        }
        else
        {
            DataController.instance.gameData.dotori++;
            GameManager.instance.invokeDotori();
            DataController.instance.gameData.nextDotoriTime = TimeSpan.FromSeconds(3600f);
            dotoriTimer.text = DataController.instance.gameData.nextDotoriTime.ToString("mm':'ss");

            //String str = DataController.instance.gameData.nextDotoriTime.ToString("hhmm");
            //GameManager.instance.dotoriTimer.text = str;

            isTimerOn = false;
            StartCoroutine(DotoriTimer());
        }
    }

    public void OnclickStartBtn()
    {
        string info_str = infoText.text.Substring(0, 5);

        dotori.SetActive(false);
        DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;
        StopCoroutine(DotoriTimer());

        if (info_str == "딸기 창고")
        {
            miniGameList[0].gameObject.SetActive(true);
            AudioManager.instance.BGMPlay(4);
        }
        else if (info_str == "잘 익은 ")
        {
            miniGameList[1].gameObject.SetActive(true);
            AudioManager.instance.BGMPlay(4);
        }
        else if (info_str == "딸기 밭에")
        {
            miniGameList[2].gameObject.SetActive(true);
            AudioManager.instance.BGMPlay(6);
        }
        else if (info_str == "우리가 정")
        {
            miniGameList[3].gameObject.SetActive(true);
            AudioManager.instance.BGMPlay(6);
        }

        
    }
}
