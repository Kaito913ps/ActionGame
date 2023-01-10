using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] Fade _fade;

    //�{�^������������1�b�ԉ��o�������āA�V�[���ړ�

    public void OnNextScene()
    {
        _fade.FadeIn(1f, () =>
        SceneManager.LoadScene(_sceneName));
    }
}
