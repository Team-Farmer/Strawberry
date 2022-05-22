using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{
    public int weedIdx;  
    public float xPos = 0f;   
    public int weedSpriteNum; // 옮김
  
    private Farm farm;
    private Animator anim;
    private BoxCollider2D farmColl;
    void Awake()
    {        
        anim = GetComponent<Animator>();
        farm = transform.parent.gameObject.GetComponent<Farm>();        
        farmColl = farm.GetComponent<BoxCollider2D>();
    }
    void OnEnable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = true;
        DataController.instance.gameData.berryFieldData[weedIdx].weedSpriteNum = Random.Range(0, 3);
        anim.SetInteger("Generate", weedSpriteNum);
    }
    void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = false;
    }
    
    public void GenerateWeed() // 잡초 생성
    {
        float prob = Random.Range(0, 100);
        
        //scale = Random.Range(1.3f, 1.8f);
        if (prob < DataController.instance.gameData.weedProb)
        {
            this.gameObject.SetActive(true); // 나 자신(잡초)를 활성화

            farmColl.enabled = false; // 밭의 콜라이더 비활성화
            DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = true; // 잡초보유여부를 확인하는 변수
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = false; // 딸기의 성장 제어

            xPos = Random.Range(-0.35f, 0.35f); // 밭의 X축의 랜덤한 위치에 잡초 생성
            transform.position = new Vector2(farm.transform.position.x + xPos, farm.transform.position.y + 0.07f);            
        }
    }
    public void DeleteWeed()
    {
        anim.SetTrigger("Delete");

        if (this.gameObject.activeSelf)
        {
            StartCoroutine(DisableWeed(0.25f)); // 애니메이션이 끝난 후 비활성화
        }      
    }
    IEnumerator DisableWeed(float time)
    {
        yield return new WaitForSeconds(time);
     
        float creatTime = DataController.instance.gameData.berryFieldData[weedIdx].createTime; // 딸기가 생성된 시간변수 참조
        if ((creatTime == 0f && !GameManager.instance.isBlackPanelOn) || creatTime >= DataController.instance.gameData.stemLevel[4] ) // BP가 꺼져있고 맨 땅이거나 딸기가 수확가능한 상태라면
        {
            Debug.Log(GameManager.instance.isBlackPanelOn);
            farmColl.enabled = true; // 밭의 Collider를 켠다.
        }
        else // 아니라면
        {
            farmColl.enabled = false; // 끈다.
        }
        DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = false; // 잡초 제거됨
        if (!DataController.instance.gameData.berryFieldData[weedIdx].hasBug) // 벌레가 없다면
        {
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = true; // 딸기는 다시 자랄 수 있다.
        }
        this.gameObject.SetActive(false); // 잡초 비활성화
    }
}
