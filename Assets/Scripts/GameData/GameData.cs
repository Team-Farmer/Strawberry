using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* 세이브되어야 할 변수를 넣어주세요! */

public class GameData
{
    //Truck
    public int berryCnt; // 트럭 딸기 개수

    //딸기 밭 데이터 생성
    public BerryFieldData berryFieldData = new BerryFieldData();

    //해금된 딸기
    public bool[] isBerryUnlock = new bool[192];

    //딸기 발생 확률
    public int[] berryRankProb = { 50, 35, 15 };

    //알바 가지고있는지 여부
    //public bool[] isEmployed;

    //연구 레벨
    public int[] researchLevel=new int[7];

    //마지막 출석 날짜 저장.
    //DateTime Lastday = new DateTime();

}
