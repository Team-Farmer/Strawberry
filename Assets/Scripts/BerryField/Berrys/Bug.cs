using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{    
    private Animator anim;   
    public int bugIdx;
      
    void Awake()
    {         
        anim = GetComponent<Animator>();        
    }
    void OnEnable()
    {
        DataController.instance.gameData.berryFieldData[bugIdx].isBugEnable = true;
        SetAnim("isGenerate", true);
        
        transform.localScale = new Vector2(DataController.instance.gameData.berryFieldData[bugIdx].scale, DataController.instance.gameData.berryFieldData[bugIdx].scale);

        DataController.instance.gameData.berryFieldData[bugIdx].canGrow = false;
        DataController.instance.gameData.berryFieldData[bugIdx].hasBug = true;
    }
    void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[bugIdx].isBugEnable = false;
        if (!DataController.instance.gameData.berryFieldData[bugIdx].hasWeed)
        {
            DataController.instance.gameData.berryFieldData[bugIdx].canGrow = true;
        }
        DataController.instance.gameData.berryFieldData[bugIdx].hasBug = false;
    }
    // Update is called once per frame    
    public void GenerateBug()
    {      
        float prob = Random.Range(0, 100);
        DataController.instance.gameData.berryFieldData[bugIdx].scale = Random.Range(1.2f, 1.5f);
        if (prob < DataController.instance.gameData.berryFieldData[bugIdx].bugProb)
        {           
            this.gameObject.SetActive(true);            
        }
    }
    public void DieBug()
    {
        SetAnim("isDie", true);

        if(this.gameObject.activeSelf)
        {
            StartCoroutine(DisableBug(0.25f));
        }           
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
