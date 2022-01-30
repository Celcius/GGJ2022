using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public class AvatarController : MonoBehaviour, IResettableCallback
{
    public Vector2 startDirection;
    public Vector2 direction = new Vector2(200f, 0f);
    public int startRotation;
    public int rotation;
    public List<GameObject> arrows;
    public Board board;
    private Rigidbody2D body;
    private bool toMove = false;
    public bool finished = false;
    public bool isDark = false;

    [SerializeField]
    private AudioClip _collisionSound;

        [SerializeField]
    private AudioClip _turnSound;


    [SerializeField]
    private AudioClip _finishLineSound;

    [SerializeField]
    private AudioClip[] _stepSounds;

    [SerializeField]
    private AvatarArrVar _finishedAvatars;

        [SerializeField]
    private AvatarArrVar _registeredAvatars;

    void Awake() {
        EntityManager.Instance.RegisterAvatar(this);
    }

    void OnDestroy() {
        EntityManager.Instance.UnregisterAvatar(this);
        _registeredAvatars.Remove(this);
    }

    void Start() {
        _registeredAvatars.Add(this);
        _finishedAvatars.Remove(this);
        board = FindObjectsOfType<Board>()[0];
        body = this.GetComponent<Rigidbody2D>();
        changeRotation(startRotation);
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
            SoundSystem.Instance.PlaySound(_stepSounds[isDark? 0 : 1]);
            LevelManager.Instance.HasMoved = true;
            EntityManager.Instance.enableAllArrows();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<ChangingArrow>() != null) {
            board.canSwitch = false;
            ChangingArrow changingArrow = other.gameObject.GetComponent<ChangingArrow>();
            if(changingArrow.IsDark == isDark) {
                changeRotation(changingArrow.rotation);
                direction = changingArrow.newDirection;

                if(!SoundSystem.Instance.IsPlaying("Turn" + (isDark?"Dark" :"Light")))
                {
                    SoundSystem.Instance.PlaySound(_turnSound, "Turn"+ (isDark?"Dark" :"Light"));
                }
            }
            board.canSwitch = true;
        } else if (other.gameObject.GetComponent<SwitchingArrow>() != null) {
            AvatarController otherAvatar = EntityManager.Instance.getOtherAvatar(this);
            other.gameObject.GetComponent<SwitchingArrow>().disable();
            otherAvatar.GetComponent<BoxCollider2D>().isTrigger = true;
            Vector3 newPos = otherAvatar.transform.position;
            otherAvatar.transform.position = transform.position;
            transform.position = newPos;
            otherAvatar.GetComponent<BoxCollider2D>().isTrigger = false;
        } else if (other.gameObject.GetComponent<FinishLine>() != null) {
            FinishLine finishLine = other.gameObject.GetComponent<FinishLine>();
            if(finishLine.isDark == isDark) {
                print("Collision! " + (isDark? "Dark" : "Light"));
                finished = true;
                _finishedAvatars.Add(this);

                SoundSystem.Instance.PlaySound(_finishLineSound, "Finish");
            }
        } else if (other.gameObject.GetComponent<Board>() != null) 
        {
            StartCoroutine(gameOver());
            if(SoundSystem.Instance.IsPlaying("Collision"))
            {
                SoundSystem.Instance.StopSound("Collision");
            }
            SoundSystem.Instance.PlaySound(_collisionSound, "Collision");
        } 
        else if (other.gameObject.GetComponent<AvatarController>() != null) {
            StartCoroutine(gameOver());
            if(SoundSystem.Instance.IsPlaying("Collision"))
            {
                SoundSystem.Instance.StopSound("Collision");
            }
            SoundSystem.Instance.PlaySound(_collisionSound, "Collision");
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
        changeRotation(startRotation);
    }

    public void changeRotation(int newRotation) {
        rotation = newRotation;
        foreach(GameObject arrow in arrows) {
            arrow.SetActive(false);
        }
        arrows[newRotation].SetActive(true);
    }
}
