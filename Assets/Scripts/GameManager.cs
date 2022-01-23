using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("------------[ Object Pooling ]")]
    public GameObject Strawberry;
    public Transform StrawberryGroup;
    public List<StrawBerry> strawPool;
    
    int poolSize = 16;
    public int poolCursor;

    [Header("------------[ DOTWeen ]")]
    public Transform target;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        strawPool = new List<StrawBerry>();

        for(int i = 0; i < poolSize; i++)
        {
            MakeStrawBerry();
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StrawBerry st = GetStrawBerry();
            if(st != null)
                PlantStrawBerry(st);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Harvest();
        }
    }
    StrawBerry MakeStrawBerry()
    {
        GameObject instantStrawBerryObj = Instantiate(Strawberry, StrawberryGroup);
        instantStrawBerryObj.name = "StrawBerry " + strawPool.Count;
        StrawBerry instantStrawBerry = instantStrawBerryObj.GetComponent<StrawBerry>();

        instantStrawBerry.gameObject.SetActive(false);
        strawPool.Add(instantStrawBerry);

        return instantStrawBerry;
    }
    StrawBerry GetStrawBerry()
    {
        poolCursor = 0;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (!strawPool[poolCursor].gameObject.activeSelf)
            {
                return strawPool[poolCursor];
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
        return null;
    }
    void PlantStrawBerry(StrawBerry st)
    {
        st.gameObject.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        st.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    void Harvest()
    {
        poolCursor = 0;
        Vector2 pos;
        for (int i = 0; i < strawPool.Count; i++)
        {
            if (strawPool[poolCursor].gameObject.activeSelf && !strawPool[poolCursor].canGrow)
            {
                pos = strawPool[poolCursor].transform.position;
                strawPool[poolCursor].Explosion(pos, target.position, 0.5f);
            }
            poolCursor = (poolCursor + 1) % strawPool.Count;
        }
    }
}
