using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XButton : MonoBehaviour
{
    [Header("≤Ù∞ÌΩÕ¿∫ √¢")]
    public GameObject[] Obj;


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
