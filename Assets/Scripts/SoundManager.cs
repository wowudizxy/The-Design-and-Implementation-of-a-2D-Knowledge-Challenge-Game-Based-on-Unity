using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    private ScriptObjectAudioClip audioClip;
    private AudioSource audioSource;
    private AudioSource bgmSource; // 新增：专门用于背景音乐的AudioSource
    [SerializeField] private float volume = 1f;
    [SerializeField] private AudioClip backgroundMusic; // 新增：背景音乐

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 100;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = volume;

        // 新增：创建专门用于背景音乐的AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.2f;

        audioClip = Resources.Load<ScriptObjectAudioClip>("Data/AudioClips");
        PlayBackgroundMusic();
    }

    // 新增：播放背景音乐方法
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    // 新增：停止背景音乐方法
    public void StopBackgroundMusic()
    {
        bgmSource.Stop();
    }

    // 播放音效方法
    public void PlaySoundKey()
    {
            audioSource.PlayOneShot(audioClip.audioClips[0]);
    }
    public void PlaySoundError()
    {
        
            audioSource.PlayOneShot(audioClip.audioClips[1]);
    }
    public void PlaySoundCorrect()
    {
        
            audioSource.PlayOneShot(audioClip.audioClips[2]);
    }
    public void PlaySoundPickUp()
    {
        
            audioSource.PlayOneShot(audioClip.audioClips[3]);
    }
    public void PlaySoundGetCoin()
    {
            audioSource.PlayOneShot(audioClip.audioClips[4]);
    }
    public void PlaySoundFail()
    {
        audioSource.PlayOneShot(audioClip.audioClips[5]);
    }
    public void PlaySoundSuccess()
    {
        audioSource.PlayOneShot(audioClip.audioClips[6]);
    }

    // 设置音量
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 1f);
        audioSource.volume = volume;
    }
}