using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int numberOfEnemies = 1;
    public void ResetScene()
    {
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel (string lvlName)
    {
        StartCoroutine(Load(lvlName));
    }

    private IEnumerator Load(string lvlName)
    {
        yield return new WaitForSeconds(0); //uhhh
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(lvlName);
    }

    public void decreaseEnemy()
    {
        numberOfEnemies--;
        if (numberOfEnemies == 0)
        { //progress level, currently just goes to main menu
            StartCoroutine(waitBeforeLoad("LaunchMenu"));
        }
    }

    private IEnumerator waitBeforeLoad(string lvlName)
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Load(lvlName));
    }

    public void setNumEnemies(int enemyNum)
    {
        numberOfEnemies = enemyNum;
    }
}
