using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OptionsData
{
    public OptionsData(float volume, int speed, Language langauge) : this()
    {
        Volume = volume;
        Speed = speed;
        Language = langauge;
    }

    public float Volume { get; set; }
    public int Speed { get; set; }
    public Language Language { get; set; }
}
