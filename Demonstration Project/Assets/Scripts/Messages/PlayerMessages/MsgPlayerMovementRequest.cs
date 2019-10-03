using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgPlayerMovementRequest : BasePlayerMessage {

    public Vector2 StickPosition { get; private set; }

    public MsgPlayerMovementRequest(Vector2 stickPosition) : base(null)
    {
        StickPosition = stickPosition;
    }
}
