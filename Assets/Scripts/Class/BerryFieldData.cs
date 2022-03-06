using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFieldData
{
    //Bug
    public float bugProb;
    public float scale;

    //Weed
    public float weedProb;
    public float xPos;
    public int weedSpriteNum;

    //Farm
    public bool isPlant = false;
    public int farmIdx;
    public bool isHarvest = false;

    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 12f;

    //StrawBerry
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;

    public int berryIdx;
    public int level;
    public int kind = -1;
    public int rank = -1;
    public float randomTime = 0f;
    public int rankChance;
    public int kindChance;

   
}
