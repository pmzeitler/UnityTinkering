using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgPlayerFacingRequest : BasePlayerControlMessage
{
    public bool FaceAway { get; private set; }
    public MsgPlayerFacingRequest(GameObject gameObject, bool faceAway = false) : base(gameObject, ControlStep.LAST)
    {
        FaceAway = faceAway;
    }
}

