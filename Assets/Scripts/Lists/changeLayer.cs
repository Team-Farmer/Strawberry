using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeLayer : MonoBehaviour
{
    //스트롤바
    public GameObject scrollBar;

    [SerializeField]
    private GameObject[] tagButtons;

    //베리 오브젝트 부모 오브젝트
    [SerializeField]
    GameObject parent_content;


    private GameObject[] target_berry;

    void Start()
    {
        selectBerryTag("berry_classic");
    }

    
    void Update()
    {
        
    }

    public void TagImageChange(Sprite selectImage) {
        tagButtons[1].GetComponent<Image>().sprite = selectImage;
    
    }

    //버튼을 눌렀을 때 해당 분류의 딸기를 보인다.name=tag이름
    public void selectBerryTag(string name)
    {

        //선택 베리들 보이게 활성화
        Active(name);
        
        //다른 베리들 안보이게 비활성화
        switch (name) {
            case "berry_classic": inActive("berry_special"); inActive("berry_unique"); break;
            case "berry_special": inActive("berry_classic"); inActive("berry_unique"); break;
            case "berry_unique": inActive("berry_special"); inActive("berry_classic"); break;
        }


        //horizontal scrollbar 처음으로 돌리기
        

    }

    public void inActive(string name) {

        //해당 태그를 가진 오브젝트를 찾는다.
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //그 오브젝트를 비활성화한다.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(false);
        }

    }

    public void Active(string name)
    {
        //모든 베리 오브젝트 활성화
        int iCount = parent_content.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = parent_content.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
        

        //해당 베리들 보이게 활성화
        target_berry = GameObject.FindGameObjectsWithTag(name);

        //그 오브젝트를 활성화한다.
        for (int i = 0; i < target_berry.Length; i++)
        {
            target_berry[i].SetActive(true);
        }

    }


}
