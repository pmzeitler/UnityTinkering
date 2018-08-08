using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNPCInteractionScript : MonoBehaviour {

    public Text textBoxObj;

    private PlayerData playerData;

    private int countdown = 0;
    private const int COUNTDOWN_START = 90;
    private bool clearBox = false;

    public string TextToDisplay = "You are speaking to the NPC.";

    // Use this for initialization
    void Awake()
    {
        playerData = PlayerData._instance;
    }

    void FixedUpdate()
    {
        if (playerData == null)
        {
            playerData = PlayerData._instance;
            Debug.Log("PlayerData instance is " + playerData);
        }
        if (!playerData.IsPaused)
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
        if (!playerData.IsPaused)
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
