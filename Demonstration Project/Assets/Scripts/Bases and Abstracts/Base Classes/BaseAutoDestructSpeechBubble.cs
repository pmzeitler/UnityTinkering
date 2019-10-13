using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAutoDestructSpeechBubble : BaseAutoDestructComponent
{

    public GameObject TrackObject  { get; set; }
    public Vector2 offset;

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Vector2 trackPos = UIManager.Instance.AdjustPositionForUIDisplay(TrackObject.transform.position);

        gameObject.transform.localPosition = trackPos + offset;



        base.FixedUpdate();
    }

    protected override void EnterPause()
    {
        //throw new System.NotImplementedException();
    }

    protected override void ExitPause()
    {
        //throw new System.NotImplementedException();
    }
}
