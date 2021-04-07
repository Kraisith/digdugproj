using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandProcessor : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    private int currentCommIndex = -1;
    [SerializeField] Player plyrRef;

    public void ExecuteCommand(Command comm)
    {
        commands.Add(comm);
        //comm.Execute(); dont need to execute here, since movement is handled in player
        currentCommIndex=commands.Count - 1;
    }

    public void Do() //executes player input in full
    {
        Debug.Log("doing");
        plyrRef.setReplaying(true);

        for (int i = 0; i < commands.Count; i++)
        {
            commands[i].Execute();
        }
        plyrRef.setReplaying(false);
    }
}
