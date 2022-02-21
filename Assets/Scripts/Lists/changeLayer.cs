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
    //[SerializeField]
    //private GameObject scrollBar;


    private GameObject[] target_berry;

    [Header("========Swipe List=======")]
    [SerializeField]
    private Scrollbar scrollBar;                    // Scrollbar의 위치를 바탕으로 현재 페이지 검사
    [SerializeField]
    private float swipeTime = 0.1f;         // 페이지가 Swipe 되는 시간
    [SerializeField]
    private float swipeDistance = 1.0f;        // 페이지가 Swipe되기 위해 움직여야 하는 최소 거리



    private float[] scrollPageValues;           // 각 페이지의 위치 값 [0.0 - 1.0]
    private float valueDistance = 0;            // 각 페이지 사이의 거리
    private int currentPage = 0;            // 현재 페이지
    private int maxPage = 2;                // 최대 페이지 2로 설정

    private float startTouchX;              // 터치 시작 위치
    private float endTouchX;                    // 터치 종료 위치

    private float startScroll;
    private float endScroll;

    private bool isSwipeMode = false;       // 현재 Swipe가 되고 있는지 체크

    private void Awake()
    {
        // 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
        scrollPageValues = new float[2];

        // 스크롤 되는 페이지 사이의 거리
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

    }
    void Start()
    {
        //처음에는 berry classic
        selectBerryTag("berry_classic");

        // 처음 시작할 때 0번 페이지 보인다.
        SetScrollBarValue(0);
    }


	public void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value = scrollPageValues[index];
	}

	private void Update()
	{
		UpdateInput();

	}

	public void swipeButton(int value)
	{
		switch (value)
		{

			case 0:
				// 현재 페이지가 왼쪽 끝이면 종료
				if (currentPage == 0) return;
				// 왼쪽으로 이동. 현재 페이지를 1 감소
				currentPage--;
				SetScrollBarValue(0);

				break;

			case 1:
				// 현재 페이지가 오른쪽 끝이면 종료
				if (currentPage == maxPage - 1) return;
				// 오른쪽으로 이동. 현재 페이지를 1 증가
				currentPage++;
				SetScrollBarValue(1);
				break;

		}

	}

	private void UpdateInput()
	{
		// 현재 Swipe를 진행중이면 터치 불가
		if (isSwipeMode == true) return;

		#if UNITY_EDITOR
		// 마우스 왼쪽 버튼을 눌렀을 때 1회
		if (Input.GetMouseButtonDown(0))
		{
			// 터치 시작 지점 (Swipe 방향 구분)
			startTouchX = Input.mousePosition.x;

		}
		else if (Input.GetMouseButtonUp(0))
		{
			// 터치 종료 지점 (Swipe 방향 구분)
			endTouchX = Input.mousePosition.x;


			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				// 터치 시작 지점 (Swipe 방향 구분)
				startTouchX = touch.position.x;

			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// 터치 종료 지점 (Swipe 방향 구분)
				endTouchX = touch.position.x;


				UpdateSwipe();
			}
		}
		#endif


	}

	private void UpdateSwipe()
	{

		// 너무 작은 거리를 움직였을 때는 Swipe 안된다.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
		{
			// 원래 페이지로 Swipe해서 돌아간다
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
			Debug.Log("swipe");
		}



		// Swipe 방향
		bool isLeft = startTouchX < endTouchX ? true : false;

		// 이동 방향이 왼쪽일 때
		if (isLeft == true)
		{
			// 현재 페이지가 왼쪽 끝이면 종료
			if (currentPage == 0) return;

			// 왼쪽으로 이동을 위해 현재 페이지를 1 감소
			currentPage--;
		}
		// 이동 방향이 오른쪽일 떄
		else if (isLeft == false)
		{
			// 현재 페이지가 오른쪽 끝이면 종료
			if (currentPage == maxPage - 1) return;

			// 오른쪽으로 이동을 위해 현재 페이지를 1 증가
			currentPage++;
		}


		// currentIndex번째 페이지로 Swipe해서 이동
		StartCoroutine(OnSwipeOneStep(currentPage));

	}


	// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
	private IEnumerator OnSwipeOneStep(int index)
	{
		float start = scrollBar.value;
		float current = 0;
		float percent = 0;

		isSwipeMode = true;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

			yield return null;
		}

		isSwipeMode = false;
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
		//scrollBar.transform.GetComponent<Scrollbar>().value = 0;
		SetScrollBarValue(0);

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
