using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Text count_txt;
    float size;
    int time = 63;

    void OnEnable()
    {
        size = scrollbar.size/60f;

        StartGame();
    }

    void StartGame()
    {
        //3초 카운트
        count_txt.gameObject.SetActive(true);
        StartCoroutine(Count3Second());
    }

    IEnumerator Count3Second()
    {
        count_txt.text = (time - 60).ToString();
        yield return new WaitForSeconds(1);
        time -= 1;
        if (time <= 60)
        {
            count_txt.gameObject.SetActive(false);
            StartCoroutine(Timer());
        }
        else
        {
            StartCoroutine(Count3Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        scrollbar.size -= size;
        time -= 1;
        if (time == 0)
        {
            StopGame();
        }
        else
        {
            StartCoroutine(Timer());
        }
    }

    void StopGame()
    {
        Debug.Log("게임 끝");
        time = 63;
    }
}
