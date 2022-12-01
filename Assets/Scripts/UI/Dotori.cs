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
    [SerializeField] GameObject text;
    
    Sequence mySequence;
    void Start()
    {

        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //Ãß°¡
        .OnStart(() =>
        {
            transform.localScale = Vector3.one;
        })
        .Append(transform.DOMove(transform.position+new Vector3(0,0.38f,0), 0.5f))
        .Join(transform.GetComponent<Image>().DOFade(0, 0.9f))
        .Join(text.GetComponent<Text>().DOFade(0, 0.9f))
        .OnComplete(() =>
         {
             GameManager.instance.invokeDotori();
             gameObject.SetActive(false);
             minigameManager.OnclickStartBtn();
             info.SetActive(false);
         });
    }

    void OnEnable()
    {
        GameManager.instance.UseDotori();
        GameManager.instance.invokeDotori();
        mySequence.Restart();
    }
}
