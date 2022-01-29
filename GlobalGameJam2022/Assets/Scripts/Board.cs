using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private SpriteRenderer _renderer;

    private List<IBoardListener> boardListeners = new List<IBoardListener>();

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(Input.GetKeyDown(switchKey)) {
            if(boardIndex == 1) {
                _renderer.sprite = boards[0];
            } else {
                _renderer.sprite = boards[1];
            }
            boardIndex = 1 - boardIndex;
            BroadCastBoardChange();
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
