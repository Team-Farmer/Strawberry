using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public GameObject Store;
    public Sprite[] StoreSprite;
    public GameObject popup;
    public GameObject inside;
    public Button UnlockBtn;
    public Button startButton;
    public Text infoText;
    public Text daramSpeechBubbleText;
    public List<GameObject> miniGameList;
    public static bool isOpen;

    void Awake()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
            Store.GetComponent<Image>().sprite = StoreSprite[1];
    }

    public void EnterStore()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
        {
            inside.SetActive(true);
            isOpen = true;
            AudioManager.instance.BGMPlay(2);
        }
        else
        {
            //Debug.Log(DataController.instance.gameData.isStoreOpend);
            ////해금조건 - 연구레벨 15이상, 700A 소모 가능상태
            //if (DataController.instance.gameData.coin >= 700 && ResearchLevelCheck(15))
            //{
            //    UnlockBtn.interactable = true;
            //}
            //else
            //{
            //    UnlockBtn.interactable = false;
            //}
        }
    }

    public void ClickUnlockStore()
    {
        DataController.instance.gameData.isStoreOpend = true;
        popup.SetActive(false);
        Store.GetComponent<Image>().sprite = StoreSprite[1];
        AudioManager.instance.BGMPlay(2);
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

    public void OnclickStartBtn()
    {
        string info_str = infoText.text.Substring(0, 5);
        Debug.Log(info_str);
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
