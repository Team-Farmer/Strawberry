using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//프리팹
    
    [SerializeField]
    private Transform[] content = null;//프리팹이 들어갈 콘텐트

    [SerializeField]
    private int count=0;//프리팹 갯수

    [SerializeField]
    private bool isBerry;

    //===================================================================================
    private void Start()
    {
        //베리
        if (isBerry == true) 
        {
            for (int t = 0; t < 3; t++)//classic, special unique 3개
            {
                for (int j = 0; j < content.Length; j++)//layer 갯수만큼
                {
                    for (int i = 0; i < 16; i++)//content 내 갯수만큼
                    {
                        AddElement(content[j]);
                    }
                }
            }
        }
        //그 외
        else
        {
            for (int i = 0; i < count; i++)
            {
                AddElement(content[0]);
            }
        }

    }

    public void AddElement(Transform content) 
    {
        var instance = Instantiate(elementPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(content);//부모를 content로 한다. 그 안으로 들어간다.
    }

}
