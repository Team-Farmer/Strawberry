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
        public string Name;//���� ����
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
    CollectionPrefabStruct[] Info;//����ü

    //=================================================================================

    [Header("[Collection]")]
    public GameObject collectionName;
    public GameObject collectionBtn;
    public GameObject collectionNoBtn;
    public GameObject collectionNow;
    public Sprite collectionBtnSprite;
    //=================================================================================
    //=================================================================================
    //�����յ� ��ȣ �ٿ��ֱ�
    static int Prefabcount = 0;
    int prefabnum;

    public int berryClassifyNum = 0;

    private GameObject Global;

    void Start()
    {
        Global = GameObject.FindGameObjectWithTag("Global");
        //�����յ鿡�� ��ȣ�� �ٿ� ����
        prefabnum = Prefabcount;
        Prefabcount++;

        berryClassifyNum = Info[prefabnum].berryClassify.Length;//�޼��Ϸ��� ���� �����
        InfoUpdateOnce();
    }

    private void Update()
    {
        InfoUpdate();

    }

    private void InfoUpdate()
    {
        
        //���� ������ ���� ���δ�.
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
    {//�������
        for (int i = 0; i < berryClassifyNum; i++) 
        {
            if (DataController.instance.gameData.isBerryUnlock[Info[prefabnum].berryClassify[i]] == true)
            { continue; }
            else { return; }
        
        }
        collectionBtn.GetComponent<Image>().sprite = collectionBtnSprite;//3���� ������� ��ư�����Ѵ�.
    }


    private void InfoUpdateOnce()
    {

        //���� ����
        collectionName.GetComponent<Text>().text = Info[prefabnum].Name;

        //����(�� ����������)
        for (int i = 0; i < berryClassifyNum; i++)
        {
            if (Global.GetComponent<Globalvariable>().berryListAll[Info[prefabnum].berryClassify[i]] != null)//���� ���� ������쿡
            { 
                //��������Ʈ ����
                collectionNow.transform.GetChild(i).transform.GetComponent<Image>().sprite
                    = Global.GetComponent<Globalvariable>().
                    berryListAll[Info[prefabnum].berryClassify[i]].GetComponent<SpriteRenderer>().sprite;
            }
            collectionNow.transform.GetChild(i).transform.GetComponent<Image>().color = new Color(0f, 0f, 0f);
        }
        if (berryClassifyNum ==2) 
        { collectionNow.transform.GetChild(2).gameObject.SetActive(false); }

        //�̹� ���� �ް� �ٳ����Ÿ� ���̻� ��������
        if (DataController.instance.gameData.isCollectionDone[prefabnum] == true) 
        { collectionNoBtn.SetActive(true); }
    }

    public void collectionBtnClick() 
    {
        if (collectionBtn.GetComponent<Image>().sprite == collectionBtnSprite) //���� ��ư ��������Ʈ�� �Ϸ� ��ư�̸�
        {
            //��Ʈ ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);
            //�޴� ȹ��
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            
            //��ư ���̻� ��������
            collectionNoBtn.SetActive(true);

            //������ ������.
            DataController.instance.gameData.isCollectionDone[prefabnum] = true;
        }
        
    }
}