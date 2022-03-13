using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeNews : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;
        public int[] reward;
        public bool isDone;
        public string NewsExp;
        public int challengeCount;


        public ChallengeNewsStruct(string Title, int[] reward, bool isDone, string NewsExp, int challengeCount)
        {
            this.Title = Title;
            this.reward = reward;
            this.isDone = isDone;
            this.NewsExp = NewsExp;
            this.challengeCount = challengeCount;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject Title;
    [SerializeField]
    private GameObject countText_News;
    [SerializeField]
    private GameObject lock_News;
    [SerializeField]
    private GameObject nowText_Challenge;
    [SerializeField]
    private GameObject Btn_Challenge;
    


    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite[] BtnImage_Challenge;



}
