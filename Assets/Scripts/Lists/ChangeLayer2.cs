using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLayer2 : MonoBehaviour
{
	[SerializeField]
	private bool isBerry;
	[Header("====Buttons====")]
	[SerializeField]
	private GameObject[] tagButtons;//��ư gameObject
	public Sprite[] tagButtons_Image;//������ ���� ��ư �̹���
	public Sprite[] tagButtons_selectImage;//���� ��ư �̹���

	[Header("====ScrollView====")]
	//���� content�� �� Challenge News ������Ʈ
	[SerializeField]
	private GameObject[] content;


	[Header("========Swipe=======")]
	[SerializeField]
	private Scrollbar scrollBar;                    // Scrollbar�� ��ġ�� �������� ���� ������ �˻�
	[SerializeField]
	private float swipeTime = 0.1f;         // �������� Swipe �Ǵ� �ð�
	[SerializeField]
	private float swipeDistance = 1.0f;        // �������� Swipe�Ǳ� ���� �������� �ϴ� �ּ� �Ÿ�


	//============================================================================
	//SWIPE ������

	private float[] scrollPageValues;           // �� �������� ��ġ �� [0.0 - 1.0]
	private float valueDistance = 0;            // �� ������ ������ �Ÿ�
	private int currentPage = 0;            // ���� ������
	private int maxPage = 4;                // �ִ� ������ 2�� ����

	private float startTouchX;              // ��ġ ���� ��ġ
	private float endTouchX;                    // ��ġ ���� ��ġ

	private bool isSwipeMode = false;       // ���� Swipe�� �ǰ� �ִ��� üũ


	//===========================================================================================================
	private void Awake()
	{
		if (isBerry == true)
		{
			// ��ũ�� �Ǵ� �������� �� value ���� �����ϴ� �迭 �޸� �Ҵ�
			scrollPageValues = new float[4];

			// ��ũ�� �Ǵ� ������ ������ �Ÿ�
			valueDistance = 1f / (scrollPageValues.Length - 1f);

			// ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
			for (int i = 0; i < scrollPageValues.Length; ++i)
			{
				scrollPageValues[i] = valueDistance * i;
			}
		}
	}
	void Start()
	{
		if (isBerry == true)
		{
			//ó������ berry classic
			selectBerryTag("berry_classic");

			// ó�� ������ �� 0�� ������ ���δ�.
			SetScrollBarValue(0);
		}

	}

	private void Update()
	{
		if (isBerry == true) UpdateInput();
	}

	//===========================================================================================================

	#region SWIPE

	private void UpdateInput()
	{
		// ���� Swipe�� �������̸� ��ġ �Ұ�
		if (isSwipeMode == true) return;

#if UNITY_EDITOR
		// ���콺 ���� ��ư�� ������ �� 1ȸ
		if (Input.GetMouseButtonDown(0))
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			startTouchX = Input.mousePosition.x;

		}
		else if (Input.GetMouseButtonUp(0))
		{
			// ��ġ ���� ���� (Swipe ���� ����)
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
				// ��ġ ���� ���� (Swipe ���� ����)
				startTouchX = touch.position.x;

			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				endTouchX = touch.position.x;


				UpdateSwipe();
			}
		}
#endif


	}

	private void UpdateSwipe()
	{

		// �ʹ� ���� �Ÿ��� �������� ���� Swipe �ȵȴ�.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
		{
			// ���� �������� Swipe�ؼ� ���ư���
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}



		// Swipe ����
		bool isLeft = startTouchX < endTouchX ? true : false;

		// �̵� ������ ������ ��
		if (isLeft == true)
		{
			// ���� �������� ���� ���̸� ����
			if (currentPage == 0) return;

			// �������� �̵��� ���� ���� �������� 1 ����
			currentPage--;
		}
		// �̵� ������ �������� ��
		else if (isLeft == false)
		{
			// ���� �������� ������ ���̸� ����
			if (currentPage == maxPage - 1) return;

			// ���������� �̵��� ���� ���� �������� 1 ����
			currentPage++;
		}


		// currentIndex��° �������� Swipe�ؼ� �̵�
		StartCoroutine(OnSwipeOneStep(currentPage));

	}


	// �������� �� �� ������ �ѱ�� Swipe ȿ�� ���
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

	#endregion

	//��Ʈ�ѹ� ��ġ ����
	public void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value = scrollPageValues[index];
	}

	public void swipeButton(string value)
	{
		switch (value)
		{

			case "left":
				// ���� �������� ���� ���̸� ����
				if (currentPage == 0) return;

				currentPage--;// �������� �̵� = ���� ������ 1 ����
				SetScrollBarValue(currentPage);

				break;

			case "right":
				// ���� �������� ������ ���̸� ����
				if (currentPage == maxPage - 1) return;

				// ���������� �̵� = ���� ������ 1 ����
				currentPage++;
				SetScrollBarValue(currentPage);

				break;

		}

	}







	//��ư ������ ȿ��
	public void TagImageChange(int index)
	{

		//��ư ��������Ʈ �� �ȴ����ŷ�
		for (int i = 0; i < tagButtons_Image.Length; i++)
		{
			tagButtons[i].GetComponent<Image>().sprite = tagButtons_Image[i];
		}

		//�ش� ��ư ��������Ʈ�� �����ŷ�
		tagButtons[index].GetComponent<Image>().sprite = tagButtons_selectImage[index];

	}

	//challenge News â ����
	public void TurnOn(GameObject obj)
	{
		for (int i = 0; i < content.Length; i++) { content[i].SetActive(false); } //challenge, news �� �� ��Ȱ��ȭ
		obj.SetActive(true); //�ش� â�� Ȱ��ȭ
	}




	//��ư�� ������ ��
	//�ش� �з��� ���⸦ ���δ�
	public void selectBerryTag(string name)
	{

		//��� ������ ���̰� Ȱ��ȭ
		AllActive();

		//�ٸ� ������ �Ⱥ��̰� ��Ȱ��ȭ
		switch (name)
		{
			case "berry_classic": inActive("berry_special"); inActive("berry_unique"); break;
			case "berry_special": inActive("berry_classic"); inActive("berry_unique"); break;
			case "berry_unique": inActive("berry_special"); inActive("berry_classic"); break;
		}


		SetScrollBarValue(0);

	}





	public void inActive(string name)
	{
		int index = 0;
		switch (name)
		{
			case "berry_classic": index = 0; break;
			case "berry_special": index = 16; break;
			case "berry_unique": index = 32; break;
		}

		//��Ȱ��ȭ
		for (int j = 0; j < 4; j++)
		{
			for (int i = index; i < index + 16; i++)
			{
				Transform trChild = content[j].transform.GetChild(i);
				trChild.gameObject.SetActive(false);
			}
		}



	}


	public void AllActive()
	{
		//��� ���� ������Ʈ Ȱ��ȭ
		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < content[j].transform.childCount; i++)
			{
				Transform trChild = content[j].transform.GetChild(i);
				trChild.gameObject.SetActive(true);
			}

		}
	}
}