using UnityEngine;
using Unity.RemoteConfig;
using Unity.Services.RemoteConfig;
using System.Collections.Generic;
using System;

public class LevelService : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }


    private static List<LevelData> levels;
    private void Start()
    {
        CloudManager.playerAuthenticated += Initialize;
    }
    public void Initialize()
    {
        RemoteConfigService.Instance.FetchCompleted += GetLevels;
        //TODO: revisar si esto es realmente necesario (Lo vi en un tutorial y no sé si va)
        RemoteConfigService.Instance.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }
    private void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= GetLevels;
        CloudManager.playerAuthenticated -= Initialize;
    }
    private void GetLevels(ConfigResponse _)
    {
        try
        {
            string levelsText = RemoteConfigService.Instance.appConfig.GetJson("Levels");
            //Debug.Log(levelsText);
            levels = LevelConvertor.ParseJsonToLevelList(levelsText);

            //foreach (var level in levels)
            //{
            //    Debug.Log(level.Level);
            //}
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }
    public static LevelData GetALevelByDifficulty(LevelDifficulties diff)
    {
        List<LevelData> levelsToChoose= new List<LevelData>();

        foreach (LevelData level in levels)
        {
            if(diff == level.Difficulty)
            {
                levelsToChoose.Add(level);
            }
        }

        int randomIndex = levelsToChoose.Count - 1;
        return levelsToChoose[UnityEngine.Random.Range(0, randomIndex)];
    }
}
