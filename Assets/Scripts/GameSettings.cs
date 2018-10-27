using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool SFX { get { return _SFX; } }

    [SerializeField, Tooltip("The SFX value for the game")]
    private GameObject _SFX;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
