using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    //public Text daram_txt;
    //public Text gameInfo_txt;
    //public string[] daramScript;
    //public string[] gameInfoScript;

    //public Button open_yes_btn;
    public GameObject Store;
    public Sprite[] StoreSprite;
    public GameObject popup;
    public GameObject inside;
    public Button UnlockBtn;

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
            Debug.Log(DataController.instance.gameData.isStoreOpend);
            Debug.Log("팝업열림");
            popup.SetActive(true);

            //해금조건 - 연구레벨 15이상, 700A 소모 가능상태
            //if (DataController.instance.gameData.coin >= 700 && ResearchLevelCheck(14))
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
}
