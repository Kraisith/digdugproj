using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    private Vector2 dir;

    
    public MoveCommand(IEntity entity, Vector2 direction) : base(entity)
    {
        dir = direction;
    }

    public override void Execute (Player plyr) //doesnt really do anything
    {
        Debug.Log(dir);
        plyr.Move(dir); //freezes whenever i do this idk 
        //_entity.transform.Translate(dir * 0.001f);
    }

}
