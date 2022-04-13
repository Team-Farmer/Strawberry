using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globalvariable : MonoBehaviour
{
    public static Globalvariable instance;

    public List<GameObject> berryListAll;

    public List<GameObject> classicBerryList;
    public List<GameObject> specialBerryList;
    public List<GameObject> uniqueBerryList;

    public enum BerryRank
    {
        Classic,
        Special,
        Unique
    }

    public const int TRUCK_CNT_LEVEL_0 = 0;
    public const int TRUCK_CNT_LEVEL_1 = 16;
    public const int TRUCK_CNT_LEVEL_2 = 32;
    public const int TRUCK_CNT_LEVEL_MAX = 48;

    /*public const float STEM_LEVEL_0 = 0f;
    public const float STEM_LEVEL_1 = 30f;
    public const float STEM_LEVEL_2 = 60f;
    public const float STEM_LEVEL_3 = 90f;
    public const float STEM_LEVEL_MAX = 120f;*/

    void Awake()
    {
        instance = this;
        SetBerryPrice();
    }

    void SetBerryPrice()
    {
        berryListAll[0].GetComponent<Berry>().berryPrice = 10;     // 클래식의 0번 딸기
        berryListAll[64].GetComponent<Berry>().berryPrice = 20;    // 스페셜의 0번 딸기
        berryListAll[128].GetComponent<Berry>().berryPrice = 30;   // 유니크의 0번 딸기

        for (int i = 0; i < 192; i++)
        {
            if (berryListAll[i] == null) continue;

            if (i < 64)
                berryListAll[i].GetComponent<Berry>().berryPrice = berryListAll[0].GetComponent<Berry>().berryPrice + i * 3;
            else if (i < 128)
                berryListAll[i].GetComponent<Berry>().berryPrice = berryListAll[64].GetComponent<Berry>().berryPrice + (i - 64) * 5;
            else
                berryListAll[i].GetComponent<Berry>().berryPrice = berryListAll[128].GetComponent<Berry>().berryPrice + (i - 128) * 7;
        }       
    }
}
