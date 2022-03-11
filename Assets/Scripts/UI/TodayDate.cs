using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TodayDate : MonoBehaviour
{

    [SerializeField] Text today;
    DateTime Today = DateTime.Now;
    DateTime Lastday = new DateTime(2022, 3, 10);


    void Update()
    {
        //today.text = DateTime.Now.ToString("yyyy년 MM dd일 dddd");
    }

    public void isCompare()
    {
        int compare = Today.CompareTo(Lastday); // 1이나오면 날짜 다름
        Debug.Log("compare");
    }

}
