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

    public ParticleSystem rainParticle;

    public enum BerryRank
    {
        Classic,
        Special,
        Unique
    }

    public float coeffi;

    public const int TRUCK_CNT_LEVEL_0 = 0;
    public const int TRUCK_CNT_LEVEL_1 = 16;
    public const int TRUCK_CNT_LEVEL_2 = 32;
    public const int TRUCK_CNT_LEVEL_MAX = 48;

    public const int CLASSIC_FIRST = 6;
    public const int SPECIAL_FIRST = 100;
    public const int UNIQUE_FIRST = 200;

    public readonly float[] STEM_LEVEL = { 0f, 15f, 30f, 45f, 60f };

    public const float BUG_PROB = 20f;
    public const float WEED_PROB = 20f;

    public const float Rain_Duration = 5f;

    void Awake()
    {
        instance = this;     
    }
    /*
    void Start()
    {
        
        IncreaseBerryPrice();
        DecreaseBerryGrowTime();
        DecreaseBugGenerateProb();
        DecreaseWeedGenerateProb();
        
    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0] - 1) * coeffi;

        for (int i = 0; i < 192; i++)
        {
            if (berryListAll[i] == null) continue;

            if (i < 64)
                berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1] - 1) * coeffi;

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = STEM_LEVEL[i] * (1 - researchCoeffi);
        }
    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1] - 1) * coeffi;
        DataController.instance.gameData.bugProb = BUG_PROB * (1 - researchCoeffi);
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1] - 1) * coeffi;
        DataController.instance.gameData.weedProb = WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1] - 1) * coeffi;

    }*/
}
