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

    void Awake()
    {
        instance = this;
        SetBerryPrice();
    }

    void SetBerryPrice()
    {
        berryListAll[0].GetComponent<Berry>().berryPrice = 10;     // Ŭ������ 0�� ����
        berryListAll[64].GetComponent<Berry>().berryPrice = 20;    // ������� 0�� ����
        berryListAll[128].GetComponent<Berry>().berryPrice = 30;   // ����ũ�� 0�� ����

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