using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Dotori : MonoBehaviour
{
    [SerializeField] int duration;
    [SerializeField] MiniGameManager minigameManager;
    [SerializeField] GameObject info;
    
    Sequence mySequence;
    void Start()
    {
        DataController.instance.gameData.dotori--;
        GameManager.instance.invokeDotori();
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //Ãß°¡
        .OnStart(() =>
        {
            transform.localScale = Vector3.one;
        })
        .Append(transform.DOMove(Vector3.zero, 0.7f).SetEase(Ease.InBack))
        .InsertCallback(0.6f, () => AudioManager.instance.Cute1AudioPlay())
        .Append(transform.DOScale(7, 0.7f))
        .Join(transform.GetComponent<Image>().DOFade(0, 1f))
        .OnComplete(() =>
         {
             minigameManager.OnclickStartBtn();
             info.SetActive(false);
             gameObject.SetActive(false);
         });
    }

    void OnEnable()
    {
        mySequence.Restart();
    }
}
