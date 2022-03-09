using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    SpriteRenderer sprite;    
    public float bugProb; // ¿Å±è
    private Animator anim;
    private Stem stem;
    private Farm farm;
    public int bugIdx;
    public float scale; // ¿Å±è
    public bool isBugEnable;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();        
        anim = GetComponent<Animator>();
        stem = transform.parent.gameObject.GetComponent<Stem>();
        farm = GameManager.instance.farmList[stem.stemIdx];
    }
    void OnEnable()
    {
        isBugEnable = true;
        SetAnim("isGenerate", true);
        
        transform.localScale = new Vector2(scale, scale);
        
        stem.canGrow = false;
        stem.hasBug = true;
    }
    void OnDisable()
    {
        isBugEnable = false;
        if (!farm.hasWeed)
        {
            stem.canGrow = true;
        }
        stem.hasBug = false;
    }
    // Update is called once per frame    
    public void GenerateBug()
    {      
        float prob = Random.Range(0, 100);
        scale = Random.Range(1.2f, 1.5f);
        if (prob < bugProb)
        {           
            this.gameObject.SetActive(true);            
        }
    }
    public void DieBug()
    {
        SetAnim("isDie", true);
        
        StartCoroutine(DisableBug(0.25f));        
    }
    void SetAnim(string name, bool b)
    {
        anim.SetBool(name, b);
    }
    IEnumerator DisableBug(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false);
    }
}
