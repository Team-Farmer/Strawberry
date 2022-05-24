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

    public ObjectArray[] Front = new ObjectArray[7];
    public Image[] image = new Image[7];
    public Text[] text = new Text[7];
    public TextMeshProUGUI text_mesh;
    public GameObject icon;
    public GameObject Tag;

    public bool isAtd;

    private int days;
    private int hearts;
    String weeks_text;




    #endregion

    #region 출석 메인 기능

    public void Attendance()
    {
        DateTime today = DataController.instance.gameData.Today;
        DateTime lastday = DataController.instance.gameData.Lastday; //지난 날짜 받아오기
        bool isAttendance = DataController.instance.gameData.attendance; //출석 여부 판단 bool 값
        days = DataController.instance.gameData.days; // 출석 누적 날짜.
        int weeks; //주차 구분


/*        //테스트용
        lastday = DateTime.Parse("2022-04-28");
        days = 5;
        isAttendance = false;*/


        TimeSpan ts = today - lastday; //날짜 차이 계산
        int DaysCompare = ts.Days; //Days 정보만 추출.


        if (days > 6)
        {
            weeks = 1+(days / 7);
            days %= 7;

            if (weeks > 9)
            {
                weeks = 9;
            }
            weeks_text = weeks.ToString();
            text_mesh.text = weeks_text;

            for (int i = 0; i < text.Length; i++)
            {
                text[i].text = weeks_text;
            }
            Tag.SetActive(true);
        }
        else 
        {
            weeks = 1;
            weeks_text = weeks.ToString();
            for (int i = 0; i < text.Length; i++)
            {
                text[i].text = weeks_text;
            }
        }


        if (isAttendance == false)
        {
            icon.SetActive(true);

            if (DaysCompare == 1) //오늘 날짜가 지난번 출석 날짜보다 하루 미래면
            {
                selectDay(days);
            }
            else if (DateTime.Compare(today, lastday) < 0) //오늘이 과거인 경우는 없지만, 오류 방지용
            {
                DataController.instance.gameData.days = 0;
                selectDay(0);
            }
            else //연속출석이 아닌경우
            {
                //days 1으로 초기화 후 day1버튼 활성화, week 1로 변경.
                DataController.instance.gameData.days = 0;
                selectDay(0);
            }
        }
        else //출석을 이미 한 상태다
        {
            for (int i = 0; i < days; i++) //출석완료 버튼 활성화
            {
                image[i].sprite = Front[i].Behind[1];
            }
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
        
        if (number == days&& DataController.instance.gameData.attendance==false)
        {
            AudioManager.instance.RewardAudioPlay();
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //출석 정보 저장.
            DataController.instance.gameData.days += 1;
            DataController.instance.gameData.attendance = true;
            DataController.instance.gameData.Lastday = DataController.instance.gameData.Today;

            DataController.instance.gameData.accAttendance += 1; // 누적 출석 증가
                                                                 // 10*날짜*주수
                                                                 // Debug.Log("누적 출석 : " + DataController.instance.gameData.accAttendance);
                                                                 // Debug.Log("누적 하트 : " + DataController.instance.gameData.accHeart);

            hearts = number;
            Invoke("AtdHeart", 0.75f);
        }
    }

    public void AtdHeart()
    {
        GameManager.instance.GetHeart(10 * (hearts + 1));
    }

    public static implicit operator AttendanceCheck(GameManager v)
    {
        throw new NotImplementedException();
    }
}
    #endregion


