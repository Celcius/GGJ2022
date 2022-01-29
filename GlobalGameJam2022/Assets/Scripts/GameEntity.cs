using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResettableCallback
{
    public void OnReset();
}

public class GameEntity : MonoBehaviour
{
    [SerializeField]
    private bool _isResettable = false;

    private Vector3 _initialPos;

    private IResettableCallback[] _resettables;

    public void Awake()
    {
        EntityManager.Instance.RegisterEntity(this);
        _initialPos = transform.position;
        _resettables = GetComponentsInChildren<IResettableCallback>();
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
            
            foreach(IResettableCallback resettable in _resettables)
            {
                resettable.OnReset();
            }
        }
    }
}
