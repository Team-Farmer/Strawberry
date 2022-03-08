using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    //싱글톤
    public static DataController instance = null;

    //게임데이터 
    string gameDataFileName = "gameData.json";
    //[HideInInspector]
    public GameData gameData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //씬 전환이 되더라도 파괴되지 않도록 함
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance!=this)
        {
            //씬 전환이 되었는데 인스턴스가 존재하는 경우
            //이전 씬에서 넘어온 인스턴스를 사용하기 위해 
            //새로운 씬의 게임오브젝트 제거
            Destroy(this.gameObject);
        }

        LoadData();
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        if (File.Exists(filePath))
        {
            //json파일 불러오기
            //Debug.Log(filePath);
            string jsonData = File.ReadAllText(filePath);

            //역직렬화
            //gameData =JsonConvert.DeserializeObject<GameData>(jsonData);
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            Debug.Log("데이터를 로드했습니다");
        }
        else
        {
            Debug.Log("새로운 데이터 생성");
            gameData = new GameData();
            InitData();
            //SaveData();
        }
    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        //데이터 직렬화
        //string jsonData = JsonConvert.SerializeObject(gameData);
        string jsonData = JsonUtility.ToJson(gameData);

        //로컬에 저장
        File.WriteAllText(filePath, jsonData);

        Debug.Log("저장 완료 경로 : "+filePath);
    }

    public void InitData()
    {
        //Truck
        gameData.berryCnt = 48;
        //Bug
        //gameData.bugProb = 10f;
        //Weed
        //gameData.weedProb = 20f;

        //Research
        for (int i = 0; i < gameData.researchLevel.Length; i++) 
        {
            gameData.researchLevel[i] = 1;
        }
        
        
    }
    void OnApplicationQuit()
    {
        //게임종료시 저장 - 개발중이니까 일단 주석
        //SaveData();
    }
    
}
