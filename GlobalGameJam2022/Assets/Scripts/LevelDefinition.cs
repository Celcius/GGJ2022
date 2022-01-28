using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelDefinition : ScriptableObject
{
    public const int MAP_LEN = 7;

    public int MapWidth => MAP_LEN;
    public int MapHeight => MAP_LEN;

    private string[,] _mapCharacters = new string[MAP_LEN, MAP_LEN];
    public  string[,] MapCharacters
    {
        get { return _mapCharacters; }
        set { _mapCharacters = value; }
    }

    public string GetIdentifier(int x, int y)
    {
        return _mapCharacters[x,y];
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(LevelDefinition))]
public class EditorLevelDefinition : Editor
{
    LevelDefinition _definition;
    int _mapLen;
    string[] _rules;       

    const string EMPTY_LABEL = "Empty";

    public void OnEnable()
    {
        _definition = (LevelDefinition) target;
        _mapLen = LevelDefinition.MAP_LEN;

        string[] rules =  LevelInstantiator.Instance.GetRuleIdentifiers();
        _rules = new string[rules.Length+1];
        _rules[0] = EMPTY_LABEL;
        for(int i = 0; i < rules.Length;i++)
        {
            _rules[i+1] = ""+rules[i];
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        string[,] newMap = new string[_mapLen, _mapLen];

        GUILayout.BeginVertical();

        for(int i = 0; i < _mapLen; i++)
        {
            GUILayout.BeginHorizontal();
            for(int j = 0; j < _mapLen; j++)
            {
                string character = _definition.MapCharacters[i,j];
                bool isValid = LevelInstantiator.Instance.IsValidIdentifier(character);
                int index = isValid? LevelInstantiator.Instance.GetIdentifierIndex(character)+1 : 0;

                int choice = EditorGUILayout.Popup("", index, _rules, GUILayout.Width(100));
                //string text = GUILayout.TextField(isValid? (""+ character) : "", 1);
                if(choice == 0)
                {
                    newMap[i,j] = EMPTY_LABEL;
                }
                else
                {
                    string text = _rules[choice];

                    newMap[i,j] = string.IsNullOrWhiteSpace(text)? EMPTY_LABEL : text;
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        _definition.MapCharacters = newMap;

        if(GUILayout.Button("Instantiate - DO NOT PRESS IF YOU DON'T KNOW WHAT UR DOING"))
        {
            LevelInstantiator.Instance.Instantiate(_definition);
        }
    }
}

#endif