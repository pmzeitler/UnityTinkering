using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInteractorAction : BasePlayerAction
{

    private bool canSpawnInteractor = true;

    public SpawnInteractorAction() : base("SpawnInteractorAction")
    {
        
    }

    protected override void _doEndAction()
    {
        this.canSpawnInteractor = true;
    }

    protected override void _doHeldAction()
    {
        //throw new System.NotImplementedException();
    }

    protected override void _doStartAction()
    {
        if (this.canSpawnInteractor)
        {
            this.canSpawnInteractor = false;
            MessagingManager.Instance.AcceptMessage(new MsgPlayerSpawnInteraction());
        }
    }
    
}
