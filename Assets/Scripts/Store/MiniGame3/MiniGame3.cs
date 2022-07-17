using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame3 : MiniGame
{
    [Header("MiniGame3")]
    public GameObject basket;
    private RectTransform basketRect;
    public RectTransform bgRect;
    public GameObject miniGameBerryPref;
    public List<GameObject> berryPool = new List<GameObject>();
    
    private bool isDrag;
    float accTime, randTime;
    public RectTransform berryGroup;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        basketRect = basket.GetComponent<RectTransform>();     
        randTime = Random.Range(1.5f, 2.0f);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        basketRect.anchoredPosition = new Vector3(425f, 560f, 0f);
    }
    public void PointDown()
    {
        isDrag = true;
    }
    public void PointUp()
    {
        isDrag = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning) return;
        
        if (isDrag)
        {
            //Debug.Log("Dragging");
            
            Vector3 mousePos = Input.mousePosition;
            //Debug.Log("mousePos.x: " + mousePos.x + " mousePos.y: " + mousePos.y);
            
            float leftBorder = 0f;
            float rightBorder = bgRect.rect.width - basketRect.rect.width;

            //Debug.Log("leftBorder: " + leftBorder + " rightBorder: " + rightBorder);
            
            mousePos.y = 560;
            mousePos.z = 0;
            if (mousePos.x < leftBorder) mousePos.x = leftBorder;
            else if (mousePos.x > rightBorder) mousePos.x = rightBorder;
            else mousePos.x = mousePos.x - basketRect.rect.width / 2;

            basketRect.anchoredPosition = Vector3.Lerp(basketRect.anchoredPosition, mousePos, 0.2f);
        }
        accTime += Time.deltaTime;

        if(accTime >= randTime)
        {
            MinigameBerry miniBerry = GetMiniGameBerry();
            miniBerry.gameObject.SetActive(true);
            accTime = 0f;
            randTime = Random.Range(1.5f, 2.0f);
        }
    }
    MinigameBerry GetMiniGameBerry()
    {
        for (int i = 0; i < berryPool.Count; i++)
        {
            if (!berryPool[i].gameObject.activeSelf)
            {
                int rndId = unlockList[Random.Range(0, unlockList.Count)];
                berryPool[i].GetComponent<Image>().sprite = global.berryListAll[rndId].GetComponent<SpriteRenderer>().sprite;

                float bugrnd = Random.Range(0f, 10f);
                if(bugrnd <= 2f)
                {
                    Debug.Log("Bug!!");
                    berryPool[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                return berryPool[i].GetComponent<MinigameBerry>();
            }
        }
        return MakeMiniGameBerry(); // 비활성화된 딸기가 없다면 새로 만든다.
    }
    MinigameBerry MakeMiniGameBerry()
    {
        GameObject instantMiniBerryObj = Instantiate(miniGameBerryPref, berryGroup);
        int rndId = unlockList[Random.Range(0, unlockList.Count)];

        instantMiniBerryObj.GetComponent<Image>().sprite = global.berryListAll[rndId].GetComponent<SpriteRenderer>().sprite;
        instantMiniBerryObj.name = "MiniBerry " + berryPool.Count;

        berryPool.Add(instantMiniBerryObj);

        MinigameBerry instantMiniBerry = instantMiniBerryObj.GetComponent<MinigameBerry>();
        instantMiniBerry.bgRect = bgRect;
        instantMiniBerry.basketRect = basketRect;

        instantMiniBerry.transform.GetChild(0).gameObject.SetActive(false);

        return instantMiniBerry;
    }
    public override void OnClickPauseButton()
    {
        base.OnClickPauseButton();        
    }
    public override void OnClickKeepGoingButton()
    {
        base.OnClickKeepGoingButton();     
    }

    public override void StopGame()
    {
        base.StopGame();
        for (int i = 0; i < berryPool.Count; i++)
        {
            berryPool[i].SetActive(false);
        }       
    }
}
