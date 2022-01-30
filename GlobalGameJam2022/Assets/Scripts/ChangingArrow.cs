using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingArrow : MonoBehaviour, IBoardListener
{
    public Board board;
    public Vector2 newDirection;
    [SerializeField]
    private bool isDark = false;
    public bool IsDark => isDark;
    public int rotation = 0;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] sprites;

    void Start() {
        board = FindObjectsOfType<Board>()[0];
        if(transform.eulerAngles.z < 90) {
            newDirection = new Vector2(board.boardSquareSize.x, 0);
        } else if(90 <= transform.eulerAngles.z && transform.eulerAngles.z < 180) {
            newDirection = new Vector2(0, board.boardSquareSize.y);
        } else if(180 <= transform.eulerAngles.z && transform.eulerAngles.z < 270) {
            newDirection = new Vector2(-board.boardSquareSize.x, 0);
        } else {
            newDirection = new Vector2(0, -board.boardSquareSize.y);
        }

        board.AddListener(this);
        OnBoardChangeIndex(board.boardIndex);
    }

    private void SetIsDark(bool isDark)
    {
        this.isDark = isDark;
        _renderer.sprite = sprites[isDark? 0  : 1];
    }
    
    public void OnBoardChangeIndex(int boardIndex)
    {
        int index = (int)Mathf.Round(Mathf.Abs(transform.localPosition.x / board.boardSquareSize.x) + Mathf.Abs(transform.localPosition.y / board.boardSquareSize.y));
        SetIsDark((index % 2) == boardIndex);
    }

    
    private void OnDestroy()
    {
        board.RemoveListener(this);
    }
}
