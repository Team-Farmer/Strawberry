using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject content;
    public GameObject correctTxt;
    public GameObject fakeImage;

    Sprite leftSprite;
    Sprite rightSprite;


    List<GameObject> berryListAll;//global의 berryListAll

    protected override void Awake()
    {

        berryListAll = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        SetGame();
        base.Awake();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
       
    }
    void Start()
    {
        
    }



    void SetGame() 
    {
        //왼쪽과 오른쪽 정답 설정
        int leftOne;
        int rightOne;
        do { leftOne = UnityEngine.Random.Range(0, 10); } 
        while (DataController.instance.gameData.isBerryUnlock[leftOne] == false);
        
        do { rightOne = UnityEngine.Random.Range(0, 10); } 
        while (leftOne == rightOne || DataController.instance.gameData.isBerryUnlock[rightOne] == false);

        leftSprite = berryListAll[leftOne].GetComponent<SpriteRenderer>().sprite;
        rightSprite = berryListAll[rightOne].GetComponent<SpriteRenderer>().sprite;

        
        //해당 정답들을 보인다.
        leftImage.GetComponent<Image>().sprite = leftSprite;
        rightImage.GetComponent<Image>().sprite = rightSprite;

        leftImage.GetComponent<Image>().preserveAspect = true;
        rightImage.GetComponent<Image>().preserveAspect = true;


        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = UnityEngine.Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = leftSprite; }
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = rightSprite; }

            content.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;

        }

    }


    public void clickAnswer(bool isLeft) 
    {
        //효과 초기화
        StopCoroutine("FadeCoroutine");
        correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);

        StopCoroutine(MoveCoroutine(true, content.transform.GetChild(content.transform.childCount - 1).gameObject));
        fakeImage.SetActive(false);
        fakeImage.GetComponent<RectTransform>().position = new Vector3(50f,-597f,0);
        

        //정답여부 판별=======================================
        Sprite answerSprite;
        if (isLeft == true) { answerSprite = leftSprite; }
        else { answerSprite = rightSprite; }


        //정답!!
        if (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite)
        {
            score += 10;
            score_txt.text = score.ToString();

            //정답 효과//////////////////////////피드백필요////////////////////////////////////
            correctTxt.GetComponent<Text>().color=new Color(1f,1f,1f,1f);
            StartCoroutine("FadeCoroutine");
        }
        //오답!!
        else
        {
            scrollbar.size -= size * 10;
            time -= 10;

            //화면 흔들기?////////////////////////////////

        }
        //좌우 이동==========================================
        StartCoroutine(MoveCoroutine(isLeft, content.transform.GetChild(content.transform.childCount - 1).gameObject));




        //+)갯수 늘릴것인지?. 좌우 변경하기?/////////////////////////




        //updateContent 스프라이트가 업데이트=================================

        //맨 앞에 있는 스프라이트를 뒤로 뒤로 이동시킨다.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();

        //방금 막 맨 뒤로 간 스프라이트를 새로운 스프라이트로 변경한다.
        int ran = UnityEngine.Random.Range(0, 4);
        if (ran == 0 || ran == 1)
            content.transform.GetChild(0).GetComponent<Image>().sprite = leftSprite;
        else
            content.transform.GetChild(0).GetComponent<Image>().sprite = rightSprite;
    }

    protected override void FinishGame()
    {
        base.FinishGame();

        //최고기록 저장
        if (DataController.instance.gameData.highScore[3] < score)
        {
            DataController.instance.gameData.highScore[3] = score;
        }

        //결과패널
        resultPanel.SetActive(true);
        result_txt.text = "최고기록 : " + DataController.instance.gameData.highScore[3] + "\n현재점수 : " + score;

        // 미니게임 3 보상 하트 공식(미니게임 3은 해금 하트가 20이다)
        float gain_coin = score * research_level_avg * ((100 + 80 * 2) / 100f);

        Debug.Log("얻은 코인:" + Convert.ToInt32(gain_coin));
        //하트지급
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        base.StopGame();
    }


    IEnumerator FadeCoroutine()
    {
        float fadeCount = 1;
        while (fadeCount > -0.1f) 
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, fadeCount);
        }
    }
    IEnumerator MoveCoroutine(bool isLeft,GameObject content)
    {
        fakeImage.GetComponent<Image>().sprite = content.GetComponent<Image>().sprite;
        fakeImage.GetComponent<Image>().preserveAspect = true;
        fakeImage.SetActive(true);

        Vector3 moveCount = content.GetComponent<RectTransform>().position;
        float fadeCount = 1;

        while (fadeCount > -0.1f)
        {
            //점점흐려짐
            fadeCount -= 0.05f;
            fakeImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, fadeCount);

            //점점이동함
            if (isLeft == true)
            { moveCount.x -= 0.05f; }
            else
            { moveCount.x += 0.05f; }
            fakeImage.GetComponent<RectTransform>().position = moveCount;

            yield return new WaitForSeconds(0.01f);

           
        }
        fakeImage.SetActive(false);
        fakeImage.GetComponent<RectTransform>().position = new Vector3(50f, -597f, 0);
    }

}
