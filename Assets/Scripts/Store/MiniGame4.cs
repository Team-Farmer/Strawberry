using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject leftImage;
    public GameObject leftImage2;
    public GameObject rightImage;
    public GameObject rightImage2;

    public GameObject content;
    public GameObject correctTxt;
    public GameObject fakeImage;
    public GameObject leftBtn;
    public GameObject rightBtn;

    Sprite leftSprite1;
    Sprite rightSprite1;
    Sprite leftSprite2;
    Sprite rightSprite2;

    int leftOne;
    int rightOne;
    int leftOne2;
    int rightOne2;

    bool isUpgrade;

    List<GameObject> berryListAll;//global의 berryListAll

    protected override void Awake()
    {

        berryListAll = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        base.Awake();
        
    }
    protected override void OnEnable()
    {

        base.OnEnable();
        leftBtn.GetComponent<Button>().interactable = false;
        rightBtn.GetComponent<Button>().interactable = false;
        SetGame(ref leftOne,ref rightOne, ref leftSprite1,ref rightSprite1,ref leftImage,ref rightImage);
        
        //upgrade초기화
        isUpgrade = false;
        leftImage2.SetActive(false);
        rightImage2.SetActive(false);
    }
    protected override void MakeGame()
    {
        leftBtn.GetComponent<Button>().interactable = true;
        rightBtn.GetComponent<Button>().interactable = true;
    }

    void SetGame
        (ref int leftOne,ref int rightOne, 
        ref Sprite leftSprite,ref Sprite rightSprite,
        ref GameObject leftImage,ref GameObject rightImage) 
    {

        //왼쪽과 오른쪽 정답 설정
        leftOne = UnityEngine.Random.Range(0, 192);
        rightOne = UnityEngine.Random.Range(0, 192);
        do { leftOne = UnityEngine.Random.Range(0, 192); } 
        while (DataController.instance.gameData.isBerryUnlock[leftOne] == false);
        
        do { rightOne = UnityEngine.Random.Range(0, 192); } 
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
        //애니메이션 효과 초기화
        StopCoroutine("FadeCoroutine");
        correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);

        StopCoroutine(MoveCoroutine(true, content.transform.GetChild(content.transform.childCount - 1).gameObject));
        fakeImage.SetActive(false);
        fakeImage.GetComponent<RectTransform>().position = new Vector3(50f,-597f,0);
        

        //정답여부 판별=======================================
        Sprite answerSprite;
        if (isLeft == true) { answerSprite = leftSprite1; }
        else { answerSprite = rightSprite1; }


        //정답!!
        if (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite)
        {
            score += 10;
            score_txt.text = score.ToString();

            //정답 효과//////////////////////////피드백필요
            correctTxt.GetComponent<Text>().color=new Color(1f,1f,1f,1f);
            StartCoroutine("FadeCoroutine");
        }
        //오답!!
        else
        {
            scroll.fillAmount -= size * 10;
            time -= 10;
        }
        //좌우 이동==========================================
        StartCoroutine(MoveCoroutine(isLeft, content.transform.GetChild(content.transform.childCount - 1).gameObject));




        // UPGRADE!! 갯수 늘리기
        if (score > 300 && isUpgrade==false)
        { 
            SetGame(ref leftOne2, ref rightOne2, ref leftSprite2, ref rightSprite2, ref leftImage2, ref rightImage2);
            isUpgrade = true;
            leftImage2.SetActive(true);
            rightImage2.SetActive(true);
        }
        




        //updateContent 스프라이트가 업데이트=================================

        //맨 앞에 있는 스프라이트를 뒤로 뒤로 이동시킨다.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();

        //방금 막 맨 뒤로 간 스프라이트를 새로운 스프라이트로 변경한다.
        int ran = UnityEngine.Random.Range(0, 4);
        if (ran == 0 || ran == 1)
            content.transform.GetChild(0).GetComponent<Image>().sprite = leftSprite1;
        else
            content.transform.GetChild(0).GetComponent<Image>().sprite = rightSprite1;
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
