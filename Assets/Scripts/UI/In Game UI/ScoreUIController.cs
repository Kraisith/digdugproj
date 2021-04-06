using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] private Player plyrRef;
    private TMP_Text textComp;

    private void Start()
    {
        textComp = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textComp.SetText("Score = " + plyrRef.Score);
    }
}
