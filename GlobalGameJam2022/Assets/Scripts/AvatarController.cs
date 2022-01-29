using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour, IResettableCallback
{
    public Vector2 startDirection;
    public Vector2 direction = new Vector2(200f, 0f);
    public GameObject arrow;
    public Board board;
    private Rigidbody2D body;
    private bool toMove = false;
    public bool finished = false;
    public bool isDark = false;

    void Start() {
        board = FindObjectsOfType<Board>()[0];
        body = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(Input.GetKeyUp(board.moveKey)) {
            toMove = true;
        }
    }

    void FixedUpdate() {
        if(toMove && !finished) {
            body.MovePosition(transform.position + new Vector3(direction.x, direction.y));
            toMove = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<ChangingArrow>() != null) {
            ChangingArrow changingArrow = other.gameObject.GetComponent<ChangingArrow>();
            if(changingArrow.isDark == isDark) {
                arrow.transform.rotation = other.transform.rotation;
                direction = changingArrow.newDirection;
            }
        } else if (other.gameObject.GetComponent<FinishLine>() != null) {
            FinishLine finishLine = other.gameObject.GetComponent<FinishLine>();
            if(finishLine.isDark == isDark) {
                print("Collision!");
                LevelManager.Instance.FinishLevel();
                finished = true;
            }
        }
    }

    public void OnReset()
    {
        direction = startDirection;
        arrow.transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.right, direction),0);
    }
}
