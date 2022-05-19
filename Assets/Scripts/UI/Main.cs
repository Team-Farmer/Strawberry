using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Image>().DOFade(0.0f, 3.5f);
        Invoke("ActiveOff", 3.0f);
        GameManager.instance.EnableObjColliderAll();
    }


    public void ActiveOff()
    {
        gameObject.SetActive(false);
    }

}
