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
    public GameObject collectionNow;
    public GameObject medalTxt;
    public GameObject heartTxt;
    public Sprite collectionBtnSprite;//베리 다 모았을 때 완료버튼 스프라이트
    public Sprite collectionNowSprite;//베리 다 모았을 때 베리배경 스프라이트
    public GameObject FinBtn;
    public GameObject FinBG;
    public GameObject heart;
    public GameObject medal;

    [Header("[Animation]")]
    public GameObject heartMover;
    public GameObject medalMover;

    //=================================================================================
    //=================================================================================
    //프리팹들 번호 붙여주기
    static int Prefabcount = 0;
    int prefabnum;

    public int berryClassifyNum = 0;

    private GameObject Global;
 

    void Start()
    {
        Global = GameObject.FindGameObjectWithTag("Global");
        //프리팹들에게 번호를 붙여 주자
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;

        berryClassifyNum = Info[prefabnum].berryClassify.Length;//달성완료할 딸기 몇개인지
        InfoUpdateOnce();
    }

    private void Update()
    {
        InfoUpdate();
    }

    private void InfoUpdate()
    {
        medalTxt.GetComponent<Text>().text = "X" + Info[prefabnum].rewardMedal.ToString();
        heartTxt.GetComponent<Text>().text = "X" + Info[prefabnum].rewardHeart.ToString();
        //얻은 베리는 색이 보인다.
        for (int i = 0; i < berryClassifyNum; i++)
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
    {//여기수정
        for (int i = 0; i < berryClassifyNum; i++) 
        {
            if (DataController.instance.gameData.isBerryUnlock[Info[prefabnum].berryClassify[i]] == true)
            { continue; }
            else { return; }
        
        }
        collectionBtn.GetComponent<Image>().sprite = collectionBtnSprite;//3개다 얻었으면 버튼변경한다.
     
    }


    private void InfoUpdateOnce()
    {
        //수집 제목
        collectionName.GetComponent<Text>().text = Info[prefabnum].Name;

        //베리(다 검정색으로)
        for (int i = 0; i < berryClassifyNum; i++)
        {
            if (Global.GetComponent<Globalvariable>().berryListAll[Info[prefabnum].berryClassify[i]] != null)//베리 정보 있을경우에
            { 
                //스프라이트 적용
                collectionNow.transform.GetChild(i).transform.GetComponent<Image>().sprite
                    = Global.GetComponent<Globalvariable>().
                    berryListAll[Info[prefabnum].berryClassify[i]].GetComponent<SpriteRenderer>().sprite;
                collectionNow.transform.GetChild(i).transform.GetComponent<Image>().preserveAspect = true;
                collectionNow.transform.GetChild(i).transform.GetComponent<Image>().color = new Color(0f, 0f, 0f);
            }
            
        }
        if (berryClassifyNum ==2) 
        { collectionNow.transform.GetChild(2).gameObject.SetActive(false); }

        //이미 보상도 받고 다끝난거면 더이상 못누르게
        if (DataController.instance.gameData.isCollectionDone[prefabnum] == true) 
        { FinishCollect();}
    }

    public void collectionBtnClick() 
    {
        if (collectionBtn.GetComponent<Image>().sprite == collectionBtnSprite) //지금 버튼 스프라이트가 완료 버튼이면
        {
            heartMover.GetComponent<HeartMover>().HeartChMover(120);
            medalMover.GetComponent<HeartMover>().BadgeMover(120);


            GameManager.instance.GetCoin(Info[prefabnum].rewardHeart*50);
            //하트 획득
            //GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);
            //메달 획득
            //GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);

            AudioManager.instance.RewardAudioPlay();
            FinishCollect();
            //완전히 끝났다.
            DataController.instance.gameData.isCollectionDone[prefabnum] = true;

        }
        
    }

    public void FinishCollect()
    {
        //완료 UI 적용
        collectionNow.GetComponent<Image>().sprite = collectionNowSprite;
        collectionBtn.SetActive(false);
        FinBtn.SetActive(true);
        FinBG.SetActive(true);
        medalTxt.SetActive(false);
        heartTxt.SetActive(false);
    }
}
