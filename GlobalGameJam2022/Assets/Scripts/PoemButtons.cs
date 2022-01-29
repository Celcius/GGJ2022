using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PoemButtons : MonoBehaviour
{
    [SerializeField]
    private PoemLines _poem;

    [SerializeField]
    private IntVar _unlockedPoem;

    [SerializeField]
    private Transform _buttonPrefab;

    [SerializeField]
    private Transform _buttonsParent;

    private void Start()
    {
        for(int i = 0; i <= _unlockedPoem.Value; i++)
        {
            if(i < _poem.Lines.Length)
            {
                Transform button = Instantiate(_buttonPrefab, _buttonsParent);
                TextMeshProUGUI meshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                meshPro.text = _poem.Lines[i];
                button.GetComponentInChildren<PoemButton>().LevelIndex = i;
            }
        }        
    }
}
