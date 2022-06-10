using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;



public class GameManager : MonoBehaviour
{
    #region 인스펙터
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    public Text MedalText;
    public Text coinAnimText;
    public Text heartAnimText;

    [Header("[ Object ]")]
    public GameObject stemPrefab; // 프리팹
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
    public List<GameObject> stemObjList = new List<GameObject>();
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;
    public Text truckCoinBonusText;
    public int bonusTruckCoin;

    public const int TRUCK_CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int TRUCK_CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int TRUCK_CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int TRUCK_CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    //PTJ, NEWS================================
    [Header("[ PTJ ]")]
    //PTJ 알바
    public GameObject workingCountText;//고용 중인 동물 수
    public GameObject PTJList;

    [Header("PTJ === Warning Panel")]
    public GameObject warningBlackPanel;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;

    //NEWS
    [NonSerialized]
    public int NewsPrefabNum;


    //새로운딸기================================
    [Header("[ NEW BERRY === OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;

    public GameObject TimeReuce_newBerry;
    public GameObject TimeReduceBlackPanel_newBerry;
    public GameObject TimeReducePanel_newBerry;
    public Text TimeReduceText_newBerry;
    public GameObject AcheivePanel_newBerry;

    public GameObject NoPanel_newBerry;
    public GameObject BlackPanel_newBerry;

    [Header("[ NEW BERRY === SPRITE ]")]
    public Sprite StartImg;
    public Sprite DoneImg;
    public Sprite IngImg;
    public SpriteRenderer[] stemLevelSprites;


    private int price_newBerry;//이번에 개발되는 베리 가격
    private string BtnState;//지금 버튼 상태
    private int newBerryIndex2;//이번에 개발되는 뉴스 베리 넘버

    [Header("[ NEW BERRY === GLOBAL ]")]
    public GameObject Global;
    //===========================================

    [Header("[ Check/Settings Panel ]")]
    public GameObject settingsPanel;
    public GameObject checkPanel;
    

    [Header("[ Check/Day List ]")]
    public GameObject attendanceCheck;
    public string url = "";

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public GameObject noCoinPanel;
    public GameObject noHeartPanel;
    public GameObject blackPanel;
    public GameObject coinAnimManager;
    public GameObject heartAnimManager;
    


    [Header("[ Game Flag ]")]
    public bool isGameRunning;
    public bool isBlackPanelOn = false;
    private int coinUpdate;

    #endregion

    #region 기본
    void Start()
    {

        Application.targetFrameRate = 60;
        instance = this; // 게임 매니저의 싱글턴 패턴화 >> GameManager.instance.~~ 로 호출



        target = TruckObj.GetComponent<Transform>();
        
        //for(int i = 0; i < )
        //출석 관련 호출.
        StartCoroutine(WebCheck());
        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        CheckTime();


        InitDataInGM();

        //TimerStart += Instance_TimerStart;

        DisableObjColliderAll();       
       
        isGameRunning = true;


        //NEW BERRY
        NewBerryUpdate();

        ShowCoinText(CoinText, DataController.instance.gameData.coin);
        HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    public void GameStart()
    {
        isGameRunning = true;
        Invoke("EnableObjColliderAll", 4.5f);
    }

    void InitDataInGM()
    {
        for (int i = 0; i < 16; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].isStemEnable)
            {
                stemList[i].gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isWeedEnable)
            {
                farmList[i].weed.gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isBugEnable)
            {
                bugList[i].gameObject.SetActive(true);
            }
            float creatTimeTemp = DataController.instance.gameData.berryFieldData[i].createTime;
            if ((0 < creatTimeTemp && creatTimeTemp < DataController.instance.gameData.stemLevel[4]) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
        //PTJ
        //없애기
        workingCountText.GetComponent<Text>().text = DataController.instance.gameData.PTJCount.ToString();//알바중인 인원수

        //NEW BERRY 개발
        //없애기
        switch (DataController.instance.gameData.newBerryBtnState) { 
            case 0:BtnState = "start"; startBtn_newBerry.GetComponent<Image>().sprite = StartImg; break;
            case 1: BtnState = "ing"; startBtn_newBerry.GetComponent<Image>().sprite = IngImg; break;
            case 2: BtnState = "done"; startBtn_newBerry.GetComponent<Image>().sprite = DoneImg; break;
        }

        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 옵젝을 가져온다
            if (obj != null)
            {

                if (obj.CompareTag("Farm"))
                {
                    ClickedFarm(obj);
                }
                else if (obj.CompareTag("Bug"))
                {
                    ClickedBug(obj);
                }
                else if (obj.CompareTag("Weed"))
                {
                    ClickedWeed(obj);
                }
            }
        }

        //폰에서 뒤로가기 버튼 눌렀을 때/에디터에서ESC버튼 눌렀을 때 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataController.instance.SaveData();
            Application.Quit();
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        //ShowCoinText(CoinText, DataController.instance.gameData.coin); // 트럭코인 나타낼 때 같이쓰려고 매개변수로 받게 수정했어요 - 신희규
        //HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    #endregion

    #region 딸기밭
    void ClickedFarm(GameObject obj)
    {

        Farm farm = obj.GetComponent<Farm>();

        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant)
        {
            Stem st = GetStem(farm.farmIdx);
            if (st != null)
            {
                PlantStrawBerry(st, obj); // 심는다
                AudioManager.instance.SowAudioPlay();
                DataController.instance
                    .gameData.berryFieldData[farm.farmIdx].isPlant = true; // 체크 변수 갱신
            }
        }
        else
        {
            if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].canGrow)
            {
                Harvest(stemList[farm.farmIdx]); // 수확
            }
        }
    }
    void ClickedBug(GameObject obj)
    {
        Bug bug = obj.GetComponent<Bug>();
        bug.DieBug();
    }
    void ClickedWeed(GameObject obj)
    {
        Weed weed = obj.GetComponent<Weed>();
        weed.DeleteWeed();
    }
    public void ClickedTruck()
    {
        bonusTruckCoin = (int)(DataController.instance.gameData.truckCoin *
            DataController.instance.gameData.researchLevel[2] * Globalvariable.instance.getEffi());
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);
        ShowCoinText(truckCoinBonusText, bonusTruckCoin);
    }

    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    public void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더를 비활성화 (잡초와 충돌 방지)
    }
    public void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;

        AudioManager.instance.HarvestAudioPlay();//딸기 수확할때 효과음
        farm.isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.getInstantBerryObj().GetComponent<Berry>().Explosion(pos, target.position, 0.5f);
        //stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 3;

        StartCoroutine(HarvestRoutine(farm, stem)); // 연속으로 딸기가 심어지는 현상을 방지

    }
    GameObject ClickObj() // 클릭당한 오브젝트를 반환
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm, Stem stem)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // 밭을 잠시 비활성화

        yield return new WaitForSeconds(0.75f); // 0.75초 뒤에

        UpdateTruckState(stem);

        DataController.instance.gameData.totalHarvBerryCnt++; // 수확한 딸기의 총 개수 업데이트            
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // 밭을 비워준다

        //줄기에 페이드 아웃 적용
        Animator anim = stemObjList[stem.stemIdx].GetComponent<Animator>();
        anim.SetInteger("Seed", 5);

        yield return new WaitForSeconds(0.3f); // 0.3초 뒤에

        stem.gameObject.SetActive(false);

        farm.isHarvest = false; // 수확이 끝남              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !isBlackPanelOn) // 잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화 
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < TRUCK_CNT_LEVEL_MAX)
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.getInstantBerryObj().GetComponent<Berry>().berryPrice;
        }
    }
    #endregion

    #region 재화

    IEnumerator CountAnimation(int cost, String text, int num) //재화 증가 애니메이션

    {
        if (num == 0)
        {
            if (cost <= 9999)           // 0~9999까지 A
            {
                coinAnimText.text = text + cost.ToString() + "A";
            }
            else if (cost <= 9999999)   // 10000~9999999(=9999B)까지 B
            {
                cost /= 1000;
                coinAnimText.text = text + cost.ToString() + "B";
            }
            else                        // 그 외 C (최대 2100C)
            {
                cost /= 1000000;
                coinAnimText.text = text + cost.ToString() + "C";
            }
            coinAnimManager.GetComponent<HeartMover>().CountCoin(-240f);
            Invoke("invokeCoin", 1.1f);
        }
        else
        {
            heartAnimText.text = text + cost.ToString();
            heartAnimManager.GetComponent<HeartMover>().CountCoin(100f);
            Invoke("invokeHeart", 1.1f);
        }
        yield return null;
    }

    public void invokeCoin()
    {
        ShowCoinText(CoinText, DataController.instance.gameData.coin);
    }

    public void invokeHeart()
    {
        HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999까지 A
        {
            coinText.text = coin.ToString() + "A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)까지 B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + "B";
        }
        else                        // 그 외 C (최대 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + "C";
        }
    }

    public void GetCoin(int cost) // 코인 획득 함수
    {
        StartCoroutine(CountAnimation(cost,"+",0));
        DataController.instance.gameData.coin += cost; // 현재 코인 +
        DataController.instance.gameData.accCoin += cost; // 누적 코인 +
    }

    public void UseCoin(int cost) // 코인 사용 함수 (마이너스 방지 위함)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
        {
            StartCoroutine(CountAnimation(cost,"-",0));
            DataController.instance.gameData.coin -= cost;
        }
        else
        {
            //경고 패널 등장
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            blackPanel.SetActive(true);
            noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetHeart(int cost) // 하트 획득 함수
    {
        StartCoroutine(CountAnimation(cost, "+", 1));
        DataController.instance.gameData.heart += cost; // 현재 하트 +
        DataController.instance.gameData.accHeart += cost; // 누적 하트 +
    }

    public void UseHeart(int cost) // 하트 획득 함수 (마이너스 방지 위함)
    {
        int myHeart = DataController.instance.gameData.heart;

        if (myHeart >= cost)
        {
            DataController.instance.gameData.heart -= cost;
            StartCoroutine(CountAnimation(cost,"-",1));
        }
        else
        {
            //경고 패널 등장
            panelHearText.text = DataController.instance.gameData.heart.ToString() + "개";
            blackPanel.SetActive(true);
            noHeartPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetMedal(int cost)
    {
        DataController.instance.gameData.medal += cost;
        ShowMedalText();
    }

    public void UseMedal(int cost)
    {
        int myMedal = DataController.instance.gameData.medal;
        if (myMedal >= cost)
        {
            DataController.instance.gameData.medal -= cost;
            ShowMedalText();
        }
        else
        {
            //메달이 모자를때 뜨는 경고
        }
    }
    public void ShowMedalText()
    {
        MedalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();
    }
    #endregion

    #region 콜라이더
    public void DisableObjColliderAll() // 모든 오브젝트의 collider 비활성화
    {
        BoxCollider2D coll;
        isBlackPanelOn = true;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            //stemList[i].canGrow = false;
            bugList[i].GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            // Weed의 Collider 제거
            //farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // 모든 오브젝트의 collider 활성화
    {
        BoxCollider2D coll;
        isBlackPanelOn = false;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // 잡초가 없을 때만 빈 밭의 Collider활성화
            {
                coll.enabled = true;
            }
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4]) // (4)의 상황, 즉 벌레와 잡초 둘 다 없을 때 다 자란 딸기밭의 콜라이더를 켜준다.
            {
                coll.enabled = true;
            }
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // 잡초의 Collider 활성화
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region 리스트

    #region PTJ

    //고용 버튼 클릭시
    public void PTJEmployButtonClick(int prefabNum) {
        //효과음
        AudioManager.instance.Cute1AudioPlay();

        //고용중이 아닌 상태이다
        if (DataController.instance.gameData.PTJNum[prefabNum] == 0)
        {
            if (DataController.instance.gameData.PTJCount < 3)
            {
                int cost = PTJ.instance.Info[prefabNum].Price * DataController.instance.gameData.PTJSelectNum[1];
                if (cost <= DataController.instance.gameData.coin)
                {
                    int ID = DataController.instance.gameData.PTJSelectNum[0];
                    //HIRE

                    //코인사용
                    UseCoin(cost);

                    //고용
                    DataController.instance.gameData.PTJNum[prefabNum] = DataController.instance.gameData.PTJSelectNum[1];

                    //고용중인 알바생 수 증가
                    DataController.instance.gameData.PTJCount++;
                }
                else 
                {
                    //효과음
                    AudioManager.instance.Cute4AudioPlay();
                    //재화 부족 경고 패널
                    ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                    warningBlackPanel.SetActive(true);
                }
            }
            else 
            {
                //효과음
                AudioManager.instance.Cute4AudioPlay();
                //3명이상 고용중이라는 경고 패널 등장
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용 가능한 알바 수를\n넘어섰어요!";
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                warningBlackPanel.SetActive(true);
            }
        }
        //고용중인 상태이다
        else
        {
            //FIRE
            //확인창띄우기
            HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            warningBlackPanel.SetActive(true);
        }
        

    }

    public void Fire() 
    {
        int ID = DataController.instance.gameData.PTJSelectNum[0];
        //고용 해제
        DataController.instance.gameData.PTJNum[ID] = 0;
        //고용 중인 알바생 수 감소
        //PTJ에서 구현됨

        //확인창내리기
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();
        warningBlackPanel.SetActive(false);
    }
    #endregion

    #region New Berry Add
    public void NewBerryUpdate()
    {
        //새 딸기 개발======

        //PRICE
        price_newBerry = 100 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
        priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
        
        if (DataController.instance.gameData.newBerryBtnState == 1)//진행중이면
        {   StartCoroutine("Timer");  }
        else { 
            if (isNewBerryAble() == true)//개발 가능한 딸기 있는지 검사
            {
                
                DataController.instance.gameData.newBerryIndex=selectBerry();//얻을딸기, 시간 정해진다
                timeText_newBerry.GetComponent<Text>().text = "??:??";//TIME (미공개)

                //베리 없음 지우기
                NoPanel_newBerry.SetActive(false);
            }
            else { NoPanel_newBerry.SetActive(true);}
        }
    }
    public void NewBerryUpdate2() //개발 가능한 딸기 범위 넓어질때 실행되는 거. 위에 합치자
    {

        if (isNewBerryAble() == true)//한 번 더 확인
        {
            if (DataController.instance.gameData.newBerryBtnState == 0)//딸기 얻고 있는 상태 아니면
            { 
                //얻을딸기가 정해진다.->시간,값도 정해진다.
                //PRICE
                price_newBerry = 100 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
                priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
                //TIME
                DataController.instance.gameData.newBerryIndex = selectBerry();
                timeText_newBerry.GetComponent<Text>().text = "??:??";//가격 시간 아직 미공개 "?"
                
            }
            //베리 없음 지우기
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true); }

    }

    private bool isNewBerryAble()
    {
        //지금 새딸기를 개발 할 수 있나
        switch (DataController.instance.gameData.newBerryResearchAble)
        {
            case 0://classic 개발가능
                if (BerryCount("classic",false) == BerryCount("classic", true)) 
                { return false; }
                break;
            case 1://classic, special 개발가능
                if (BerryCount("classic",false) + BerryCount("special", false) == 
                    BerryCount("classic", true) + BerryCount("special", true)) 
                { return false; }
                break;
            case 2: //classic, special, unique 개발가능
                if (BerryCount("classic", false) + BerryCount("special", false) + BerryCount("unique", false) == 
                    BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true))
                { return false; }
                break;
        }
        return true;
    }


    //isUnlock-> false=현재 값이 존재하는 딸기 갯수들을 반환 / true=현재 unlock된 딸기 갯수들을 반환한다.
    private int BerryCount(string berryClssify, bool isUnlock)
    {
        int countIsExsist = 0;
        int countIsUnlock = 0;
        switch (berryClssify)
        {
            case "classic":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().classicBerryList.Count; i++)
                {
                    if (DataController.instance.gameData.isBerryUnlock[i]==true) { countIsUnlock++; }
                    if (Global.GetComponent<Globalvariable>().classicBerryList[i] == true) { countIsExsist++; } 
                }
                break;

            case "special":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().specialBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64+64; i++)
                {if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().uniqueBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().uniqueBerryList[i] == true) { countIsExsist++; } }
                for (int i = 128; i < 128+64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;
            //default:Debug.Log("잘못된 값 받았다");break;
        }


        if (isUnlock == true)
        { return countIsUnlock; }
        else { return countIsExsist; }
    }



    //새로운 딸기 개발 버튼 누르면
    public void NewBerryButton()
    {

        switch (BtnState)
        {
            case "start":
                //이번 새딸기 개발에 필요한 가격과 시간
                priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();//가격
                timeText_newBerry.GetComponent<Text>().text 
                    = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));//시간

                if (DataController.instance.gameData.coin >= price_newBerry)
                {
                    //돈소비
                    UseCoin(price_newBerry);

                    //버튼상태 ing로
                    DataController.instance.gameData.newBerryBtnState = 1;
                    
                    //타이머 시작
                    StartCoroutine("Timer");

                    //시간 감소여부 묻는 패널을 띄운다.
                    TimeReduceBlackPanel_newBerry.SetActive(true); //시원 건드림
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림
                    TimeReduceText_newBerry.GetComponent<Text>().text //시원 건드림
                        = "하트 10개로 시간을 10분으로 줄이시겠습니까?\n";//지금은 1초. 임시
                }
                else//돈이 모자름
                {  UseCoin(price_newBerry);  }
                break;

            case "ing":
                //시간 감소여부 묻는 패널을 띄운다.
                TimeReduceBlackPanel_newBerry.SetActive(true);
                TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale();
                TimeReduceText_newBerry.GetComponent<Text>().text
                    = "하트 10개로 시간을 10분으로 줄이시겠습니까?\n";//지금은 1초. 임시
                break;

            case "done": //딸기 개발
                GetNewBerry();
                break;

        }
        
    }

    //TimeReucePanel_newBerry
    //하트 써서 시간을 줄인지 여부 패널
    public void TimeReduce(bool isTimeReduce)
    {
        //하트 써서 시간을 줄일거면
        if (isTimeReduce == true)
        { 
            if (DataController.instance.gameData.heart >= 10)
            {
                //시간을 10분 줄여준다.
                if (DataController.instance.gameData.newBerryTime < 10 * 60) 
                { DataController.instance.gameData.newBerryTime = 1; }
                else 
                { DataController.instance.gameData.newBerryTime -= 10 * 60; }
                
                timeText_newBerry.GetComponent<Text>().text 
                    = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));
                //하트를 소비한다.
                UseHeart(10);
            }
            else 
            {    UseHeart(10);   }
        }

        //창 끄기
        TimeReduceBlackPanel_newBerry.SetActive(false);
        TimeReducePanel_newBerry.GetComponent<PanelAnimation>().CloseScale();

    }

    IEnumerator Timer()
    {

        while (true)
        {
            //1초씩 감소
            yield return new WaitForSeconds(1f);
            DataController.instance.gameData.newBerryTime--;

            //감소하는 시간 보이기
            timeText_newBerry.GetComponent<Text>().text
                = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));

            //타이머 끝나면
            if (DataController.instance.gameData.newBerryTime < 0.1f)
            {
                DataController.instance.gameData.newBerryBtnState = 2;//Done상태로
                break;
            }
        }
        StopCoroutine("Timer");

    }


    private int selectBerry() 
    {
        int newBerryIndex = 1;

        while (true)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0: newBerryIndex = UnityEngine.Random.Range(1, 64);
                    DataController.instance.gameData.newBerryTime = 10*60;
                    break;
                case 1: newBerryIndex = berryPercantage(128);
                    break;
                case 2: newBerryIndex = berryPercantage(192); 
                    break;
            }

            if (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == false
            && Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex] != null)
            { break; }
        }
        return newBerryIndex;
    }


    private void GetNewBerry()
    {

        //새로운 딸기가 추가된다.
        DataController.instance.gameData.isBerryUnlock[DataController.instance.gameData.newBerryIndex] = true;
        DataController.instance.gameData.unlockBerryCnt++;

        //느낌표 표시
        DataController.instance.gameData.isBerryEM[DataController.instance.gameData.newBerryIndex] = true;

        //딸기 얻음 효과음(짜잔)
        AudioManager.instance.TadaAudioPlay();

        //얻은 딸기 설명창
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().sprite
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;

        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Text>().text
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<Berry>().berryName;

        //검정창 띄우기
        BlackPanel_newBerry.SetActive(true);


        DataController.instance.gameData.newBerryBtnState = 0;

        NewBerryUpdate();

    }
    
    private int berryPercantage(int endIndex) 
    { 
        int randomNum=0;
        int newBerryIndex = 0;

        //RANDOM NUM -> classic(45)=0~44  special(35)=45~79  unique(20)=80~101
        if (endIndex == 128) { randomNum = UnityEngine.Random.Range(0, 80); }//지금 클래식이랑 스페셜만 가능하면
        else if (endIndex == 192) { randomNum = UnityEngine.Random.Range(0, 100 + 1); }//지금 전부다 가능하면



        if (randomNum < 45) 
        { newBerryIndex = UnityEngine.Random.Range(1, 64); 
            DataController.instance.gameData.newBerryTime = 10*60; }//classic
        else if (randomNum < 80) 
        { newBerryIndex = UnityEngine.Random.Range(64, 128); 
            DataController.instance.gameData.newBerryTime = 20*60; }//special
        else if (randomNum <= 100) 
        { newBerryIndex = UnityEngine.Random.Range(128, 192); 
            DataController.instance.gameData.newBerryTime = 30*60; }//unique


        return newBerryIndex;
    }


    public void newsBerry()
    {
        if (isNewBerryAble())
        {
            do { newBerryIndex2 = selectBerry(); }
            while (newBerryIndex2 == DataController.instance.gameData.newBerryIndex);

            //새딸기 개발이랑 뉴스 해금 동시에 했는데 같은 딸기 얻으려고 하면 문제생길수도 있다고 생각
            
            //새로운 딸기가 추가된다.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex2] = true;
            DataController.instance.gameData.unlockBerryCnt++;
            //느낌표 표시
            DataController.instance.gameData.isBerryEM[newBerryIndex2] = true;

            //딸기 얻음 효과음(짜잔)
            AudioManager.instance.TadaAudioPlay();

        }
        else { Debug.Log("더이상 개발가능한 딸기 없음."); }
    
    }
    
    #endregion

    #region Explanation
    /*
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //설명창 띄운다
                berryExp_BlackPanel.SetActive(true); //시원 건드림
                berryExp_Panel.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림

                //GameObject berryExpImage = berryExp.transform.GetChild(1).GetChild(1).gameObject; //시원 건드림
                //GameObject berryExpName = berryExp.transform.GetChild(1).GetChild(2).gameObject; //시원 건드림
                //GameObject berryExpTxt = berryExp.transform.GetChild(1).GetChild(3).gameObject; //시원 건드림


                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//이미지 설정

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//이름 설정

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryExplain;//설명 설정    
            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }
    }
    */
    #endregion

    public void NewsUnlock() 
    {
        News.instance.NewsUnlock(NewsPrefabNum);
    }

    #region 기타
    //활성화 비활성화로 창 끄고 켜고
    public void turnOff(GameObject Obj)
    {

        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
  
    }

    public string TimeForm(int time)//초단위 시간을 분:초로 변경
    {
        int M = 0, S = 0;//M,S 계산용
        string Minutes, Seconds;//M,S 텍스트 적용용

        M = (time / 60);
        S = (time % 60);


        //M,S적용
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S가 10미만이면 01, 02... 식으로 표시
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S가 0이면 00으로 표시한다.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #endregion

    #region 출석

    //인터넷 시간 가져오기.

    IEnumerator WebCheck() 
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date);
                DataController.instance.gameData.Today = dateTime;
            }
        }
    }

    //자정 체크 및 정보갱신

    public void CheckTime()
    {
        //플레이 도중 자정이 넘어갈 경우 출석 가능
        // 자정시간 구하기.
        DateTime target = new DateTime(DataController.instance.gameData.Today.Year, DataController.instance.gameData.Today.Month, DataController.instance.gameData.Today.Day);
        target = target.AddDays(1);
        // 자정시간 - 현재시간
        TimeSpan ts = target - DataController.instance.gameData.Today;
        // 남은시간 만큼 대기 후 OnTimePass 함수 호출.
        Invoke("OnTimePass", (float)ts.TotalSeconds);
    }

    public void OnTimePass()
    {
        //정보갱신
        DataController.instance.gameData.attendance = false;
        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
    }

    #endregion

    #region 메인 메뉴
    public void OnclickStart()
    {
    }

    public void OnclickOption()
    {

    }

    public void OnclickQuit()
    {

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion

}