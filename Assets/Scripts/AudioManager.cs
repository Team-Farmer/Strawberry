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
    public AudioClip[] BgClipList;//0=메인게임 1=미니게임

    //효과음 오디오
    public AudioClip SFXclip;
    public AudioClip SowClip;
    public AudioClip HarvestClip;
    public AudioClip Cute1Clip;
    public AudioClip BlopClip;
    public AudioClip Cute2Clip;
    public AudioClip Cute4Clip;
    public AudioClip TadaClip;
    public AudioClip RainClip;
    public AudioClip RightClip;
    public AudioClip WrongClip;
    public AudioClip DoorOpenClip;
    public AudioClip DoorCloseClip;
    // 아래 우연이가 추가
    public AudioClip CoinClip;
    public AudioClip RewardClip;
    public AudioClip TimerClip;
    public AudioClip RemoveClip;
    public AudioClip Remove2Clip;


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
        BGSoundVolume();
        SFXVolume();

        BgSoundPlay2(true);
    }

    //========================================================================================================
    //========================================================================================================
    
    //배경음 변경(새로운 씬 안가져오니까 이거 변경할것)
    private void OnSceneLoaded(Scene sceneNow,LoadSceneMode mode) 
    {

        for (int i = 0; i < BgClipList.Length; i++) {
            if (sceneNow.name == BgClipList[i].name)//씬이름과 같은 배경음 틀기
            {
                BgSoundPlay(BgClipList[i]);//재생
            }
        }
    
    }


    public void BGSoundVolume() {

        //배경음 음량변화
        if (BGSoundSlider.GetComponent<Slider>().value == 0)
        { mixer.SetFloat("BGSoundVolume", -80); }
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20);

    }

    public void SFXVolume() {
        //효과음 음량변화
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) 
        { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

    }



    //효과음
    //이거 미완성
    public void SFXPlay(string sfxName,AudioClip clip) {

        GameObject go = new GameObject(sfxName+"Sound");//해당이름을 가진 오브젝트가 소리 날때마다 생성된다.
        
        //오디오 재생
        AudioSource audioSource = go.AddComponent<AudioSource>();//그 오브젝트에 오디오 소스 컴포넌트 추가
        audioSource.clip = clip;//오디오 소스 클립 추가
        audioSource.loop = false;
        audioSource.Play();//재생

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        Destroy(go, clip.length);//효과음 재생 후(clip.length 시간 지난후) 파괴
    
    }



    //얘네 줄일수있긴한데 그러면 오디오 수작ㅇ업으로 넣어야하는데 ..

    public void SowAudioPlay()//씨뿌리기 효과음
    { SFXPlay("SowSFX", SowClip); }
    public void HarvestAudioPlay() //수확 효과음
    { SFXPlay("HarvestSFX", HarvestClip); }



    public void SFXAudioPlay() //X버튼 blop
    { SFXPlay("ButtonSFX", SFXclip); }
    public void Cute1AudioPlay() //버튼 효과음
    { SFXPlay("Cute1SFX", Cute1Clip); }
    public void Cute2AudioPlay()//패널 등장
    { SFXPlay("Cute2SFX", Cute2Clip); }
    public void Cute4AudioPlay()//에러패널
    { SFXPlay("Cute4SFX", Cute4Clip); }
    public void TadaAudioPlay() //딸기 획득 효과음 
    { SFXPlay("TadaSFX", TadaClip); }
    public void RainAudioPlay() //비 효과음
    { SFXPlay("RainSFX", RainClip); }
    public void RightAudioPlay() //맞았아요 
    { SFXPlay("RightSFX", RightClip); }
    public void WrongAudioPlay() //틀렸어요 
    { SFXPlay("WrongSFX", WrongClip); }


    public void DoorOpenAudioPlay() //문열기
    { SFXPlay("DoorOpenSFX", DoorOpenClip); }
    public void DoorCloseAudioPlay() //문닫기 
    { SFXPlay("DoorCloseSFX", DoorCloseClip); }

    public void CoinAudioPlay() // 코인 획득
    { SFXPlay("CoinSFX", CoinClip); }
    public void RewardAudioPlay() // 보상 획득
    { SFXPlay("RewardSFX", RewardClip); }
    public void TimerCloseAudioPlay() // 미니게임 타이머
    { SFXPlay("TimerSFX", TimerClip); }
    public void RemoveAudioPlay() // 제거1
    { SFXPlay("RemoveSFX", RemoveClip); }
    public void Remove2AudioPlay() // 제거2
    { SFXPlay("Remove2SFX", Remove2Clip); }

    //직접 오디오 넣는 함수
    public void SFXPlayButton(AudioClip clip) 
    {
        SFXPlay("", clip);
    }


    public void BgSoundPlay(AudioClip clip) //이건 안쓰기는 할듯
    {
        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = false;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
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
