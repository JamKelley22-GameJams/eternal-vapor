using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/*
 * Choke point for all Sounds. Sounds are seperated into Music and SFX
 */
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [Header("Sounds")]
    public Sound[] sounds;
    private AudioMixerGroup amGroup;
    private List<Sound> playingSoundList;
    private Sound lastPlayingVoice;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else { _instance = this; }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        try
        {
            amGroup = Manager.Instance.AudioMixerGroup;
        }
        catch(Exception e)
        {

        }

        //Initalize
        playingSoundList = new List<Sound>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            // Debug.Log(s.source);
            s.source.clip = s.clip;
            if (s.parent)
                s.source.transform.parent = s.parent.transform;//may need to go in update???
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = amGroup;
        }
    }

    private void Start()
    {
        
    }

    public void ChangePitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            s.source.pitch = Mathf.Clamp01(pitch);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }

    /*
	 * Attempt to play the sound(music) from sound list
	 */
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Debug.Log(s);
        //Debug.Log(s.source);
        //Debug.Log(s.name);
        if (s != null)
        {
            s.source.Play();
            this.playingSoundList.Add(s);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }

    /*
	 * Attempt to play the sound(sfx) from sound list
	 */
    public void PlaySFX(string name)
    {
        if (!GameSettings.Instance.SFX)
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            //Debug.Log("Played");
            this.playingSoundList.Add(s);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }

    /*
	 * Attempt to play the sound(voice) from sound list
	 */
    public void PlayVoice(string name)
    {
        //Debug.Log("Why: " + name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            this.lastPlayingVoice = s;
            this.playingSoundList.Add(s);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }

    public void StopCurrentVoice()
    {
        if (this.lastPlayingVoice != null)
        {
            this.lastPlayingVoice.source.Stop();
            this.lastPlayingVoice = null;
        }
    }

    /*
	 * Attempt to stop all of the the sounds from playingSoundList with name
	 */
    public void StopAll(string name)
    {
        //UpdatePlayingAudioSourceList();
        foreach (Sound sound in this.sounds)
        {
            if (sound.name == name)
            {
                sound.source.Stop();
            }
        }
    }

    /*
	 * Attempt to stop all of the the sounds from playingSoundList
	 */
    public void StopAll()
    {
        //UpdatePlayingAudioSourceList();
        foreach (Sound sound in this.sounds)
        {
            sound.source.Stop();
        }
    }

    /*
	 * Attempt to stop one sound from playingSoundList
	 */
    public void StopOne(string name)
    {
        UpdatePlayingAudioSourceList();
        foreach (Sound sound in this.playingSoundList)
        {
            if (sound.name == name)
            {
                sound.source.Stop();
                break;
            }
        }
    }

    void UpdatePlayingAudioSourceList()
    {
        List<Sound> tempPlayingSoundList = this.playingSoundList;//Make a copy so collection is not modified while enumerating
        foreach (Sound sound in tempPlayingSoundList)
        {
            if (!sound.source.isPlaying)
            {
                tempPlayingSoundList.Remove(sound);
            }
        }
        this.playingSoundList = tempPlayingSoundList;
    }
}

public enum AudioType
{
    MUSIC,
    SFX,
    VOICE
}

/*
 * The internal class for holding one Sound and its props
 */
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public GameObject parent;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    public AudioType audioType;

    [HideInInspector]
    public AudioSource source;

}