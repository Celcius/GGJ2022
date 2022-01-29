using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : SingletonScriptableObject<EntityManager>
{
    private List<GameEntity> _entities = new List<GameEntity>();
    
    public  AvatarController _darkAvatar;
    public  AvatarController _lightAvatar;

    public void PrepareGame()
    {
        List<GameEntity> toDestroy = new List<GameEntity>();

        if(_entities.Count > 0)
        {
            foreach(GameEntity entity in _entities)
            {
                if(!entity.KeepEntity)
                {
                    toDestroy.Add(entity);
                }
            }
        }

        foreach(GameEntity entity in toDestroy)
        {
            _entities.Remove(entity);
            Destroy(entity.gameObject);
        }
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
        LevelManager.Instance.inputEnabled = true;
        foreach(GameEntity entity in _entities)
        {
            entity.Reset();
        }
    }
}
