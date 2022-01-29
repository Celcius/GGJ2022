using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemLines : ScriptableObject
{
    [SerializeField]
    private string[] _lines;
    public string[] Lines => _lines;
}
