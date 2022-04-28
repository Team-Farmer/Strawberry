using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* 세이브되어야 할 변수를 넣어주세요! */

public class GameData
{
    //클라우드 저장 날짜
    public DateTime cloudSaveTime;

    //재화
    public int coin;
    public int heart;
    public int medal;

    //Truck
    public int truckBerryCnt; // 트럭 딸기 개수
    public int truckCoin;

    // 딸기 성장 시간
    public float[] stemLevel = { 0f, 30f, 60f, 90f, 120f};

    //딸기 밭 데이터 생성
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //해금된 딸기
    //배열 크기 조절 안되는건 원래 C#배열의 특성이다.
    //List를 사용하거나 Linq사용해야 함
    //=====================================================================================
    public bool[] isBerryUnlock = new bool[192];

    //새로운 딸기 개발
    public int newBerryResearch;

    //연구 레벨
    public int[] researchLevel=new int[7];

    //도전과제 달성 수치
    public int[] challengeGauge = new int[7];
    //도전과제 보상 받은 여부
    public bool[] challengeEnd = new bool[7];

    //도전과제를 위한 누적 저장 변수 (누적 : accumulate)
    public int unlockBerryCnt; // 해금 딸기 개수 (누적)
    public int totalHarvBerryCnt; // 수확 딸기 개수 (누적)
    public int accCoin; // 누적 코인
    public int accHeart; // 누적 하트
    public int accAttendance; // 누적 출석
    public int mgPlayCnt; // 미니게임 플레이 횟수
    //여태까지 모은 딸기 총 개수
    //public int totalBerryCnt; => 이게 수확한거 맞죠?

    //뉴스
    public bool[] isNewsUnlock=new bool[7];
    public bool[] NewsEnd = new bool[7];

    //PTJ
    public int[] PTJNum = new int[6];

    //=====================================================================================


    //마지막 출석 날짜 저장.
    public DateTime Lastday = new DateTime();
    public DateTime Today = new DateTime();
    //출석 일 수
    public int days=0;
    //오늘 출석 여부 판단
    public bool attendance = false;

}
