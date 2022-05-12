using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{

    [Serializable]
    public class CollectionPrefabStruct
    {
        public string Name;//수집 제목
        public int[] berryClassify;
        public int rewardMedal;
        public int rewardHeart;

        public CollectionPrefabStruct(string Name, int[] berryClassify, int rewardMedal, int rewardHeart)
        {
            this.Name = Name;
            this.berryClassify = berryClassify;
            this.rewardHeart = rewardHeart;
            this.rewardMedal = rewardMedal;
        }
    }

    [Header("==========COLLECTION INFO STRUCT==========")]
    [SerializeField]
    CollectionPrefabStruct[] Info;//구조체

    //=================================================================================

    [Header("[Collection]")]
    public GameObject collectionName;
    public GameObject collectionBtn;
    public GameObject collectionNoBtn;
    public GameObject collectionNow;
    public Sprite collectionBtnSprite;
    //=================================================================================
    //=================================================================================
    //프리팹들 번호 붙여주기
    static int Prefabcount = 0;
    int prefabnum;


    void Start()
    {
        //프리팹들에게 번호를 붙여 주자
        prefabnum = Prefabcount;
        Prefabcount++;

        InfoUpdateOnce();

    }

    private void Update()
    {
        InfoUpdate();

    }
    private void InfoUpdate()
    {
        //얻은 베리는 색이 보인다.
        for (int i = 0; i < 3; i++)
        {
            int berryIndex = Info[prefabnum].berryClassify[i];
            if (DataController.instance.gameData.isBerryUnlock[berryIndex] == true)
            {
                collectionNow.transform.GetChild(i).transform.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                buttonUpdate();
            }
        }
    }
    private void buttonUpdate()
    {
        if (
            DataController.instance.gameData.isBerryUnlock[Info[prefabnum].berryClassify[0]] == true &&
            DataController.instance.gameData.isBerryUnlock[Info[prefabnum].berryClassify[1]] == true &&
            DataController.instance.gameData.isBerryUnlock[Info[prefabnum].berryClassify[2]] == true
           )
        { collectionBtn.GetComponent<Image>().sprite = collectionBtnSprite; }//3개다 얻었으면 버튼변경한다.
    }


    private void InfoUpdateOnce()
    {
        //수집 제목
        collectionName.GetComponent<Text>().text = Info[prefabnum].Name;

        //베리(다 검정색으로)
        for (int i = 0; i < 3; i++)
        {
            collectionNow.transform.GetChild(i).transform.GetComponent<Image>().sprite
                = Globalvariable.instance.berryListAll[Info[prefabnum].berryClassify[i]]
                .GetComponent<SpriteRenderer>().sprite;
            collectionNow.transform.GetChild(i).transform.GetComponent<Image>().color = new Color(0f, 0f, 0f);
        }
        //이미 보상도 받고 다끝난거면 더이상 못누르게
        if (DataController.instance.gameData.isCollectionDone[prefabnum] == true) 
        { collectionNoBtn.SetActive(true); }
    }

    public void collectionBtnClick() 
    {
        if (collectionBtn.GetComponent<Image>().sprite == collectionBtnSprite) //지금 버튼 스프라이트가 완료 버튼이면
        {
            //하트 획득
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);
            //메달 획득
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            
            //버튼 더이상 못누르게
            collectionNoBtn.SetActive(true);

            //완전히 끝났다.
            DataController.instance.gameData.isCollectionDone[prefabnum] = true;
        }
        
    }
}
