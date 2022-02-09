using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject berryExp;//berryExplanation


    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;
    void Start()
    {
        prefabnum = Prefabcount;
        Debug.Log(prefabnum);
        Prefabcount++;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //누르면 설명창 뜨고 다시 누르면 설명차 내려간다
    public void Explanation() { 

        if (berryExp.activeSelf == false)
        {
            berryExp.SetActive(true);
            berryExp.SetActive(true);
        }
        else
        {
            berryExp.SetActive(false);
            berryExp.SetActive(false);
        } 
    }


}
