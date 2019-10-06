using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ActionSlotExecuteRequestAction : BasePlayerAction
{
    public SetActionSlot ActionSlot { get; private set; }

    public ActionSlotExecuteRequestAction(SetActionSlot actionSlot) : base("ActionSlotExecuteRequestAction")
    {
        ActionSlot = actionSlot;
    }

    protected override void _doEndAction()
    {
        // do nothing
    }

    protected override void _doHeldAction()
    {
        MessagingManager.Instance.AcceptMessage(new MsgPlayerExecuteSetAction(ActionSlot));
    }

    protected override void _doStartAction()
    {
        MessagingManager.Instance.AcceptMessage(new MsgPlayerExecuteSetAction(ActionSlot));
    }
}

