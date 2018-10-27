using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.UI;

/*
 * The Main Manager Class that handles most control
 */
public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public static class SceneNames
    {
        public static string
            Main = "Main",
            End = "End",
            Settings = "Settings",
            Title = "Title",
            Credits = "Credits";
    }

    public GameObject PlayerGameObject { get { return _PlayerGameObject; } }
    public AudioMixerGroup AudioMixerGroup { get { return _AudioMixerGroup; } }

    [Header("Buttons")]
    public Button StartButton;
    public Button SettingsButton;
    public Button CreditsButton;

    [Header("Common used vars")]
    [SerializeField, Tooltip("The Player GameObject")]
    private GameObject _PlayerGameObject;
    [SerializeField, Tooltip("The Player GameObject")]
    private AudioMixerGroup _AudioMixerGroup;

    void Start()
    {
        //Add onClick Listeners 
        StartButton.onClick.AddListener(delegate {GoToScene(SceneNames.Main);});
        SettingsButton.onClick.AddListener(delegate { GoToScene(SceneNames.Settings); });
        CreditsButton.onClick.AddListener(delegate { GoToScene(SceneNames.Credits); });
    }

    void Update()
    {
        
    }

    void GoToScene(string SceneName)
    {
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName);
    }

    void OnApplicationQuit()
    {
        
    }
}