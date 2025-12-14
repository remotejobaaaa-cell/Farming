using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    [Space(2)]
    public AudioClip backgroundMusicAudioClip;
    public AudioClip menubackgroundAudioClip;
    public AudioClip clickButtonAudioClip;
    public AudioClip coinsAudioClip;
    public AudioClip confettiAudioClip;
    public AudioClip inGameReward;
    public AudioClip checkPointClip;
    public AudioClip finalCheckPointClip;
    

    [Header("Audio Sources")]
    [Space(2)]
    public AudioSource backgroundMusicAudioSource;
    public AudioSource buttonAudioSource;

    [HideInInspector]
    public Image levelSelectImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
        
    private void OnEnable()
    {
        Play_MenuBackgroundMusic();
    }
    public void Play_MenuBackgroundMusic()
    {
        backgroundMusicAudioSource.clip = menubackgroundAudioClip;
        backgroundMusicAudioSource.Stop();
        //this.GetComponent<AudioListener>().enabled = false;
        backgroundMusicAudioSource.Play();
        backgroundMusicAudioSource.loop = true;

        //this.GetComponent<AudioListener>().enabled = true;
    }
    public void Play_BackgroundMusic()
    {
        backgroundMusicAudioSource.clip = backgroundMusicAudioClip;
        backgroundMusicAudioSource.Stop();
       // this.GetComponent<AudioListener>().enabled = false;
        backgroundMusicAudioSource.Play();
        backgroundMusicAudioSource.loop = true;
        //this.GetComponent<AudioListener>().enabled = true;
    }
    public void Stop_BackgroundMusic()
    {
        backgroundMusicAudioSource.Stop();
    }

    public void Play_ClickButtonSound()
    {
        buttonAudioSource.PlayOneShot(clickButtonAudioClip);
        buttonAudioSource.loop = true;
    }
    
    public void Play_ClickConfettiSound()
    {
        buttonAudioSource.PlayOneShot(confettiAudioClip);
        buttonAudioSource.loop = true;
    }
    public void Play_ClickInGameRewardSound()
    {
        buttonAudioSource.PlayOneShot(inGameReward);
        buttonAudioSource.loop = true;
    }
    public void Play_ClickCoinsSound()
    {
        buttonAudioSource.PlayOneShot(coinsAudioClip);
        buttonAudioSource.loop = true;
    } 
    public void Play_CheckPointSound()
    {
        buttonAudioSource.PlayOneShot(checkPointClip);
        buttonAudioSource.loop = true;
    }
    public void Play_Final_CheckPointSound()
    {
        buttonAudioSource.PlayOneShot(finalCheckPointClip);
        buttonAudioSource.loop = true;
    }
}
