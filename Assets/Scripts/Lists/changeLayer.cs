using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeLayer : MonoBehaviour
{
    public GameObject[] ScrollViews;
    GameObject selectScrollView;
    GameObject selectTag;

    public Sprite selectTagImage;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //버튼을 눌렀을 때 해당 분류의 딸기를 보인다.
    public void selectBerryTag(int index)
    {


        //다른 스크롤뷰 비활성화
        for (int i = 0; i < ScrollViews.Length; i++) {
            ScrollViews[i].SetActive(false);
        }
        //해당 스크롤뷰 활성화
        ScrollViews[index].SetActive(true);

        //horizontal scrollbar 처음으로 돌리기


        //다른 버튼 스프라이트 보통으로 변경
        //해당 버튼 스프라이트 눌림으로 변경

    }
}
