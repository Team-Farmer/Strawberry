using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Setting : MonoBehaviour
{
    #region
    public Text versionDate_text;
    public Text version_text;
    public Text saveDate_text;
    public Button cloudLoad_btn;
    public Button cloudSave_btn;

    // 잠금 해제 코드
    public InputField code_input;
    public Text code_text;
    public GameObject money_btn;
    public GameObject berry_btn;

    private string money = "money";
    private string nomoney = "nomoney";
    private string berry = "berry";
    private string noberry = "noberry";
    //private string store = "store";
    #endregion

    void Awake()
    {
        SetVersionInfo();
    }

    void Start()
    {
        SetCloudSave();
#if UNITY_EDITOR

#elif UNITY_ANDROID
        GPGSManager.instance.OnSaveSucceed += SetCloudSave;
#endif
    }

    public void SetCloudSave()
    {
        cloudSave_btn.interactable = GPGSManager.instance.isLogined();
        cloudLoad_btn.interactable = GPGSManager.instance.isLogined();

        if (DataController.instance.gameData.cloudSaveTime == System.DateTime.MinValue)
        {
            cloudLoad_btn.interactable = false;
            saveDate_text.text = "저장기록 없음";
        }
        else
        {
            saveDate_text.text = "마지막 저장 날짜\n" + DataController.instance.gameData.cloudSaveTime.ToString("yyyy년 MM월 dd일 HH:mm:ss");
        }
    }

    private void SetVersionInfo() // 버전 정보 세팅
    {
        versionDate_text.text = "22-05-09";
        version_text.text = "V 1.0.1";
    }

    public void PrivacyPolicy() // 개인정보처리방침
    {
        Application.OpenURL("https://woos-workspace.notion.site/e9c66b72d3ef4e5082909afd2f5cf0a7");
    }

    public void ContactByEmail() // 이메일로 문의하기
    {
        string mailto = "teamfarmer.ttalgi@gmail.com";
        string subject = EscapeURL("[새콤달콤 딸기농장] 버그 / 문의");
        string body = EscapeURL
            (
             "\n\n\n\n\n" +
             "__________\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n"
            );

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    public void Instagram() // 공식 인스타 계정
    {
        Application.OpenURL("https://www.instagram.com/team_farmer_/");
    }

    public void OpenCodePanel()
    {
        code_input.text = "";
        code_text.text = "";
    }

    public void EnterCodeBtn()
    {
        if (code_input.text == money)
        {
            code_text.text = "코인버튼 해제";
            money_btn.SetActive(true);
        }
        else if (code_input.text == nomoney)
        {
            code_text.text = "코인버튼 잠금";
            money_btn.SetActive(false);
        }
        else if (code_input.text == berry)
        {
            code_text.text = "딸기버튼 해제";
            berry_btn.SetActive(true);
        }
        else if (code_input.text == noberry)
        {
            code_text.text = "딸기버튼 잠금";
            berry_btn.SetActive(false);
        }
        /*else if (code_input.text == store)
        {
            code_text.text = "가게 오픈";
            DataController.instance.gameData.isStoreOpend = true;
        }*/
    }
}
