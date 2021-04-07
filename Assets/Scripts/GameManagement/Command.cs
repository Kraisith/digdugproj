using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected IEntity _entity;

    public Command (IEntity ent)
    {
        _entity = ent;
    }

    public abstract void Execute(Player plyr);
}
