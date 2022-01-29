using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private IntVar _selectedLevel;

    [SerializeField]
    private LevelDefinitionRegistry _levels;

    [SerializeField]
    private float timeToReset = 0.15f;
    private float resetTimer = 0;

    public void Start()
    {
        LoadCurrentLevel();
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            resetTimer += Time.deltaTime;
            if(resetTimer > timeToReset)
            {
                resetTimer = 0;
                EntityManager.Instance.ResetLevel();
            }
        }
        else
        {
            resetTimer = 0;
        }
                
                
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousLevel();
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel();
        }
    }

    private void LoadCurrentLevel()
    {
        int currentLevel = _selectedLevel.Value;
        if(currentLevel < _levels.Value.Length)
        {
            LevelInstantiator.Instance.InstantiateLevel(_levels.Value[_selectedLevel.Value]);    
        }
        else
        {
            UnityEngine.Debug.Log("End of game");
        }
        
    }

    public void PreviousLevel()
    {
        _selectedLevel.Value = Mathf.Clamp(_selectedLevel.Value - 1, 0, _levels.Value.Length);
        LoadCurrentLevel();
    }

    public void NextLevel()
    {
        _selectedLevel.Value = Mathf.Clamp(_selectedLevel.Value + 1, 0, _levels.Value.Length);
        LoadCurrentLevel();
    }



}
