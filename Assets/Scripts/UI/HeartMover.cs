using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class HeartMover : MonoBehaviour
{
    public HeartAcquireFx prefab;
    public GameObject toObject;

    private int days;

    [SerializeField] RectTransform point;



    public void AttHeartMover(int num)
    {
        for (int i = 0; i < 10; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
            itemFx.Explosion(toObject.transform.position, point.transform.position, 120.0f);
        }

    }

    public void RewardMover(string name)
    {
        int num = 0;
        if (name == "coin")
            num = 10;
        else if (name == "heart")
            num = 5;
        else if (name == "medal")
            num = 1;

        for (int i = 0; i < num; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
            itemFx.Explosion2(toObject.transform.position + new Vector3(-50, 0, 0), 120.0f);
        }

    }


    public void CountCoin(float dis)
    {
        var itemFx2 = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
        itemFx2.Coin(toObject.transform.position, dis);
    }
}
