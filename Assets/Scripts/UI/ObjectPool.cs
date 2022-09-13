using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    RectTransform parent;

    public static ObjectPool Instance;

    [SerializeField]
    private HeartAcquireFx poolingObjectPrefab;

    Queue<HeartAcquireFx> poolingObjectQueue = new Queue<HeartAcquireFx>();

    void Awake()
    {
        Instance = this;
        Initialize(10);
    }


    #region 텍스트 애니메이션 풀링

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private HeartAcquireFx CreateNewObject()
    {
        var newObj = GameObject.Instantiate<HeartAcquireFx>(poolingObjectPrefab, transform.position,Quaternion.identity, GameObject.Find("MainGame").transform);
        //와 UI 오브젝트 풀링 열받네
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static HeartAcquireFx GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(Instance.parent);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(Instance.parent);
            return newObj;
        }
    }

    public static void ReturnObject(HeartAcquireFx obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

    #endregion
}
