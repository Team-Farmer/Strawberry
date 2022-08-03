using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject content;


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
        //왼쪽과 오른쪽에 해당하는 정답을 고른다.
        int leftOne;
        int rightOne;

        leftOne = Random.Range(0, 10);
        do { rightOne = Random.Range(0, 10); } while (leftOne == rightOne);

        leftSprite = berryListAll[leftOne].GetComponent<SpriteRenderer>().sprite;
        rightSprite = berryListAll[rightOne].GetComponent<SpriteRenderer>().sprite;

        
        //해당 정답들을 보인다.
        leftImage.GetComponent<Image>().sprite = leftSprite;
        rightImage.GetComponent<Image>().sprite = rightSprite;

        leftImage.GetComponent<Image>().preserveAspect = true;
        rightImage.GetComponent<Image>().preserveAspect = true;


        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = leftSprite; }
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = rightSprite; }

            content.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;

        }

    }


    public void clickAnswer(bool isLeft) 
    {

        //정답여부 판별=======================================
        Sprite answerSprite;
        if (isLeft == true) { answerSprite = leftSprite; }
        else { answerSprite = rightSprite; }


        //정답!!
        if (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite)
        {
            score += 10;
            score_txt.text = score.ToString();
            //정답 효과///////////////////////////////////
        }
        //오답!!
        else
        {
            scrollbar.size -= size * 10;
            time -= 10;
            //화면 흔들기?////////////////////////////////
            
        }
        //좌우 이동==========================================
        //왼쪽 혹은 오른쪽으로 이동///////////////////////////////////

        //+)갯수 늘릴것인지?. 좌우 변경하기?/////////////////////////

        //updateContent 스프라이트가 업데이트 된다.=================================

        //맨 앞에 있는 스프라이트를 뒤로 뒤로 이동시킨다.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();

        //방금 막 맨 뒤로 간 스프라이트를 새로운 스프라이트로 변경한다.
        int ran = Random.Range(0, 4);
        if (ran == 0 || ran == 1)
            content.transform.GetChild(0).GetComponent<Image>().sprite = leftSprite;
        else
            content.transform.GetChild(0).GetComponent<Image>().sprite = rightSprite;
    }

}
