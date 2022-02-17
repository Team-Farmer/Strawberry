using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{   
    public float weedProb = 100f;
    public float xPos = 0f;   
    public Vector2 pos;
    public int spNum;

    private Farm farm;
    private StrawBerry berry;
    private Animator anim;
    private BoxCollider2D farmColl;
    void Awake()
    {        
        anim = GetComponent<Animator>();
        farm = transform.parent.gameObject.GetComponent<Farm>();

        berry = GameManager.instance.berryList[farm.farmIdx];
        farmColl = farm.GetComponent<BoxCollider2D>();
        pos = farm.transform.position;
    }
    void OnEnable()
    {
        spNum = Random.Range(0, 3);
        anim.SetInteger("Generate", spNum);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateWeed() // 잡초 생성
    {
        float prob = Random.Range(0, 100);
        
        //scale = Random.Range(1.3f, 1.8f);
        if (prob < weedProb)
        {
            this.gameObject.SetActive(true); // 나 자신(잡초)를 활성화

            farmColl.enabled = false; // 밭의 콜라이더 비활성화
            farm.hasWeed = true; // 잡초보유여부를 확인하는 변수
            berry.canGrow = false; // 딸기의 성장 제어

            xPos = Random.Range(-0.35f, 0.35f); // 밭의 X축의 랜덤한 위치에 잡초 생성
            transform.position = new Vector2(pos.x + xPos, pos.y + 0.07f);            
        }
    }
    public void DeleteWeed()
    {
        anim.SetTrigger("Delete");

        StartCoroutine(DisableWeed(0.25f)); // 애니메이션이 끝난 후 비활성화
    }
    IEnumerator DisableWeed(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false); // 잡초 비활성화

        float creatTime = berry.createTime; // 딸기가 생성된 시간변수 참조
        if (creatTime == 0f || creatTime >= 20f) // 맨 땅이거나 딸기가 수확가능한 상태라면
        {
            farmColl.enabled = true; // 밭의 Collider를 켠다.
        }
        else // 아니라면
        {
            farmColl.enabled = false; // 끈다.
        }
        farm.hasWeed = false; // 잡초 제거됨
        if (!berry.hasBug) // 벌레가 없다면
        {
            berry.canGrow = true; // 딸기는 다시 자랄 수 있다.
        }
    }
}
