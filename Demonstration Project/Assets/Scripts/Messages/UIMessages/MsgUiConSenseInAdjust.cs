using UnityEngine;
using System.Collections;

public class MsgUiConSenseInAdjust : BaseUIMessage
{
    public bool Activate { get; private set; }

    public MsgUiConSenseInAdjust(GameObject originObject, bool activate) : base(originObject)
    {
        Activate = activate;
    }

}
