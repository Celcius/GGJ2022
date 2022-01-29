using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class LevelDefinition : ScriptableObject, ISerializationCallbackReceiver
{
    public const int MAP_LEN = 7;

    public int MapWidth => MAP_LEN;
    public int MapHeight => MAP_LEN;

    [HideInInspector]
    [SerializeField]
    private string[] _flattenedMap;

    [HideInInspector]
    [SerializeField]
    private int _flattenedRows;

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

    public void OnBeforeSerialize()
     {
         int c1 = _mapCharacters.GetLength(0);
         int c2 = _mapCharacters.GetLength(1);
         int count = c1*c2;
         _flattenedMap = new string[count];
         _flattenedRows = c1;
         for(int i = 0; i < count; i++)
         {
             _flattenedMap[i] = _mapCharacters[i % c1, i / c1];
         }
     }

     public void OnAfterDeserialize()
     {
         int count = _mapCharacters.Length;
         int c1 = _flattenedRows;
         if(c1 == 0)
         {
             return;
         }
         int c2 = count / c1;
         _mapCharacters = new string[c1,c2];
         for(int i = 0; i < count; i++)
         {
             _mapCharacters[i % c1, i / c1] = _flattenedMap[i];
         }
     }

}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelDefinition))]
public class EditorLevelDefinition : Editor
{
    LevelDefinition _definition;
    int _mapLen;
    string[] _rules;       

    const string EMPTY_LABEL = "-";

    SerializedObject levelObj;

    public void OnEnable()
    {
        _definition = (LevelDefinition) target;
        _mapLen = LevelDefinition.MAP_LEN;

        levelObj = new SerializedObject(target);

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
        levelObj.Update();

        DrawDefaultInspector();
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
                    _definition.MapCharacters[i,j] = EMPTY_LABEL;
                }
                else
                {
                    string text = _rules[choice];

                    _definition.MapCharacters[i,j] = string.IsNullOrWhiteSpace(text)? EMPTY_LABEL : text;
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();


        if(GUILayout.Button("Clear Map"))
        {
            _definition.MapCharacters = new string[_mapLen, _mapLen];
        }

        GUILayout.Space(10);

        if(GUILayout.Button("Instantiate - DO NOT PRESS IF YOU DON'T KNOW WHAT UR DOING"))
        {
            LevelInstantiator.Instance.InstantiateLevel(_definition);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        levelObj.ApplyModifiedProperties();
    }
}

#endif