using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame; //cloudSave
using Firebase.Auth;    //login
using Newtonsoft.Json; //serialize/deserialize
using System.Text;     //for encoding
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager instance = null;

    FirebaseAuth auth = null; //auth용 instance
    FirebaseUser user = null; //현재 사용자 계정
    string authCode = "";

    ISavedGameClient SavedGame => PlayGamesPlatform.Instance.SavedGame;
    string fileName = "gameData";
    bool saving;

    private byte[] cloudData = null;
    public bool isCloudDataNull = true; //클라우드 데이터가 없을 때 true
    public GameObject subText; //클라우드 저장 팝업의 서브텍스트
    public PanelAnimation loadingPanel;
    public GameObject panelBlack;
    public PanelAnimation cloudConfirmPanel;
    public Text confirm_tx;
    public Action OnSaveSucceed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        //Debug.Log("안드로이드에서 시도하세용");
#elif UNITY_ANDROID
        Init();
#endif
    }

    void Init()
    {
        var config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames() //게임세이브
            .RequestServerAuthCode(false) //playgames 클라이언트 구성
            .Build();

        //GPGS초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        //구글플레이게임 활성화
        PlayGamesPlatform.Activate();

        //다했으면 로그인
        Login();
    }


    //login
    void Login()
    {
        //구글 로그인
        Social.localUser.Authenticate((success, error) =>
        {
            if (!success) Debug.Log("로그인 실패 : " + error);
            else
            {
                //firebase 사용자 인증 정보를 사용하여 플레이어 인증
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                auth = FirebaseAuth.DefaultInstance;
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("파이어베이스 인증 취소됨");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("파이어베이스 인증 에러: " + task.Exception);
                        return;
                    }
                    user = task.Result;
                    //Debug.Log(user.DisplayName + "님 로그인!");
                });
            }
        });
    }

    //logout
    void Logout()
    {
        auth.SignOut();
    }

    public bool isLogined()
    {
        return auth != null;
    }


    public void CloudSaveOrLoad()
    {
        if (subText.activeSelf)
        {
            //Load
            LoadFromCloud();
        }
        else
        {
            //Save
            SaveToCloud();
        }
    }

    //overwrites old file or saves a new one
    void SaveToCloud()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (user != null)
            {
                //LoadingImg SetActive True
                loadingPanel.OpenScale();

                Debug.Log("클라우드로 게임 데이터를 전송합니다...");
                saving = true;
                SavedGame.OpenWithAutomaticConflictResolution(
                    fileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SavedGameOpened);
            }
            else
            {
                Debug.Log("로그인이 되어있지 않음");
            }
        }
        else
        {
            print("안드로이드에서 테스트 해주세요");
        }
    }


    //load from cloud
    void LoadFromCloud()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("클라우드로부터 게임 데이터가 있는지 확인중...");
            saving = false;
            if (user != null)
            {
                //LoadingImg Setactive true
                loadingPanel.OpenScale();

                SavedGame.OpenWithAutomaticConflictResolution(
                    fileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SavedGameOpened);
            }
            else
            {
                Debug.Log("로그인이 되어있지 않음");
            }
        }
        else print("안드로이드에서 테스트 해주세요");
    }


    //저장하거나 로드
    private void SavedGameOpened(SavedGameRequestStatus state, ISavedGameMetadata game)
    {
        //check success
        if (state == SavedGameRequestStatus.Success)
        {
            if (saving)
            {
                //saving
                //read bytes from save
                //gameData to string -> byte[]
                //이렇게해도 되는지 확인필요!!!!!
                string saveString = JsonConvert.SerializeObject(DataController.instance.gameData);
                byte[] data = Encoding.UTF8.GetBytes(saveString);

                //create builder
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                SavedGameMetadataUpdate updateMetaData = builder.Build();

                //saving to cloud
                SavedGame.CommitUpdate(game, updateMetaData, data, CheckSaving);
            }
            else
            {
                //loading
                SavedGame.ReadBinaryData(game, CheckLoading);
            }
        }
        else
        {
            Debug.Log("Error opening game : " + state);
        }
    }


    //callback from SavedGameOpened. Check saving result was successful or not
    private void CheckSaving(SavedGameRequestStatus state, ISavedGameMetadata game)
    {
        Debug.Log("클라우드 저장 확인중...");
        if (state == SavedGameRequestStatus.Success)
        {
            Debug.Log("클라우드 저장 성공");

            //날짜 갱신
            DataController.instance.gameData.cloudSaveTime = System.DateTime.Now;
            //로컬저장
            DataController.instance.SaveData();
            //설정창 날짜 갱신 - 클라우드 저장 후 실행되어야 하므로 액션으로 설정
            OnSaveSucceed();
            //텍스트변경
            confirm_tx.text = "저장을 완료했어요!";
        }
        else
        {
            confirm_tx.text = "저장에 실패했어요!";
            Debug.LogError("클라우드 저장 실패 : " + state);
        }

        //LoadingImg Setactive false
        loadingPanel.CloseScale();

        //확인창 띄우기
        cloudConfirmPanel.OpenScale();
        panelBlack.SetActive(true);
    }


    //callback from SavedGameOpened. Check loading result was successful or not
    private void CheckLoading(SavedGameRequestStatus state, byte[] cloudData)
    {
        Debug.Log("클라우드 로딩 확인중...");
        if (state == SavedGameRequestStatus.Success)
        {
            Debug.Log("로딩 성공");
            if (cloudData == null)
            {
                isCloudDataNull = true;
                this.cloudData = null;

                confirm_tx.text = "불러올 데이터가 없어요!";
                
                //확인창 띄우기
                cloudConfirmPanel.OpenScale();
                panelBlack.SetActive(true);
            }
            else
            {
                isCloudDataNull = false;
                this.cloudData = cloudData;
                OverrideData();
            }
        }
        else
        {
            Debug.Log("로딩 실패 : " + state);
        }

        //LoadingImg Setactive false
        loadingPanel.CloseScale();
    }


    //데이터 덮어쓰기
    private void OverrideData()
    {
        Debug.Log("데이터가져옴");
        confirm_tx.text = "저장된 내용을 불러왔어요!";

        //클라우드 데이터 유저데이터에 넣고 로컬저장..이렇게 해도 바로 반영 되는건지 확인해야 함
        //byte[] to gameData
        DataController.instance.gameData = JsonConvert.DeserializeObject<GameData>(Encoding.UTF8.GetString(cloudData));
        DataController.instance.SaveData();

        //확인창 띄우기
        cloudConfirmPanel.OpenScale();
        panelBlack.SetActive(true);

        //LoadingImg SetActive true
        loadingPanel.OpenScale();

        //씬 재로드
        SceneManager.LoadScene(0);
    }


    //클라우드 데이터 삭제
    public void DeleteCloudData()
    {
        loadingPanel.OpenScale();

        SavedGame.OpenWithAutomaticConflictResolution(
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            DeleteSavedGame);
    }

    void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame.Delete(game);
        }
        else
        {
            Debug.Log("클라우드 데이터 지우기 실패ㅠ");
        }
        loadingPanel.CloseScale();
    }
}
