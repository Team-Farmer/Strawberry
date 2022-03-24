using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else { Destroy(gameObject); }
    }
    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1) {
        for (int i = 0; i < bglist.Length; i++) {
            if (arg0.name == bglist[i].name)
            {
                BgSoundPlay(bglist[i]);
            }
        }
    
    }
    //효과음
    public void SFXPlay(string sfxName,AudioClip clip) {

        GameObject go = new GameObject(sfxName+"Sound");//해당이름을 가진 오브젝트가 소리 날때마다 생성된다.
        
        //오디오 재생
        AudioSource audioSource = go.AddComponent<AudioSource>();//그 오브젝트에 오디오 소스 컴포넌트 추가
        audioSource.clip = clip;//오디오 소스 클립 추가
        audioSource.Play();//재생

        Destroy(go, clip.length);//효과음 재생 후(clip.length 시간 지난후) 파괴
    
    }
    /*다른스크립트에 적용
    public AudioClip clip;
    AudioManager.instance.SFXPlay("Hook",clip);
    */

    public void BgSoundPlay(AudioClip clip) {

        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();


    
    }
}
