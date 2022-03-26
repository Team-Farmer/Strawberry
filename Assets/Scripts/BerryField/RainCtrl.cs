using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            mult = 3;
            rainTime = 0;
            rainParticle.Play();

            StartCoroutine(RainRoutine());
        }
    }
    IEnumerator RainRoutine()
    {
        yield return new WaitForSeconds(5f);
        mult = 1;
    }
}
