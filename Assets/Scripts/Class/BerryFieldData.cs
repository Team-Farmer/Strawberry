using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFieldData
{
    //Bug
    public float bugProb = 15f; // 옮김
    public float scale; // 옮김
    public bool isBugEnable = false; // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고

    //Weed
    public float weedProb = 20f; // // 임의로 초기화 시킨 변수임
    public float xPos = 0f;   
    public int weedSpriteNum; 
    public bool isWeedEnable = false; // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고

    //Farm
    public bool isPlant = false;   
    public bool isHarvest = false;

    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 60f; // 임의로 초기화 시킨 변수임

    //Stem
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
   
    public int berryPrefabNowIdx;
    public bool isStemEnable = false; // // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고
    public float randomTime = 0f;
}
