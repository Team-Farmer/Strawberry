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
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;
    public AudioSource bgSound;
    //배경음 오디오(오디오 이름과 씬이름이 같을경우 플레이 해줍니다)
    public AudioClip[] bglist;
    //효과음 오디오
    public AudioClip SFXclip;
    public AudioClip HarvestClip;
    public AudioClip TadaClip;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;//씬 시작시 실행
        }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = 0.5f;
        SFXSoundSlider.GetComponent<Slider>().value = 0.5f;
        BGSoundVolume();
        SFXVolume();
    }
    //씬에 따라 배경음 변경
    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1) {
        for (int i = 0; i < bglist.Length; i++) {
            if (arg0.name == bglist[i].name)//씬이름과 같은 배경음 틀기
            {
                BgSoundPlay(bglist[i]);//재생
            }
        }
    
    }


    public void BGSoundVolume() {

        //배경음 음량변화
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20);

    }

    public void SFXVolume() {
        //효과음 음량변화
        mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20);

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



    //얘네 다른 스크립트에서하면 줄일수있긴한데 너무 분산될까봐 이렇게
    public void SFXAudioPlay() //버튼 효과음
    { SFXPlay("ButtonSFX", SFXclip); }
    public void HarvestAudioPlay() //수확 효과음
    { SFXPlay("HarvestSFX", HarvestClip); }
    public void TadaAudioPlay() 
    { SFXPlay("TadaSFX", TadaClip); }


    public void BgSoundPlay(AudioClip clip) {
        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = false;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }
}
