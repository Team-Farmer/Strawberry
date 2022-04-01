using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainCtrl : MonoBehaviour
{
    ParticleSystem rainParticle;
    public float rainPeriod;
    public int mult;
    public float rainTime;
    
    void Awake()
    {
        rainParticle = GetComponent<ParticleSystem>();       
    }
    void Update()
    {
        rainTime += Time.deltaTime;
        if(rainTime >= rainPeriod)
        {
            Raining();
            StartCoroutine(RainingRoutine());
        }
    }
    void Raining()
    {
        mult = 3;
        rainTime = 0;
        rainParticle.Play();
        
        //Color bgColor = GameObject.FindGameObjectWithTag("BG").GetComponent<Image>().color;
        //bgColor.a = 200/255f;
        //Debug.Log(bgColor.a);
        //GameObject.FindGameObjectWithTag("BG").GetComponent<Image>().color = bgColor;
    }
    IEnumerator RainingRoutine()
    {        
        yield return new WaitForSeconds(rainParticle.main.duration);
        mult = 1;
    }
}
