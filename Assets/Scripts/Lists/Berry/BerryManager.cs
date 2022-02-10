using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] berryImg;//띄울 베리 이미지 소스들. 스프라이트들

    [SerializeField]
    private GameObject berryImage;

    GameObject berryExp;


    static int Prefabcount = 0;
    int prefabnum;



    void Start()
    {

        berryExp = GameObject.Find("berryExplanation");



        //프리팹들에게 번호를 붙여 주자
        if (Prefabcount >= 32)
            Prefabcount -= 32;
        prefabnum = Prefabcount;
        //Debug.Log(prefabnum);
        Prefabcount++;




        berryImageChange();

    }

    void Update()
    {

    }



    //누르면 설명창 뜨고 다시 누르면 설명차 내려간다
    public void Explanation()
    {

        if (berryExp.transform.GetChild(0).transform.gameObject.activeSelf == true)
        {
            berryExp.transform.GetChild(0).transform.gameObject.SetActive(false);
        }
        else
        {
            berryExp.transform.GetChild(0).transform.gameObject.SetActive(true);
        }

    }

    public void OffExplanation() 
    { 
    

    }

    public void berryImageChange()
    {
        for (int i = 0; i < berryImg.Length; i++)
        {
            if (prefabnum == i)
                berryImage.GetComponent<Image>().sprite = berryImg[i];
        }
    }


}
