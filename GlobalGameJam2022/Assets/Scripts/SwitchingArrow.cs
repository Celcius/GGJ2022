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

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        EntityManager.Instance.UnregisterSwitchingArrow(this);
    }
}
