using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    Animator anim;
    public int berryCnt = 0;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(0 <= berryCnt && berryCnt < 30)
        {
            if (anim.GetInteger("Truck") == 0) return;
            SetAnim(0);
        }
        if (30 <= berryCnt && berryCnt < 60)
        {
            if (anim.GetInteger("Truck") == 1) return;
            SetAnim(1);
        }
        if (60 <= berryCnt && berryCnt < 90)
        {
            if (anim.GetInteger("Truck") == 2) return;
            SetAnim(2);
        }
        if (90 <= berryCnt)
        {
            if (anim.GetInteger("Truck") == 3) return;
            SetAnim(3);
        }      
    }    
    void SetAnim(int level)
    {
        anim.SetInteger("Truck", level);
    }
}
