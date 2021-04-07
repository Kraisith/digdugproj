using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(CommandProcessor))]

public class ReplayManager : MonoBehaviour, IEntity
{
    private InputReader iReader;
    private CommandProcessor cProcessor;
    // Start is called before the first frame update
    void Start()
    {
        iReader = GetComponent<InputReader>();
        cProcessor = GetComponent<CommandProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = iReader.ReadInput();
        MoveCommand moveComm = new MoveCommand(this, direction);
        cProcessor.ExecuteCommand(moveComm);
        
    }
}
