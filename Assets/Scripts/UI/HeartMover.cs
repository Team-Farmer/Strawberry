using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class HeartMover : MonoBehaviour
{
    public HeartAcquireFx prefab;

    private int days;

    [SerializeField]
    GameObject fromObject;

    [SerializeField]
    RectTransform toObject;

    public void AttHeartMover()
    {
        for (int i = 0; i < 10; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, fromObject.transform);
            itemFx.Explosion(fromObject.transform.position, toObject.transform.position, 120.0f);
        }
    }

    public void RewardMover(string name)
    {
        int num = 0;

        if (name == "coin") num = 10;
        else if (name == "heart") num = 5;
        else if (name == "medal") num = 1;

        for (int i = 0; i < num; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, fromObject.transform);
            itemFx.Explosion2(fromObject.transform.position, 120f);

        }
    }

    public void CountMoney(float distance, int cost, String text, int num)
    {
        var itemFx2 = ObjectPool.GetObject();
        itemFx2.Coin(fromObject.transform.position, distance, cost, text, num);
    }

}
