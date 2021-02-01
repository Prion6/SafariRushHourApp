﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MultiLanguageUI
{
    public Text languageDisplay;
    public Text volumeDisplay;
    public Text speedDisplay;

    public Slider volume;
    public Slider speed;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        volume.value = GameManager.Volume;
        speed.value = GameManager.Speed;

        SetVolume();
        SetSpeed();
        SetLanguage(0);
    }

    public void SetVolume()
    {
        volumeDisplay.text = ((int)(volume.value * 100)).ToString();
        GameManager.Volume = (volume.value);
    }

    public void SetSpeed()
    {
        speedDisplay.text = speed.value.ToString();
        GameManager.Speed = ((int)speed.value);
    }

    public void SetLanguage(int i)
    {
        int length = Enum.GetValues(typeof(Language)).Length;
        int id = (int)GameManager.Language + i;
        if (id < 0)
        {
            id += length; 
        }
        else if(id >= length)
        {
            id -= length;
        }
        languageDisplay.text = ((Language)id).ToString();
        GameManager.SetLanguage((Language)id);
        LoadLanguage();
    }

    override public void LoadLanguage()
    {
        base.LoadLanguage();
    }

    public void Surrender()
    {
        FindObjectOfType<PuzzleManager>().GoToMainMenu();
    }

    public void Restart()
    {
        
    }
}
