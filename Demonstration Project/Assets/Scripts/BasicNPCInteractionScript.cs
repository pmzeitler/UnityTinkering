using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNPCInteractionScript : BaseControllerObject
{
    public const int COUNTDOWN_START = 90;

    public string TextToDisplay = "You are speaking to the NPC.";

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void EnterPause()
    {
        //this is an awful hack but oh well
        gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>().enabled = false;
    }

    protected override void ExitPause()
    {
        gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>().enabled = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            if (collision.gameObject.tag == "InteractionCircle")
            {
                messenger.AcceptMessage(new WindowMessage(gameObject, TextToDisplay, COUNTDOWN_START));
            }
            else if (collision.gameObject.tag == "Player")
            {
                Debug.Log("playerin");
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(gameObject, true));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            if (collision.gameObject.tag == "Player")
            {
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(gameObject, false));
            }
        }
    }
}
