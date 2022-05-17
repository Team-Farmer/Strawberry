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
    [SerializeField]
    private GameObject ExclamationMark;


    //베리 설명창
    private GameObject berryExp;

    //=====================================================================================================
    //=====================================================================================================

    private void Awake()
    {
        //프리팹들에게 번호를 붙여 주자
        prefabnum = Prefabcount;
        Prefabcount++;
    }
    void Start()
    {
        berryExp = GameObject.FindGameObjectWithTag("BerryExplanation");
        berryExp = berryExp.transform.GetChild(0).gameObject;

        //베리 정보 가져오기
        BERRY = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        //베리들을 보인다.
        berryImageChange();

    }
    private void Update()
    {
        //베리들을 보인다.
        berryImageChange();
        //이거 없애기
        //gameManager에서 newBerry추가될때마다 갱신하는 방법도 있다.
    }

    //=====================================================================================================
    //=====================================================================================================
    //베리 리스트에 이미지를 보인다
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

        //베리 아직 한번도 확인하지 않았다면 느낌표 표시. 이미 한 번 봤으면 없애기
        if (DataController.instance.gameData.isBerryEM[prefabnum] == true)
        { ExclamationMark.SetActive(true); }
        else { ExclamationMark.SetActive(false); }

    }


    //베리 설명창 띄우기
    public void BerryExplanation() {

        try
        {

            if (DataController.instance.gameData.isBerryEM[prefabnum] == true) 
            { DataController.instance.gameData.isBerryEM[prefabnum] = false; }

            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                
                //설명창 띄운다
                berryExp.SetActive(true);
                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;
                GameObject berryExpPrice= berryExp.transform.GetChild(5).gameObject;

                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//이미지 설정

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//이름 설정

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//설명 설정

                berryExpPrice.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryPrice.ToString()+"A";//설명 설정


            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }

    }

}

