using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2 boardSquareSize => LevelInstantiator.Instance.GridSize;
    public KeyCode moveKey = KeyCode.Space;
}
