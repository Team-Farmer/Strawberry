using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivation : MonoBehaviour
{
    //Active Deactivation if user has no strawberry
    public GameObject deactivation;

    private void OnEnable()
    {
        int cnt = 0;
        for(int i = 0; i < 192; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i] == true)
                cnt++;
        }

        if (cnt<5)
        {
            deactivation.SetActive(true);
        }
        else deactivation.SetActive(false);
    }
}
