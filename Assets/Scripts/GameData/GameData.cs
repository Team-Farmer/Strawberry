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

    //밭, 딸기

    // 딸기 성장 시간
    public float[] stemLevel = { 0f, 5f, 10f, 15f, 20f };

    //이거 바꾸려면 연구에서 직접 접근해서 바꿔야됨 여기는 초기화부분
    public float bugProb = 20f;
    public float weedProb = 20f; 

    //잡초 생성 주기
    public float weedPeriod = 30f;

    // 비의 지속 시간
    public float rainDuration = 5.0f;

    //딸기 밭 데이터 생성
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //해금된 딸기
    //배열 크기 조절 안되는건 원래 C#배열의 특성이다.
    //List를 사용하거나 Linq사용해야 함

    //=====================================================================================
    public bool[] isBerryUnlock = new bool[192];

    //새로운 딸기 개발
    public int newBerryBtnState;//딸기 개발 버튼 상태
    public int newBerryIndex;
    public int newBerryTime;//딸기 개발 시간

    //베리 개발 가능 구역
    //0이면 classic만, 1이면 unique까지, 2이면 전부 개발가능
    public int newBerryResearchAble;

    //연구 레벨
    public int[] researchLevel=new int[6];

    //도전과제
    public int[] challengeLevel = new int[7];//현재 달성 수치

    //뉴스
    public int[] newsState = new int[7];//0=Lock, 1=Unlock Able 2=Unlock

    //수집
    public bool[] isCollectionDone = new bool[7];

    //PTJ 알바생의 현재 고용횟수
    public int[] PTJNum = new int[6];
    public int PTJCount;
    public int[] PTJSelectNum =new int[2];//지금 선택된 (prefabNum , 슬라이더 넘버)

    //느낌표 ! 이거 없앨ㄴ수있음?
    public bool[] isBerryEM = new bool[192];
    public bool[] isNewsEM = new bool[7];
    //=====================================================================================

    //도전과제를 위한 누적 저장 변수 (누적 : accumulate)
    public int unlockBerryCnt; // 해금 딸기 개수 (누적)
    public int totalHarvBerryCnt; // 수확 딸기 개수 (누적)
    public int accCoin; // 누적 코인
    public int accHeart; // 누적 하트
    public int accAttendance; // 누적 출석
    public int mgPlayCnt; // 미니게임 플레이 횟수
    //여태까지 모은 딸기 총 개수
    //public int totalBerryCnt; => 이게 수확한거 맞죠?
    //=====================================================================================


    //마지막 출석 날짜 저장.
    public DateTime Lastday = new DateTime();
    public DateTime Today = new DateTime();
    //출석 일 수
    public int days=0;
    //오늘 출석 여부 판단
    public bool attendance = false;

    //가게&미니게임
    public bool isStoreOpend;
    public int[] highScore=new int[4];
}
