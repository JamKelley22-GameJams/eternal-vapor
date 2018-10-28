using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MushroomGet : MonoBehaviour
{

    public Vector3 StartPosition;
    public SpriteRenderer spriteRenderer;
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
            StartCoroutine(Manager.Instance.DoPlayerReturn(go, StartPosition));
            if (this.gameObject.tag == "Walljump")
            {
                Manager.Instance.eye.sprite = Manager.Instance.barelyOpenEye;
                go.GetComponent<Movement>().canWalljump = true;

            }
            if (this.gameObject.tag == "Dash")
            {
                Manager.Instance.eye.sprite = Manager.Instance.slightlyOpenEye;
                go.GetComponent<Movement>().canDash = true;

            }
            if (this.gameObject.tag == "Grenade")
            {
                Manager.Instance.eye.sprite = Manager.Instance.openEye;
                go.GetComponent<Movement>().canTeleport = true;

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

    
}
