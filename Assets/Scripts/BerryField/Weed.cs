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
    public void GenerateWeed() // 説段 持失
    {
        float prob = Random.Range(0, 100);
        
        //scale = Random.Range(1.3f, 1.8f);
        if (prob < weedProb)
        {
            this.gameObject.SetActive(true);

            farmColl.enabled = false;
            farm.hasWeed = true;
            berry.canGrow = false;

            xPos = Random.Range(-0.35f, 0.35f);
            transform.position = new Vector2(pos.x + xPos, pos.y + 0.07f);            
        }
    }
    public void DeleteWeed()
    {
        anim.SetTrigger("Delete");
               
        StartCoroutine(DisableWeed(0.25f));
    }
    IEnumerator DisableWeed(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false);

        float creatTime = berry.createTime;
        if (creatTime == 0f || creatTime >= 20f)
        {
            farmColl.enabled = true;
        }
        else
        {
            farmColl.enabled = false;
        }
        farm.hasWeed = false;
        if(!berry.hasBug)
        {
            berry.canGrow = true;
        }
    }
}
