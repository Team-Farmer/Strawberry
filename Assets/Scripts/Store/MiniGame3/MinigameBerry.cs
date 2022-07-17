using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBerry : MonoBehaviour
{
    [Header("Berry State")]

    public float velocity = 10.0f;
    private RectTransform rect;
    float berry_rad = 85f;

    [Header("Basket")]
    float basket_rad = 75f;
    public RectTransform basketRect;

    [Header("BackGround")]
    public RectTransform bgRect;

    // Start is called before the first frame update
    void Awake()
    {
        rect = this.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        float rectXPos = Random.Range(100f, 950f);
        rect.anchoredPosition = new Vector3(rectXPos, bgRect.rect.height - 80f);
        // 어떤 스프라이트 쓸 건지?
    }
    private void OnDisable() // 딸기 정보들 초기화
    {
        rect.anchoredPosition = Vector3.zero;
        velocity = 10.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if(MiniGame.isGameRunning)
        {
            float berry_x = rect.anchoredPosition.x, berry_y = rect.anchoredPosition.y;                    
            float basket_x = basketRect.anchoredPosition.x, basket_y = basketRect.anchoredPosition.y;
            float dist = (berry_x - basket_x) * (berry_x - basket_x) + (berry_y - basket_y) * (berry_y - basket_y);
            
            float r = berry_rad + basket_rad;
            
            if (dist <= r * r || berry_y <= 75f)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                return;
            }
            
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y - velocity);           
        }
    }
}
