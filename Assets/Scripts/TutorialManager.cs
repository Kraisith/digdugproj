using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LightBandit lB;
    [SerializeField] private HeavyBandit hB;
    [SerializeField] private GameObject tut1, tut2, tut3, tut4, tut5, tut6, tut7;
    // Start is called before the first frame update
    void Start()
    {
        lB.setTutorialStatus(true);
        hB.setTutorialStatus(true);

    }

    public void displayTutorial(int tutNum)
    {
        if (tutNum == 2)
        {
            tut1.SetActive(false);
            tut2.SetActive(true);
        } else if (tutNum == 3)
        {
            tut2.SetActive(false);
            tut3.SetActive(true);
        }
        else if (tutNum == 4)
        {
            tut3.SetActive(false);
            tut4.SetActive(true);
        }
        else if (tutNum == 5)
        {
            tut4.SetActive(false);
            tut5.SetActive(true);
        }
        else if (tutNum == 6)
        {
            tut5.SetActive(false);
            tut6.SetActive(true);
        }
        else if (tutNum == 7)
        {
            tut6.SetActive(false);
            tut7.SetActive(true);
        } 
        else if (tutNum == 8) //after last tutorial is clicked
        {
            tut7.SetActive(false);
            lB.setTutorialStatus(false);
            hB.setTutorialStatus(false);
        }
    }

    

    
}
