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

    private float coeffi = 0.02f;

    public const int TRUCK_CNT_LEVEL_0 = 0;
    public const int TRUCK_CNT_LEVEL_1 = 16;
    public const int TRUCK_CNT_LEVEL_2 = 32;
    public const int TRUCK_CNT_LEVEL_MAX = 48;

    public readonly int CLASSIC_FIRST = 1;
    public readonly int SPECIAL_FIRST = 100;
    public readonly int UNIQUE_FIRST = 200;

    public readonly float[] STEM_LEVEL = { 0f, 5f, 10f, 15f, 20f };

    public const float BUG_PROB = 20f;
    public const float WEED_PROB = 20f;

    public const float RAIN_DURATION = 5f;

    void Start()
    {
        instance = this;     
    }
    public float getEffi()
    {
        return this.coeffi;
    }
}
