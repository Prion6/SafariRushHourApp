using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Manager/PHPQuerySet")]
public class PHPQuerySet : ScriptableObject
{
    public string URL;
    
    public IEnumerator GetPuzzle(string URN, int ranking, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("ranking", ranking);
        
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
                //Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
