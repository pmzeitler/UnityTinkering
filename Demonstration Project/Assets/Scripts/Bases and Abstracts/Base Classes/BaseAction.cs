using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class BaseAction
{
    protected BaseAction(string actionName)
    {
        this.ActionName = actionName;
    }

    protected abstract void _doStartAction();
    protected abstract void _doHeldAction();
    protected abstract void _doEndAction();

    public string ActionName { get; protected set; }

    public sealed override string ToString()
    {
        return ActionName;
    }

    public void StartAction()
    {
        this._doStartAction();
    }

    public void HeldAction()
    {
        this._doHeldAction();
    }

    public void EndAction()
    {
        this._doEndAction();
    }

}
