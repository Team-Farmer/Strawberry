using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    int tutoIndex=-1;
    [SerializeField] RectTransform txBox_transform;
    [SerializeField] Text tutorial_tx;
    [SerializeField] Button left_btn;
    [SerializeField] Button right_btn;
    [SerializeField] Button end_btn;
    [TextArea][SerializeField] string[] tuto_tx;
    [SerializeField] Image tuto_img;
    [SerializeField] Sprite[] tuto_sprite;

    [SerializeField] BannerAd bannerAd;

    void Start()
    {
        left_btn.onClick.AddListener(OnClickLeftBtn);
        right_btn.onClick.AddListener(OnClickRightBtn);
        end_btn.onClick.AddListener(OnClickEndBtn);
    }

    void OnEnable()
    {
        OnClickRightBtn();   
    }

    void OnClickLeftBtn()
    {
        tutoIndex--;
        ChangeIndex();
    }

    void OnClickRightBtn()
    {
        tutoIndex++;
        ChangeIndex();
    }

    void ChangeIndex()
    {
        left_btn.gameObject.SetActive(tutoIndex != 0);
        right_btn.gameObject.SetActive(tutoIndex != 9);
        end_btn.gameObject.SetActive(tutoIndex == 9);

        if(tutoIndex>=4 && tutoIndex < 7)
        {
            txBox_transform.anchoredPosition = new Vector2(txBox_transform.localPosition.x, 640f);
        }
        else
        {
            txBox_transform.anchoredPosition = new Vector2(txBox_transform.localPosition.x, 238f);
        }

        tutorial_tx.text = tuto_tx[tutoIndex];
        tuto_img.sprite = tuto_sprite[tutoIndex];
    }

    void OnClickEndBtn()
    {
        DataController.instance.gameData.isTutorialDone = true;
        AudioManager.instance.ResumePlayAudio("RainSFXSound");
        GameManager.instance.EnableObjColliderAll();
        bannerAd.ShowBanner();

        //½Ã¿ø Æ©Åä¸®¾ó ³¡³ª°í ÀçÈ­È¹µæ
        GameManager.instance.StartTutorialRewardCo();
    }



}
