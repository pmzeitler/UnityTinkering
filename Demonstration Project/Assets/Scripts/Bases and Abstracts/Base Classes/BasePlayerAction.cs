using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class BasePlayerAction : BaseAction
{
    public BasePlayerAction(String actionName) : base("BasePlayerAction: " + actionName)
    {

    }
}

