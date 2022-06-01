using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    [Header("Game")]
    public Scrollbar scrollbar;//스크롤바
    public Text score_txt;     //점수
    public Image quiz_img;     //퀴즈딸기 이미지
    public int quizIndex;      //퀴즈딸기 인덱스
    public Button[] answer_btn;//정답딸기 버튼(4개)
    public Image[] answer_img; //정답딸기 이미지(4개)
    public int[] answerIndex;  //정답딸기 인덱스(4개)
    public GameObject O;       //O 이미지
    public GameObject X;       //X 이미지


    [Header("UI")]
    public GameObject countImgs;//카운트 이미지
    public Button pause_btn;   //일시정지 버튼
    public Button exit_btn;    //나가기 버튼
    public GameObject resultPanel;//결과패널
    public Text result_txt;    //결과 텍스트

    float size;                         //스크롤바 사이즈
    int time;                           //초
    int score;                          //점수
    List<int> unlockList=new List<int>(); //해금된 딸기 번호 리스트

    Globalvariable global;
    void Start()
    {
        size = scrollbar.size / 60f;
        for(int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(()=>OnClickAnswerButton(_i));
        }
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
    }

    void OnEnable()
    {
        StartGame();
    }

    void StartGame()
    {
        //보유한 딸기 리스트 구성
        for (int i = 0; i < 192; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                unlockList.Add(i);
            }
        }
        scrollbar.size = 1;
        score = 0;
        time = 64;
        score_txt.text = score.ToString() + "점";

        //4초 카운트
        StartCoroutine(Count4Second());
    }

    IEnumerator Count4Second()
    {
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(false);
        time -= 1;
        if (time <= 60)
        {
            quiz_img.gameObject.SetActive(true);
            for(int i = 0; i < 4; i++){ 
                answer_img[i].gameObject.SetActive(true);
                answer_btn[i].enabled = true;
            }
            StartCoroutine(Timer());
            MakeQuiz();
        }
        else
        {
            StartCoroutine(Count4Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        scrollbar.size -= size;
        time -= 1;
        if (time <= 0)
        {
            FinishGame();
        }
        else
        {
            StartCoroutine(Timer());
        }
    }

    void MakeQuiz()
    {
        //퀴즈딸기 만들고 이미지 배치
        quizIndex = Random.Range(0, unlockList.Count);
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
                answerIndex[i] = Random.Range(0, unlockList.Count);
                while (CheckIndex(i))
                {
                    answerIndex[i] = Random.Range(0, unlockList.Count);
                }
                answer_img[i].sprite = global.berryListAll[answerIndex[i]].GetComponent<SpriteRenderer>().sprite;
            }
        }

        bool CheckIndex(int idx)
        {
            if (answerIndex[idx] == quizIndex) return true;
            for(int i = 0; i < 4; i++)
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
            score_txt.text = score.ToString()+"점";
        }
        //오답 : 10초 줄기
        else
        {
            X.SetActive(true);
            scrollbar.size -= size*10;
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
        MakeQuiz();
    }

    void FinishGame()
    {
        //딸기 안보이게
        quiz_img.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++) { 
            answer_img[i].gameObject.SetActive(false);
            answer_btn[i].enabled=false;
        }
        O.SetActive(false);
        X.SetActive(false);

        //최고기록 저장
        if (DataController.instance.gameData.highScore[0] < score)
        {
            DataController.instance.gameData.highScore[0] = score;
        }

        //결과패널
        resultPanel.SetActive(true);
        result_txt.text = "최고기록 : " + DataController.instance.gameData.highScore[0] + "\n현재점수 : " + score;

        //하트지급
        DataController.instance.gameData.heart += score / 10;

        //미니게임 플레이 횟수 증가
        DataController.instance.gameData.mgPlayCnt++;
    }

    public void ReStart()
    {
        score = 0;
        time = 64;
        unlockList.Clear();
        StartGame();
    }

    public void OnClickPauseButton()
    {
        Time.timeScale = 0;
    }

    public void OnClickKeepGoingButton()
    {
        Time.timeScale = 1;
    }
}
