using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour //for a single score to be displayed
{
    private int currentSaveNum = 0;
    private TMP_Text textComp;

    private void Start()
    {
        textComp = GetComponent<TMP_Text>();
        LoadScore();
    }
    public void LoadScore()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave" + currentSaveNum + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave" + currentSaveNum + ".save", FileMode.Open);
            SaveGame saveGame = (SaveGame)bf.Deserialize(file);
            file.Close();
            textComp.SetText(textComp.text + System.Environment.NewLine + saveGame.score.ToString());
            currentSaveNum++;
            LoadScore(); //recursively go through save numbers and attempt to add onto the text comp
        } else
        {
            //idk do nothing i should delete this
        }

        

    }
}
