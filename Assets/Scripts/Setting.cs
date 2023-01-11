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

    private string coin = "yc";
    private string nocoin = "nc";
    private string berry = "yb";
    private string noberry = "nb";
    private string store = "ys";
    private string nostore = "ns";
    private string dotori = "yd";
    #endregion

    void Awake()
    {
        SetVersionInfo();
#if UNITY_EDITOR

#elif UNITY_ANDROID
        GPGSManager.instance.OnSaveSucceed += SetCloudSave;
#endif
    }

    void OnEnable()
    {
        SetCloudSave();
    }

    public void SetCloudSave()
    {
        cloudSave_btn.interactable = false;//GPGSManager.instance.isLogined();
        cloudLoad_btn.interactable = false;//GPGSManager.instance.isLogined();

        if (DataController.instance.gameData.cloudSaveTime == System.DateTime.MinValue)
        {
            cloudLoad_btn.interactable = false;
            saveDate_text.text = "저장기록 없음";
        }
        else
        {
            saveDate_text.text = "마지막 저장 시간\n" + DataController.instance.gameData.cloudSaveTime.ToString("yyyy년 MM월 dd일 HH:mm:ss");
        }
    }

    private void SetVersionInfo() // 버전 정보 세팅
    {
        versionDate_text.text = "23-01-08";
        version_text.text = "V 1.0.3";
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
        if (code_input.text == coin)
        {
            code_text.text = "코인버튼 해제";
            money_btn.SetActive(true);
        }
        else if (code_input.text == nocoin)
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
        else if (code_input.text == store)
        {
            code_text.text = "가게 오픈";
            DataController.instance.gameData.isStoreOpend = true;
        }
        else if (code_input.text == nostore)
        {
            code_text.text = "가게 닫기";
            DataController.instance.gameData.isStoreOpend = false;
        }
        else if (code_input.text == dotori)
        {
            code_text.text = "도토리 충전";
            DataController.instance.gameData.dotori = 5;
        }
        else if (code_input.text == "")
        {
            code_text.text = "코드 입력 필요";
        }
        else
        {
            code_text.text = "해당 코드 없음";
        }
    }
}
