using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeList2 : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollBar;                    // Scrollbar의 위치를 바탕으로 현재 페이지 검사
    [SerializeField]
    private float swipeTime = 0.2f;         // 페이지가 Swipe 되는 시간
    [SerializeField]
    private float swipeDistance = 50.0f;        // 페이지가 Swipe되기 위해 움직여야 하는 최소 거리


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //마우스를 n만큼 끌면

        //스크롤바가 몇초동안 중간으로 이동한다.
    }
}
