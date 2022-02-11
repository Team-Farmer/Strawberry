using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttandManager : MonoBehaviour
{
    static protected AttandManager s_AttandInstance;
    static public AttandManager AttandInstance { get { return s_AttandInstance; } }


    public bool[] AttandDay = new bool[31];
    // Use this for initialization
    void Awake()
    {
        s_AttandInstance = this;

        AttandDay[0] = true;
        AttandDay[3] = true;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
