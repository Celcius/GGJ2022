using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    [SerializeField]
    private bool _isResettable = false;

    private Vector3 _initialPos;

    public void Awake()
    {
        EntityManager.Instance.RegisterEntity(this);
        _initialPos = transform.position;
    }

    private void OnDestroy()
    {
        EntityManager.Instance.UnregisterEntity(this);
    }

    public virtual void Reset()
    {
        if(_isResettable)
        {
            transform.position = _initialPos;
        }
    }
}
