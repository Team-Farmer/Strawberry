using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGPanelMgr : MonoBehaviour
{
    public GameObject MiniGamePanels;
    public Button MiniGamePanel_1;
    public Button MiniGamePanel_2;
    public Button MiniGamePanel_3;
    public Button MiniGamePanel_4;

    private GameObject lock_1;
    private GameObject lock_2;
    private GameObject lock_3;
    private GameObject lock_4;

    private bool enable_flag, disable_flag;
    // Start is called before the first frame update
    void Start()
    {
        lock_1 = MiniGamePanel_1.transform.GetChild(2).gameObject;
        lock_2 = MiniGamePanel_2.transform.GetChild(2).gameObject;
        lock_3 = MiniGamePanel_3.transform.GetChild(2).gameObject;
        lock_4 = MiniGamePanel_4.transform.GetChild(2).gameObject;
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
            lock_1.SetActive(false); lock_2.SetActive(false);
            lock_3.SetActive(false); lock_4.SetActive(false);
            MiniGamePanel_1.interactable = true; MiniGamePanel_2.interactable = true;
            MiniGamePanel_3.interactable = true; MiniGamePanel_4.interactable = true;

        }
        else
        {
            if (enable_flag) return;
           
            enable_flag = true;
            disable_flag = false;
            lock_1.SetActive(true); lock_2.SetActive(true);
            lock_3.SetActive(true); lock_4.SetActive(true);

            MiniGamePanel_1.interactable = false; MiniGamePanel_2.interactable = false;
            MiniGamePanel_3.interactable = false; MiniGamePanel_4.interactable = false;
        }
    }
}
