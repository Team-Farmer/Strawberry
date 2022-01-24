using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Object ]")]
    public GameObject Strawberry;
    public GameObject Farm;
    public List<Farm> FarmList = new List<Farm>();

    [Header("------------[ Object Pooling ]")]
    
    public Transform StrawberryGroup;
    public List<StrawBerry> strawPool;
    
    int poolSize = 16;
    public int poolCursor;

    [Header("------------[ DOTWeen ]")]
    public Transform target;

    //[Header("------------[ Other ]")]
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        strawPool = new List<StrawBerry>();
        
        for (int i = 0; i < poolSize; i++) // 오브젝트 풀링으로 미리 딸기 생성
        {
            MakeStrawBerry();
        }
    }    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼으로
        {           
            StrawBerry stb = GetStrawBerry(); // 딸기를 풀링에서 가져온다
            if(stb != null)
            {
                GameObject farmObj = ClickObj(); // 클릭한 오브젝트를 가져온다
                if(farmObj != null && !farmObj.GetComponent<Farm>().isPlant) // 밭이 클릭 되었으며 밭에 딸기가 심어져 있지 않다면
                {
                    PlantStrawBerry(stb, farmObj); // 심는다
                    farmObj.GetComponent<Farm>().isPlant = true; // 체크 변수 갱신
                }                                  
            }                
        }
        if(Input.GetMouseButtonDown(1)) // 오른쪽 마우스 버튼으로
        {
            Harvest(); // 수확
        }
    }
    void MakeStrawBerry() // 딸기 생성
    {
        GameObject instantStrawBerryObj = Instantiate(Strawberry, StrawberryGroup);
        instantStrawBerryObj.name = "StrawBerry " + strawPool.Count;
        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();

        instantStrawBerry.gameObject.SetActive(false);
        strawPool.Add(instantStrawBerry);       
    }
    StrawBerry GetStrawBerry()
    {
        poolCursor = 0;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (!strawPool[poolCursor].gameObject.activeSelf)
            {
                return strawPool[poolCursor]; // 비활성화된 딸기 반환
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
        return null; // 딸기가 16개 가득 찼다면 null 반환
    }
    void PlantStrawBerry(StrawBerry stb, GameObject obj)
    {
        stb.gameObject.SetActive(true); // 딸기 활성화      
        stb.transform.position = obj.transform.position; // 밭의 Transform에 딸기를 심는다
    }
    void Harvest()
    {
        poolCursor = 0;
        Vector2 pos;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (strawPool[poolCursor].gameObject.activeSelf && !strawPool[poolCursor].canGrow) // 딸기가 활성화 된 상태 && 다 자란 딸기라면
            {
                StrawBerry stb = strawPool[poolCursor];

                stb.SetAnim(5); // 수확 이미지로 변경
                pos = stb.transform.position;
                stb.Explosion(pos, target.position, 0.5f); // DOTWeen 효과 구현
                FarmList[poolCursor].isPlant = false; // 밭을 비워준다
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
    }
    GameObject ClickObj() // 클릭당한 오브젝트를 반환
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if(hit.collider == null) return null;

        return hit.collider.gameObject;
    }
}
