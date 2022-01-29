using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<Sprite> boards = new List<Sprite>();
    public Vector2 boardSquareSize = new Vector2(2, 1.3f);
    public KeyCode moveKey = KeyCode.Space;
    public KeyCode switchKey = KeyCode.Return;
    public int boardIndex = 0;

    void Update() {
        if(Input.GetKeyUp(switchKey)) {
            if(boardIndex == 1) {
                this.GetComponent<SpriteRenderer>().sprite = boards[0];
            } else {
                this.GetComponent<SpriteRenderer>().sprite = boards[1];
            }
            boardIndex = 1 - boardIndex;
        }
    }
}
