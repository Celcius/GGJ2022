using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public enum PlayerDir
    {
        Right,
        Left,
        Up,
        Down
    }

    [SerializeField]
    private PlayerDir _startDir;

    [SerializeField]
    private Transform _playerPrefab;

    private void Start()
    {
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        float rotation = 0;
        Vector2 direction = LevelInstantiator.Instance.GridSize;
        switch(_startDir)
        {
            case PlayerDir.Right:
                rotation = 0;
                direction.y = 0;
                break;
            case PlayerDir.Left:
                rotation = 180;
                direction.x = -direction.x;
                direction.y = 0;
                break;
            case PlayerDir.Up:
                rotation = -270;
                direction.x = 0;
                break;
            case PlayerDir.Down:
                rotation = 90;
                direction.x = 0;
                direction.y = -direction.y;
                break;
        }
        Transform instance = Instantiate(_playerPrefab, transform.position, Quaternion.Euler(0,rotation,0));
        AvatarController controller = instance.GetComponent<AvatarController>();
        controller.startDirection = direction;
        controller.OnReset();
    }
}
