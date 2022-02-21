using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeList : MonoBehaviour
{
	[Header("========Swipe List=======")]
	[SerializeField]
	private Scrollbar scrollBar;                    // Scrollbar의 위치를 바탕으로 현재 페이지 검사
	[SerializeField]
	private float swipeTime = 0.2f;         // 페이지가 Swipe 되는 시간
	[SerializeField]
	private float swipeDistance = 10.0f;        // 페이지가 Swipe되기 위해 움직여야 하는 최소 거리
	[SerializeField]
	private float swipeDistance_scrollBar = 0.01f;


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

	private void Start()
	{
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

		//스크롤바 위치 0혹은 1로 변경
		//if (scrollBar.value < 0.5f) { scrollBar.value = 0f; }
		//else { scrollBar.value = 1f; }
		Debug.Log(currentPage);
	}

	public void swipeButton(int value) {
		switch (value) {

			case 0: 
				// 현재 페이지가 왼쪽 끝이면 종료
				if (currentPage == 0) return;
				// 왼쪽으로 이동. 현재 페이지를 1 감소
				currentPage--; 
				break;

			case 1: 
				// 현재 페이지가 오른쪽 끝이면 종료
				if (currentPage == maxPage - 1) return;

				// 오른쪽으로 이동. 현재 페이지를 1 증가
				currentPage++; 
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
			// 스크롤 시작 지점
			startScroll = scrollBar.value;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// 터치 종료 지점 (Swipe 방향 구분)
			endTouchX = Input.mousePosition.x;
			//스크롤 종료 지점
			endScroll = scrollBar.value;

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
				// 스크롤 시작 지점
				startScroll = scrollBar.value;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// 터치 종료 지점 (Swipe 방향 구분)
				endTouchX = touch.position.x;
				//스크롤 종료 지점
				endScroll = scrollBar.value;

				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{

		// 너무 작은 거리를 움직였을 때는 Swipe 안된다.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance || Mathf.Abs(startScroll - endScroll) < swipeDistance_scrollBar)
		{
			// 원래 페이지로 Swipe해서 돌아간다
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}

		


		// Swipe 방향
		bool isLeft = startTouchX < endTouchX ? true : false;
		bool isLeft2 = startScroll < endScroll ? true : false;

		// 이동 방향이 왼쪽일 때
		if (isLeft == true || isLeft2==false)
		{
			// 현재 페이지가 왼쪽 끝이면 종료
			if (currentPage == 0) return;

			// 왼쪽으로 이동을 위해 현재 페이지를 1 감소
			currentPage--;
		}
		// 이동 방향이 오른쪽일 떄
		else if(isLeft==false||isLeft2==true)
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


}


