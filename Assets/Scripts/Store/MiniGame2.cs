using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame2 : MiniGame
{
    [Header("MiniGame2")]
    public Berry[] rotten_berry; // 상한(정답) 딸기 인덱스
    public int normalIndex;      //퀴즈딸기 인덱스
    public int[] answerIndex_4x4;  //정답딸기 인덱스(16개)
    public Button[] answer_btn_4x4;//정답딸기 버튼(16개)
    public Image[] answer_img_4x4; //정답딸기 이미지(16개)
    public GameObject O;       //O 이미지
    public GameObject X;       //X 이미지
    public int rottenIndex; // 4(무른) or 8(상한)인 인덱스

    protected override void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            int _i = i;
            answer_btn_4x4[_i].onClick.AddListener(() => OnClickAnswerButton(_i));
        }
        answerIndex_4x4 = new int[16];
        base.Awake();
    }

    protected override void MakeGame()
    {
        //normal_img.gameObject.SetActive(true);
        for (int i = 0; i < 16; i++)
        {
            answer_img_4x4[i].gameObject.SetActive(true);
            answer_btn_4x4[i].enabled = true;
        }

        //정상 딸기 만들고 이미지 배치
        while (true) // 상한, 무른 딸기를 제외하고 선정
        {
            normalIndex = unlockList[UnityEngine.Random.Range(0, unlockList.Count)];
            if (normalIndex != 4 && normalIndex != 8) break;
        }
        Debug.Log("normalIndex.: " + normalIndex);

        //랜덤의 상한 딸기 인덱스(0~16)에 상한 딸기 배치
        int randomAnswerIndex = UnityEngine.Random.Range(0, 16);
        for (int i = 0; i < 16; i++)
        {
            if (randomAnswerIndex == i)
            {
                // 상한 or 무른 딸기 선정
                rottenIndex = rotten_berry[UnityEngine.Random.Range(0, 2)].berryIdx;
                // 상한 or 무른 딸기 배치
                answerIndex_4x4[i] = rottenIndex;               
            }
            else
            {
                // 정상 딸기 배치
                answerIndex_4x4[i] = normalIndex;
            }
            answer_img_4x4[i].sprite = global.berryListAll[answerIndex_4x4[i]].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void OnClickAnswerButton(int index)
    {
        //정답 : 10점 추가, 다음문제 출제
        if (answerIndex_4x4[index] == rottenIndex)
        {
            O.SetActive(true);
            score += 10;
            score_txt.text = score.ToString() + "점";
        }
        //오답 : 10초 줄기
        else
        {
            X.SetActive(true);
            scrollbar.size -= size * 10;
            time -= 10;
        }
        if (time > 0)
        {
            Invoke("MakeNextQuiz", 0.3f);
        }
    }

    void MakeNextQuiz()
    {
        O.SetActive(false);
        X.SetActive(false);
        MakeGame();
    }

    public override void StopGame()
    {
        base.StopGame();
        //딸기 안보이게
        for (int i = 0; i < 16; i++)
        {
            answer_img_4x4[i].gameObject.SetActive(false);
            answer_btn_4x4[i].enabled = false;
        }
        O.SetActive(false);
        X.SetActive(false);
    }

    protected override void FinishGame()
    {
        base.FinishGame();

        //최고기록 저장
        if (DataController.instance.gameData.highScore[1] < score)
        {
            DataController.instance.gameData.highScore[1] = score;
        }

        //결과패널
        resultPanel.SetActive(true);
        result_txt.text = "최고기록 : " + DataController.instance.gameData.highScore[1] + "\n현재점수 : " + score;

        

        // 미니게임 1 보상 하트 공식(미니게임 2은 해금 하트가 40이다)
        float gain_coin = score * research_level_avg * ((100 + 40 * 2) / 100f);

        Debug.Log("얻은 코인:" + Convert.ToInt32(gain_coin));

        //코인지급
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        StopGame();
    }
}
