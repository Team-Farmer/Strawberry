using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{

    void Start()
    {

    }

    //근데 update안ㅆ구하느방벙은 없나..
    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying == false)
        { AudioManager.ReturnObject(this); }
    }
}
