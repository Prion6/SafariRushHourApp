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
            www.timeout = 3;

            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
                //Debug.Log("Puzzle");
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
            www.timeout = 5;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
                //Debug.Log("Player registered:" + www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterGame(string URN, StatisticData gameData, System.Action<bool> callback, System.Action<string> action)
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
        $proficiency = $_POST["proficiency"];
        */
        form.AddField("playerID", gameData.PlayerID);
        form.AddField("puzzleID", gameData.PuzzleID);
        form.AddField("date", gameData.Date.ToString("yyyy-MM-dd"));
        form.AddField("duration", gameData.Duration);
        form.AddField("rawMoves", gameData.RawMoves);
        form.AddField("playerDifficultyEvaluation", gameData.PlayerDifficultyEvaluation);
        form.AddField("hints", gameData.HintUsed);
        form.AddField("restarts", gameData.RestartUsed);
        form.AddField("undos", gameData.UndoUsed);
        form.AddField("proficiency", gameData.Proficiency.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 30;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                Debug.Log(www.downloadHandler.text);
                callback(false);
            }
            else
            {
                Debug.Log("Game Registered: " + www.downloadHandler.text);
                callback(true);
                action(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetHint(string URN, string puzzle, System.Action<string> getHint)
    {
        WWWForm form = new WWWForm();
        /*
        $puzzle = $_POST["puzzle"];
        */

        form.AddField("puzzle", puzzle);

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 60;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                getHint(www.downloadHandler.text);
                //Debug.Log("Hint: " + www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetPlayerInfo(string URN, System.Action<string> fetchPlayerInfo)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.PlayerID);

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 5;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Player: " + www.downloadHandler.text);
                fetchPlayerInfo(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetLeaderBoard(string URN, System.Action<string> fetchLeaderBoard)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(URL + URN + ".php", form))
        {
            www.timeout = 5;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                fetchLeaderBoard(www.downloadHandler.text);
                //Debug.Log("leaderBoard: " + www.downloadHandler.text);
            }
        }
    }
}
