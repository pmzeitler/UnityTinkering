using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TogglePauseAction : BasePlayerAction
{
    private bool _canPause = true;

    public TogglePauseAction() : base("TogglePauseAction")
    {

    }

    protected override void _doEndAction()
    {
        _canPause = true;
    }

    protected override void _doHeldAction()
    {
        // do nothing;
    }

    protected override void _doStartAction()
    {
        _canPause = false;
        MessagingManager.Instance.AcceptMessage(new MsgGaStTogglePause(null));
    }
}

