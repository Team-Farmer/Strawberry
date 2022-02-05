using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    private Animator SettingBtnAnimation;

    private void Awake()
    {
        SettingBtnAnimation = GetComponent<Animator>();
    }

    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        SettingBtnAnimation.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        SettingBtnAnimation.ResetTrigger("close");
    }
}
