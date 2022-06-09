using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame3 : MiniGame
{
    [Header("MiniGame3")]
    public GameObject basket;
    private RectTransform basketRect;
    private bool isDrag;
    public RectTransform bgRect;
    // Start is called before the first frame update
    void Start()
    {
        basketRect = basket.GetComponent<RectTransform>();
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
        if (isDrag)
        {
            Debug.Log("Dragging");
            
            Vector3 mousePos = Input.mousePosition;
            Debug.Log("mousePos.x: " + mousePos.x + " mousePos.y: " + mousePos.y);
            //float leftBorder = -3.93f + transform.localScale.x / 2f;
            float leftBorder = 0f;
            float rightBorder = bgRect.rect.width - basketRect.rect.width;

            Debug.Log("leftBorder: " + leftBorder + " rightBorder: " + rightBorder);
            //mousePos.x = 800;
            mousePos.y = 560;
            mousePos.z = 0;
            if (mousePos.x < leftBorder) mousePos.x = leftBorder;
            else if (mousePos.x > rightBorder) mousePos.x = rightBorder;
            basketRect.anchoredPosition = Vector3.Lerp(basketRect.anchoredPosition, mousePos, 0.2f);
        }
    }
}
