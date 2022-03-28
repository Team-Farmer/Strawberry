using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Strawberry : MonoBehaviour
{
    //프리팹들 번호 붙여주기 용
    static int Prefabcount = 0;
    int prefabnum;

    [Header("======berry Image=====")]
    [SerializeField]
    private Sprite yesBerryImage;//베리 있을 시 배경 이미지
    [SerializeField]
    private GameObject berryImagePanel;//이미지를 보일 오브젝트 대상




    //베리 설명 관련
    /*
    GameObject berryExp;
    GameObject ExpChildren;
    GameObject ExpChildren2;
    */

    List<GameObject> BERRY;

    //=====================================================================================================
    void Start()
    {
        //berryExp = GameObject.Find("berryExplanation");//이거 과부화 위험. 다른 방법은 없을까
        BERRY = Globalvariable.instance.berryListAll;


        //프리팹들에게 번호를 붙여 주자 0~96
        prefabnum = Prefabcount;
        Prefabcount++;

        //베리들을 보인다.
        berryImageChange();

    }
    //=====================================================================================================


    public void berryImageChange()//베리 리스트에 이미지를 보인다.
    {

        //classic=0~31  special=32~63   unique=64~95
        if (BERRY[prefabnum] == null) {
            berryImagePanel.transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);//투명 -> 불투명
        }
        else
        {
            this.transform.GetComponent<Image>().sprite = yesBerryImage;//배경 이미지 변경
            berryImagePanel.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);//투명 -> 불투명

            berryImagePanel.GetComponent<Image>().sprite = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//해당 베리 이미지 보이기
        }
    }


    //=====================================================================================================
    /*
    public void Explanation()
    {
        ExpChildren = berryExp.transform.GetChild(0).transform.gameObject;//berryExp
        ExpChildren2 = ExpChildren.transform.GetChild(1).transform.gameObject;//berryExpImage

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //설명창 띄운다
                ExpChildren.SetActive(true);

                //Explanation 내용을 채운다.
                ExpChildren2.transform.GetChild(0).transform.gameObject.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//이미지 설정
                ExpChildren2.transform.GetChild(1).transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//이름 설정
                ExpChildren2.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//설명 설정
            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }
    

    }
    */






}

