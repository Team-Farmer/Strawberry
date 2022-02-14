using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    SpriteRenderer sprite;    
    public float bugProb = 100f;
    private Animator anim;
    private StrawBerry berry;

    public float scale = 1.5f;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();        
        anim = GetComponent<Animator>();
        berry = transform.parent.gameObject.GetComponent<StrawBerry>();
    }
    void OnEnable()
    {
        transform.localScale = new Vector2(scale, scale);
        SetAnim("isGenerate", true);
        
        berry.canGrow = false;
        berry.hasBug = true;
    }
    void OnDisable()
    {
        Color color = sprite.color; 
        
        color.a = 0f;
        sprite.color = color;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateBug()
    {
        float prob = Random.Range(0, 100);
        scale = Random.Range(1.3f, 1.8f);
        if (prob < bugProb)
        {           
            this.gameObject.SetActive(true);            
        }
    }
    public void DieBug()
    {
        SetAnim("isDie", true);
        berry.canGrow = true;
        berry.hasBug = false;
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
