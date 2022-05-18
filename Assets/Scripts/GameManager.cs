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


    [Header("[ PartTime/Search/Berry List ]")]
    //PTJ 알바
    public GameObject workingCountText;//고용 중인 동물 수
    public GameObject[] working;//고용 중인 동물 리스트 상단에
    [NonSerialized]
    public List<int> workingID = new List<int>();//지금 일하고 있는 알바생 Id
    public GameObject PTJList;

    public GameObject berryExp;//베리 설명창

    //새로운딸기================================
    [Header("[ OBJECT ]")]
    [Header("[ ==============NEW BERRY============== ]")]

    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;

    public GameObject TimeReucePanel_newBerry;
    public GameObject AcheivePanel_newBerry;

    public GameObject NoPanel_newBerry;
    public GameObject BlackPanel_newBerry;

    [Header("[ SPRITE ]")]
    public Sprite StartImg;
    public Sprite DoneImg;
    public Sprite IngImg;

    private float time_newBerry;
    private int price_newBerry;

    private string BtnState;//지금 버튼 상태
    private int newBerryIndex;//이번에 개발되는 베리 넘버
    //===========================================

    [Header("[ Check/Settings Panel ]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;
    public bool isblackPanelOn = false;

    [Header("[ Check/Day List ]")]
    public GameObject AttendanceCheck;
    public string url = "";

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public GameObject NoCoinPanel;
    public GameObject NoHeartPanel;
    public GameObject BP;



    #endregion

    #region 기본
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this; // 게임 매니저의 싱글턴 패턴화 >> GameManager.instance.~~ 로 호출

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //출석 관련 호출.
        StartCoroutine(WebCheck());
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        CheckTime();


        InitDataInGM();

        //TimerStart += Instance_TimerStart;
    }



    private void Start()
    {
        PTJList.SetActive(true);

        //NEW BERRY
        NewBerryUpdate();
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
        workinCountApply();

        //NEW BERRY 개발
        //이부분 시간들이면 없앨 수있음. 일단 지금은 시간없으니까 이렇게 하고 나중에 없애는게 나으면 말해주세요
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
        ShowCoinText(CoinText, DataController.instance.gameData.coin); // 트럭코인 나타낼 때 같이쓰려고 매개변수로 받게 수정했어요 - 신희규
        HeartText.text = DataController.instance.gameData.heart.ToString();
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
        stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 4;

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
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !BP.activeSelf) // 잡초가 없다면
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
        DataController.instance.gameData.coin += cost; // 현재 코인 +
        DataController.instance.gameData.accCoin += cost; // 누적 코인 +
    }

    public void UseCoin(int cost) // 코인 사용 함수 (마이너스 방지 위함)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
            DataController.instance.gameData.coin -= cost;
        else
        {
            //경고 패널 등장
            GameManager.instance.DisableObjColliderAll();
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            BP.SetActive(true);
            NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetHeart(int cost) // 하트 획득 함수
    {
        DataController.instance.gameData.heart += cost; // 현재 하트 +
        DataController.instance.gameData.accHeart += cost; // 누적 하트 +
    }

    public void UseHeart(int cost) // 하트 획득 함수 (마이너스 방지 위함)
    {
        int myHeart = DataController.instance.gameData.heart;
        if (myHeart >= cost)
            DataController.instance.gameData.heart -= cost;
        else
        {
            //경고 패널 등장
            GameManager.instance.DisableObjColliderAll();
            panelHearText.text = DataController.instance.gameData.heart.ToString() + "개";
            BP.SetActive(true);
            NoHeartPanel.GetComponent<PanelAnimation>().OpenScale();
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
        isblackPanelOn = true;
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
        isblackPanelOn = false;
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
    public void workingApply(List<Sprite> workingList, int ID)
    {
        for (int i = 0; i < workingList.Count; i++)
        {
            try
            {
                if (workingList[i] == null) { working[i].SetActive(false); } //알바중인 사람 없으면 비활성화
                else //알바중 있으면
                {
                    working[i].SetActive(true); //활성화
                    working[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = workingList[i];//얼굴 이미지 넣기

                }
            }
            catch { }
        }
    }
    public void workinCountApply()
    {
        //지금 알바중인 알바생들의 남은 고용횟수
        //엉망진창코드
        for (int i = 0; i < 3; i++)
        {
            if (working[i].activeSelf == true)
            {
                working[i].transform.GetChild(1).transform.GetComponent<Text>().text
                    = DataController.instance.gameData.PTJNum[workingID[i]].ToString();
                if (DataController.instance.gameData.PTJNum[workingID[i]] == 0)
                { working[i].SetActive(false); }
            }
        }
    }

    //PTJ몇명 고용했는지
    public void workingCount(int employCount)
    { workingCountText.GetComponent<Text>().text = employCount.ToString(); }

    #endregion

    #region New Berry Add
    public void NewBerryUpdate()
    {
        //현재 새 딸기 개발 레벨
        
        if (isNewBerryAble() == true)
        {
            //얻을딸기가 정해진다.->시간,값도 정해진다.
            price_newBerry = 100 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
            selectBerry();

            //이번 새딸기 개발에 필요한 가격과 시간
            priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));

            //베리 없음 지우기
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true);}
    }

    private bool isNewBerryAble()
    {

        //지금 새딸기를 개발 할 수 있나?
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
                for (int i = 0; i < Globalvariable.instance.classicBerryList.Count; i++)
                {
                    if (DataController.instance.gameData.isBerryUnlock[i]==true) { countIsUnlock++; }
                    if (Globalvariable.instance.classicBerryList[i] == true) { countIsExsist++; } 
                }
                break;

            case "special":
                for (int i = 0; i < Globalvariable.instance.specialBerryList.Count; i++)
                { if (Globalvariable.instance.specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64+64; i++)
                {if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Globalvariable.instance.uniqueBerryList.Count; i++)
                { if (Globalvariable.instance.uniqueBerryList[i] == true) { countIsExsist++; } }
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
                //시간 감소여부 묻는 패널을 띄운다.
                TimeReucePanel_newBerry.SetActive(true);
                //패널안의 정보를 채운다.
                TimeReucePanel_newBerry.transform.GetChild(2).GetComponent<Text>().text
                    = "하트 10개로 시간을 줄이시겠습니까?\n" + TimeForm((int)time_newBerry) + "→" + "00:01";//임시
                break;
            case "ing": /*진행중에는 누를 수 없다.*/ break;
            case "done": /*딸기 개발*/
                GetNewBerry();
                break;
                //default:Debug.Log("NowButton이름이 잘못됬습니다."); break;
        }
        
    }

    //TimeReucePanel_newBerry
    //하트 써서 시간을 줄일건가요? 패널
    public void TimeReduce(bool isTimeReduce)
    {
        //하트 써서 시간을 줄일거면
        if (isTimeReduce == true)
        {
            //시간을 줄여준다.
            time_newBerry = 1;
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));
            //하트를 소비한다.
            UseHeart(10);
        }

        //버튼상태는 ing로
        DataController.instance.gameData.newBerryBtnState = 1;
        //NewBerryUpdate();

        //창 끄기
        TimeReucePanel_newBerry.SetActive(false);

        //돈소비
        UseCoin(price_newBerry);

        

        //타이머 시작
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        //1초씩 감소
        yield return new WaitForSeconds(1f);
        time_newBerry--;
        //감소하는 시간 보이기
        timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time_newBerry));
        
        //타이머 끝나면
        if (time_newBerry < 0.1f) 
        {
            DataController.instance.gameData.newBerryBtnState =2;//Done상태로
            NewBerryButton();//==GetNewBerry()
            StopCoroutine("Timer");  
        }
        else { StartCoroutine("Timer"); }
        
    }


    private void selectBerry() 
    {
        newBerryIndex = 0;
        while (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == true
            || Globalvariable.instance.berryListAll[newBerryIndex] == null)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0: newBerryIndex = UnityEngine.Random.Range(1, 64);
                    time_newBerry = 10;
                    break;
                case 1: newBerryIndex = berryPercantage(128);
                    break;
                case 2: newBerryIndex = berryPercantage(192); 
                    break;
            }
            //아직 unlock되지 않은 베리 중에서 존재하는 베리를 고르기
        }
    }


    private void GetNewBerry()
    {

        //새로운 딸기가 추가된다.
        DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;
        //느낌표 표시
        DataController.instance.gameData.isBerryEM[newBerryIndex] = true;

        //딸기 얻음 효과음(짜잔)
        AudioManager.instance.TadaAudioPlay();

        //얻은 딸기 설명창
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().sprite
            = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Text>().text
            = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<Berry>().berryName;

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


        //if (berryCountComparision() == 3)
        //{
        if (randomNum < 45) { newBerryIndex = UnityEngine.Random.Range(0, 64); time_newBerry = 10; }//classic
        else if (randomNum < 80) { newBerryIndex = UnityEngine.Random.Range(64, 128); time_newBerry = 20; }//special
        else if (randomNum <= 100) { newBerryIndex = UnityEngine.Random.Range(128, 192); time_newBerry = 30; }//unique
        //}

        return newBerryIndex;
    }
    private int berryCountComparision() //<-문제.
    {

        int classicCnt = BerryCount("classic", true);
        int specialCnt = BerryCount("special", true);
        int uniqueCnt = BerryCount("unique", true);

        if (Mathf.Abs(classicCnt - specialCnt) > 10 ||
            Mathf.Abs(classicCnt - uniqueCnt) > 10 ||
            Math.Abs(specialCnt - uniqueCnt) > 10)
        { return Mathf.Min(classicCnt, specialCnt, uniqueCnt); }
        else 
        { return 3; }// 적절하게 베리들이 있으므로 완전랜덤으로
    }


    public void newsBerry()
    {
        if (isNewBerryAble())
        {
            selectBerry();//새딸기 개발이랑 뉴스 해금 동시에 했는데 같은 딸기 얻으려고 하면 문제생길수도 있음
            
            //새로운 딸기가 추가된다.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;
            //느낌표 표시
            DataController.instance.gameData.isBerryEM[newBerryIndex] = true;

            //딸기 얻음 효과음(짜잔)
            AudioManager.instance.TadaAudioPlay();

            //얻은 딸기 설명창
            /*
            AcheivePanel_newBerry.SetActive(true);
            AcheivePanel_newBerry.transform.GetChild(0).GetComponent<Image>().sprite
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
            AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Text>().text
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<Berry>().berryName;
            */
            //검정창 띄우기
            //BlackPanel_newBerry.SetActive(true);

        }
        else { Debug.Log("더이상 개발가능한 딸기 없음. 꽝"); }
    
    }
    
    #endregion

    #region Explanation
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //설명창 띄운다
                berryExp.SetActive(true);

                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;

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
    
    #endregion

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
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
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