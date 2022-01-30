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
        int rotation = 0;
        Vector2 direction = LevelInstantiator.Instance.GridSize;
        switch(_startDir)
        {
            case PlayerDir.Right:
                rotation = 0;
                direction.y = 0;
                //direction = (GridSize.x, 0)
                break;
            case PlayerDir.Left:
                rotation = 1;
                direction.x = -direction.x;
                direction.y = 0;
                //direction = (-GridSize.x, 0)
                break;
            case PlayerDir.Up:
                rotation = 2;
                direction.x = 0;
                //direction = (0, GridSize.y)
                break;
            case PlayerDir.Down:
                rotation = 3;
                direction.x = 0;
                direction.y = -direction.y;
                //direction = (0, -GridSize.y)
                break;
        }
        Transform instance = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        AvatarController controller = instance.GetComponent<AvatarController>();
        controller.startDirection = direction;
        controller.startRotation = rotation;
        controller.OnReset();
    }
}
