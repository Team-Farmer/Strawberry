using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    static public bool isGameRunning = false;           //게임이 실행 중인지
    static public bool isMiniGameWork = false;           //게임이 실행 중인지

    [Header("UI")]
    public Scrollbar scrollbar;//스크롤바
    public Image scroll;
    public Text score_txt;     //점수
    public GameObject countImgs;//카운트 이미지
    public Button pause_btn;   //일시정지 버튼
    public GameObject resultPanel;//결과패널
    public Text result_txt;    //결과 텍스트

    public float size;                         //스크롤바 사이즈
    public int time;                           //초
    public int score;                          //점수
    protected List<int> unlockList=new List<int>(); //해금된 딸기 번호 리스트

    protected Globalvariable global;
    protected int research_level_sum;
    protected float research_level_avg;


    protected virtual void Awake()
    {
        //size = scrollbar.size / 60f; 
        size = scroll.fillAmount / 60f;
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
    }

    protected virtual void OnEnable()
    {
        StartGame();
        isMiniGameWork = true;
    }

    void StartGame()
    {   
        // 리스트 초기화
        unlockList.Clear();

        //보유한 딸기 리스트 구성
        for (int i = 0; i < 192; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                unlockList.Add(i);
            }
        }
        Debug.Log("unlockList.Count: " + unlockList.Count);
        //scrollbar.size = 1;
        scroll.fillAmount = 1;
        score = 0;
        time = 64;
        score_txt.text = score.ToString() + "점";
        isGameRunning = false;
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
            StartCoroutine(Timer());          
            MakeGame(); //각 게임에서 오버라이딩 하기
            isGameRunning = true; // Update 함수 쓰려면 그냥 게임 시작 변수 쓰는게 편함
        }
        else
        {
            StartCoroutine(Count4Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        //scrollbar.size -= size;
        scroll.fillAmount -= size;
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

    IEnumerator Timer2()
    {    
        while (time >= 0)
        {
            scroll.fillAmount -= size;
            time -= 1;
            yield return new WaitForSeconds(1);
        }
        FinishGame();
    }

    protected virtual void MakeGame()
    {
        
    }

    protected virtual void FinishGame()
    {
        research_level_sum = 0;

        for (int i = 0; i < DataController.instance.gameData.researchLevel.Length; i++)
        {
            // 연구레벨의 합을 구한다.
            research_level_sum += DataController.instance.gameData.researchLevel[i];
        }

        // 연구레벨의 평균을 구한다.
        research_level_avg = research_level_sum / DataController.instance.gameData.researchLevel.Length;

        //미니게임 플레이 횟수 증가
        DataController.instance.gameData.mgPlayCnt++;       
    }

    public virtual void StopGame()
    {
        score = 0;
        time = 64;
        unlockList.Clear();

        isGameRunning = false;
        isMiniGameWork = false;
    }

    public virtual void ReStart() //다시하기
    {
        score = 0;
        time = 64;
        unlockList.Clear();
        //StartGame();
        OnEnable();
    }

    public virtual void OnClickPauseButton() //일시정지
    {
        Time.timeScale = 0;
        isGameRunning = false;
    }

    public virtual void OnClickKeepGoingButton() //일시정지 해제
    {
        Time.timeScale = 1;
        isGameRunning = true;
    }

    public void OnclickExitButton() //게임 나가기
    {
        OnClickKeepGoingButton();
        StopGame();
    }
}
