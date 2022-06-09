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
    public Text infoText;

    void Awake()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
            Store.GetComponent<Image>().sprite = StoreSprite[1];
    }
    
    public void EnterStore()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
            inside.SetActive(true);
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
        switch(btnIdx)
        {
            case 1:
                infoText.text = "딸기 창고가 정전이 되었어요! 빨리 딸기들을 분류해서 팔아야 하는데... 안은 깜깜하고 어둑어둑한 바깥은 비까지 내려요. 하지만 우리는 하루 이틀 딸기를 수확한 게 아니죠! 어둠 속에서 최대한 많은 딸기들을 분류해 봐요!";
                break;

        }
    }
}
