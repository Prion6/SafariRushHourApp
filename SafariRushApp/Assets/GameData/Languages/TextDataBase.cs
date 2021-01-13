using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/TextDatabase")]
public class TextDataBase : ScriptableObject
{
    public Langauge langauge;
    public List<UIText> texts;
    private Dictionary<string, string> _Dictionary;
    public Dictionary<string, string> Dictionary
    { get
        {
            if(_Dictionary == null)
            {
                Load();
            }
            return _Dictionary;
        }
    }

    public void Load()
    {
        _Dictionary = new Dictionary<string, string>();
        foreach(UIText t in texts)
        {
            _Dictionary.Add(t.uiElement, t.text);
        }
    }
}

[System.Serializable]
public struct UIText
{
    public string uiElement;
    [TextArea]
    public string text;
}

public enum Langauge
{
    ENGLISH,
    SPANISH
}
