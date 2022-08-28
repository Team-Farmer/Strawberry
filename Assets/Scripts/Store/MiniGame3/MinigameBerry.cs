using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBerry : MonoBehaviour
{
    [Header("Berry State")]

    public float velocity = 0.0f;
    private RectTransform rect;
    float berry_rad = 90f;
    private float berry_src_speed;
    private float berry_dst_speed;
    private int minigame_3_score;

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

        minigame_3_score = this.gameObject.transform.GetComponentInParent<MiniGame3>().score;

        if (minigame_3_score < 100) // 점수에 따라 딸기 낙하속도 변경
        {
            berry_src_speed = 10.0f;
            berry_dst_speed = 20.0f;
        }
        else
        {
            berry_src_speed = 15.0f;
            berry_dst_speed = 25.0f;
        }

        velocity = Random.Range(berry_src_speed, berry_dst_speed);
        // 어떤 스프라이트 쓸 건지?
    }
    private void OnDisable() // 딸기 정보들 초기화
    {
        rect.anchoredPosition = Vector3.zero;
        velocity = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {       
        if(MiniGame.isGameRunning)
        {
            float berry_x = rect.anchoredPosition.x + berry_rad, berry_y = rect.anchoredPosition.y + berry_rad;                    
            float basket_x = basketRect.anchoredPosition.x + 150f, basket_y = basketRect.anchoredPosition.y + 75f;
            float dist = (berry_x - basket_x) * (berry_x - basket_x) + (berry_y - basket_y) * (berry_y - basket_y);
            
            float r = berry_rad + basket_rad;
           
            // 딸기가 바구니에 닿았을 경우
            if (dist <= r * r)
            {
                // 벌레붙은 딸기를 먹었을 시
                if(this.gameObject.transform.GetChild(0).gameObject.activeSelf)
                {
                    // 시간 감소
                    float size = this.gameObject.transform.GetComponentInParent<MiniGame3>().size;

                    this.gameObject.transform.GetComponentInParent<MiniGame3>().scroll.fillAmount -= size * 10;
                    this.gameObject.transform.GetComponentInParent<MiniGame3>().time -= 10;
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    AudioManager.instance.WrongAudioPlay();
                }
                else
                {
                    this.gameObject.transform.GetComponentInParent<MiniGame3>().score += 5;                                                    
                    this.gameObject.transform.GetComponentInParent<MiniGame3>().score_txt.text
                        = this.gameObject.transform.GetComponentInParent<MiniGame3>().score.ToString();
                    AudioManager.instance.Cute1AudioPlay();
                }

                this.gameObject.SetActive(false);
                return;
            }
            else if (berry_y <= 75f)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                return;
            }
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y - velocity);           
        }
    }
}
