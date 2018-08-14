using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControllerObject : MonoBehaviour {

    protected PlayerDataManager playerData;
    protected GameStateManager gameState;
    protected MessagingManager messenger;

    // Use this for initialization
    protected virtual void Awake () {
        playerData = PlayerDataManager.Instance;
        gameState = GameStateManager.Instance;
        messenger = MessagingManager.Instance;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    protected virtual void FixedUpdate()
    {

    }
}
