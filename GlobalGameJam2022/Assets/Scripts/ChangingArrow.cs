using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingArrow : MonoBehaviour
{
    public Board board;
    public Vector2 newDirection;
    public bool isDark = false;

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

        isDark = ((transform.localPosition.x / board.boardSquareSize.x + transform.localPosition.y / board.boardSquareSize.y) % 2) >= 1f;
    }

    void Update() {
        if(Input.GetKeyUp(board.switchKey)) {
            isDark = !isDark;
        }
    }
}
