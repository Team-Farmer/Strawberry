using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BerryManager : MonoBehaviour
{
    //프리팹들 번호 붙여주기 용
    static int Prefabcount = 0;
    int prefabnum;


    [Serializable]
    public class BerryStruct 
    {
        public Sprite berryImage;
        public string berryName, berryTxt;
        public int berryValue;
        //public bool exist;
        
        public BerryStruct(Sprite berryImage, string berryName, string berryTxt, int berryValue, bool exist) 
        {
            this.berryImage = berryImage;
            this.berryName = berryName;
            this.berryTxt = berryTxt;
            this.berryValue = berryValue;
            //this.exist = DataController.instance.gameData.isBerryUnlock[0];

        }
    }

    [Header("======berry Image=====")]
    public Sprite yesBerryImage;

    [SerializeField]
    private GameObject berryImagePanel;//이미지를 보일 오브젝트 대상

    [Header("==========BERRY STRUCT==========")]
    [SerializeField]
    BerryStruct[] berryInfo;



    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;


    



    //=====================================================================================================
    void Start()
    {
        berryExp = GameObject.Find("berryExplanation");//이거 과부화 위험. 다른 방법은 없을까

        //프리팹들에게 번호를 붙여 주자
        /*
        if (Prefabcount >= 32)
        {    Prefabcount -= 32;    }
        */
        if (Prefabcount % 32==0)
        { Prefabcount =0; }

        prefabnum = Prefabcount;
        Prefabcount++;

        berryImageChange();

        //Debug.Log("exist====" + berryInfo[prefabnum].exist);
    }



    //=====================================================================================================
    public void Explanation()
    {

            ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//berryExp
            ExpChildren2 = ExpChildren.transform.GetChild(1).transform.gameObject;//berryExpImage

            try
            {
            //if (berryInfo[prefabnum].exist == true)
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                    //검정창 띄운다.
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
                //ExpChildren.SetActive(false);
                Debug.Log("여기에 해당하는 베리는 아직 없다");
            }


    }




    public void berryImageChange()//베리 리스트에 이미지를 보인다.
    {
        for (int i = 0; i < berryInfo.Length; i++)
        {
            if (prefabnum == i && DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                this.transform.GetComponent<Image>().sprite = yesBerryImage;
                berryImagePanel.transform.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                berryImagePanel.GetComponent<Image>().sprite = berryInfo[i].berryImage;
            }
        }

    }


}

