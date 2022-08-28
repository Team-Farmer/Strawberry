using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject[] correctImage;

    public GameObject content;
    public GameObject correctTxt;//정답시 나오는 텍스트
    public GameObject fakeImage;//애니메이션용 이미지
    public GameObject leftBtn;
    public GameObject rightBtn;


    int[] correctNum;
    Sprite[] correctSprite;
    Sprite[] answerSprite;

    

    bool isUpgrade;

    List<GameObject> berryListAll;//global의 berryListAll

    //0:좌 아래    1:우 아래    2:좌 위    3:우 위

    protected override void Awake()
    {

        berryListAll = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;
        
        correctNum = new int[4];
        correctSprite = new Sprite[4];
        answerSprite = new Sprite[4];

        base.Awake();
        
    }
    protected override void OnEnable()
    {

        base.OnEnable();
        leftBtn.GetComponent<Button>().interactable = false;
        rightBtn.GetComponent<Button>().interactable = false;
        SetGame();
        
        //upgrade초기화
        isUpgrade = false;
        correctImage[2].SetActive(false);
        correctImage[3].SetActive(false);
    }
    protected override void MakeGame()
    {
        leftBtn.GetComponent<Button>().interactable = true;
        rightBtn.GetComponent<Button>().interactable = true;
    }

    void SetGame()
    {

        //왼쪽과 오른쪽 정답을 랜덤으로 정하자(랜덤수 4개 설정)
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                do { correctNum[i] = UnityEngine.Random.Range(0, 192); }
                while(DataController.instance.gameData.isBerryUnlock[correctNum[i]] == false);
            }


            correctNum = correctNum.Distinct().ToArray();//배열 중복 삭제하는 명령어
            if (correctNum.Length == 4) { break; }

        }



        for (int i = 0; i < 4; i++)
        {
                //정답 베리 스프라이트 
                correctSprite[i]= berryListAll[correctNum[i]].GetComponent<SpriteRenderer>().sprite;


            //정답을 보인다.
            correctImage[i].GetComponent<Image>().sprite = correctSprite[i];
            correctImage[i].GetComponent<Image>().preserveAspect = true;
        }




        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = UnityEngine.Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = correctSprite[0]; }//left
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = correctSprite[1]; }//right

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
        if (isUpgrade == true) //정답이4개일때
        {
            if (isLeft == true) //left
            {
                answerSprite[0] = correctSprite[0];
                answerSprite[1] = correctSprite[2];
            }
            else //right
            { 
                answerSprite[0] = correctSprite[1];
                answerSprite[1] = correctSprite[3];
            }
        }
        else
        {
            if (isLeft == true) { answerSprite[0] = correctSprite[0]; }
            else { answerSprite[0] = correctSprite[1]; }
        }
       


        //정답!!
        if (isUpgrade==false && (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[0])||
            isUpgrade == true && 
            (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[0]|| 
            content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[1])
            )
        {
            score += 5;
            score_txt.text = score.ToString();

            //정답 효과
            correctTxt.GetComponent<Text>().color=new Color(1f,1f,1f,1f);
            StartCoroutine("FadeCoroutine");
            AudioManager.instance.Cute1AudioPlay();
        }
        //오답!!
        else
        {
            scroll.fillAmount -= size * 10;
            time -= 10;
            AudioManager.instance.Cute5AudioPlay();
        }

        //좌우 이동==========================================
        StartCoroutine(MoveCoroutine(isLeft, content.transform.GetChild(content.transform.childCount - 1).gameObject));




        // UPGRADE!! 갯수 늘리기
        if (score > 300 && isUpgrade==false)
        { 
            isUpgrade = true;
            correctImage[2].SetActive(true);
            correctImage[3].SetActive(true);
        }
        




        //updateContent 스프라이트가 업데이트=================================

        //맨 앞에 있는 스프라이트를 뒤로 뒤로 이동시킨다.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();


        //방금 막 맨 뒤로 간 스프라이트를 새로운 스프라이트로 변경한다.
        int ran = UnityEngine.Random.Range(0, 4);
        if (isUpgrade == true)
        {
            switch (ran)
            {
                case 0: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 1: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 2: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 3: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
            }

        }
        else 
        {
            if (ran == 0 || ran == 1)
                content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[0];
            else
                content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[1];
        }
        
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        AudioManager.instance.EndAudioPlay();

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
            fadeCount -= 0.05f;
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
