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
    public Sprite[] normalState = new Sprite[7];
    public Image[] image = new Image[7];
    public Text[] text = new Text[7];
    public GameObject icon;
    public GameObject m_tag;
    public GameObject[] heartMover = new GameObject[7];
    public TextMeshProUGUI text_mesh;

    private int days;
    private int hearts;
    private int weeks;
    private int multiply_tag;
    private int daysCompare;
    public bool[] isClicked = new bool[7];

    String weeksText;
    TimeSpan ts;
    DateTime now;

    #endregion

    #region 출석 메인 기능


    public void Attendance()
    {
        days = DataController.instance.gameData.accDays; // 출석 누적 날짜
        now = DataController.instance.gameData.currentTime;

        for (int i = 0; i < 7; i++) //출석 버튼 초기화
        {
            image[i].sprite = Front[i].Behind[0];
            isClicked[i] = false;
            image[i].gameObject.SetActive(false);
        }

        if (DaysCalculate() == 0) //연속 출석
        {
            DataController.instance.gameData.isAttendance = false;
            icon.SetActive(true);
            selectDay(days);
        }
        else if (DaysCalculate() == 1 && DataController.instance.gameData.isFirstGame)
        {
            for (int i = 0; i < days; i++)
            {
                image[i].gameObject.SetActive(true);
                image[i].sprite = Front[i].Behind[1];
                isClicked[i] = true;
            }
        }
        else //연속출석이 아닌경우
        {
            DataController.instance.gameData.isAttendance = false;
            icon.SetActive(true);
            DataController.instance.gameData.accDays = 0;
            days = DataController.instance.gameData.accDays;
            DataController.instance.gameData.weeks = 1;
            selectDay(days);
        }
        WeeksTag();

        //Debug.Log("날짜 차이:" + daysCompare);
        //Debug.Log("지난 날짜" +":" + DataController.instance.gameData.atdLastday);
    }

    public int DaysCalculate()
    {
        ts = now - DataController.instance.gameData.atdLastday.Date; //날짜 차이 계산
        daysCompare = ts.Days; //Days 정보만 추출.

        DataController.instance.gameData.weeks = (DataController.instance.gameData.accDays / 7) + 1;
        if (DataController.instance.gameData.weeks > 9)
            DataController.instance.gameData.weeks = 9;

        if (days > 7)
        {
            days %= 7;            
        }

        if (daysCompare==1)
            return 0;
        else if (daysCompare == 0)
            return 1;

        return 2;
    }

    public void WeeksTag()
    {
        weekTMP.text = DataController.instance.gameData.weeks.ToString();


        if (DataController.instance.gameData.weeks > 1)
        {
            for (int i = 0; i < tagTMP.Length; i++)
            {
                tagTMP[i].text = DataController.instance.gameData.weeks.ToString();
            }

            m_tag.SetActive(true);
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
                image[i].gameObject.SetActive(true);
                image[i].sprite = Front[i].Behind[1];
                isClicked[i] = true;
            }
        }
        image[day].gameObject.SetActive(true);
        isClicked[day] = false;
        /*        if (day != 0)
                {
                    for (int i = 0; i < day; i++)
                    {
                        image[i].sprite = Front[i].Behind[1];
                    }
                }
                image[day].sprite = Front[day].Behind[0];*/
    }

    #endregion

    #region 출석 정보 저장


    public void AttandanceSave(int number)
    {
        if (number == days && !isClicked[number])
        {
            isClicked[number] = true;
            AudioManager.instance.RewardAudioPlay();
            heartMover[number].GetComponent<HeartMover>().HeartMove(number);
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //출석 정보 저장.
            DataController.instance.gameData.accDays += 1;
            DataController.instance.gameData.isAttendance = true;
            DataController.instance.gameData.atdLastday
                = DataController.instance.gameData.currentTime;
            DataController.instance.gameData.accAttendance++;

            hearts = number;
            Invoke("AtdHeart", 0.75f);

        }
    }

    public void AtdHeart()
    {
        int num = 0;
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
        GameManager.instance.GetHeart(num * DataController.instance.gameData.weeks);
    }
}
#endregion


