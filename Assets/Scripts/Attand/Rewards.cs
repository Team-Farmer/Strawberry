using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    private ControlAttandSprite[] rewardSprite;
    [SerializeField]
    private GameObject rewards;
    // Use this for initialization
    void Start()
    {

        rewardSprite = rewards.GetComponentsInChildren<ControlAttandSprite>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangingSprite()
    {
        rewardSprite[System.DateTime.Now.Day - 1].UpdateSprite();
        AttandManager.AttandInstance.AttandDay[System.DateTime.Now.Day - 1] = true;
    }
}
