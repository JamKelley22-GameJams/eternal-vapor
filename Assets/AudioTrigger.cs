using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTrigger : MonoBehaviour
{

    public AudioClip colliderAudio;
    AudioSource audioSource;
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
