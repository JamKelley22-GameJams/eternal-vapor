using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public void FootFallL()
    {
        AudioManager.Instance.PlaySFX("FootFallL");
        //Show footstep sprite
    }

    public void FootFallR()
    {
        AudioManager.Instance.PlaySFX("FootFallR");
        //Show footstep sprite
    }
}
