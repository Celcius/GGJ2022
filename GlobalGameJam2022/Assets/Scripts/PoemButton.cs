using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoemButton : MonoBehaviour
{
    [SerializeField]
    private IntVar _currentLevel;

    public int LevelIndex { get; set; }
    public void OnButtonClick()
    {
        _currentLevel.Value = LevelIndex;
        SceneManager.LoadScene("LevelScene");
    }
}
