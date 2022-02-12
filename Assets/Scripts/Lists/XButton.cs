using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XButton : MonoBehaviour
{
    [Header("끄고싶은 창")]
    public GameObject[] Obj;
    [Header("검정패널")]
    public GameObject PanelBlack;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void turnOff() {

        for (int i = 0; i < Obj.Length; i++) 
        {
            if (Obj[i].activeSelf == true)
            {
                Obj[i].SetActive(false);
            }
            else
            {
                Obj[i].SetActive(true);
            }
        }
        
    }
}
