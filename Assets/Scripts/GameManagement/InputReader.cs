using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] Player plyrRef; //assign in inspector

    public Vector2 ReadInput()
    {
        return plyrRef.getCurrentMovement();

    }
}
