using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private IntVar _selectedLevel;

    [SerializeField]
    private IntVar _unlockedLevels;

    [SerializeField]
    private LevelDefinitionRegistry _levels;

    [SerializeField]
    private float timeToReset = 0.15f;
    private float resetTimer = 0;

    [SerializeField]
    private AvatarArrVar _finishedAvatars;

    public void Start()
    {
        LoadCurrentLevel();
    }

    public void OnEnable()
    {
        _finishedAvatars.OnChange += CheckAvatarsFinished;
    }

    public void OnDisable()
    {
        _finishedAvatars.OnChange -= CheckAvatarsFinished;
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
            _finishedAvatars.Clear();
            LevelInstantiator.Instance.InstantiateLevel(_levels.Value[_selectedLevel.Value]);    
        }
        else
        {
            UnityEngine.Debug.Log("End of game");
        }
    }

    public void FinishLevel()
    {
        NextLevel();
        _unlockedLevels.Value = Mathf.Max(_unlockedLevels.Value, 
                                          _selectedLevel.Value);
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

    public void CheckAvatarsFinished(AvatarController[] oldVal, AvatarController[] newVal)
    {
        if(newVal == null || newVal.Length < 2)
        {
            return;
        }

        for(int i = 0; i < newVal.Length; i++)
        {
            if(newVal == null)
            {
                return;
            }
        }

        if(newVal.Length > 2)
        {
            UnityEngine.Debug.LogError("More than one avatar registered");
        }
        else if(newVal[0].isDark != newVal[1].isDark)
        {
            FinishLevel();
        }
    }

}
