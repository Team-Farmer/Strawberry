using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlAttandSprite : MonoBehaviour
{
    private Image CurrentSprite;


    [SerializeField]
    private Sprite Late;
    [SerializeField]
    private Sprite TodayGet;
    [SerializeField]
    private Sprite NotGet;

    // Use this for initialization
    void Start()
    {
        /*
         * AttandDay 에 대한 정보 복사
         * 
         */

        CurrentSprite = GetComponent<Image>();

        int day = int.Parse(gameObject.name);

        if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.AttandDay[day - 1] == false)
            CurrentSprite.sprite = Late;
        else if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.AttandDay[day - 1] == true)
            CurrentSprite.sprite = TodayGet;
        else
            CurrentSprite.sprite = NotGet;


    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateSprite()
    {
        CurrentSprite.sprite = TodayGet;
    }

}
