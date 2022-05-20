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

    public Button open_yes_btn;
    public GameObject popup;
    public GameObject inside;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CheckStoreState);
        open_yes_btn.onClick.AddListener(OnClickYesBtn);
    }

    void CheckStoreState()
    {
        if (DataController.instance.gameData.isStoreOpend)
        {
            inside.SetActive(true);
        }
        else
        {
            popup.SetActive(true);

            //해금조건 확인 : 연구레벨 15이상, 300A이상 보유
            bool flag = DataController.instance.gameData.coin >= 300;
            if (flag)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (DataController.instance.gameData.researchLevel[i] < 15)
                    {
                        open_yes_btn.enabled = false;
                        open_yes_btn.GetComponent<Image>().color = Color.gray;
                        //flag = false;
                        break;
                    }
                }
            }

            if (flag)
            {
                open_yes_btn.enabled = true;
                open_yes_btn.GetComponent<Image>().color = Color.white;
            }
        }
    }

    void OnClickYesBtn()
    {
        DataController.instance.gameData.isStoreOpend = true;
    }
}
