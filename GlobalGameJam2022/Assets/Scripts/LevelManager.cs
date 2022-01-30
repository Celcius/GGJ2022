using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;
using TMPro;

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

    public bool HasMoved = false;
    
    [SerializeField]
    private AvatarArrVar _availableAvatars;

    [SerializeField]
    private AvatarArrVar _finishedAvatars;

    [SerializeField]
    private Animator _poemController;

    [SerializeField]
    private PoemLines _poems;

    [SerializeField]
    private TextMeshProUGUI _poemLabel;

    public bool inputEnabled = false;

    [SerializeField]
    private AudioClip _restartSound;

    [SerializeField]
    private AudioClip _finishSound;

    [SerializeField]
    private Board _board;
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
                if(HasMoved)
                {
                    SoundSystem.Instance.PlaySound(_restartSound);
                }
                    
                resetTimer = 0;
                EntityManager.Instance.ResetLevel();
                HasMoved = false;
                EnableInput();
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
            int index = _selectedLevel.Value;
            _poemController.SetTrigger("Enter");
            _poemLabel.text = _poems.Lines[index];
            DisableInput();

            StartCoroutine(LoadLevelRoutine(index));
        }
        else
        {
            UnityEngine.Debug.Log("End of game");
        }
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }

    public void DisableInput()
    {
        inputEnabled = false;
    }

    private IEnumerator LoadLevelRoutine(int index)
    {
        yield return new WaitForSeconds(2.0f);
        LevelInstantiator.Instance.InstantiateLevel(_levels.Value[index]);
        _board.SetBoardIndex(0);
        yield return new WaitForSeconds(1.0f);
        EnableInput();
    }

    public void FinishLevel()
    {
        SoundSystem.Instance.PlaySound(_finishSound);
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
        if(newVal == null 
           || _availableAvatars == null 
           || newVal.Length <= 0 
           || newVal.Length != _availableAvatars.Value.Length)
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
        else if(newVal.Length == 1 || (newVal[0].isDark != newVal[1].isDark))
        {
            FinishLevel();
        }
    }

}
