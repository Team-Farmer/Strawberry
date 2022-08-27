using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            //SceneManager.sceneLoaded += OnSceneLoaded;//씬 시작될때마다 실행
        }
        else { Destroy(gameObject); }
    }


    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = 0.5f;
        SFXSoundSlider.GetComponent<Slider>().value = 0.5f;

        BgSoundPlay2(true);


        //슬라이드값 변할때마다 아래 함수 실행
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }

    
    //========================================================================================================
    //========================================================================================================
    

    public void BGSoundVolume() {

        //배경음 음량조절
        if (BGSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("BGSoundVolume", -80); }
        else { mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }
        


    }

    public void SFXVolume() {
        //효과음 음량조절
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }


    }



    //효과음
    //이거 미완성
    public void SFXPlay(string sfxName,AudioClip clip) 
    {

        GameObject go = new GameObject(sfxName+"Sound");//해당이름을 가진 오브젝트가 소리 날때마다 생성된다.
        
        //오디오 재생
        AudioSource audioSource = go.AddComponent<AudioSource>();//그 오브젝트에 오디오 소스 컴포넌트 추가
        audioSource.clip = clip;//오디오 소스 클립 추가
        audioSource.loop = false;
        audioSource.Play();//재생

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        Destroy(go, clip.length);//효과음 재생 후(clip.length 시간 지난후) 파괴
    
    }



    //얘네 줄일수있긴한데 그러면 오디오 수작ㅇ업으로 넣어야함

    public void HarvestAudioPlay()  //수확 효과음
    { SFXPlay("HarvestSFX", HarvestClip); }
    public void SowAudioPlay()      //씨뿌리기 효과음
    { SFXPlay("SowSFX", SowClip); }
    public void Cute1AudioPlay()// 클릭 효과음
    { SFXPlay("Cute1SFX", ClickClip); }
    public void Cute2AudioPlay()// 패널 등장 (확인)
    { SFXPlay("Cute2SFX", OKClip); }
    public void SFXAudioPlay()  // X버튼 blop
    { SFXPlay("ButtonSFX", BackClip); }
    public void Cute4AudioPlay()// 에러 패널
    { SFXPlay("Cute4SFX", ErrorClip); }
    public void TadaAudioPlay() // 딸기 획득
    { SFXPlay("TadaSFX", TadaClip); }
    public void RainAudioPlay() // 소나기
    { SFXPlay("RainSFX", RainClip); }
    public void RightAudioPlay()// 미니게임 정답
    { SFXPlay("RightSFX", RightClip); }
    public void WrongAudioPlay()// 미니게임 오답 
    { SFXPlay("WrongSFX", WrongClip); }
    public void DoorOpenAudioPlay()     // 가게 열기
    { SFXPlay("DoorOpenSFX", DoorOpenClip); }
    public void DoorCloseAudioPlay()    // 가게 닫기 
    { SFXPlay("DoorCloseSFX", DoorCloseClip); }
    public void CoinAudioPlay()         // 코인 획득
    { SFXPlay("CoinSFX", CoinClip); }
    public void RewardAudioPlay()       // 보상 획득
    { SFXPlay("RewardSFX", RewardClip); }
    public void TimerCloseAudioPlay()   // 미니게임 타이머
    { SFXPlay("TimerSFX", TimerClip); }
    public void TimerVeryCloseAudioPlay()   // 미니게임 더 빠른 타이머
    { SFXPlay("FastTimerSFX", FastTimerClip); }
    public void RemoveAudioPlay()       // 잡초 제거
    { SFXPlay("RemoveWeedSFX", RemoveWeedClip); }
    public void Remove2AudioPlay()      // 벌레 제거
    { SFXPlay("RemoveBugSFX", RemoveBugClip); }
    public void CountdownAudioPlay()      // 미니게임 카운트 다운
    { SFXPlay("CountdownSFX", CountdownClip); }
    public void EndAudioPlay()      // 미니게임 종료
    { SFXPlay("EndSFX", EndClip); }
    public void ScrollbarAudioPlay()      // 스크롤바
    { SFXPlay("ScrollbarSFX", ScrollbarClip); }


    //직접 오디오 넣는 함수
    public void SFXPlayButton(AudioClip clip) 
    {
        SFXPlay("", clip);
    }

    public void BgSoundPlay2(bool isGameMain)
    {
        AudioClip clip;
        if (isGameMain == true) {clip= BgClipList[0]; }
        else { clip = BgClipList[1]; }

        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }

}
