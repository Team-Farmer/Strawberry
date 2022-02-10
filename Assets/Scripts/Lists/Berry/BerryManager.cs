using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] berryImg;//베리 이미지 소스들. 스프라이트들


    [Header("BERRY EXPLANATION")]//설명 저장 구조체로 저장할까..?
    public string[] berryName;
    public string[] berryTxt;

    [SerializeField]
    private GameObject berryImage;//이미지를 보일 오브젝트 대상


    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;

    //프리팹들 번호 붙여주기 용
    static int Prefabcount = 0;
    int prefabnum;



void Start()
    {
        berryExp = GameObject.Find("berryExplanation");


        //프리팹들에게 번호를 붙여 주자
        if (Prefabcount >= 32)
        {    Prefabcount -= 32;    }
        prefabnum = Prefabcount;
        Prefabcount++;


        berryImageChange();
    }

    void Update()
    {

    }




    public void Explanation()
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//Expchildren은 하이라키의 berryExplanation의 자식 berryExp를 의미한다.

        //베리를 누르면 설명창이 뜬다.
        if (ExpChildren.activeSelf == true)
        {
            ExpChildren.SetActive(false);//지금 얘네는 무용지물
        }
        else
        {
            ExpChildren.SetActive(true);//설명창이 뜬다.

            ExpChildren2 = ExpChildren.transform.GetChild(0).transform.gameObject;//Expchlidren2는 Expchildren1의 자식인 berryExpImage를 의미
            ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite = berryImg[prefabnum];
            ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text = berryName[prefabnum];
            ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = berryTxt[prefabnum];

        }



    }

    public void OffExplanation() 
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;
        ExpChildren.SetActive(false);

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
