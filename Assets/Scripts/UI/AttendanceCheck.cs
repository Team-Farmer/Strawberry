using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using TMPro;


[System.Serializable]
public class ObjectArray
{
    public Sprite[] Behind = new Sprite[2];
}

public class AttendanceCheck : MonoBehaviour
{
    #region 인스펙터 및 변수 생성

    [SerializeField] TMP_Text weekTMP;
    [SerializeField] TMP_Text[] tagTMP;

    public ObjectArray[] Front = new ObjectArray[7];
    public Image[] image = new Image[7];
    public Text[] text = new Text[7];
    public GameObject icon;
    public GameObject m_tag;
    public TextMeshProUGUI text_mesh;

    private int days;
    private int hearts;
    private int weeks;
    private int multiply_tag;
    private bool isAttendance;
    private int daysCompare;

    String weeksText;
    DateTime today;
    DateTime lastday;
    TimeSpan ts;

    #endregion

    #region 출석 메인 기능

    public void Attendance()
    {
        #region 변수 초기화

        today = DataController.instance.gameData.currentDay;
        lastday = DataController.instance.gameData.atdLastday; //지난 날짜 받아오기
        isAttendance = DataController.instance.gameData.isAttendance; //출석 여부 판단 bool 값
        days = DataController.instance.gameData.accDays; // 출석 누적 날짜

        #endregion

        if (isAttendance == false)
        {
            ts = today - lastday; //날짜 차이 계산
            daysCompare = ts.Days; //Days 정보만 추출.
            icon.SetActive(true);
            Debug.Log("날짜 차이: "+daysCompare);

            if (daysCompare == 1) //연속 출석
            {
                WeeksInit();
                selectDay(days);
            }
            else if(daysCompare > 1|| daysCompare == 0) //연속출석이 아닌경우
            {
                DataController.instance.gameData.accDays = 0;
                days = DataController.instance.gameData.accDays;
                weeks = 1;
                multiply_tag = 1;
                selectDay(days);
            }
        }
        else //출석을 이미 한 상태다
        {
            AlreadyAtd();
        }

        WeeksTag();
    }

    public void AlreadyAtd()
    {
        WeeksInit();

        for (int i = 0; i < days; i++) //출석완료 버튼 활성화
        {
            image[i].sprite = Front[i].Behind[1];
        }
    }

    public void WeeksTag()
    {
        if (weeks < 9 && weeks > 1)
        {
            weekTMP.text = weeks.ToString();

            for (int i = 0; i < tagTMP.Length; i++)
            {
                tagTMP[i].text = weeks.ToString();
            }
            m_tag.SetActive(true);
        }
        else if (weeks > 9)
        {
            weekTMP.text = weeks.ToString();

            for (int i = 0; i < tagTMP.Length; i++)
            {
                tagTMP[i].text = "9";
            }
            m_tag.SetActive(true);

        }
    }

    public void WeeksInit()
    {
        if (days > 6)
        {
            weeks = 1 + (days % 6);
            days %= 6;
            multiply_tag = weeks;
        }
        else
        {
            weeks = 1;
            multiply_tag = 1;
        }
    }

    #endregion

    #region 날짜 선택

    public void selectDay(int day)
    {
        if (day != 0)
        {
            for (int i = 0; i < day; i++)
            {
                image[i].sprite = Front[i].Behind[1];
            }
        }
        image[day].sprite = Front[day].Behind[0];
    }

    #endregion

    #region 출석 정보 저장

    public void AttandanceSave(int number)
    {
        
        if (number == days&& isAttendance==false)
        {
            AudioManager.instance.RewardAudioPlay();
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //출석 정보 저장.
            DataController.instance.gameData.accDays += 1; 
            DataController.instance.gameData.isAttendance = true;
            DataController.instance.gameData.atdLastday 
                = DataController.instance.gameData.currentDay;
            //DataController.instance.gameData.accAttendance += 1; // 누적 출석 증가
                                                                 // 10*날짜*주수
                                                                 // Debug.Log("누적 출석 : " + DataController.instance.gameData.accAttendance);
                                                                 // Debug.Log("누적 하트 : " + DataController.instance.gameData.accHeart);
            hearts = number;
            Invoke("AtdHeart", 0.75f);
        }
    }

    public void AtdHeart()
    {
        int num=0;
        switch (hearts)
        {
            case 0:
            case 1:
            case 2:
                num = 10;
                break;
            case 3:
            case 4:
            case 5:
                num = 20;
                break;
            case 6:
                num = 30;
                break;
            default:
                break;
        }
        GameManager.instance.GetHeart(num  * multiply_tag);
    }
}
    #endregion


