using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Manager/PHPQuerySet")]
public class PHPQuerySet : ScriptableObject
{
    public string URL;
    
    public IEnumerator GetPuzzle(string URN, int delta, int id, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("delta", delta);

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 5;

            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
                //Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterPlayer(string URN, PlayerData playerData, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        /*$nickname = $_POST["nickname"];
            $ranking = $_POST["ranking"];
            $age = $_POST["age"];
            $rushHourExperise = $_POST["rushHourExperise"];
            $puzzleGameExpertise = $_POST["puzzleGameExpertise"];
            $mobileGameExpertise = $_POST["mobileGameExpertise"];
            $educationalLevel = $_POST["educationalLevel"];*/
        form.AddField("nickname", playerData.Nickname);
        form.AddField("ranking", playerData.Ranking);
        form.AddField("age", playerData.Age);
        form.AddField("rushHourExperise", playerData.RushHourExpertise);
        form.AddField("puzzleGameExpertise", playerData.PuzzleGameExpertise);
        form.AddField("mobileGameExpertise", playerData.MobileGameExpertise);
        form.AddField("educationalLevel", playerData.EducationalLevel);

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 1;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterGame(string URN, StatisticData gameData, System.Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        /*
        $playerID = $_POST["playerID"];
        $puzzleID = $_POST["puzzleID"];
        $date = $_POST["date"];
        $duration = $_POST["duration"];
        $rawMoves = $_POST["rawMoves"];
        $playerDifficultyEvaluation = $_POST["playerDifficultyEvaluation"];
        $hints = $_POST["hints"];
        $restarts = $_POST["restarts"];
        $undos = $_POST["undos"];
        */

        form.AddField("playerID", gameData.PlayerID);
        form.AddField("puzzleID", gameData.PuzzleID);
        form.AddField("date", gameData.Date.ToString());
        form.AddField("duration", gameData.Duration);
        form.AddField("rawMoves", gameData.RawMoves);
        form.AddField("playerDifficultyEvaluation", gameData.PlayerDifficultyEvaluation);
        form.AddField("hints", gameData.HintUsed);
        form.AddField("restarts", gameData.RestartUsed);
        form.AddField("undos", gameData.UndoUsed);

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 5;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                Debug.Log("Error");
                callback(false);
            }
            else
            {
                Debug.LogError(www.downloadHandler.text);
                Debug.Log("Succes");
                callback(true);
            }
        }
    }
}
