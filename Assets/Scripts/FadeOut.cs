using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] Fade _fade;

    private void Start()
    {
        _fade.FadeOut(1f);
    }
}
