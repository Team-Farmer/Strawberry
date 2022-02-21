using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//프리팹
    
    [SerializeField]
    private Transform content1 = null;//프리팹이 들어갈 콘텐트
    [SerializeField]
    private Transform content2 = null;

    [SerializeField]
    private int count=0;//프리팹 갯수

    [SerializeField]
    private bool isBerry;//베리 리스트인가?

    private void Start()
    {
        if (isBerry == true)
        {
            for (int i = 0; i < count/2; i++)
            {
                AddElement(content1);
                
            }
            for (int i = 0; i < count / 2; i++)
            {
                AddElement(content2);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                AddElement(content1);
            }
        }
    }

    public void AddElement(Transform content) 
    {
        var instance = Instantiate(elementPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(content);//부모를 content로 한다. 그 안으로 들어간다.
    }

}
