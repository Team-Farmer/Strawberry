using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[System.Serializable]
public class ObjectArray //GameObject의 다차원 배열 만들기 위해 MonoBehaviour 외부에서 선언
{
    public GameObject[] Behind = new GameObject[3];
}

public class GameManager : MonoBehaviour
{
    #region 인스펙터
    public static GameManager instance;

    [Header("------------[ Money ]")]
    //[SerializeField] public int coin;
    //[SerializeField] public int heart;
    public Text CoinText;
    public Text HeartText;   
    public int[,] BerryPrice = new int[3, 32];

    [Header("------------[ Object ]")]
    public GameObject stemPrefab; // 프리팹
    public GameObject bugPrefab;
      
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();
   
    public List<GameObject> berryPrefabListAll = new List<GameObject>();   
    public List<GameObject> berryPrefabListUnlock = new List<GameObject>();
    public List<GameObject> berryPrefabListlock = new List<GameObject>();

    [Header("------------[Truck List]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;        
    Transform target;
    
    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;
    public GameObject PanelBlack;
    public GameObject panelBlack_Exp;
    internal object count;
    public GameObject[] working;
    //새로운딸기 관련========================================
    [Header("OBJECT")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;
    [Header("INFO")]
    public float[] time_newBerry;
    public int[] price_newBerry;
    [Header("SPRITE")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;

    private int index_newBerry = 0;//현재 인덱스
    private bool isStart_newBerry = false;//시작을 눌렀는가

    [Header("------------[Check/Settings Panel]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;

    [Header("------------[Check/Day List]")]
    public bool[] Today;
    public ObjectArray[] Front = new ObjectArray[7];

    #endregion

    #region 기본
    void Awake()
    {
        Attendance();

            Application.targetFrameRate = 60;
        instance = this; // 게임 매니저의 싱글턴 패턴화 >> GameManager.instance.~~ 로 호출                               
        target = TruckObj.GetComponent<Transform>();
        
        SetBerryPrice();
        InitDataInGM();
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
            if ((0 < creatTimeTemp && creatTimeTemp < 20) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 옵젝을 가져온다
            if (obj != null)
            {
                if (obj.GetComponent<Farm>() != null)
                {
                    ClickedFarm(obj);
                }
                else if (obj.GetComponent<Bug>() != null)
                {
                    ClickedBug(obj);
                }
                else if (obj.GetComponent<Weed>() != null)
                {
                    ClickedWeed(obj);
                }
            }
        }

        updateInfo(index_newBerry);
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        ShowCoinText();
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
                DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant = true; // 체크 변수 갱신
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
        DataController.instance.gameData.berryCnt = 0;
    }
    /*void MakeStemAndBug() // 줄기 생성
    {
        GameObject instantStemObj = Instantiate(stemPrefab, stemGroup);
        instantStemObj.name = "stem " + stemList.Count;

        Stem instantStem = instantStemObj.GetComponent<Stem>();
        instantStem.stemIdx = stemList.Count;

        instantStem.gameObject.SetActive(false);
        stemList.Add(instantStem);

        GameObject instantBugObj = Instantiate(bugPrefab, instantStemObj.transform);
        instantBugObj.name = "Bug " + bugList.Count;

        Bug instantBug = instantBugObj.GetComponent<Bug>();
        instantBug.bugIdx = bugList.Count;

        instantBug.gameObject.SetActive(false); // 냅둬
        bugList.Add(instantBug);
    }*/
    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더를 비활성화 (잡초와 충돌 방지)
    }
    void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest) return;

        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // 밭을 비워준다
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest = true;
        Vector2 pos = stem.transform.position; ;
        stem.instantBerry.Explosion(pos, target.position, 0.5f);
        stem.instantBerry.GetComponent<SpriteRenderer>().sortingOrder = 4;
                
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

        UpdateBerryCnt();
        stem.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f); // 0.25초 뒤에

        DataController.instance.gameData.berryFieldData[farm.farmIdx].isHarvest = false; // 수확이 끝남              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed) // 잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화 
        }     
    }
    void UpdateBerryCnt()
    {       
        if(DataController.instance.gameData.berryCnt < 48)
        {
            DataController.instance.gameData.berryCnt += 1;
        }
    }

    #endregion

    #region 재화
    void SetBerryPrice()
    {
        BerryPrice[0, 0] = 10; // 클래식의 0번 딸기

        for (int i = 0; i < 32; i++)
        {
            if (i != 0)
                BerryPrice[0, i] = BerryPrice[0, i - 1] + 5; // 클래식 딸기값 세팅 (1번부터)
            BerryPrice[1, i] = BerryPrice[0, i] * 2;
            BerryPrice[2, i] = BerryPrice[0, i] * 3;
        }

        Debug.Log("딸기가치 : " + BerryPrice[0, 0] + " " + BerryPrice[1, 0] + " " + BerryPrice[2, 0]);
    }

    public void ShowCoinText()
    {
        int show = DataController.instance.gameData.coin;
        if (show <= 9999)           // 0~9999까지 A
        {
            CoinText.text = show.ToString() + " A";
        }
        else if (show <= 9999999)   // 10000~9999999(=9999B)까지 B
        {
            show /= 1000;
            CoinText.text = show.ToString() + " B";
        }
        else                        // 그 외 C (최대 2100C)
        {
            show /= 1000000;
            CoinText.text = show.ToString() + " C";
        }
    }
    #endregion

    #region 콜라이더
    public void DisableObjColliderAll() // 모든 오브젝트의 collider 비활성화
    {
        BoxCollider2D coll;
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
        for (int i = 0; i < farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // 잡초가 없을 때만 밭의 Collider활성화
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            /*if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed) // (4)의 상황, 즉 벌레와 잡초 둘 다 없을 때
            {
                DataController.instance.gameData.berryFieldData[i].canGrow = true;
            }*/
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // 잡초의 Collider 활성화
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region 리스트
    //활성화 비활성화
    public void turnOff(GameObject Obj)
    {
        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
    }
    public void workingApply(List<Sprite> workingList) 
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (workingList[i] == null) { working[i].SetActive(false); }
                else {
                    working[i].SetActive(true);
                    working[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = workingList[i]; 
                }
            }
            catch{ Debug.Log("error test"); }
        }
    }
    public void updateInfo(int index)
    {

        try
        {
            if (isStart_newBerry == true)
            {
                if (GameManager.instance.time_newBerry[index] > 0) //시간이 0보다 크면 1초씩 감소
                {
                    GameManager.instance.time_newBerry[index] -= Time.deltaTime;
                    startBtn_newBerry.GetComponent<Image>().sprite = ingImg;
                }
                else
                { startBtn_newBerry.GetComponent<Image>().sprite = doneImg; }


            }
            //현재 price와 time text를 보인다.
            priceText_newBerry.GetComponent<Text>().text = price_newBerry[index].ToString();
            timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(GameManager.instance.time_newBerry[index])); //정수부분만 출력한다.

        }
        catch
        {            Debug.Log("다음 레벨 정보 없음");        }
    }
    //버튼을 누르면
    public void newBerryAdd()
    {
        //타이머가 0 이라면 
        if (GameManager.instance.time_newBerry[index_newBerry] < 0.9)
        {
            //새로운 딸기가 추가된다.
            Debug.Log("새로운 딸기!!");


            //금액이 빠져나간다.
            //GameManager.instance.coin -= price_newBerry[index_newBerry];
            DataController.instance.gameData.coin-= price_newBerry[index_newBerry];
            //GameManager.instance.ShowCoinText(GameManager.instance.coin);
            ShowCoinText();

            //업스레이드 레벨 상승 -> 그 다음 업그레이드 금액이 보인다.
            index_newBerry++;
            updateInfo(index_newBerry);

            //시작버튼으로 변경
            startBtn_newBerry.GetComponent<Image>().sprite = startImg;
            isStart_newBerry = false;
        }
        else
        {
            Debug.Log("새로운 딸기를 위해 조금 더 기다리세요");
            isStart_newBerry = true;
        }
    }
    public string TimeForm(int time)
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

