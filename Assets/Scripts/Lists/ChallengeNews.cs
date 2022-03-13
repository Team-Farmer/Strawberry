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
        public bool isDone_c;
        public bool isUnlock_n;
        public string Exp_n;
        public int Count_c;


        public ChallengeNewsStruct(string Title, int[] reward, bool isDone_c, bool isUnlock_n, string Exp_n, int Count_c)
        {
            this.Title = Title;
            this.reward = reward;
            this.isDone_c = isDone_c;
            this.isUnlock_n = isUnlock_n;
            this.Exp_n = Exp_n;
            this.Count_c = Count_c;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject titleText;
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

    [Header("==========기타==========")]
    [SerializeField]
    private bool isChallenge;

    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;


    private void Start()
    {
        InfoUpdate();
    }
    public void InfoUpdate() {

        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 프리팹 숫자와 관련되어 있다!!! 같이 조절해야함
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 7)
        { Prefabcount =0; }
        prefabnum = Prefabcount;



        //내용표시
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;


        if (isChallenge == true && Info[prefabnum].isDone_c == false) 
        {
            Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];
        }
        else if (Info[prefabnum].isUnlock_n == false)
        {
            lock_News.SetActive(true);
        }



        Prefabcount++;

    }
}
