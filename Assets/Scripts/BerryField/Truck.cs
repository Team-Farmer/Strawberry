using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public GameObject MaxPanel;    
    public int berryCnt = 0;
    private Animator anim;
    public enum Count
    {
        Cnt0 = 0,
        Cnt1 = 16,
        Cnt2 = 32,       
        Max = 48
    }
 
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if((int)Count.Cnt0 <= berryCnt && berryCnt < (int)Count.Cnt1)
        {
            if (anim.GetInteger("Truck") == 0) return;
            MaxPanel.SetActive(false);
            SetAnim(0);
        }
        if ((int)Count.Cnt1 <= berryCnt && berryCnt < (int)Count.Cnt2)
        {
            if (anim.GetInteger("Truck") == 1) return;
            SetAnim(1);
        }
        if ((int)Count.Cnt2 <= berryCnt && berryCnt < (int)Count.Max)
        {
            if (anim.GetInteger("Truck") == 2) return;
            SetAnim(2);
        }
        if ((int)Count.Max == berryCnt)
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
