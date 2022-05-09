using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{

    string CURRENT_EVENT = "";
    void Update()
    {
        if (Input.GetMouseButton(0)) { currentEvent = "LEFT"; }
        if (Input.GetMouseButton(2)) { currentEvent = "CENTER"; }
        if (Input.GetMouseButton(1)) { currentEvent = "RIGHT"; }

    }
    string currentEvent
    {
        set
        {
            if (CURRENT_EVENT == value) return;

            CURRENT_EVENT = value;

            Debug.Log(CURRENT_EVENT);
        }
        get
        {
            return CURRENT_EVENT;
        }
    }
    

    
}
