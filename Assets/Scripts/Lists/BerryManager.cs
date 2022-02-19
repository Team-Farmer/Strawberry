using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BerryManager : MonoBehaviour
{
    [Serializable]
    public class BerryStruct 
    {
        public Sprite berryImage;
        public string berryName, berryTxt;
        public int berryValue;
        public bool exist;
        
        public BerryStruct(Sprite berryImage, string berryName, string berryTxt, int berryValue, bool exist) 
        {
            this.berryImage = berryImage;
            this.berryName = berryName;
            this.berryTxt = berryTxt;
            this.berryValue = berryValue;
            this.exist = exist;

        }
    }



    [SerializeField]
    private GameObject berryImagePanel;//이미지를 보일 오브젝트 대상

    [Header("==========BERRY STRUCT==========")]
    [SerializeField]
    BerryStruct[] berryInfo;



    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;


    //프리팹들 번호 붙여주기 용
    static int Prefabcount = 0;
    int prefabnum;



    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//이거 과부화 위험. 다른 방법은 없을까

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



    //=====================================================================================================
    public void Explanation()
    {


            ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//Expchildren = 하이라키의 berryExplanation의 자식 berryExp를 의미
            ExpChildren2 = ExpChildren.transform.GetChild(0).transform.gameObject;//Expchlidren2 = Expchildren1의 자식인 berryExpImage를 의미


            try
            {
                if (berryInfo[prefabnum].exist == true)
                {
                    //설명창이 뜬다.
                    ExpChildren.SetActive(true);
                    //Explanation 내용을 채운다.
                    ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite = berryInfo[prefabnum].berryImage;//이미지 설정
                    ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryName;//이름 설정
                    ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = berryInfo[prefabnum].berryTxt;//설명 설정
                }
            }
            catch
            {
                ExpChildren.SetActive(false);
                Debug.Log("여기에 해당하는 베리는 아직 없다");
            }


    }




    public void berryImageChange()//베리 리스트에 이미지를 보인다.
    {
        for (int i = 0; i < berryInfo.Length; i++)
        {
            if (prefabnum == i && berryInfo[i].exist==true)
                berryImagePanel.GetComponent<Image>().sprite = berryInfo[i].berryImage;
        }
    }


}

