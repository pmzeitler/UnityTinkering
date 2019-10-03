using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgPlayerMovementRequest : BasePlayerMessage {

    public Vector2 StickPosition { get; private set; }
    public bool MovingHorizontal { get; private set; }
    public bool MovingVertical { get; private set; }

    public MsgPlayerMovementRequest(Vector2 stickPosition, bool movingHoriz, bool movingVert) : base(null)
    {
        StickPosition = stickPosition;
        MovingHorizontal = movingHoriz;
        MovingVertical = movingVert;
    }
}
