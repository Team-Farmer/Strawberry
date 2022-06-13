using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame3 : MiniGame
{
    [Header("MiniGame3")]
    public GameObject basket;
    private RectTransform basketRect;
    public RectTransform bgRect;

    
    private bool isDrag;
    float accTime, randTime;
    public RectTransform berryGroup;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isGameRunning = true;
        basketRect = basket.GetComponent<RectTransform>();
        randTime = Random.Range(4.5f, 6.0f);
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
            basketRect.anchoredPosition = Vector3.Lerp(basketRect.anchoredPosition, mousePos, 0.2f);
        }
        accTime += Time.deltaTime;

        if(accTime >= randTime)
        {
            //MakeMiniGame3Berry();
            accTime = 0f;
            randTime = Random.Range(1.5f, 2.0f);
        }
    }
    public override void OnClickPauseButton()
    {
        base.OnClickPauseButton();
        isGameRunning = false;
    }
    public override void OnClickKeepGoingButton()
    {
        base.OnClickKeepGoingButton();
        isGameRunning = true;
    }
}
