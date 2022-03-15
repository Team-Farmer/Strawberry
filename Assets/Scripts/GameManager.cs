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

    public Transform stemGroup;
    public Transform berryGroup;

    public List<Farm> farmList = new List<Farm>(); // 일단 보류
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();
   
    public List<GameObject> berryPrefabListAll = new List<GameObject>();   
    public List<GameObject> berryPrefabListUnlock = new List<GameObject>();
    public List<GameObject> berryPrefabListlock = new List<GameObject>();

    [Header("------------[Truck List]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;    
    Truck truck;
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
        Application.targetFrameRate = 60;
        instance = this; // 게임 매니저의 싱글턴 패턴화 >> 타 스크립트에서 GameManager의 컴포넌트 쓰고 싶으시면
                         // 굳이 스크립트 마다 게임매니저 할당 안해도 GameManager.instance.~~ 로 호출하시면 돼요!!        
               
        truck = TruckObj.GetComponent<Truck>();
        target = TruckObj.GetComponent<Transform>();

        if(stemList.Count != 16)
        {
            for (int i = 0; i < 16; i++) // 오브젝트 풀링으로 미리 딸기 생성
            {
                MakeStemAndBug();
            }
        }

        // 임시 재화 설정
        //coin = 10000;
        //heart = 300;
        //ShowCoinText(coin);
        //CoinText.text = coin.ToString() + " A";
        //HeartText.text = heart.ToString();

        SetBerryPrice();
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
        
        if (!farm.isPlant)
        {
            Stem st = GetStem(farm.farmIdx);
            if (st != null)
            {
                PlantStrawBerry(st, obj); // 심는다                            
                farm.GetComponent<Farm>().isPlant = true; // 체크 변수 갱신
            }
        }
        else
        {
            if (!stemList[farm.farmIdx].canGrow)
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
        truck.berryCnt = 0;
    }
    void MakeStemAndBug() // 줄기 생성
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
    }
    Stem GetStem(int idx)
    {
        if (farmList[idx].isPlant) return null;

        return stemList[idx];
    }
    void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        stem.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더를 비활성화 (잡초와 충돌 방지)
    }
    void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;

        farm.isPlant = false; // 밭을 비워준다
        farm.isHarvest = true;
        Vector2 pos = stem.transform.position; ;
        stem.instantBerry.Explosion(pos, target.position, 0.5f);
        stem.instantBerry.GetComponent<SpriteRenderer>().sortingOrder = 4;
        
        //stem.SetAnim(5); // 수확 이미지로 변경
        //pos = stem.transform.position;
        //stem.Explosion(pos, target.position, 0.5f); // DOTWeen 효과 구현

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

        farm.isHarvest = false; // 수확이 끝남              
        if (!farm.hasWeed) // 잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화 
        }     
    }
    void UpdateBerryCnt()
    {       
        if(truck.berryCnt < (int)Truck.Count.Max)
        {
            truck.berryCnt += 1;
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
            stemList[i].canGrow = false;
            bugList[i].GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            // Weed의 Collider 제거
            farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // 모든 오브젝트의 collider 활성화
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            if (!farmList[i].isPlant && !farmList[i].hasWeed) // 잡초가 없을 때만 밭의 Collider활성화
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            if (!stemList[i].hasBug && !farmList[i].hasWeed) // (4)의 상황, 즉 벌레와 잡초 둘 다 없을 때
            {
                stemList[i].canGrow = true;
            }
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // 잡초의 Collider 활성화
            farmList[i].canGrowWeed = true;
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

    public void selectDay1()
    {
        if (Today[0] == false)
        {
            Front[0].Behind[1].SetActive(true);
            Front[0].Behind[2].SetActive(false);
            Front[1].Behind[2].SetActive(true);
            Today[0] = true;
            Today[1] = true;
        }
        else
        {
            Front[0].Behind[1].SetActive(false);
            Front[0].Behind[2].SetActive(true);
            Today[0] = false;
        }
    }
    public void selectDay2()
    {
        if (Today[1] == true)
        {
            Front[1].Behind[1].SetActive(true);
            Front[1].Behind[2].SetActive(false);
            Front[2].Behind[2].SetActive(true);
            Today[2] = true;
        }
        else
        {
            Front[1].Behind[1].SetActive(false);
            Front[1].Behind[2].SetActive(true);
        }
    }
    public void selectDay3()
    {
        if (Today[2] == true)
        {
            Front[2].Behind[1].SetActive(true);
            Front[2].Behind[2].SetActive(false);
            Front[3].Behind[2].SetActive(true);
            Today[3] = true;
        }
        else
        {
            Front[2].Behind[1].SetActive(false);
            Front[2].Behind[2].SetActive(true);
        }
    }
    public void selectDay4()
    {
        if (Today[3] == true)
        {
            Front[3].Behind[1].SetActive(true);
            Front[3].Behind[2].SetActive(false);
            Front[4].Behind[2].SetActive(true);
            Today[4] = true;
        }
        else
        {
            Front[3].Behind[1].SetActive(false);
            Front[3].Behind[2].SetActive(true);
        }
    }
    public void selectDay5()
    {
        if (Today[4] == true)
        {
            Front[4].Behind[1].SetActive(true);
            Front[4].Behind[2].SetActive(false);
            Front[5].Behind[2].SetActive(true);
            Today[5] = true;
        }
        else
        {
            Front[4].Behind[1].SetActive(false);
            Front[4].Behind[2].SetActive(true);
        }
    }
    public void selectDay6()
    {
        if (Today[5] == true)
        {
            Front[5].Behind[1].SetActive(true);
            Front[5].Behind[2].SetActive(false);
            Front[6].Behind[2].SetActive(true);
            Today[6] = true;
        }
        else
        {
            Front[5].Behind[1].SetActive(false);
            Front[5].Behind[2].SetActive(true);
        }
    }
    public void selectDay7()
    {
        if (Today[6] == true)
        {
            Front[6].Behind[1].SetActive(true);
            Front[6].Behind[2].SetActive(false);
        }
        else
        {
            Front[6].Behind[1].SetActive(false);
            Front[6].Behind[2].SetActive(true);
        }
    }

    public void ResetDays()
    {
        //reset기능 추가 예정


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
