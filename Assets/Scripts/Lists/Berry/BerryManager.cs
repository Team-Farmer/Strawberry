using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject berryExpPanel;//berryExplanation 로 화면에 뜰 것
    
    [SerializeField]
    private Sprite[] berryImg;//띄울 베리 이미지들 스프라이트들

    [SerializeField]
    private GameObject berryImage;

    [SerializeField]
    private int startCount;


    Image nowBerryImg;


    static int Prefabcount = 0;
    int prefabnum;



    private void Awake()
    {
        Prefabcount = 0;
    }

    void Start()
    {
        nowBerryImg = berryImage.GetComponent<Image>();

        if (Prefabcount >= 32)
            Prefabcount -= 32;

        prefabnum = Prefabcount;
        Debug.Log(prefabnum);
        Prefabcount++;

        
        berryImageChange();

    }

    void Update()
    {

    }



    //누르면 설명창 뜨고 다시 누르면 설명차 내려간다
    public void Explanation() {

        if (berryExpPanel.activeSelf == false)
        {
            berryExpPanel.SetActive(true);
        }
        else
        {
            berryExpPanel.SetActive(false);
        }
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
