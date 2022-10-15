using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("object pooling Sound Prefab");


    }

    
    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying == false)
        { AudioManager.ReturnObject(this); }
    }
}
