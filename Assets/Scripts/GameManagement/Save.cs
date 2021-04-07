using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Save : MonoBehaviour
{
    private int saveNum = 0;
    public void createSaveGame()
    {
        Player plyrRef = FindObjectOfType<Player>();
        SaveGame svG = new SaveGame();
        svG.score = plyrRef.Score;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + saveNum +".save");
        bf.Serialize(file, svG);
        file.Close();
        saveNum++;
        Debug.Log("game saved at " + Application.persistentDataPath);
    }
}
