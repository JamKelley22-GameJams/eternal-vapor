using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    private static GameSettings _instance;
    public static GameSettings Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public bool SFX { get; set; } = true;

    public AudioMixer mixer;

    public Dropdown qualityDropdown;


    void Start()
    {
        if(qualityDropdown)
            qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    void Update()
    {
        
    }

    public void SetVolume(float vol)
    {
        mixer.SetFloat("Volume", vol);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void isFullscreen(bool val)
    {
        Screen.fullScreen = val;
    }
}
