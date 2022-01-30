using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : SingletonScriptableObject<EntityManager>
{
    private List<GameEntity> _entities = new List<GameEntity>();
    
    public  AvatarController _darkAvatar;
    public  AvatarController _lightAvatar;
    private List<AvatarController> avatars = new List<AvatarController>();
    private List<SwitchingArrow> switchingArrows = new List<SwitchingArrow>();

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

    public void RegisterAvatar(AvatarController avatar)
    {
        if(!avatars.Contains(avatar))
        {
            avatars.Add(avatar);
        }
    }

    public void RegisterSwitchingArrow(SwitchingArrow arrow)
    {
        if(!switchingArrows.Contains(arrow))
        {
            switchingArrows.Add(arrow);
        }
    }

    public void UnregisterSwitchingArrow(SwitchingArrow arrow)
    {
        if(switchingArrows.Contains(arrow))
        {
            switchingArrows.Remove(arrow);
        }
    }

    public void UnregisterAvatar(AvatarController avatar)
    {
        if(avatars.Contains(avatar))
        {
            avatars.Remove(avatar);
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

    public AvatarController getOtherAvatar(AvatarController mainAvatar) {
        if(avatars.Contains(mainAvatar) && avatars.Count == 2) {
            return avatars[1 - avatars.IndexOf(mainAvatar)];
        } else {
            return null;
        }
    }
}
