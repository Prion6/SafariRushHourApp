using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Manager/PHPQuerySet")]
public class PHPQuerySet : ScriptableObject
{
    public string URL;

    public IEnumerator Testing(string URN)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + URN + ".php");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    public IEnumerator GetPuzzle(string URN, DIFFICULTY difficulty, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("difficulty", (int)difficulty);
        
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
