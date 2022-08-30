using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    #region 인스펙터 및 변수 생성
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    public Text MedalText;
    public Text coinAnimText;
    public Text heartAnimText;

    [Header("[ Object ]")]
    private Globalvariable globalVar;
    public GameObject stemPrefab; //프리팹
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
    public GameObject workingCountText;//고용중인 동물수
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
    public Sprite[] AcheiveClassify_newBerry;

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
    private int revenue;

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public Text AbsenceMoneyText;
    public Text AbsenceTimeText;
    public GameObject noCoinPanel;
    public GameObject noHeartPanel;
    public GameObject blackPanel;
    public GameObject coinAnimManager;
    public GameObject heartAnimManager;
    public GameObject AbsencePanel;
    public GameObject AbsenceBlackPanel;



    [Header("[ Game Flag ]")]
    public bool isGameRunning;
    public bool isBlackPanelOn = false;
    private int coinUpdate;
    public bool isStart;
    public bool isMiniGameMode = false;
    #endregion

    #region 출석 메인 기능


    void Start()
    {
        StartCoroutine(PreWork());
        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        //PrintTime();


        Application.targetFrameRate = 60;
        instance = this; 

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //TimerStart += Instance_TimerStart;

        DisableObjColliderAll();

        isGameRunning = true;

        //NEW BERRY
        NewBerryUpdate();

        ShowCoinText(CoinText, DataController.instance.gameData.coin);
        HeartText.text = DataController.instance.gameData.heart.ToString();

        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

        isStart = true;

        InitDataInGM();
    }

    public void GameStart()
    {
        isGameRunning = true;
        Invoke("EnableObjColliderAll", 4.5f);
    }

    void InitDataInGM()
    {
        // 게임 시작시 딸기 가격 한번 업데이트 해주기
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.SPECIAL_FIRST + (i - 64) * 2) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.UNIQUE_FIRST + (i - 128) * 3) * (1 + researchCoeffi));
            /*if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((50) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((1000) * (1 + researchCoeffi));*/
        }

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
        switch (DataController.instance.gameData.newBerryBtnState)
        {
            case 0: BtnState = "start"; startBtn_newBerry.GetComponent<Image>().sprite = StartImg; break;
            case 1: BtnState = "ing"; startBtn_newBerry.GetComponent<Image>().sprite = IngImg; break;
            case 2: BtnState = "done"; startBtn_newBerry.GetComponent<Image>().sprite = DoneImg; break;
        }

        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 오브젝트 가져옴
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

        //폰에서 뒤로가기 버튼 눌렀을 때/에디터에서 ESC 버튼 눌렀을 때 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataController.instance.SaveData();
            //isStart = false;
            Application.Quit();
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        //ShowCoinText(CoinText, DataController.instance.gameData.coin); //트럭코인 나타낼 때 같이쓰려고 매개변수로 받게 수정했어요 - 신희규
        //HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (DataController.instance.gameData.isPrework == true)
                DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

            DataController.instance.SaveData();
        }
        else
        {
            if (isStart)//&&Intro.isEnd)
                StartCoroutine(CheckElapseTime());
        }

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
            DataController.instance.gameData.researchLevel[5] * Globalvariable.instance.getEffi());
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
        //stem.transform.position = obj.transform.position; ; // 밭의 Transform에 달기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더 비활성화 (잡초와 충돌 방지)
    }
    public void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;

        AudioManager.instance.HarvestAudioPlay();//딸기 수확할 때 효과음
        farm.isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.getInstantBerryObj().GetComponent<Berry>().Explosion(pos, target.position, 0.5f);
        //stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 3;

        StartCoroutine(HarvestRoutine(farm, stem)); // 연속으로 딸기가 심어지는 현상 방지

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
        farm.GetComponent<BoxCollider2D>().enabled = false; //밭 잠시 비활성화

        yield return new WaitForSeconds(0.75f); // 0.75뒤에

        UpdateTruckState(stem);

        DataController.instance.gameData.totalHarvBerryCnt++; // 수확한 딸기의 총 개수 업데이트           
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // 밭을 비워둔다

        //줄기에 페이드아웃 적용
        Animator anim = stemObjList[stem.stemIdx].GetComponent<Animator>();
        anim.SetInteger("Seed", 5);

        yield return new WaitForSeconds(0.3f); // 0.3초 뒤에

        stem.gameObject.SetActive(false);

        farm.isHarvest = false; // 수확 끝              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !isBlackPanelOn) //잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭 다시 활성화
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
        else                        //  그 외 C (최대 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + "C";
        }
    }

    public void GetCoin(int cost) // 코인 획득 함수
    {
        StartCoroutine(CountAnimation(cost, "+", 0));
        DataController.instance.gameData.coin += cost; // 현재 코인 +
        DataController.instance.gameData.accCoin += cost; // 누적 코인 +
        AudioManager.instance.CoinAudioPlay();
    }

    public void UseCoin(int cost) // 코인 사용 함수(마이너스 방지 위함)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
        {
            StartCoroutine(CountAnimation(cost, "-", 0));
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
            StartCoroutine(CountAnimation(cost, "-", 1));
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
            //메달이 모자를 떄 뜨는 경고
        }
    }
    public void ShowMedalText()
    {
        MedalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();
    }
    #endregion

    #region 콜라이더
    public void DisableObjColliderAll() // 모든 오브젝트의 콜라이더 비활성화
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
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4]) //(4)의 상황, 즉 벌레와 잡초 둘 다 없을 때 다 자란 딸기밭의 콜라이더를 켜준다.
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
    public void PTJEmployButtonClick(int prefabNum)
    {
        //효과음
        AudioManager.instance.Cute1AudioPlay();

        //고용중이 아닌 상태다
        if (DataController.instance.gameData.PTJNum[prefabNum] == 0)
        {
            if (DataController.instance.gameData.PTJCount < 3)
            {
                int cost = PTJ.instance.Info[prefabNum].Price * DataController.instance.gameData.PTJSelectNum[1];
                if (cost <= DataController.instance.gameData.coin)
                {
                    int ID = DataController.instance.gameData.PTJSelectNum[0];
                    //HIRE

                    //코인 사용
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
                //3명 이상 고용중이라는 패널 등장
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용 가능한 알바 수를\n넘어섰어요!";
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                warningBlackPanel.SetActive(true);
            }
        }
        //고용중인 상태이다
        else
        {
            //FIRE
            //확인창 띄우기
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

        //확인창 내리기
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();
        warningBlackPanel.SetActive(false);
    }
    #endregion

    #region New Berry Add
    public void NewBerryUpdate()
    {
        //새 딸기 개발======

        //PRICE
        price_newBerry = 90 + 10 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
        //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
        ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

        if (DataController.instance.gameData.newBerryBtnState == 1)//개발 가능한 딸기
        { StartCoroutine(Timer()); }
        else
        {
            if (isNewBerryAble() == true)//개발 가능한 딸기 있는지 검사
            {

                DataController.instance.gameData.newBerryIndex = selectBerry();//얻을딸기, 시간 정해진다
                timeText_newBerry.GetComponent<Text>().text = "??:??";//TIME (미공개)

                //베리 없음 지우기
                NoPanel_newBerry.SetActive(false);
            }
            else { NoPanel_newBerry.SetActive(true); }
        }
    }
    public void NewBerryUpdate2() //개발 가능한 딸기 범위 넓어질때 실행되는 거. 위에 합치자
    {

        if (isNewBerryAble() == true)//한번 더 확인
        {
            if (DataController.instance.gameData.newBerryBtnState == 0)//딸기 얻고 있는 상태 아니면
            {
                //얻을 딸기가 정해진다 -> 시간, 값도 정해진다
                //PRICE
                price_newBerry = 10 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);
                //TIME
                DataController.instance.gameData.newBerryIndex = selectBerry();
                timeText_newBerry.GetComponent<Text>().text = "??:??";//가격 시간 아직 미공개 "?"

            }
            //베리 없음 지우기
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true); }

    }

    public bool isNewBerryAble()
    {
        //지금 새 딸기 개발 할 수 있나
        switch (DataController.instance.gameData.newBerryResearchAble)
        {
            case 0://classic 개발가능
                if (BerryCount("classic", false) == BerryCount("classic", true))
                { return false; }
                break;
            case 1://classic, special 개발가능
                if (BerryCount("classic", false) + BerryCount("special", false) ==
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
                    if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; }
                    if (Global.GetComponent<Globalvariable>().classicBerryList[i] == true) { countIsExsist++; }
                }
                break;

            case "special":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().specialBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64 + 64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().uniqueBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().uniqueBerryList[i] == true) { countIsExsist++; } }
                for (int i = 128; i < 128 + 64; i++)
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
                //이번에 새딸기 개발에 필요한 가격과 시간
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();//가격
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

                if (DataController.instance.gameData.coin >= price_newBerry)
                {
                    timeText_newBerry.GetComponent<Text>().text
                        = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));//시간
                    //돈소비
                    UseCoin(price_newBerry);

                    //버튼상태 ing로
                    DataController.instance.gameData.newBerryBtnState = 1;

                    //타이머 시작
                    StartCoroutine(Timer());

                    //시간 감소여부 묻는 패널 띄움.
                    TimeReduceBlackPanel_newBerry.SetActive(true); //시원 건드림
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림
                    TimeReduceText_newBerry.GetComponent<Text>().text //시원 건드림
                        = "하트 3개로 시간을 3분 단축하시겠습니까??\n";
                }
                else//돈 부족
                { UseCoin(price_newBerry); }
                break;

            case "ing":
                if (DataController.instance.gameData.newBerryTime > 1)
                {
                    //시간 감소 여부 묻는 패널 띄움.
                    TimeReduceBlackPanel_newBerry.SetActive(true);
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale();
                    TimeReduceText_newBerry.GetComponent<Text>().text
                        = "하트 3개로 시간을 3분 단축하시겠습니까?\n";
                }
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
            if (DataController.instance.gameData.heart >= 3)
            {
                //시간을 10분 줄여준다.
                if (DataController.instance.gameData.newBerryTime < 3 * 60)
                { DataController.instance.gameData.newBerryTime = 0; }
                else
                { DataController.instance.gameData.newBerryTime -= 3 * 60; }

                timeText_newBerry.GetComponent<Text>().text
                    = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));
                //하트를 소비한다.
                UseHeart(3);
            }
            else
            { UseHeart(3); }
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
            if (DataController.instance.gameData.newBerryTime <= 0)
            { timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(0)); }
            else
            {
                timeText_newBerry.GetComponent<Text>().text
             = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));
            }


            //타이머 끝나면
            if (DataController.instance.gameData.newBerryTime < 0.1f)
            {
                DataController.instance.gameData.newBerryBtnState = 2;//Done상태로
                break;
            }
        }
        StopCoroutine(Timer());

    }


    private int selectBerry()
    {
        int newBerryIndex = 1;

        while (true)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0:
                    newBerryIndex = berryPercantage(64);
                    /*newBerryIndex = UnityEngine.Random.Range(1, 64);
                    DataController.instance.gameData.newBerryTime = 10 * 60;*/
                    break;
                case 1:
                    newBerryIndex = berryPercantage(128);
                    break;
                case 2:
                    newBerryIndex = berryPercantage(192);
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

        //새로운 딸기가 추가된다
        DataController.instance.gameData.isBerryUnlock[DataController.instance.gameData.newBerryIndex] = true;
        DataController.instance.gameData.unlockBerryCnt++;

        //느낌표 표시
        DataController.instance.gameData.isBerryEM[DataController.instance.gameData.newBerryIndex] = true;

        //딸기 얻음 효과(짜잔)
        AudioManager.instance.TadaAudioPlay();

        //얻은딸기 설명창
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().sprite //얻은 딸기 이미지
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;

        AcheivePanel_newBerry.transform.GetChild(2).GetComponent<Text>().text //얻은 딸기 이름
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<Berry>().berryName;
        //베리 분류 이미지
        if (DataController.instance.gameData.newBerryIndex < 64)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[0]; }
        else if (DataController.instance.gameData.newBerryIndex < 128)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[1]; }
        else
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[2]; }


        //검정창 띄우기
        BlackPanel_newBerry.SetActive(true);


        DataController.instance.gameData.newBerryBtnState = 0;

        NewBerryUpdate();

    }

    private int berryPercantage(int endIndex)
    {
        int randomNum = 0;
        int newBerryIndex = 0;

        //RANDOM NUM -> classic(45)=0~44  special(35)=45~79  unique(20)=80~101
        if (endIndex == 128) { randomNum = UnityEngine.Random.Range(0, 80); }//지금 클래식이랑 스페셜만 가능하다면
        else if (endIndex == 192) { randomNum = UnityEngine.Random.Range(0, 100 + 1); }//지금 전부 다 가능하면



        if (randomNum < 45)
        {
            newBerryIndex = UnityEngine.Random.Range(1, 64);
            DataController.instance.gameData.newBerryTime = 3 * 60;
        }//classic
        else if (randomNum < 80)
        {
            newBerryIndex = UnityEngine.Random.Range(64, 128);
            DataController.instance.gameData.newBerryTime = 6 * 60;
        }//special
        else if (randomNum <= 100)
        {
            newBerryIndex = UnityEngine.Random.Range(128, 192);
            DataController.instance.gameData.newBerryTime = 9 * 60;
        }//unique


        return newBerryIndex;
    }


    public bool newsBerry()
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

            //새딸기 얻음 팝업창
            GetNewBerry();

            return true;

        }
        else { return false; }

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

                //설명창 띄우기
                berryExp_BlackPanel.SetActive(true); //시원 건드림
                berryExp_Panel.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림

                //GameObject berryExpImage = berryExp.transform.GetChild(1).GetChild(1).gameObject; //시원 건드림
                //GameObject berryExpName = berryExp.transform.GetChild(1).GetChild(2).gameObject; //시원 건드림
                //GameObject berryExpTxt = berryExp.transform.GetChild(1).GetChild(3).gameObject; //시원 건드림


                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//이미지 설정

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//이름설정

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

    public string TimeForm(int time)//초단위 시간을 분:초로
    {
        int M = 0, S = 0;//M,S 계산용
        string Minutes, Seconds;//M,S 텍스트 적용용

        M = (time / 60);
        S = (time % 60);


        //M,S 적용
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S가 10미만이면 01, 02... 식으로 표시
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S가 0이면 00으로 표시
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #endregion

    #region 출석

    //인터넷 시간 가져오기.

    public static IEnumerator UpdateCurrentTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("https://naver.com"))
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
                    DataController.instance.gameData.currentTime = dateTime;
                }
            }
        }
    }

    public static IEnumerator TryGetCurrentTime()
    {
        while (DataController.instance.gameData.isPrework == false)
        {
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("https://naver.com"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    DataController.instance.gameData.isPrework = false;
                }
                else
                {
                    string date = request.GetResponseHeader("date");
                    DateTime dateTime = DateTime.Parse(date);
                    DataController.instance.gameData.currentTime = dateTime;
                    DataController.instance.gameData.isPrework = true;
                }
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    //자정 체크 및 정보갱신
    void ResetTime()
    {
        DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        //DataController.instance.gameData.currentTime.Date.AddDays(1); //다음날 자정 정보 저장.

        //���� Ÿ�̸�
        Invoke(nameof(ResetTime),
            (float)(DataController.instance.gameData.nextMidnightTime
            - DataController.instance.gameData.currentTime).TotalSeconds);
    }

    public void ResetInvoke()
    {
        DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
        Invoke(nameof(ResetTime),
        (float)(DataController.instance.gameData.nextMidnightTime
            - DataController.instance.gameData.currentTime).TotalSeconds);
    }


    IEnumerator CheckElapseTime() //게임 복귀할때
    {
        DataController.instance.gameData.isPrework = false;
        yield return StartCoroutine(TryGetCurrentTime());

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        if (gap.TotalSeconds > 3610f)
        {
            yield return StartCoroutine(CalculateTime());
        }

        MidNightCheck();

        //PrintTime();

        if (!MiniGameManager.isOpen && DataController.instance.gameData.rewardAbsenceTime.TotalMinutes >= 60)//&&Intro.isEnd)
        {
            //부재중 이벤트
            AbsenceTime();
        }

    }

    IEnumerator PreWork() //접속할 때
    {
        yield return StartCoroutine(TryGetCurrentTime()); //현재시간 불러오기 체크
        yield return StartCoroutine(CalculateTime());

        StartCoroutine(UpdateCurrentTime()); //30초 갱신

        MidNightCheck();

        if (DataController.instance.gameData.rewardAbsenceTime.TotalMinutes >= 60)//&&Intro.isEnd)
        {
            //부재중 이벤트
            AbsenceTime();
        }
    }


    public void MidNightCheck()
    {
        DateTime temp = new DateTime();
        if (temp != DataController.instance.gameData.nextMidnightTime && temp != DataController.instance.gameData.currentTime)
        {
            //예외처리
            TimeSpan test = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime.Date;
            if (test.Days >= 2)
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

            //자정시간을 지났다면
            TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.nextMidnightTime;
            if (gap.TotalSeconds >= 0)
            {
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
                attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
            }
            //자정시간을 지나지 않았다면
            gap = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime;
            if (gap.TotalSeconds >= 0)
                Invoke(nameof(ResetTime), (float)gap.TotalSeconds);
        }
    }

    public static bool CheckFirstGame()
    {
        if (!DataController.instance.gameData.isFirstGame)
        {
            DataController.instance.gameData.isFirstGame = true;

            DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
            DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            DataController.instance.gameData.atdLastday = DataController.instance.gameData.currentTime.Date.AddDays(-1);
            DataController.instance.gameData.accDays = 0;
            GameManager.instance.attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
            return true;
        }
        return false;
    }

    public static IEnumerator CalculateTime() //부재중 시간
    {
        if (CheckFirstGame() == true) yield break;

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

        if ((DataController.instance.gameData.rewardAbsenceTime + gap).TotalMinutes >= 1440) //부재중 수익 최대치 고정 24시간
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromMinutes(1440);
        else
            DataController.instance.gameData.rewardAbsenceTime += gap;

        //알바 남은 시간 갱신 자리
    }

    void PrintTime()
    {
        Debug.Log("현재 시간: " + DataController.instance.gameData.currentTime);
        Debug.Log("다음 자정시간: " + DataController.instance.gameData.nextMidnightTime);
        Debug.Log("다음 자정시간까지 남은시간: " + (DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime));
        Debug.Log("마지막 종료 시간: " + DataController.instance.gameData.lastExitTime);
        Debug.Log("부재중 시간: " + (DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime));
        Debug.Log("누적출석:" + DataController.instance.gameData.accDays);
        Debug.Log("마지막 출석:" + DataController.instance.gameData.atdLastday);
    }

    public void AbsenceTime()
    {
        int researchLevelAdd = 0;
        int minute = DataController.instance.gameData.rewardAbsenceTime.Minutes;
        int hour=0;
        if (minute > 59)
        {
            hour = minute / 60;
            minute &= minute;
        }

        AbsenceTimeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);

        for (int i = 0; i < 6; i++)
        {
            researchLevelAdd += DataController.instance.gameData.researchLevel[i];
        }
        revenue = (DataController.instance.gameData.rewardAbsenceTime.Minutes / 5) * researchLevelAdd / 6 * 2;

        //if (Intro.isEnd&&
        if (MiniGameManager.isOpen)
        {
            if (revenue == 0)
                return;

            if (revenue <= 9999)           // 0~9999까지 A
            {
                AbsenceMoneyText.text =  revenue + "A";
            }
            else if (revenue <= 9999999)   // 10000~9999999(=9999B)까지 B
            {
                revenue /= 1000;
                AbsenceMoneyText.text = revenue + "B";
            }
            else                        // 그 외 C (최대 2100C)
            {
                revenue /= 1000000;
                AbsenceMoneyText.text = revenue + "C";
            }
            AbsenceBlackPanel.SetActive(true);
            AbsencePanel.GetComponent<PanelAnimation>().OpenScale();
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
        }
    }

    public void AbsenceBtn()
    {
        GetCoin(revenue);
        AbsencePanel.GetComponent<PanelAnimation>().CloseScale();

    }

    /*    public void CheckTime()
        {
            //플레이 도중 자정이 넘어갈 경우 출석 가능하게
            // 자정시간 구하기.
            DateTime target = new DateTime(DataController.instance.gameData.currentTime.Year, 
                DataController.instance.gameData.currentTime.Month, DataController.instance.gameData.currentTime.Day);
            target = target.AddDays(1);
            // 자정시간 - 현재시간
            TimeSpan ts = target - DataController.instance.gameData.currentTime;
            // 남은시간 만큼 대기 후 OnTimePass 호출.
            Invoke("OnTimePass", (float)ts.TotalSeconds);
            Debug.Log("자정까지 남은 시간(분): " + ts.TotalMinutes);
        }*/

    /*    public void OnTimePass()
        {
            //정보갱신
            Debug.Log("출석 정보가 갱신되었습니다.");

            StartCoroutine(UpdateCurrentTime());
            CheckTime();
            if (DataController.instance.gameData.currentTime.Day != DataController.instance.gameData.atdLastday.Day)
                DataController.instance.gameData.isAttendance = false;

            attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        }*/


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