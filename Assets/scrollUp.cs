using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollUp : MonoBehaviour
{
    [Range(1, 50)]
    public float multiplier;
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * multiplier);
    }
}
