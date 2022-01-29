using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : SingletonScriptableObject<EntityManager>
{
    private List<GameEntity> _entities = new List<GameEntity>();

    public void PrepareGame()
    {
        if(_entities.Count > 0)
        {
            foreach(GameEntity entity in _entities)
            {
                Destroy(entity.gameObject);
            }
        }
        _entities.Clear();
    }

    public void RegisterEntity(GameEntity entity)
    {
        if(!_entities.Contains(entity))
        {
            _entities.Add(entity);
        }
    }

    public void UnregisterEntity(GameEntity entity)
    {
        if(_entities.Contains(entity))
        {
            _entities.Remove(entity);
        }
    }

    public void ResetLevel()
    {
        foreach(GameEntity entity in _entities)
        {
            entity.Reset();
        }
    }
}
