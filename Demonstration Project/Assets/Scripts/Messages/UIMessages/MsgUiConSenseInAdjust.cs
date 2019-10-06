using UnityEngine;
using System.Collections;

public class MsgUiConSenseInAdjust : BaseUIMessage
{
    public bool Increase { get; private set; }

    public MsgUiConSenseInAdjust(GameObject originObject, bool increase) : base(originObject)
    {
        Increase = increase;
    }

}
