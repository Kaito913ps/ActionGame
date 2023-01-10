using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] Fade _fade;

    //ボタンを押したら1秒間演出があって、シーン移動

    public void OnNextScene()
    {
        _fade.FadeIn(1f, () =>
        SceneManager.LoadScene(_sceneName));
    }
}
