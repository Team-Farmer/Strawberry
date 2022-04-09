using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Strawberry : MonoBehaviour
{
    //프리팹들 번호 붙여주기
    static int Prefabcount = 0;
    int prefabnum;

    //베리 정보를 담아올 리스트
    List<GameObject> BERRY;

    [Header("======Strawberry=====")]
    [SerializeField]
    private Sprite yesBerryImage;//베리 있을 시 배경 이미지
    [SerializeField]
    private GameObject berryImagePanel;//이미지를 보일 오브젝트 대상


    

    //=====================================================================================================
    void Start()
    {

        //프리팹들에게 번호를 붙여 주자
        prefabnum = Prefabcount;
        Prefabcount++;

        //베리 정보 가져오기
        BERRY = Globalvariable.instance.berryListAll;//이거 변경!!!!!!!!!!!!!!!!!!!!

        //베리들을 보인다.
        berryImageChange();

    }
    private void Update()
    {
        //베리들을 보인다.
        berryImageChange();//이럴필요가있나
    }

    //베리 리스트에 이미지를 보인다.===========================================================================
    public void berryImageChange()
    { 
        //베리 정보가 존재하고 && 베리가 unlock 되었다면 
        if (BERRY[prefabnum] != null && DataController.instance.gameData.isBerryUnlock[prefabnum]==true)
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = yesBerryImage;//배경 이미지 변경
            berryImagePanel.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);//투명 -> 불투명

            berryImagePanel.GetComponent<Image>().sprite 
                = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//해당 베리 이미지 보이기
        }
    }


    //베리 설명창 띄우기========================================================================================
    public void BerryExplanation() {

        GameManager.instance.Explanation(BERRY[prefabnum],prefabnum);
    
    }

}

