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
            if (time[index] > 0) { time[index] -= Time.deltaTime; }//시간이 0보다 크면 1초씩 감소

            //현재 price와 time text를 보인다.
            priceText.GetComponent<Text>().text = price[index].ToString();
            timeText.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(time[index])); //정수부분만 출력한다.

        }
        catch
        {
            Debug.Log("다음 레벨 정보 없음");
            //버튼 누르지 못하게 하기
        }


    }

    public string TimeForm(int time)
    {
        int M=0, S=0;//M,S 계산용
        string Minutes, Seconds;//M,S 텍스트 적용용

        M = (time / 60); 
        S = (time % 60);


        //M,S적용
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S가 10미만이면 01, 02... 식으로 표시
        if (M < 10 && M>0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S>0) { Seconds = "0" + S.ToString(); }

        //M,S가 0이면 00으로 표시한다.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
}
