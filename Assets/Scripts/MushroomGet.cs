using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MushroomGet : MonoBehaviour
{

    public Vector3 StartPosition;
    public SpriteRenderer spriteRenderer;
    public AudioClip collectSound;
    public float displayTime = 5.0f;
    public string walljumpMessage;
    public string dashMessage;
    public string teleportMessage;
    public string currentMessage = "Hello";
    public bool displayMessage;

    [Range(0,3), Tooltip("Time before mushroom is destroyed")]
    public float TTL = 1f;

    [Range(0,3), Tooltip("Control how fast the mushroom shrinks")]
    public float MushroomShrinkValue = 1.1f;

    private Material pcMat;

    public GameObject WinScreen;

    // Start is called before the first frame update
    void Start()
    {
        pcMat = Manager.Instance.PC_Materal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject go = col.gameObject;
        if (go.tag == "Player")
        {
            
            AudioSource.PlayClipAtPoint(collectSound, this.transform.position);
            if (this.gameObject.tag == "Walljump")
            {
                StartCoroutine(Manager.Instance.DoPlayerReturn(go, StartPosition));
                Manager.Instance.eye.sprite = Manager.Instance.barelyOpenEye;
                go.GetComponent<Movement>().canWalljump = true;

            }
            if (this.gameObject.tag == "Dash")
            {
                StartCoroutine(Manager.Instance.DoPlayerReturn(go, StartPosition));
                Manager.Instance.eye.sprite = Manager.Instance.slightlyOpenEye;
                go.GetComponent<Movement>().canDash = true;

            }
            if (this.gameObject.tag == "VaporTeleport")
            {
                StartCoroutine(Manager.Instance.DoPlayerReturn(go, StartPosition));
                Manager.Instance.eye.sprite = Manager.Instance.openEye;
                go.GetComponent<Movement>().canTeleport = true;

            }
            if (this.gameObject.tag == "Last")
            {
                Manager.Instance.eye.sprite = Manager.Instance.thirdEyeOpen;
                StartCoroutine(Win());
            }
            StartCoroutine(DoPickupMushroom());
        }
    }

    IEnumerator DoPickupMushroom()
    {
        //Destroy(this.gameObject, TTL);
        while(transform.localScale.x > 0)
        {
            transform.localScale *= 1/MushroomShrinkValue;
            yield return null;
        }
    }

    IEnumerator Win()
    {
        //AudioManager.Instance.PlaySFX("Win");
        Time.timeScale = .5f;
        AudioManager.Instance.ChangePitch("SuperSynthAction", .5f);
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.ChangePitch("SuperSynthAction", 1f);
        float numSec = 10f;
        Time.timeScale = 1f;
        WinScreen.SetActive(true);
        yield return new WaitForSeconds(numSec);
        //WinScreen.SetActive(false);
        SceneManager.LoadScene(3);
    }
}
