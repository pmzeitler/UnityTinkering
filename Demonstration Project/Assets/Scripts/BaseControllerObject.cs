using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControllerObject : MonoBehaviour {

    protected PlayerDataManager playerData;
    protected GameStateManager gameState;

    // Use this for initialization
    protected virtual void Awake () {
        playerData = PlayerDataManager._instance;
        gameState = GameStateManager._instance;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    protected virtual void FixedUpdate()
    {

    }
}
