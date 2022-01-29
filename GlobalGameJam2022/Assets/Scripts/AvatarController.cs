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

    [SerializeField]
    private AvatarArrVar _finishedAvatars;

    void Start() {
        _finishedAvatars.Remove(this);
        board = FindObjectsOfType<Board>()[0];
        body = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(Input.GetKeyUp(board.moveKey) && !finished && LevelManager.Instance.inputEnabled) {
            toMove = true;
        }
    }

    void FixedUpdate() {
        if(toMove) {
            body.MovePosition(transform.position + new Vector3(direction.x, direction.y));
            toMove = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<ChangingArrow>() != null) {
            ChangingArrow changingArrow = other.gameObject.GetComponent<ChangingArrow>();
            if(changingArrow.IsDark == isDark) {
                arrow.transform.rotation = other.transform.rotation;
                direction = changingArrow.newDirection;
            }
        } else if (other.gameObject.GetComponent<FinishLine>() != null) {
            FinishLine finishLine = other.gameObject.GetComponent<FinishLine>();
            if(finishLine.isDark == isDark) {
                print("Collision! " + (isDark? "Dark" : "Light"));
                finished = true;
                _finishedAvatars.Add(this);
            }
        } else if (other.gameObject.GetComponent<AvatarController>() != null) {
            StartCoroutine(gameOver());
        }
    }

    public IEnumerator gameOver() {
        yield return new WaitForEndOfFrame();
        LevelManager.Instance.inputEnabled = false;
        EntityManager.Instance.ResetLevel();
    }

    public void OnReset()
    {
        _finishedAvatars.Remove(this);
        finished = false;
        direction = startDirection;
        arrow.transform.rotation = Quaternion.Euler(0, Vector2.Angle(Vector2.right, direction),0);
    }
}
