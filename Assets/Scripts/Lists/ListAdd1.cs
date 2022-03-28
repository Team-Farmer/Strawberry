using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd1 : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//프리팹

    [SerializeField]
    private Transform content1 = null;//프리팹이 들어갈 콘텐트
    [SerializeField]
    private Transform content2 = null;

    [SerializeField]
    private int count = 0;//프리팹 갯수



    //===================================================================================
    private void Start()
    {
        for (int t = 0; t < 3; t++)
        {
            for (int i = 0; i < 16; i++)
            {
                AddElement(content1);
            }
            for (int i = 0; i < 16; i++)
            {
                AddElement(content2);
            }
        }

    }

    public void AddElement(Transform content)
    {
        var instance = Instantiate(elementPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(content);//부모를 content로 한다. 그 안으로 들어간다.
    }

}
