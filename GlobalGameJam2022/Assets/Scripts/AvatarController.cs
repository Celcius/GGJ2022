using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Vector2 direction = new Vector2(200f, 0f);
    public GameObject arrow;
    public Board board;
    private Rigidbody2D body;
    private bool toMove = false;

    void Start() {
        body = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(Input.GetKeyUp(board.moveKey)) {
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
            arrow.transform.rotation = other.transform.rotation;
            direction = changingArrow.newDirection;
        }
    }
}