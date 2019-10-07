using System.Collections;
using UnityEngine;

public abstract class BasePlayerControlMessage : BasePlayerMessage
{
    public ControlStep ControlStep { get; private set; }

    public BasePlayerControlMessage(GameObject originObject, ControlStep controlStep = ControlStep.GENERAL) : base(originObject)
    {
        ControlStep = controlStep;
    }

}

