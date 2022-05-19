using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("[ Intro ]")]
    public GameObject[] introObject=new GameObject[3];
    [Header("[ Main ]")]
    public GameObject[] mainObject=new GameObject[1];
    public static Intro instance;


    void Awake()
    {
        if (Intro.instance == null)
            Intro.instance = this;
    }
    // Use this for initialization
    void Start()
    {
        introObject[0].SetActive(false);
        StartCoroutine(ShowReady());

        Invoke("FadeIn", 0.5f); 
        Invoke("ActiveOff", 2.0f);
        Invoke("ActiveOn", 3.0f); 
 
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 4)
        {
            introObject[0].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            introObject[0].SetActive(false);
            yield return new WaitForSeconds(0.3f);
            count++;
        }
    }

    public void FadeIn()
    {
        introObject[1].GetComponent<Image>().DOFade(1.0f, 2.0f);
    }

    public void ActiveOn()
    {
        mainObject[0].SetActive(true);
    }

    public void ActiveOff()
    {
        introObject[2].SetActive(false);
    }




}
