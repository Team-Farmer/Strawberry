using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public float bugProb = 100f;
    private Animator anim;
    private StrawBerry berry;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        berry = transform.parent.gameObject.GetComponent<StrawBerry>();
    }
    void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateBug()
    {
        float prob = Random.Range(0, 100);
        if (prob < bugProb)
        {
            this.gameObject.SetActive(true);
            SetAnim("isGenerate", true);
            berry.canGrow = false;
            berry.hasBug = true;
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
