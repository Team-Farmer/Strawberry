using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public Scrollbar scrollbar;
    float size;
    int time = 60;

    void OnEnable()
    {
        size = scrollbar.size/60f;

        //3초 카운트

        //게임시작
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        scrollbar.size -= size;
        time -= 1;
        yield return new WaitForSeconds(1);
        if (time == 0) StopGame();
        else StartCoroutine(Timer());
    }

    void StopGame()
    {
        Debug.Log("게임 끝");
    }
}
