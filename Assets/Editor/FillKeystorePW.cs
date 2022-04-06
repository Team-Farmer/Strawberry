using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class FillKeystorePW
{
#if UNITY_EDITOR
    static FillKeystorePW()
    {
        PlayerSettings.Android.keystorePass = "ttalgi0118";
        PlayerSettings.Android.keyaliasName = "teamfarmer";
        PlayerSettings.Android.keyaliasPass = "ttalgi0118";
    }
#endif
}
