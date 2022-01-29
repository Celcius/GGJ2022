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
        public Vector2 GridSize;
    }

    [SerializeField]
    private GridDefinition _gridDefinition;

    public Vector2 GridSize => _gridDefinition.GridSize;
    
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

    public void OnValidate()
    {
        LoadRules();
    }

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

    public void InstantiateLevel(LevelDefinition level)
    {
        EntityManager.Instance.PrepareGame();

        float widthOffset = _gridDefinition.GridSize.x;
        float heightOffset = _gridDefinition.GridSize.y;
        
        Vector3 startPos = new Vector3(-level.MapWidth * widthOffset/2.0f + widthOffset/2.0f,
                                       level.MapHeight * heightOffset/2.0f - heightOffset/2.0f, 
                                      0);

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
                        Vector3 pos = startPos
                                      + new Vector3(widthOffset * y, -heightOffset*x, 0);
                        Instantiate(rule.Prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
