using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    public GameObject elementPrefab = null;//프리팹
    

    [SerializeField]
    private Transform content = null;//프리팹이 들어갈 콘텐트

    public int count = 0;//현재 프리팹 수를 알기위해


    public static ListAdd insatnce;
    public void Awake()
    {
        insatnce = this;
    }

    private void Start()
    {
        for (int i = 0; i < 7; i++) {
            AddElement();
        }
    }

    public void AddElement() 
    {
        var instance = Instantiate(elementPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(content);//부모를 content로 한다. 그 안으로 들어간다.
    }

}
