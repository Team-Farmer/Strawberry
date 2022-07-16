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
    private bool isAtd;

    [SerializeField] RectTransform point;

    private void Start()
    {
        days = DataController.instance.gameData.accDays;
        isAtd = DataController.instance.gameData.isAttendance;
    }

    public void HeartMove(int num)
    {

        if (days > 6)
            days %= 6;

        if (isAtd == false && days == num)
        {
            //Random.Range(5, 10);
            for (int i = 0; i < 10; ++i)
            {
                var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
                itemFx.Explosion(toObject.transform.position, point.transform.position, 120.0f);
            }
            isAtd = true;
        }
    }

    public void BadgeMover(float range)
    {
        for (int i = 0; i < 1; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
            itemFx.Explosion2(toObject.transform.position+ new Vector3(-50, 0, 0), range);
        }
    }

    public void HeartChMover(float range)
    {
        for (int i = 0; i < 5; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, toObject.transform);
            itemFx.Explosion2(toObject.transform.position+new Vector3(-50,0,0), range);
        }
    }


    public void CountCoin(float dis)
    {
        var itemFx2 = GameObject.Instantiate<HeartAcquireFx>(prefab, transform);
        itemFx2.Coin(toObject.transform.position,dis);
    }
}
