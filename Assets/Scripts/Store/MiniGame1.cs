using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniGame
{
    [Header("MiniGame1")]
    public Image quiz_img;     //퀴즈딸기 이미지
    public int quizIndex;      //퀴즈딸기 인덱스
    public int[] answerIndex;  //정답딸기 인덱스(4개)
    public Button[] answer_btn;//정답딸기 버튼(4개)
    public Image[] answer_img; //정답딸기 이미지(4개)
    public GameObject O;       //O 이미지
    public GameObject X;       //X 이미지

    protected override void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(() => OnClickAnswerButton(_i));
        }
        answerIndex = new int[4];
        base.Awake();
    }

    protected override void MakeGame()
    {
        quiz_img.gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            answer_img[i].gameObject.SetActive(true);
            answer_btn[i].enabled = true;
        }

        //퀴즈딸기 만들고 이미지 배치
        quizIndex = unlockList[Random.Range(0, unlockList.Count)];
        quiz_img.sprite = global.berryListAll[quizIndex].GetComponent<SpriteRenderer>().sprite;

        //랜덤의 정답딸기 인덱스(0~4)에 퀴즈딸기 배치
        int randomAnswerIndex = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            if (randomAnswerIndex == i)
            {
                answer_img[i].sprite = quiz_img.sprite;
                answerIndex[i] = quizIndex;
            }
            else
            {
                //정답인덱스나 다른 정답딸기들이랑 다른 딸기번호 나올때까지 랜덤번호로 뽑아서 정답딸기에 배치
                answerIndex[i] = unlockList[Random.Range(0, unlockList.Count)];
                while (CheckIndex(i))
                {
                    answerIndex[i] = unlockList[Random.Range(0, unlockList.Count)] ;
                }
                answer_img[i].sprite = global.berryListAll[answerIndex[i]].GetComponent<SpriteRenderer>().sprite;
            }
        }

        bool CheckIndex(int idx)
        {
            if (answerIndex[idx] == quizIndex) return true;
            for (int i = 0; i < 4; i++)
            {
                if (i == idx) continue;
                if (answerIndex[idx] == answerIndex[i]) return true;
            }
            return false;
        }
    }

    public void OnClickAnswerButton(int index)
    {
        //정답 : 10점 추가, 다음문제 출제
        if (answerIndex[index] == quizIndex)
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
        quiz_img.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            answer_img[i].gameObject.SetActive(false);
            answer_btn[i].enabled = false;
        }
        O.SetActive(false);
        X.SetActive(false);
    }
}
