using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBerry : MonoBehaviour
{

    [Header("=====OBJECT=====")]
    [SerializeField]
    private GameObject priceText;
    [SerializeField]
    private GameObject timeText;

    [Header("=====INFO=====")]
    public int[] price;//업그레이드에 필요한 가격 배열
    public float[] time;//업그레이드에 필요한 시간 배열

    private int index=0;//현재 인덱스



    //===================================================================================================================
    void Start()
    {
        
    }

    void Update()
    {
        updateInfo(index);

    }


    //update버튼을 누르면
    public void newBerryAdd() 
    {
        //타이머가 0 이라면 
        if (time[index] < 0.9)
        {
            //새로운 딸기가 추가된다.
            Debug.Log("새로운 딸기!!");

            //금액이 빠져나간다.
            GameManager.instance.coin -= price[index];
            GameManager.instance.ShowCoinText(GameManager.instance.coin);

            //업스레이드 레벨 상승 -> 그 다음 업그레이드 금액이 보인다.
            index++;
            updateInfo(index);

            //타이머가 시작된다.(지금은 일단 10초)
            //버튼을 누르지 못한다.

        }
        else 
        {
            Debug.Log("새로운 딸기를 위해 조금 더 기다리세요");
        
        }
    }

    public void updateInfo(int index) {

        try
        {
            if (time[index] > 0) { time[index] -= Time.deltaTime; }//시간이 0보다 크면 조금씩 줄인다.

            //현재 price와 time text를 보인다.
            priceText.GetComponent<Text>().text = price[index].ToString();
            //timeText.GetComponent<Text>().text = string.Format("{0:N2}", time);//!!!!!!!!!!string.Foramt을 사용한다. 문자열 내에 {}을 쓰고 변수를 넣을 순서대로 0, 1, 2를 쓴다. 추가적인 형식이 필요한 경우 인덱스 다음에 : 을 쓰고 형식을 서술한다. N과 F를 사용해 소수점 몇 번째 까지만 표시할 수 있다. N2를 쓰면 소수점 둘째 자리까지 표시하겠다는 의미
            timeText.GetComponent<Text>().text = Mathf.Ceil (time[index]).ToString (); //정수부분만 출력한다. CeilTolnt함수는 int형으로 반환해주기도 한다.
        }
        catch
        {
            Debug.Log("다음 레벨 정보 없음");
            //버튼 누르지 못하게 하기
        }


    }
}
