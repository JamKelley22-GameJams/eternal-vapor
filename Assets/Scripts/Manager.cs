using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.UI;
using System;


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
    public Sprite barelyOpenEye { get { return _barelyOpenEye; } }
    public Sprite slightlyOpenEye { get { return _slightlyOpenEye; } }
    public Sprite openEye { get { return _openEye; } }
    public SpriteRenderer eye { get { return _eye; } }
    public Sprite thirdEyeOpen { get { return _thirdEyeOpen; } }
    public Material PC_Materal { get { return _PC_Materal; } }

    [Header("Buttons")]
    public ButtonWrapper StartButton;
    public ButtonWrapper SettingsButton;
    public ButtonWrapper CreditsButton;
    public ButtonWrapper QuitButton;

    [Header("Common used vars")]
    [SerializeField, Tooltip("The Player GameObject")]
    private GameObject _PlayerGameObject;
    [SerializeField, Tooltip("The Player GameObject")]
    private AudioMixerGroup _AudioMixerGroup;
    [SerializeField]
    private Sprite _barelyOpenEye;
    [SerializeField]
    private Sprite _slightlyOpenEye;
    [SerializeField]
    private Sprite _openEye;
    [SerializeField]
    private SpriteRenderer _eye;
    [SerializeField]
    private Sprite _thirdEyeOpen;
    [SerializeField]
    private Material _PC_Materal;

    public GameObject PauseMenuUI;
    private bool gamePaused;

    void Start()
    {
        //Add onClick Listeners 
        /*
        if(SceneManager.GetActiveScene().name == SceneNames.Title)
        {
            StartButton.btn.onClick.AddListener(delegate { GoToScene(SceneNames.Main); });
            SettingsButton.btn.onClick.AddListener(delegate { GoToScene(SceneNames.Settings); });
            CreditsButton.btn.onClick.AddListener(delegate { GoToScene(SceneNames.Credits); });
            QuitButton.btn.onClick.AddListener(delegate { QuitGame(); });
        }
        */

        //Debug.Log("PlayMusic");
        AudioManager.Instance.PlayMusic("SuperSynthAction");
        try
        {
            PauseMenuUI.SetActive(false);
        }
        catch(Exception e)
        {

        }
    }

    void Update()
    {
        //UnityEngine.Debug.Log("Actual: " + PC_Materal.GetFloat("___AlpahClip___"));
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void GoToScene(string SceneName)
    {
        try
        {
            ResumeGame();
        }
        catch(Exception e)
        {

        }
        
        if (SceneName == SceneNames.Title)
        {
            //AudioManager.Instance.PlayMusic("SuperSynthAction");
        }
        else
        {
            AudioManager.Instance.StopOne("SuperSynthAction");
        }
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator DoPlayerReturn(GameObject go, Vector3 startPosition, bool restart = false)
    {
        float alphaClip = 0f;
        float disapearTime = 50;
        PC_Materal.SetFloat("___AlpahClip___", 0f);
        while (alphaClip < 1f)
        {
            //UnityEngine.Debug.Log(alphaClip);
            PC_Materal.SetFloat("___AlpahClip___", alphaClip);
            //UnityEngine.Debug.Log("Actual: " + PC_Materal.GetVector("___AlpahClip___"));
            alphaClip += (1 / disapearTime);
            yield return null;
        }
        go.transform.position = startPosition;
        go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PC_Materal.SetFloat("___AlpahClip___", 0f);
        if(restart)
        {
            AudioManager.Instance.StopOne("SuperSynthAction");
            AudioManager.Instance.PlayMusic("SuperSynthAction");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.ChangePitch("SuperSynthAction", .75f);
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Instance.ChangePitch("SuperSynthAction", 1f);
    }

    public void ResetScene()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnApplicationQuit()
    {
        
    }
}

[System.Serializable]
public class ButtonWrapper
{
    public Button btn;
    public GameObject selected;
}