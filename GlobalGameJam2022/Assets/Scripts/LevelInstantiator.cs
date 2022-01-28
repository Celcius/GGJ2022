using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

[CreateAssetMenu(fileName = "LevelInstantiator", menuName = "Scriptables/LevelInstantiator")]
public class LevelInstantiator : SingletonScriptableObject<LevelInstantiator>
{
    [System.Serializable]
    public struct GridDefinition
    {
        public Vector3 StartPosition;

        public float WidthOffset;

        public float HeightOffset;
    }

    [SerializeField]
    private GridDefinition _gridDefinition;
    
    [System.Serializable]
    public struct InstantiatorRule
    {
        public string Identifier;
        public Transform Prefab;
    }

    [SerializeField]
    private InstantiatorRule[] _rules;

    private Dictionary<string, InstantiatorRule> _rulesDict = new Dictionary<string, InstantiatorRule>();
    private string[] _ruleIdentifiers;
    private Dictionary<string, int> _identifierIndexes = new Dictionary<string, int>();

#if UNITY_EDITOR

    public void OnValidate()
    {
        LoadRules();
    }
#endif
    public void OnAwake()
    {
        LoadRules();
    }

    private async void LoadRules()
    {
        _rulesDict.Clear();
        foreach(InstantiatorRule rule in _rules)
        {
            _rulesDict.Add(rule.Identifier, rule);
        }
        _ruleIdentifiers = _rulesDict.Keys.ToArray();

        _identifierIndexes.Clear();
        for(int i = 0; i < _ruleIdentifiers.Length; i++)
        {
            _identifierIndexes[_ruleIdentifiers[i]] = i;
        }
    }

    public string[] GetRuleIdentifiers()
    {
        return _ruleIdentifiers;
    }

    public bool IsValidIdentifier(string identifier)
    {
        return identifier != null && _rulesDict.ContainsKey(identifier);
    }

    public int GetIdentifierIndex(string identifier)
    {
        return _identifierIndexes.ContainsKey(identifier) ? _identifierIndexes[identifier] : 0;
    }

    public void Instantiate(LevelDefinition level)
    {
        for(int x = 0; x < level.MapWidth; x++)
        {
            for(int y = 0; y < level.MapWidth; y++)
            {
                string identifier = level.GetIdentifier(x,y);
                if(_rulesDict.ContainsKey(identifier))
                {
                    InstantiatorRule rule = _rulesDict[identifier];
                    if(rule.Prefab != null)
                    {
                        Vector3 pos = _gridDefinition.StartPosition 
                                      + new Vector3(_gridDefinition.WidthOffset * x, _gridDefinition.HeightOffset*y, 0);
                        Instantiate(rule.Prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
