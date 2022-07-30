using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public GameObject readText;
    public static Blink instance;

    void Awake()
    {
        if (Blink.instance == null)
            Blink.instance = this;
    }
    // Use this for initialization
    void Start()
    {
        readText.SetActive(false);
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        yield return new WaitForSeconds(2f);
        while (count < 2)
        {
            readText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            readText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
        readText.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}