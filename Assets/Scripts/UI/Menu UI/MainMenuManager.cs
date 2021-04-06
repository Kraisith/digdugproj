using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private LevelManager lvlMnger;
    public void LaunchLevel1()
    {
        lvlMnger = FindObjectOfType<LevelManager>();
        Debug.Log("Launch lvl 1");
        lvlMnger.LoadLevel("Level1");
        //StartCoroutine(LoadLevel1Async());
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    IEnumerator LoadLevel1Async()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
