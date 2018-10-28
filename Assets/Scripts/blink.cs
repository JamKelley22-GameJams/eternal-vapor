using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class blink : MonoBehaviour
{
    private Image i;

    private void Start()
    {
        i = GetComponent<Image>();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while(true)
        {
            Debug.Log("test");
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0f);
            yield return new WaitForSeconds(1f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1f);
        }
    }
}
