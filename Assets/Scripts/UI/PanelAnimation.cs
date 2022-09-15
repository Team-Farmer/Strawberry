using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public int UpPosition;
    public int DownPosition;
    public Text ScriptTxt;


    #region 애니메이션

    public void OpenUP()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(UpPosition, 0.3f);
    }

    public void CloseUp()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(-1940, 0.2f);
        Invoke("ActiveOff", 0.3f);
    }

    public void OpenDown()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(DownPosition, 0.3f);
    }

    public void CloseDown()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(1870, 0.3f);
        Invoke("ActiveOff", 0.3f);
    }

    public void FadeOut()
    {
        gameObject.GetComponent<Image>().DOFade(0.0f, 0.3f);
        Invoke("ActiveOff", 0.3f);
    }

    public void Fadein()
    {
        gameObject.GetComponent<Image>().DOFade(0.5f, 0.3f);
    }

    public void ActiveOff()
    {
        gameObject.SetActive(false);
    }

    public void OpenScale()
    {
        gameObject.transform.DOScale(Vector3.zero, 0f);
        gameObject.SetActive(true);
        gameObject.transform.DOScale(Vector3.one, 0.3f);
    }

    public void CloseScale()
    {
        gameObject.transform.DOScale(Vector3.zero, 0.3f);
        Invoke("ActiveOff", 0.2f);
    }

    #endregion

    public void ChangeTextCloudLoad()
    {
        ScriptTxt.text = "저장된 내용을 불러올까요?";
    }

    public void ChangeTextCloudSave()
    {
        ScriptTxt.text = "진행사항을 저장할까요?";
    }

    public void CloudConfirm()
    {
        ScriptTxt.text = "저장을 완료했습니다!";
    }
}
