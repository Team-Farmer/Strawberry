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

    // 트럭 내의 딸기 구간 개수 2차원 배열로 설정
    public int[,] truckCntLevel = { { 0, 0, 0 }, { 16, 24, 32 }, { 32, 48, 64 }, { 48, 72, 96 } };

    public readonly int CLASSIC_FIRST = 6;
    public readonly int SPECIAL_FIRST = 50;
    public readonly int UNIQUE_FIRST = 100;

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
