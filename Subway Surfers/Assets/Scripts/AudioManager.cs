using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer AudioMixer;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource music;

    [Header("Audio Clips")]
    public AudioClip Music_Main;
    public AudioClip Music_PauseMenu;
    public AudioClip SFX_Swoosh;
    public AudioClip SFX_StartRun;

    [Header("Volume Sliders")]
    public Slider MasterVolSlider;
    public Slider MusicVolSlider;
    public Slider SFXVolSlider;

    [Header("Json Serialization")]
    private string m_Path;

    class SaveOptions
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;
    }

    private void Start()
    {
        m_Path = Application.persistentDataPath + "/options.json";
        LoadFromJson();
        
        if (SFX_StartRun != null) sfx.PlayOneShot(SFX_StartRun);
        music.clip = Music_Main;
        music.Play();
    }
    
    public void PlaySFXSound(AudioClip sound)
    {
        sfx.PlayOneShot(sound);
    }

    public void ChangeVolume()
    {
        AudioMixer.SetFloat("MasterVol", MasterVolSlider.value);
        AudioMixer.SetFloat("MusicVol", MusicVolSlider.value);
        AudioMixer.SetFloat("SFXVol", SFXVolSlider.value);

        SaveOptions currentOptions = new SaveOptions()
        {
            MasterVolume = MasterVolSlider.value,
            MusicVolume = MusicVolSlider.value,
            SFXVolume = SFXVolSlider.value
        };
        
        SaveToJson(currentOptions);
    }
    
    // Json Serialization
    
    private void SaveToJson(SaveOptions saveOptions)
    {
        string json = JsonConvert.SerializeObject(saveOptions, Formatting.None);
        File.WriteAllText(m_Path, json);
    }

    private void LoadFromJson()
    {
        if (!File.Exists(m_Path))
        {
            SaveOptions defaultConfig = new SaveOptions()
            {
                MasterVolume = 0f,
                MusicVolume = 0f,
                SFXVolume = 0f
            };
            
            SaveToJson(defaultConfig);

            return;
        }

        string json = File.ReadAllText(m_Path);
        SaveOptions loadOptions = JsonConvert.DeserializeObject<SaveOptions>(json);

        MasterVolSlider.value = loadOptions.MasterVolume;
        MusicVolSlider.value = loadOptions.MusicVolume;
        SFXVolSlider.value = loadOptions.SFXVolume;
        
        ChangeVolume();
    }
}
