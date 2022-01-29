using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _playerPrefab;

    private void Start()
    {
        Instantiate(_playerPrefab, transform.position, Quaternion.identity);
    }
}
