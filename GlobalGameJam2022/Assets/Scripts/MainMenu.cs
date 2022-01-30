using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AmoaebaUtils;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private IntVar _currentLevel;

    public void  OnClick()
    {
        _currentLevel.Value = 0;
        SceneManager.LoadScene("LevelScene");
    }
}
