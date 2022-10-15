using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;

    [Header("object")]
    //소리 조절 슬라이더
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//오디오 믹서
    public AudioSource bgSound;//오디오 매니저

    [Header("Pooling")]
    [SerializeField]
    private GameObject poolingObjectPrefab; //미리 생성될 프리팹
    Queue<Sound> poolingObjectQueue = new Queue<Sound>(); //큐 생성
    public GameObject AudioManagerObj;



    //public bool isGameMain;//true=메인게임 false=미니게임

    //배경음 오디오
    public AudioClip[] BgClipList;//0=튜토리얼, 1=배경음악, 2=가게, 3=크레딧, 4~7=미니게임

    //효과음 오디오 (갈아엎음)
    public AudioClip HarvestClip;
    public AudioClip SowClip;
    public AudioClip ClickClip; //Cute1Clip;
    public AudioClip OKClip;    //Cute2Clip;
    public AudioClip BackClip;  //SFXclip;
    public AudioClip ErrorClip; //Cute4Clip;
    public AudioClip TadaClip;
    public AudioClip RainClip;
    public AudioClip RightClip;
    public AudioClip WrongClip;
    public AudioClip DoorOpenClip;
    public AudioClip DoorCloseClip;
    public AudioClip CoinClip;
    public AudioClip RewardClip;
    public AudioClip TimerClip;
    public AudioClip FastTimerClip;
    public AudioClip RemoveWeedClip;
    public AudioClip RemoveBugClip;
    public AudioClip CountdownClip;
    public AudioClip EndClip;
    public AudioClip ScrollbarClip;
    public AudioClip Cute5Clip;

    //instance
    public static AudioManager instance;


    //========================================================================================================
    //========================================================================================================
    private void Awake()
    {
        //isGameMain = true;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else { Destroy(gameObject); }

        Initialize(10);//10개 미리 만들기
    }


    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = DataController.instance.gameData.BGSoundVolume;
        SFXSoundSlider.GetComponent<Slider>().value = DataController.instance.gameData.SFXSoundVolume;
        BGSoundVolume();
        SFXVolume();

        AudioManager.instance.BGMPlay(1);


        //슬라이드값 변할때마다 아래 함수 실행
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }


    //========================================================================================================
    //========================================================================================================

    //----------오디오 음량 조절----------
    public void BGSoundVolume()
    {

        //배경음 음량조절
        if (BGSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("BGSoundVolume", -80); }
        else { mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }


        DataController.instance.gameData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

    }

    public void SFXVolume()
    {
        //효과음 음량조절
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        DataController.instance.gameData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
    }


    //========================================================================================================
    //========================================================================================================

    //효과음 플레이 함수
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");//해당이름을 가진 오브젝트가 소리 날때마다 생성된다.

        //오디오 재생
        AudioSource audioSource = go.AddComponent<AudioSource>();//그 오브젝트에 오디오 소스 컴포넌트 추가
        audioSource.clip = clip;//오디오 소스 클립 추가
        audioSource.loop = false;
        audioSource.Play();//재생

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        Destroy(go, clip.length);//효과음 재생 후(clip.length 시간 지난후) 파괴
    }
    public void SFXPlayPoolingVersion(string sfxName, AudioClip clip) 
    {
        var obj = GetObject();
        obj.gameObject.GetComponent<AudioSource>().clip = clip;
        obj.gameObject.GetComponent<AudioSource>().loop = false;
        obj.gameObject.GetComponent<AudioSource>().Play();
        obj.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

    }

    //========================================================================================================
    //========================================================================================================

    //예림 오디오 멈춤
    public void PauseAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player!=null)
        {
            player.GetComponent<AudioSource>().Pause();
        }
    }

    //오디오 재개
    public void ResumePlayAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            player.GetComponent<AudioSource>().Play();
        }
    }

    //오디오 멈춤
    public void StopPlayAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            player.GetComponent<AudioSource>().Stop();
            Destroy(player);
        }
    }

    //소나기재새-파티클 꺼질 때까지 재생해야한다!
    public void RainAudioPlay()
    {
        GameObject go = new GameObject("RainSFXSound");

        //오디오 재생
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = RainClip;//오디오 소스 클립 추가
        audioSource.loop = true;
        audioSource.Play();//재생

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        
    }

    //========================================================================================================
    //========================================================================================================


    //얘네 줄일수있긴한데 그러면 오디오 수작ㅇ업으로 넣어야함

    public void HarvestAudioPlay()  //수확 효과음
    { SFXPlayPoolingVersion("HarvestSFX", HarvestClip); }
    public void SowAudioPlay()      //씨뿌리기 효과음
    { SFXPlayPoolingVersion("SowSFX", SowClip); }
    public void Cute1AudioPlay()// 클릭 효과음
    { SFXPlayPoolingVersion("Cute1SFX", ClickClip); }
    public void Cute2AudioPlay()// 패널 등장 (확인)
    { SFXPlayPoolingVersion("Cute2SFX", OKClip); }
    public void SFXAudioPlay()  // X버튼 blop
    { SFXPlayPoolingVersion("ButtonSFX", BackClip); }
    public void Cute4AudioPlay()// 에러 패널
    { SFXPlayPoolingVersion("Cute4SFX", ErrorClip); }
    public void TadaAudioPlay() // 딸기 획득
    { SFXPlayPoolingVersion("TadaSFX", TadaClip); }
    //public void RainAudioPlay() // 소나기
    //{ SFXPlay("RainSFX", RainClip); }
    public void RightAudioPlay()// 미니게임 정답
    { SFXPlayPoolingVersion("RightSFX", RightClip); }
    public void WrongAudioPlay()// 미니게임 오답 
    { SFXPlayPoolingVersion("WrongSFX", WrongClip); }
    public void DoorOpenAudioPlay()     // 가게 열기
    { SFXPlayPoolingVersion("DoorOpenSFX", DoorOpenClip); }
    public void DoorCloseAudioPlay()    // 가게 닫기 
    { SFXPlayPoolingVersion("DoorCloseSFX", DoorCloseClip); }
    public void CoinAudioPlay()         // 코인 획득
    { SFXPlayPoolingVersion("CoinSFX", CoinClip); }
    public void RewardAudioPlay()       // 보상 획득
    { SFXPlayPoolingVersion("RewardSFX", RewardClip); }
    public void TimerCloseAudioPlay()   // 미니게임 타이머
    { SFXPlayPoolingVersion("TimerSFX", TimerClip); }
    public void TimerVeryCloseAudioPlay()   // 미니게임 더 빠른 타이머
    { SFXPlayPoolingVersion("FastTimerSFX", FastTimerClip); }
    public void RemoveAudioPlay()       // 잡초 제거
    { SFXPlayPoolingVersion("RemoveWeedSFX", RemoveWeedClip); }
    public void Remove2AudioPlay()      // 벌레 제거
    { SFXPlayPoolingVersion("RemoveBugSFX", RemoveBugClip); }
    public void CountdownAudioPlay()      // 미니게임 카운트 다운
    { SFXPlayPoolingVersion("CountdownSFX", CountdownClip); }
    public void EndAudioPlay()      // 미니게임 종료
    { SFXPlayPoolingVersion("EndSFX", EndClip); }
    public void ScrollbarAudioPlay()      // 스크롤바
    { SFXPlayPoolingVersion("ScrollbarSFX", ScrollbarClip); }

    public void Cute5AudioPlay()      // 스크롤바
    { SFXPlayPoolingVersion("Cute5SFX", Cute5Clip); }


    //직접 오디오 넣는 함수
    public void SFXPlayButton(AudioClip clip)
    {
        SFXPlay("", clip);
    }

    //배경음악 플레이 함수
    public void BGMPlay(int index)
    {
        AudioClip clip;
        clip = AudioManager.instance.BgClipList[index];

        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }


    //========================================================================================================
    //========================================================================================================
    //오브젝트 풀링

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); //10번 Enqueue
        }
    }

    
    private Sound CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Sound>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(AudioManagerObj.transform);
        return newObj; //그리고 Queue에 넣게 반환
    }

    public static Sound GetObject() // 미리 만든거 가져다가 쓴다!
    {
        if (instance.poolingObjectQueue.Count > 0) // 미리 생성된게 안부족하면
        {
            var obj = instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true); // 미리 생성되어있는거 ON
            return obj;
        }
        else // 부족하면
        {
            var newObj = instance.CreateNewObject(); // 하나 새로 만들어서
            newObj.gameObject.SetActive(true); // 밑에는 위와 같음
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(Sound obj) //썼던 거 다시 반환
    {
        obj.gameObject.SetActive(false); //끄고
        obj.transform.SetParent(instance.transform); // 다시 원래 부모로 돌아와서 Object Pool 자식으로 만듬
        instance.poolingObjectQueue.Enqueue(obj); // 다시 Enqueue 삽입
    }
    //부모 변경하는거 필요없을듯 사운드는 
}