    #region 출석
    public void Attendance()
    {
        DateTime today = DateTime.Now;
        DateTime lastday = DataController.instance.gameData.Lastday; //지난 날짜 받아오기
        bool isAttendance = DataController.instance.gameData.attendance;
        int days = DataController.instance.gameData.days;
        int weeks;

        // days = 6; 출석 기능 테스트 용. 누적 날짜 조정.

        if (isAttendance == false)
        {
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //주차별로 Week 텍스트 설정 추후 추가예정.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else if (days == 0)
            {
                days += 1;
                weeks = days;
            }
            else
            {
                weeks = days;
                //Week 1 로 텍스트 설정
            }

            if (DateTime.Compare(today, lastday) > 0) //오늘 날짜가 지난번 출석 날짜보다 미래면
            {
                //days에 맞는 버튼 활성화
                //
                switch (weeks)
                {
                    case 1:
                        selectDay1();
                        break;
                    case 2:
                        selectDay2();
                        break;
                    case 3:
                        selectDay3();
                        break;
                    case 4:
                        selectDay4();
                        break;
                    case 5:
                        selectDay5();
                        break;
                    case 6:
                        selectDay6();
                        break;
                    case 7:
                        selectDay7();
                        break;
                }
            }
            else // 과거면
            {
                //days 1으로 초기화 후 day1버튼 활성화, week 1로 변경.
                DataController.instance.gameData.days = 0;
                selectDay1();
                //week 1 텍스트 변경.
            }
        }
    }

