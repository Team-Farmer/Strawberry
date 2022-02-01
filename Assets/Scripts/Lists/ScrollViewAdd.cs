using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject _elementPrefab = null;

    [SerializeField]
    private Transform _content = null;

    private void Start()
    {
        for (int i = 0; i < 7; i++) {
            AddElement();
        }
    }


    public void AddElement() {

        var instance=Instantiate(_elementPrefab);//해당 프리팹을 인스턴스화해서 만든다.
        instance.transform.SetParent(_content);//부모를 content로 한다. 그 안으로 들어간다.
        
        
    }
}
