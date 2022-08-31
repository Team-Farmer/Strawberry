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
    public Button cloudSave_btn;
    public Button cloudLoad_btn;
    #endregion

    void Awake()
    {
        SetVersionInfo();
    }

    void Start()
    {
        SetCloudSave();
//#if UNITY_EDITOR

//#elif UNITY_ANDROID
//        GPGSManager.instance.OnSaveSucceed += SetCloudSave;
//#endif
    }

    public void SetCloudSave()
    {
        if (DataController.instance.gameData.cloudSaveTime == System.DateTime.MinValue)
        {
            cloudSave_btn.interactable = false;
            cloudLoad_btn.interactable = false;
            saveDate_text.text = "저장기록 없음";
        }
        else
        {
            cloudLoad_btn.interactable = true;
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
}
