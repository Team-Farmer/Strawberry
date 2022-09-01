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
    public Image Sandy;
    public Sprite[] SandySprite;

    private float[] shaded = { 0.5f, 0.75f, 0.9f};
    private int shade_idx = 0;
    private int randomAnswerIndex = 0;

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
        // 음영 계수를 점수에 따라 정하기
        if(score < 100) shade_idx = 0;
        else if (100 <= score && score <= 200) shade_idx = 1;
        else if(score >= 200) shade_idx = 2;

        answer_img_4x4[randomAnswerIndex].color = new Vector4(1, 1, 1, 1);
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
        Debug.Log("normalIndex: " + normalIndex);

        //랜덤의 상한 딸기 인덱스(0~16)에 상한 딸기 배치
        randomAnswerIndex = UnityEngine.Random.Range(0, 16);
        Debug.Log("randomAnswerIndex: " + randomAnswerIndex);
        for (int i = 0; i < 16; i++)
        {
            answerIndex_4x4[i] = normalIndex; // 딸기 배치
            answer_img_4x4[i].sprite = global.berryListAll[answerIndex_4x4[i]].GetComponent<SpriteRenderer>().sprite;
            if (randomAnswerIndex == i)
            {
                // 상한 or 무른 딸기 배치(음영으로)               
                float rgb = shaded[shade_idx];
                answer_img_4x4[i].color = new Vector4(rgb, rgb, rgb, 1);
                Debug.Log("answer_img_4x4[i].color.r: " + answer_img_4x4[i].color.r);
            }               
        }
    }

    public void OnClickAnswerButton(int index)
    {
        Color color = answer_img_4x4[index].color;
        //정답 : 10점 추가, 다음문제 출제
        if (!color.Equals(new Vector4(1, 1, 1, 1)))
        {
            O.SetActive(true);
            score += 10;
            score_txt.text = score.ToString();
            AudioManager.instance.RightAudioPlay();

            Sandy.GetComponent<Image>().sprite = SandySprite[0];
        }
        //오답 : 10초 줄기
        else
        {
            X.SetActive(true);
            //scrollbar.size -= size * 10;
            scroll.fillAmount -= size * 10;
            time -= 10;
            AudioManager.instance.WrongAudioPlay();

            Sandy.GetComponent<Image>().sprite = SandySprite[1];
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
        AudioManager.instance.EndAudioPlay();

        ManageScore(1, score);

        //결과패널
        resultPanel.SetActive(true);
        result_cur_score_txt.text = score + "점";
        result_highscore_txt.text = DataController.instance.gameData.highScore[1].ToString();



        // 미니게임 2 보상 하트 공식(미니게임 2은 해금 하트가 40이다)
        float gain_coin = score * research_level_avg * ((100 + 40 * 2) / 100f);
        //result_coin_txt.text = gain_coin.ToString();
        GameManager.instance.ShowCoinText(result_coin_txt.GetComponent<Text>(), Convert.ToInt32(gain_coin));

        Debug.Log("얻은 코인:" + Convert.ToInt32(gain_coin));

        //코인지급
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        StopGame();
    }
}
