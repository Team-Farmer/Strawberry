using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;


[System.Serializable]
public class ObjectArray
{
    public Sprite[] Behind = new Sprite[2];
}

public class AttendanceCheck : MonoBehaviour
{
    #region 인스펙터

    public ObjectArray[] Front = new ObjectArray[7];
    public Image[] image = new Image[7];
    public GameObject icon;
    int count;
    int days;

    #endregion

    private void Start()
    {
        count = 0;
        
    }

    #region 출석 메인 기능

    public void Attendance()
    {
        DateTime today = DataController.instance.gameData.Today;
        DateTime lastday = DataController.instance.gameData.Lastday; //지난 날짜 받아오기
        bool isAttendance = DataController.instance.gameData.attendance; //출석 여부 판단 bool 값
        days = DataController.instance.gameData.days; // 출석 누적 날짜.
        int weeks; //주차 구분

        //테스트용
        //lastday = DateTime.Parse("2022-04-05");
        //days = 6;
        //isAttendance = false;
  

        TimeSpan ts = today - lastday; //날짜 차이 계산
        int DaysCompare = ts.Days; //Days 정보만 추출.

        if (isAttendance == false)
        {
            icon.SetActive(true);
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //주차별로 Week 텍스트 설정 추후 추가예정.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else if (days == 0)
            {
                weeks = days;
            }
            else
            {
                weeks = days;
            }

            if (DaysCompare == 1) //오늘 날짜가 지난번 출석 날짜보다 하루 미래면
            {
                selectDay(weeks);
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
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //주차별로 Week 텍스트 설정 추후 추가예정.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else
            {
                weeks = days;
            }
            for (int i = 0; i < weeks; i++) //출석완료 버튼 활성화
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
        count = day;
    }

    #endregion

    #region 출석 정보 저장

    public void AttandanceSave(int number)
    {
        
        if (number == days)
        {
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //출석 정보 저장.
            DataController.instance.gameData.days += 1;
            DataController.instance.gameData.attendance = true;
            DataController.instance.gameData.Lastday = DataController.instance.gameData.Today;

            DataController.instance.gameData.accAttendance += 1; // 누적 출석 증가
            GameManager.instance.GetHeart(10 * (number + 1)); // 10*날짜*주수
                                                              // Debug.Log("누적 출석 : " + DataController.instance.gameData.accAttendance);
                                                              // Debug.Log("누적 하트 : " + DataController.instance.gameData.accHeart);
        }
    }

    public static implicit operator AttendanceCheck(GameManager v)
    {
        throw new NotImplementedException();
    }
}
    #endregion
