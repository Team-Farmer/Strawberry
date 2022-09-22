using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainCtrl : MonoBehaviour
{
    public GameObject rainPanel;
    public GameObject truckPanel;
    public GameObject storePanel;

    private ParticleSystem rainParticle;
    private ArbeitMgr arbeit;
    private float rainPeriod = 10f;//240
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
        if (isRaining) { return; }

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

        Color color = new Color(0.6f, 0.6f, 0.6f);

        truckPanel.GetComponent<Image>().DOColor(color, 0.3f);
        storePanel.GetComponent<Image>().DOColor(color, 0.3f);

        mult = arbeit.lluvia();
        //Debug.Log("mult: " + mult);
        rainTime = 0;
      
        //Debug.Log("duration: " + rainParticle.main.duration);
        rainParticle.Play();

        //예림 sound
        AudioManager.instance.RainAudioPlay();
        if (GameManager.instance.isMiniGameMode||Blink.instance.gameObject.activeSelf)
        {
            AudioManager.instance.PauseAudio("RainSFXSound");
        }
    }

    IEnumerator RainingRoutine()
    {        
        yield return new WaitForSeconds(rainParticle.main.duration);

        rainPanel.GetComponent<PanelAnimation>().FadeOut();

        Color color = new Color(1f, 1f, 1f);

        truckPanel.GetComponent<Image>().DOColor(color, 0.3f);
        storePanel.GetComponent<Image>().DOColor(color, 0.3f);


        mult = 1;
        isRaining = false;

        //예림 sound
        AudioManager.instance.StopPlayAudio("RainSFXSound");
    }
}
