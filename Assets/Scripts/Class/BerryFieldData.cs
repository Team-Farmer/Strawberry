using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFieldData
{
    //Bug
    
    public float scale; // 옮김
    public bool isBugEnable = false; // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고

    //Weed   
    public float xPos = 0f;   
    public int weedSpriteNum; 
    public bool isWeedEnable = false; // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고

    //Farm
    public bool isPlant = false;       
    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;   

    //Stem
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
    public int seedAnimLevel = 0;
    public int berryPrefabNowIdx;
    public bool isStemEnable = false; // // true면은 있었던거니까 켜주고 false면은 없던거니깐 건들지말고
    public float randomTime = 0f;
}
