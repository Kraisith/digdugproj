using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MoveCommand : Command
{
    private Vector2 dir;
    private InputAction.CallbackContext input;

    
    public MoveCommand(IEntity entity, InputAction.CallbackContext inputAct) : base(entity)
    {
        input = inputAct;
    }

    public override void Execute() //doesnt really do anything
    {
        Debug.Log(dir);
        var keyboard = InputSystem.AddDevice<Keyboard>();
        
        //freezes whenever i do this idk 
        //_entity.transform.Translate(dir * 0.001f);
    }

}
