using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("close");
    }
}
