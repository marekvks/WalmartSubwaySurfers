using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource music;

    [Header("Audio Clips")]
    public AudioClip Music_Main;
    public AudioClip Music_PauseMenu;
    public AudioClip SFX_Swoosh;
    public AudioClip SFX_StartRun;

    private void Start()
    {
        if (SFX_StartRun != null) sfx.PlayOneShot(SFX_StartRun);
        music.clip = Music_Main;
        music.Play();
    }
    
    public void PlaySFXSound(AudioClip sound)
    {
        sfx.PlayOneShot(sound);
    }
}
