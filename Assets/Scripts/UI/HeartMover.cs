using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class HeartMover : MonoBehaviour
{
    public HeartAcquireFx prefab;
    public GameObject Day;
    public RectTransform rect;
    public int daysCompare;
    private int days;
    private bool isAtd;
    Vector2 vec;

    private void Start()
    {
        days = DataController.instance.gameData.days;
        isAtd = DataController.instance.gameData.attendance;
    }

    public void HeartMove(int j)
    {
        daysCompare = j;
  

        if (days > 6)
            days %= 7;


        if (j == 0)
            vec = new Vector2(153, 714);
        else if (j == 1)
            vec = new Vector2(-133, 714);
        else if (j == 2)
            vec = new Vector2(-410, 724);
        else if (j == 3)
            vec = new Vector2(153, 1014);
        else if (j == 4)
            vec = new Vector2(-133, 1014);
        else if (j == 5)
            vec = new Vector2(-410, 1014);
        else if (j == 6)
            vec = new Vector2(-133, 1314);



        if (isAtd == false&&days==daysCompare)
       {
            int randCount = 5;//Random.Range(5, 10);
            for (int i = 0; i < randCount; ++i)
            {
                var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, rect);
                itemFx.Explosion(Day.transform.position, vec, 100.0f);
            }
            isAtd = true;
       }
    }


}
