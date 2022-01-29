using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Object ]")]
    public GameObject Strawberry; // 프리팹
    public GameObject Farm; // 프리팹
    
    public List<Farm> farmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    
    public Transform berryGroup;
    public List<StrawBerry> strawList;
  
    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]

    [Header("------------[PartTime/Search/Berry List]")]
    public GameObject PartTimeList;
    public GameObject ResearchList;
    public GameObject BerryList;

    
    void Awake()
    {
        Application.targetFrameRate = 60;
        strawList = new List<StrawBerry>();
        
        for (int i = 0; i < 16; i++) // 오브젝트 풀링으로 미리 딸기 생성
        {
            MakeStrawBerry();            
        }
    }    
    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 옵젝을 가져온다
            if(obj != null)
            {
                if(obj.GetComponent<Farm>() != null)
                {
                    Farm farm = obj.GetComponent<Farm>();
                    if (!farm.isPlant)
                    {
                        StrawBerry stb = GetStrawBerry();
                        if (stb != null)
                        {
                            PlantStrawBerry(stb, obj); // 심는다
                            stb.farmIdx = farm.farmIdx; // 딸기와 밭을 연결
                            farm.berryIdx = stb.berryIdx; // 밭과 딸기를 연결

                            farm.GetComponent<Farm>().isPlant = true; // 체크 변수 갱신
                        }
                    }
                    else
                    {
                        if(!strawList[farm.berryIdx].canGrow)
                        {
                            Harvest(strawList[farm.berryIdx]); // 수확
                        }
                    }
                }                
            }
        }       
    }
    void MakeStrawBerry() // 딸기 생성
    {
        GameObject instantStrawBerryObj = Instantiate(Strawberry, berryGroup);
        instantStrawBerryObj.name = "StrawBerry " + strawList.Count;

        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();
        instantStrawBerry.berryIdx = strawList.Count;

        instantStrawBerry.gameObject.SetActive(false);
        strawList.Add(instantStrawBerry);       
    }    
    StrawBerry GetStrawBerry()
    {       
        for (int i = 0; i < strawList.Count; i++)
        {
            if (!strawList[i].gameObject.activeSelf)
            {
                return strawList[i]; // 비활성화된 딸기 반환
            }            
        }
        return null; // 딸기가 16개 가득 찼다면 null 반환
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        stb.gameObject.SetActive(true); // 딸기 활성화      
        stb.transform.position = obj.transform.position; // 밭의 Transform에 딸기를 심는다
    }
    void Harvest(StrawBerry berry)
    {       
        Vector2 pos;
        Farm farm = farmList[berry.farmIdx];

        berry.SetAnim(5); // 수확 이미지로 변경
        pos = berry.transform.position;
        berry.Explosion(pos, target.position, 0.5f); // DOTWeen 효과 구현
        farm.isPlant = false; // 밭을 비워준다

        StartCoroutine(HarvestRoutine(farm)); // 연속으로 딸기가 심어지는 현상을 방지
    }
    GameObject ClickObj() // 클릭당한 오브젝트를 반환
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if(hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // 밭을 잠시 비활성화

        yield return new WaitForSeconds(0.5f); // 0.5초 뒤에

        farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화
    }


    //리스트 활성화 비활성화===========================================================================================
    //중복.... 개선필요 공부
    public void selectPTJList()
    {
        if (PartTimeList.activeSelf == false)
            PartTimeList.SetActive(true);
        else
            PartTimeList.SetActive(false);

    }
    public void selectSearchList()
    {
        if (ResearchList.activeSelf == false)
            ResearchList.SetActive(true);
        else
            ResearchList.SetActive(false);
    }

    public void selectBerryList()
    {
        if (BerryList.activeSelf == false)
            BerryList.SetActive(true);
        else
            BerryList.SetActive(false);
    }
}
