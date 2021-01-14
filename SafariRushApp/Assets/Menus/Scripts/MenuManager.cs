using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public virtual void LoadScene(int sceneID)
    {
        GameManager.LoadScene(sceneID);
    }

    public virtual void LoadScene(string sceneName)
    {
        GameManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        GameManager.Quit();
    }

    private void OnApplicationQuit()
    {
        GameManager.Quit();
    }
}
