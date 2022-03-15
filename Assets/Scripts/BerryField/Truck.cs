using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Truck : MonoBehaviour
{
    public GameObject MaxPanel;    
    //public int berryCnt = 0; // ¿Å±è
    private Animator anim;
    public enum Count
    {
        Cnt0 = 0,
        Cnt1 = 16,
        Cnt2 = 32,       
        Max = 48
    }   
    void Awake()
    {
        anim = GetComponent<Animator>();        
    }
    
    void FixedUpdate()
    {
        if(0 <= DataController.instance.gameData.berryCnt && DataController.instance.gameData.berryCnt < 16)
        {
            if (anim.GetInteger("Truck") == 0) return;
            MaxPanel.SetActive(false);
            SetAnim(0);
        }
        if (16 <= DataController.instance.gameData.berryCnt && DataController.instance.gameData.berryCnt < 32)
        {
            if (anim.GetInteger("Truck") == 1) return;
            SetAnim(1);
        }
        if (32 <= DataController.instance.gameData.berryCnt && DataController.instance.gameData.berryCnt < 48)
        {
            if (anim.GetInteger("Truck") == 2) return;
            SetAnim(2);
        }
        if (DataController.instance.gameData.berryCnt == 48)
        {
            if (anim.GetInteger("Truck") == 3) return;
            MaxPanel.SetActive(true);
            SetAnim(3);
        }      
    }    
    void SetAnim(int level)
    {
        anim.SetInteger("Truck", level);
    }
}
