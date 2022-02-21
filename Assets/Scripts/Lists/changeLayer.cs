using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeLayer : MonoBehaviour
{

    [Header("====Berry List Buttons====")]
    [SerializeField]
    private GameObject[] tagButtons;
    public Sprite[] tagButtons_Image;
    public Sprite[] tagButtons_selectImage;

    [Header("====ScrollView====")]
    //베리 오브젝트 부모 오브젝트
    [SerializeField]
    private GameObject content1;
    [SerializeField]
    private GameObject content2;

    //스트롤바
    [SerializeField]
    private GameObject scrollBar;


    private GameObject[] target_berry;

    void Start()
    {
        //처음에는 berry classic
        selectBerryTag("berry_classic");
    }

    
    void Update()
    {
        
    }

    public void TagImageChange(int index) {

        //버튼 스프라이트 다 안눌린거로
        for (int i = 0; i < tagButtons_Image.Length; i++) {
            tagButtons[i].GetComponent<Image>().sprite = tagButtons_Image[i];
        }

        //해당 버튼 스프라이트만 눌린거로
        tagButtons[index].GetComponent<Image>().sprite = tagButtons_selectImage[index];
    
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
        scrollBar.transform.GetComponent<Scrollbar>().value = 0;

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
        int iCount = content1.transform.childCount;
        for (int i = 0; i < iCount; i++)
        {
            Transform trChild = content1.transform.GetChild(i);
            trChild.gameObject.SetActive(true);
        }
        int iCount2 = content2.transform.childCount;
        for (int i = 0; i < iCount2; i++)
        {
            Transform trChild2 = content2.transform.GetChild(i);
            trChild2.gameObject.SetActive(true);
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
