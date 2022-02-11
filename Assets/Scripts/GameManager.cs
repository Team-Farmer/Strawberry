using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Money ]")]
    [SerializeField] public int coin;
    [SerializeField] int heart;
    public Text CoinText;
    public Text HeartText;

    [Header("------------[ Object ]")]
    public GameObject berryPrefab; // 프리팹
    public Truck truck;
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]

    public Transform berryGroup;
    public List<StrawBerry> berryList;

    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]   

    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;
    public GameObject PanelBlack;
    internal object count;

    [Header("------------[Check/Settings Panel]")]
    public GameObject Setting;
    public GameObject Check;

    [Header("------------[CheckList]")]
    public bool Day1, Day2, Day3, Day4, Day5, Day6, Day7;
    public GameObject Day1_1, Day1_2, Day1_3, Day2_1, Day2_2, Day2_3, Day3_1, Day3_2, Day3_3, Day4_1, Day4_2, Day4_3
        , Day5_1, Day5_2, Day5_3, Day6_1, Day6_2, Day6_3, Day7_1, Day7_2, Day7_3;



    void Awake()
    {

        Application.targetFrameRate = 60;
        berryList = new List<StrawBerry>();


        for (int i = 0; i < 16; i++) // 오브젝트 풀링으로 미리 딸기 생성
        {
            MakeStrawBerry();
        }

        // 임시 재화 설정
        coin = 100000;
        heart = 300;
        CoinText.text = coin.ToString() + " A";
        HeartText.text = heart.ToString();
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
                else if (obj.GetComponent<Truck>() != null)
                {
                    ClickedTruck();
                }
            }
        }
    }
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
        stb.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stb.gameObject.SetActive(true); // 딸기 활성화              
    }
    void Harvest(StrawBerry berry)
    {
        Farm farm = farmList[berry.berryIdx];
        if (farm.isHarvest) return;

        farm.isHarvest = true;
        Vector2 pos;

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

        yield return new WaitForSeconds(0.65f); // 0.65초 뒤에

        UpdateBerryCnt(berryList[farm.farmIdx]);

        yield return new WaitForSeconds(0.25f); // 0.25초 뒤에

        farm.isHarvest = false; // 수확이 끝남
        farm.isPlant = false; // 밭을 비워준다
        farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화      
    }
    void UpdateBerryCnt(StrawBerry berry)
    {
        truck.berryCnt += berry.route + 1;
    }
    public void DisableObjColliderAll() // 모든 오브젝트의 collider 비활성화
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            berryList[i].canGrow = false;
        }
    }
    public void EnableObjColliderAll() // 모든 오브젝트의 collider 활성화
    {
        BoxCollider2D coll;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = true;
            berryList[i].canGrow = true;
        }
    }

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

    public void selectPTJList()
    {
        if (PartTimeList.activeSelf == false)
        {
            PartTimeList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            PartTimeList.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }
    public void selectSearchList()
    {
        if (ResearchList.activeSelf == false)
        {
            ResearchList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            ResearchList.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }
    public void selectBerryList()
    {
        if (BerryList.activeSelf == false)
        {
            BerryList.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            BerryList.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }
    public void selectSettingPanel()
    {
        if (Setting.activeSelf == false)
        {
            Setting.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            Setting.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }
    public void selectCheckPanel()
    {
        if (Check.activeSelf == false)
        {
            Check.SetActive(true);
            PanelBlack.SetActive(true);
        }
        else
        {
            Check.SetActive(false);
            PanelBlack.SetActive(false);
        }

    }


    public void selectPanelBlack() // 검은창 클릭시 UI 종료
    {
        if (Setting.activeSelf == true)
        {
            Setting.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (Check.activeSelf == true)
        {
            Check.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (PartTimeList.activeSelf == true)
        {
            PartTimeList.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (ResearchList.activeSelf == true)
        {
            ResearchList.SetActive(false);
            PanelBlack.SetActive(false);
        }
        else if (BerryList.activeSelf == true)
        {
            BerryList.SetActive(false);
            PanelBlack.SetActive(false);
        }
    }

    public void selectDay1()
    {
        if (Day1 == false)
        {
            Day1_2.SetActive(true);
            Day1_3.SetActive(false);
            Day1 = true;
            Day2_3.SetActive(true);
        }
    }

    public void selectDay2()
    {
        if (Day1 == true)
        {
            Day2_2.SetActive(true);
            Day2_3.SetActive(false);
            Day2 = true;
            Day3_3.SetActive(true);
        }
    }

    public void selectDay3()
    {
        if (Day2 == true)
        {
            Day3_2.SetActive(true);
            Day3_3.SetActive(false);
            Day3 = true;
            Day4_3.SetActive(true);

        }
    }

    public void selectDay4()
    {
        if (Day3 == true)
        {
            Day4_2.SetActive(true);
            Day4_3.SetActive(false);
            Day4 = true;
            Day5_3.SetActive(true);

        }
    }

    public void selectDay5()
    {
        if (Day4 == true)
        {
            Day5_2.SetActive(true);
            Day5_3.SetActive(false);
            Day5 = true;
            Day6_3.SetActive(true);

        }
    }

    public void selectDay6()
    {
        if (Day5 == true)
        {
            Day6_2.SetActive(true);
            Day6_3.SetActive(false);
            Day6 = true;
            Day7_3.SetActive(true);

        }
    }

    public void selectDay7()
    {
        if (Day6 == true)
        {
            Day7_2.SetActive(true);
            Day7_3.SetActive(false);
            Day7 = true;
        }
    }

}
