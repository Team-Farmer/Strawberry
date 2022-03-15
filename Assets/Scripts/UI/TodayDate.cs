using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TodayDate : MonoBehaviour
{

    [SerializeField] Text today; //현재 날짜 표시


    void Update()
    {
        today.text = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");
    }


}
