using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNPCInteractionScript : BaseControllerObject, IInteractible
{
    public const int COUNTDOWN_START = 90;

    private int cooldown = 0;
    private const int MAX_COOLDOWN = 120;

    public string TextToDisplay = "You are speaking to the NPC.";
    public Vector2 InteractionOffset;

    public void DoInteract(GameObject playerObject)
    {
        if (!IsPaused && (cooldown <= 0))
        {
            //messenger.AcceptMessage(new WindowMessage(gameObject, TextToDisplay, COUNTDOWN_START));
            messenger.AcceptMessage(new MsgUISmallSpeechBubble(gameObject, TextToDisplay, COUNTDOWN_START, InteractionOffset));
            cooldown = MAX_COOLDOWN;
        }
    }

    public string shortName = "shortName";

    public string ShortName
    {
        get
        { return shortName; }
        set
        {
            shortName = value;
        }
    }

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
        if (!IsPaused && (cooldown > 0))
        {
            cooldown--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            if (collision.gameObject.tag == "InteractionCircle")
            {
                messenger.AcceptMessage(new WindowMessage(gameObject, TextToDisplay, COUNTDOWN_START));
            }

            /*
            else if (collision.gameObject.tag == "Player")
            {
                Debug.Log("playerin");
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(gameObject, true));
            }

            */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            /*
            if (collision.gameObject.tag == "Player")
            {
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(gameObject, false));
            }
            */
        }
    }
}