    public void selectDay1()
    {
        Front[0].Behind[2].SetActive(true);
    }
    public void selectDay2()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[2].SetActive(true);
    }
    public void selectDay3()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[2].SetActive(true);
    }
    public void selectDay4()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[2].SetActive(true);
    }
    public void selectDay5()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[2].SetActive(true);
    }
    public void selectDay6()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[1].SetActive(true);
        Front[5].Behind[2].SetActive(true);
    }
    public void selectDay7()
    {
        Front[0].Behind[1].SetActive(true);
        Front[1].Behind[1].SetActive(true);
        Front[2].Behind[1].SetActive(true);
        Front[3].Behind[1].SetActive(true);
        Front[4].Behind[1].SetActive(true);
        Front[5].Behind[1].SetActive(true);
        Front[6].Behind[2].SetActive(true);
    }


    public void clickDay1()
    {
        Front[0].Behind[1].SetActive(true);
        Front[0].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay2()
    {
        Front[1].Behind[1].SetActive(true);
        Front[1].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay3()
    {
        Front[2].Behind[1].SetActive(true);
        Front[2].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay4()
    {
        Front[3].Behind[1].SetActive(true);
        Front[3].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay5()
    {
        Front[4].Behind[1].SetActive(true);
        Front[4].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay6()
    {
        Front[5].Behind[1].SetActive(true);
        Front[5].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }
    public void clickDay7()
    {
        Front[6].Behind[1].SetActive(true);
        Front[6].Behind[2].SetActive(false);
        DataController.instance.gameData.days += 1;
        DataController.instance.gameData.attendance = true;
        DataController.instance.gameData.Lastday = DateTime.Now;
    }


    public void CheckTime()
    {
        //플레이 도중 자정이 넘어갈 경우 출석 가능
        // 자정시간 구하기.
        DateTime target = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        target = target.AddDays(1);
        // 자정시간 - 현재시간
        TimeSpan ts = target - DateTime.Now;
        // 남은시간 만큼 대기 후 OnTimePass 함수 호출.
        Invoke("OnTimePass", (float)ts.TotalSeconds);
    }

    public void OnTimePass()
    {
        //정보갱신
        DataController.instance.gameData.attendance = false;
        Attendance();
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
