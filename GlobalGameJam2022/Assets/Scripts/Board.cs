using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public interface IBoardListener
{
    public void OnBoardChangeIndex(int boardIndex);
}

public class Board : MonoBehaviour, IResettableCallback
{
    public List<Sprite> boards = new List<Sprite>();
    public Vector2 boardSquareSize = new Vector2(2, 1.3f);
    public KeyCode moveKey = KeyCode.Space;
    public KeyCode switchKey = KeyCode.Return;
    public int boardIndex = 0;

    [SerializeField]
    private AudioClip[] boardAudios;

    private SpriteRenderer _renderer;

    private List<IBoardListener> boardListeners = new List<IBoardListener>();

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        if(Input.GetKeyDown(switchKey) && LevelManager.Instance.inputEnabled) 
        {
            if(boardIndex == 1) {
                _renderer.sprite = boards[0];
            } else {
                _renderer.sprite = boards[1];
            }
            boardIndex = 1 - boardIndex;
            BroadCastBoardChange();
            SoundSystem.Instance.PlaySound(boardAudios[boardIndex]);
        }
    }

    public void OnReset()
    {
        boardIndex = 0;
        BroadCastBoardChange();
        _renderer.sprite = boards[0];
    }

    private void BroadCastBoardChange()
    {
        foreach(IBoardListener listener in boardListeners)
        {
            listener.OnBoardChangeIndex(boardIndex);
        }
    }

    public void AddListener(IBoardListener listener)
    {
        if(!boardListeners.Contains(listener))
        {
            boardListeners.Add(listener);
        }
    }

    public void RemoveListener(IBoardListener listener)
    {
        if(boardListeners.Contains(listener))
        {
            boardListeners.Remove(listener);
        }
    }
}
