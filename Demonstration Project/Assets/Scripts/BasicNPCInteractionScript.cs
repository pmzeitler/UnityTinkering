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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameState.IsPaused)
        {
            if (collision.gameObject.tag == "InteractionCircle")
            {
                messenger.AcceptMessage(new WindowMessage(gameObject, TextToDisplay, COUNTDOWN_START));
            }
        }
    }
}
