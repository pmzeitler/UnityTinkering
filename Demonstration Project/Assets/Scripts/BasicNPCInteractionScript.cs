using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNPCInteractionScript : BaseControllerObject
{

    public Text textBoxObj;

    private int countdown = 0;
    private const int COUNTDOWN_START = 90;
    private bool clearBox = false;

    public string TextToDisplay = "You are speaking to the NPC.";

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!gameState.IsPaused)
        {
            if (countdown > 0)
            {
                countdown--;
            }
            if ((countdown <= 0) && clearBox)
            {
                textBoxObj.text = "";
                clearBox = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameState.IsPaused)
        {
            if (collision.gameObject.tag == "InteractionCircle")
            {
                textBoxObj.text = TextToDisplay;
                countdown = COUNTDOWN_START;
                clearBox = true;
            }
        }
    }
}
