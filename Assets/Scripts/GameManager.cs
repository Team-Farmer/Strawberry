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
    [SerializeField] public int coin;
    [SerializeField] public int heart;
    public Text CoinText;
    public Text HeartText;
    //public int[] berryPrice = { 10, 20, 30, 40 };
    public int[,] BerryPrice = new int[3, 32];

    [Header("------------[ Object ]")]
    public GameObject berryPrefab; // 프리팹    
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    public Transform berryGroup;
    public List<StrawBerry> berryList;

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
    public GameObject berryExpPanel;
    internal object count;

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
        berryList = new List<StrawBerry>();
        truck = TruckObj.GetComponent<Truck>();
        target = TruckObj.GetComponent<Transform>();

        for (int i = 0; i < 16; i++) // 오브젝트 풀링으로 미리 딸기 생성
        {
            MakeStrawBerry();
        }

        // 임시 재화 설정
        coin = 10000;
        heart = 300;
        ShowCoinText(coin);
        //CoinText.text = coin.ToString() + " A";
        HeartText.text = heart.ToString();

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
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        ShowCoinText(coin);
        HeartText.text = heart.ToString();
    }
    #endregion

    #region 딸기밭
    void ClickedFarm(GameObject obj)
    {
        Farm farm = obj.GetComponent<Farm>();
        if (!farm.isPlant)
        {
            StrawBerry stb = GetStrawBerry(farm.farmIdx);
            if (stb != null)
            {
                PlantStrawBerry(stb, obj); // 심는다                            
                farm.GetComponent<Farm>().isPlant = true; // 체크 변수 갱신
            }
        }
        else
        {
            if (!berryList[farm.farmIdx].canGrow)
            {
                Harvest(berryList[farm.farmIdx]); // 수확
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
    void MakeStrawBerry() // 딸기 생성
    {
        GameObject instantStrawBerryObj = Instantiate(berryPrefab, berryGroup);
        instantStrawBerryObj.name = "Berry " + berryList.Count;

        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();
        instantStrawBerry.berryIdx = berryList.Count;

        instantStrawBerry.gameObject.SetActive(false);
        berryList.Add(instantStrawBerry);
    }
    StrawBerry GetStrawBerry(int idx)
    {
        if (farmList[idx].isPlant) return null;

        return berryList[idx];
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        stb.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stb.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더를 비활성화 (잡초와 충돌 방지)
    }
    void Harvest(StrawBerry berry)
    {
        Farm farm = farmList[berry.berryIdx];
        if (farm.isHarvest) return;

        farm.isHarvest = true;
        Vector2 pos;

        berry.GetComponent<SpriteRenderer>().sortingOrder = 4;
        berry.SetAnim(5); // 수확 이미지로 변경
        pos = berry.transform.position;
        berry.Explosion(pos, target.position, 0.5f); // DOTWeen 효과 구현

        StartCoroutine(HarvestRoutine(farm)); // 연속으로 딸기가 심어지는 현상을 방지
    }
    GameObject ClickObj() // 클릭당한 오브젝트를 반환
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // 밭을 잠시 비활성화

        yield return new WaitForSeconds(0.75f); // 0.75초 뒤에

        UpdateBerryCnt();

        yield return new WaitForSeconds(0.25f); // 0.25초 뒤에

        farm.isHarvest = false; // 수확이 끝남
        farm.isPlant = false; // 밭을 비워준다
        if(!farm.hasWeed) // 잡초가 없다면
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

    public void ShowCoinText(int coin)
    {
        int show = coin;
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
            berryList[i].canGrow = false;
            berryList[i].bug.GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // 모든 오브젝트의 collider 활성화
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {           
            if (!farmList[i].isPlant && !farmList[i].hasWeed)
            {
                coll = farmList[i].GetComponent<BoxCollider2D>();
                coll.enabled = true;
            }
            if(!berryList[i].hasBug && !farmList[i].hasWeed)
            {
                berryList[i].canGrow = true;
            }
            berryList[i].bug.GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true;
            farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region 리스트
    //리스트 활성화 비활성화===========================================================================================
    //중복.... 개선필요 공부

    public void Xbutton()
    {
        //X버튼 -> 부모 오브젝트 비활성화, 블랙 패널 비활성화
        //X.transform.parent.gameObject.SetActive(false);
    }

    public void ListButton()
    {
        //리스트 버튼 -> 자식 오브젝트 활성화, 블랙패널 활성화
        //L.transform.GetChild(0).gameObject.SetActive(true);

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
