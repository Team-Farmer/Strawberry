using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    RectTransform parent; //하이어라키 상 부모의 위치 정보 가져옴

    public static ObjectPool Instance; // 싱글톤

    [SerializeField]
    private HeartAcquireFx poolingObjectPrefab; //미리 생성될 프리팹

    Queue<HeartAcquireFx> poolingObjectQueue = new Queue<HeartAcquireFx>(); //큐 생성

    void Awake()
    {
        Instance = this;
        Initialize(10); //오브젝트 10개 미리 생성
    }


    #region 텍스트 애니메이션 풀링

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); //10번 Enqueue
        }
    }

    private HeartAcquireFx CreateNewObject()
    {
        var newObj = GameObject.Instantiate<HeartAcquireFx>(poolingObjectPrefab, transform.position,Quaternion.identity, GameObject.Find("MainGame").transform);
        //재화 이벤트 실행 후 오브젝트가 생성될 위치 정보(MainGame UI 캔버스 안에다가)를 Instantiate 만들 때 넣어줬음 
        newObj.gameObject.SetActive(false); //미리 생성되어 대기하고 있는 애들 SetActive 꺼주고
        newObj.transform.SetParent(transform); //스크립트 붙여져있는 ObjectPool 자식으로 넣어줌
        return newObj; //그리고 Queue에 넣게 반환
    }

    public static HeartAcquireFx GetObject() // 나 미리 만든거 가져다가 쓴다!
    {
        if (Instance.poolingObjectQueue.Count > 0) // 미리 생성된게 안부족하면
        {
            var obj = Instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.gameObject.SetActive(true); // 미리 생성되어있는거 ON
            obj.transform.SetParent(Instance.parent); //static 이라 Instance.parent /하이어라키 상 ObjectPool 이라는 오브젝트의 상위에 출현하게 부모를 변경
            return obj;
        }
        else // 부족하면
        {
            var newObj = Instance.CreateNewObject(); // 하나 새로 만들어서
            newObj.gameObject.SetActive(true); // 밑에는 위와 같음
            newObj.transform.SetParent(Instance.parent); 
            return newObj;
        }
    }

    public static void ReturnObject(HeartAcquireFx obj) //썼던 거 다시 반환
     {
        obj.gameObject.SetActive(false); //끄고
        obj.transform.SetParent(Instance.transform); // 다시 원래 부모로 돌아와서 Object Pool 자식으로 만듬
        Instance.poolingObjectQueue.Enqueue(obj); // 그리고 다시 Enqueue 삽입
    }

    //저 부모 왔다갔다 하는걸 왜 했냐? -> 그냥 오브젝트 풀링하면 캔버스 밖에다가 생성되어서 화면 상 표시가 안된다. 즉 UI라서 해줬다. 
    //사운드는 아마 부모 변경하고 되돌아오는거 나처럼 안해도 될테니 보내준 링크 참고!

    #endregion
}
