using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class SpeedBlock : MonoBehaviour
{

    public float colliderDisabledTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (colliderDisabledTime > 0)
        {
            colliderDisabledTime = colliderDisabledTime - Time.deltaTime;
        } else
        {
            this.GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Movement>().timeKeeper > 0.0f)
            {
                this.GetComponent<Collider2D>().enabled = false;
                colliderDisabledTime = 2.0f;
            }
        }

    }
}
