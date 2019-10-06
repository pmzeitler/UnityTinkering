using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MsgPlayerExecuteSetAction : BasePlayerMessage
{
    public SetActionSlot ActionSlot { get; private set; }
    public MsgPlayerExecuteSetAction(SetActionSlot actionSlot) : base(null)
    {
        this.ActionSlot = actionSlot;
    }
}

