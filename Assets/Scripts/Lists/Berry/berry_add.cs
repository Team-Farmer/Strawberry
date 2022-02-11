using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class berry_add : MonoBehaviour
{
    [SerializeField]
    private GameObject berryPrefab = null;

    [SerializeField]
    private Transform _content = null;


    private void Start()
    {
        for (int i = 0; i < 32; i++) {
            AddElement();
        }
    }
    public void AddElement()
    {

        var instance = Instantiate(berryPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(_content);//부모를 content로 한다. 그 안으로 들어간다.


    }
}
