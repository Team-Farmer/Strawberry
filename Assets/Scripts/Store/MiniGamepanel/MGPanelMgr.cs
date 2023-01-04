using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGPanelMgr : MonoBehaviour
{
    //public GameObject MiniGamePanels;
    public Button[] MiniGamePanel; // 배열로 교체

    private GameObject[] dark_;
    private GameObject[] lock_;

    private bool enable_flag, disable_flag;
    // Start is called before the first frame update
    void Start()
    {
        dark_ = new GameObject[4];
        lock_ = new GameObject[4];

        for (int i=0; i<4; i++)
        {
            dark_[i] = MiniGamePanel[i].transform.GetChild(2).gameObject;
            lock_[i] = MiniGamePanel[i].transform.GetChild(3).gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int dotoriCnt = DataController.instance.gameData.dotori;

        if(dotoriCnt > 0)
        {
            if (disable_flag) return;
            
            enable_flag = false;
            disable_flag = true;

            for (int i = 0; i < 4; i++)
            {
                dark_[i].SetActive(false);
                lock_[i].SetActive(false);
                MiniGamePanel[i].interactable = true;
            }
        }
        else
        {
            if (enable_flag) return;
           
            enable_flag = true;
            disable_flag = false;

            for (int i = 0; i < 4; i++)
            {
                dark_[i].SetActive(true);
                lock_[i].SetActive(true);
                MiniGamePanel[i].interactable = false;
            }
        }
    }
}
