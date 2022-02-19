using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChangeLayer : MonoBehaviour
{

    //classic,special,unique,all
    public Button[] btns;

    //베리 오브젝트들의 부모 오브젝트
    [SerializeField]
    GameObject parent_content;

    //타겟 베리들을 저장하는 배열
    private GameObject[] target_berry;

    String nowTap;

    void Start()
    {
        //탭버튼 인식기능 시작
        makeBtn();
    }

    void Update()
    {
        //현재 탭이 무엇인지 인식하고 해당 태그의 베리들을 보인다
        tap_changed();
    }

    //======================================================================================================================
    


    //버튼
    private void makeBtn() {
        for (int i = 0; i < this.btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => this.TaskOnClick(index));
        }
        
    }

    //버튼 클릭시 
    private void TaskOnClick(int index)
    {
        //누른 버튼의 부모의 순서를 맨 앞으로
        btns[index].transform.parent.SetAsLastSibling();
        
        //현재 탭 이름 업데이트
        switch (index)
        {
            case 3: nowTap = "all"; break;
            case 0: nowTap = "classic"; break;
            case 1: nowTap = "special"; break;
            case 2: nowTap = "unique"; break;
        }

        //===========추가 정보===============
        //가장 처음 마지막으로 순서 변경    Transform.SetAsLastSibling  Transform.SetAsFirstSibling
        //순서 설정(index 값)    Transform.SetSiblingIndex   현재 순서 반환(index 값)    Transform.GetSiblingIndex

    }


    private void tap_changed()
    {
        //현재 탭에 따라 오브젝트를 활성화 비활성화 시키자
        //노가다;;;; 
        switch (nowTap)
        {
            case "classic":
                
                find_tag_active("berry_classic");
                find_tag_inactive("berry_special"); find_tag_inactive("berry_unique");
                break;
            case "special":
                
                find_tag_active("berry_special");
                find_tag_inactive("berry_classic"); find_tag_inactive("berry_unique");
                break;
            case "unique":
               
                find_tag_active("berry_unique");
                find_tag_inactive("berry_classic"); find_tag_inactive("berry_special");
                break;

        }

    }



    //해당 태그의 오브젝트를 찾아서 활성화시킨다
    private void find_tag_active(string tag_name) {

        //모든 베리 오브젝트 활성화한다
        int iCount = parent_content.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = parent_content.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }

        //해당 태그를 가진 오브젝트를 찾는다.
        target_berry = GameObject.FindGameObjectsWithTag(tag_name);
            
        //그 오브젝트를 활성화한다.
        for (int i = 0; i < target_berry.Length; i++) {
            target_berry[i].SetActive(true);
        }

    }
   
    //해당태그의 오브젝트를 찾아서 비활성화 시킨다.
    private void find_tag_inactive(string tag_name) {

        //해당 태그를 가진 오브젝트를 찾는다.
        target_berry = GameObject.FindGameObjectsWithTag(tag_name);

        //그 오브젝트를 비활성화한다.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(false);
        }
    }


    


}
