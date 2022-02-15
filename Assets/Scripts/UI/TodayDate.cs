using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TodayDate : MonoBehaviour
{

    [SerializeField] Text today;

    void Update()
    {
        today.text = DateTime.Now.ToString("yyyy≥‚ MM dd¿œ dddd");
    }
}
