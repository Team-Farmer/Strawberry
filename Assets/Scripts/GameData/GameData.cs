using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* 세이브되어야 할 변수를 넣어주세요! */

public class GameData
{
    //재화
    public int coin;
    public int heart;
    public int medal;

    //Truck
    public int berryCnt; // 트럭 딸기 개수

    //딸기 밭 데이터 생성
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //해금된 딸기
    //배열 크기 조절 안되는건 원래 C#배열의 특성이다.
    //List를 사용하거나 Linq사용해야 함
    public bool[] isBerryUnlock = new bool[192];



    //연구 레벨
    public int[] researchLevel=new int[7];

    //도전과제 달성 수치
    public int[] challengeGauge = new int[7];
    //도전과제 보상 받은 여부
    public bool[] challengeEnd = new bool[7];

    //뉴스
    public bool[] isNewsUnlock=new bool[7];

    //마지막 출석 날짜 저장.
    public DateTime Lastday = new DateTime(2022, 3, 14);
    //출석 일 수
    public int days=0;
    //오늘 출석 여부 판단
    public bool attendance = false;

}
