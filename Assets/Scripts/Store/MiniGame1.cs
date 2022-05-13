using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public Scrollbar scrollbar;//스크롤바
    public Text count_txt;     //3초 텍스트
    public Text score_txt;     //점수
    public Image quiz_img;     //퀴즈딸기 이미지
    public int quizIndex;      //퀴즈딸기 인덱스
    public Image[] answer_img; //정답딸기 이미지(4개)
    public int[] answerIndex;  //정답딸기 인덱스(4개)
    public GameObject O;       //O 이미지
    public GameObject X;       //X 이미지
    public Button[] answer_btn;//정답딸기 버튼(4개)
    public Button pause_btn;   //일시정지 버튼
    public Button exit_btn;    //나가기 버튼


    float size;                           //스크롤바 사이즈
    int time = 63;                        //초
    int score;                            //점수
    List<int> unlockList=new List<int>(); //해금된 딸기 번호 리스트

    void Start()
    {
        size = scrollbar.size / 60f;
        for(int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(()=>OnClickAnswerButton(_i));
        }
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
            MakeQuiz();
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

    void MakeQuiz()
    {
        //퀴즈딸기 만들고 이미지 배치
        quizIndex = Random.Range(0, unlockList.Count);
        quiz_img.sprite = Globalvariable.instance.berryListAll[quizIndex].GetComponent<Sprite>();

        //정답딸기 만들고 이미지 배치
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
                answerIndex[i] = Random.Range(0, unlockList.Count);
                //정답인덱스나 다른 정답딸기들이랑 다른 딸기번호 나올때까지 랜덤번호로 뽑기
                while (answerIndex[i] == quizIndex)
                {
                    answerIndex[i] = Random.Range(0, unlockList.Count);
                }
                answer_img[i].sprite = Globalvariable.instance.berryListAll[answerIndex[i]].GetComponent<Sprite>();
            }
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

    void StopGame()
    {
        Debug.Log("게임 끝");

        //시간초기화, 리스트초기화, 하트획득
        time = 63;
        unlockList.Clear();
        DataController.instance.gameData.heart += score / 10;
    }
}
