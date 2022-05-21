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
    public int mult = 1;
    public float rainTime;
    private bool isRaining;
    void Start()
    {
        rainParticle = GetComponent<ParticleSystem>();
        arbeit = GameObject.FindGameObjectWithTag("Arbeit").GetComponent<ArbeitMgr>();

        var main = rainParticle.main;
        main.duration = DataController.instance.gameData.rainDuration;
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
        Debug.Log("mult: " + mult);
        rainTime = 0;
      
        Debug.Log("duration: " + rainParticle.main.duration);
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
