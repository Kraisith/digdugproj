using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int numberOfEnemies = 1;
    [SerializeField] private Player plyrRef;
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
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Save save = gameObject.AddComponent<Save>();
                save.createCurrScore();
                SceneManager.LoadScene("Level2");
                
            } else if (SceneManager.GetActiveScene().name == "Level2")
            {
                Save save = gameObject.AddComponent<Save>();
                save.createSaveGame(); //on completion of the game, save automatically
                SceneManager.LoadScene("LaunchMenu"); //goes to main menu after level 2
            } else if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("LaunchMenu"); //go to main menu after completing tutorial
            }
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
