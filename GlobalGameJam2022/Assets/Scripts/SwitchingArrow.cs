using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingArrow : MonoBehaviour
{
    public Board board;

    void Awake()
    {
        EntityManager.Instance.RegisterSwitchingArrow(this);
    }

    void Update()
    {
        
    }

    void OnDestroy() {
        EntityManager.Instance.UnregisterSwitchingArrow(this);
    }

    public void disable() {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void enable() {
        this.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
