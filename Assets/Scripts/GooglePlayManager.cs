using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Common;
using Google.Play.AppUpdate;
using System;

public class GooglePlayManager : MonoBehaviour
{
    // 출처1: https://dodnet.tistory.com/4779
    // 출처2: https://wonjuri.tistory.com/entry/unity-%EA%B5%AC%EA%B8%80%ED%94%8C%EB%A0%88%EC%9D%B4-%EC%9D%B8%EC%95%B1-%EC%97%85%EB%8D%B0%EC%9D%B4%ED%8A%B8-%EB%B0%8F-%EC%9D%B8%EC%95%B1-%EB%A6%AC%EB%B7%B0-%EC%97%B0%EB%8F%99-%ED%81%B4%EB%9E%98%EC%8A%A4
    public static GooglePlayManager Instance { get; private set; }

    bool Updating = false;

    public void Awake()
    {
        Instance = this;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    // 인앱 업데이트 호출 함수
    public IEnumerator UpdateApp()
    {
        // 매니저 정의
        AppUpdateManager appUpdateManager = new AppUpdateManager();

        // 가능한 업데이트가 있는지 확인
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        // 업데이트 정보를 받아오기까지 기다림
        yield return appUpdateInfoOperation;

        // 업데이트 정보를 성공적으로 받아옴
        if (appUpdateInfoOperation.IsSuccessful)
        {
            // 업데이트 정보 정의
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            // 업데이트 가능 여부 확인
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // 실제 업데이트 실행
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                yield return startUpdateRequest;
            }

        }
        else
        {
            Debug.Log(appUpdateInfoOperation.Error);
        }
    }
}