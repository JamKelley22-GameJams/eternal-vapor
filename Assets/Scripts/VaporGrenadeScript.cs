using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporGrenadeScript : MonoBehaviour
{
    public AudioClip collideSound;
    private Collider2D collider;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        GameObject[] playerArr = GameObject.FindGameObjectsWithTag("Player");
        if (playerArr.Length == 1)
        {
            AudioSource.PlayClipAtPoint(collideSound, this.transform.position);
            playerArr[0].gameObject.transform.position = this.transform.position;
        }
        Destroy(this.gameObject);
    }
}
