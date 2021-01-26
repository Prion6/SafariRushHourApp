using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageUI : MonoBehaviour
{
    public List<Text> texts;

    public virtual void LoadLanguage()
    {
        foreach(Text t in texts)
        {
            t.text = GameManager.GetText(t.name);
        }
    }
}
