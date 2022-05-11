using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainCtrl : MonoBehaviour
{
    public GameObject rainPanel;

    private ParticleSystem rainParticle;
    private ArbeitMgr arbeit;
    public float rainPeriod;
    public int mult;
    public float rainTime;
    private bool isRaining;
    void Awake()
    {
        rainParticle = GetComponent<ParticleSystem>();
        arbeit = GameObject.FindGameObjectWithTag("Arbeit").GetComponent<ArbeitMgr>();
    }
    void Update()
    {
        if (isRaining) return;
        rainTime += Time.deltaTime;
        if(rainTime >= rainPeriod)
        {
            isRaining = true;
            Raining();
            StartCoroutine(RainingRoutine());
        }
    }
    void Raining()
    {       
        rainPanel.gameObject.SetActive(true);
        rainPanel.GetComponent<Image>().DOFade(0.3f, 0.3f);        
        mult = arbeit.lluvia();
        Debug.Log(mult);
        rainTime = 0;
        rainParticle.Play();            
    }
    IEnumerator RainingRoutine()
    {        
        yield return new WaitForSeconds(rainParticle.main.duration);
        rainPanel.GetComponent<PanelAnimation>().FadeOut();
        mult = 1;
        isRaining = false;
    }
}
