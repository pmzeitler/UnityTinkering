using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseControllerObject : MonoBehaviour {

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
	protected virtual void Update ()
    {
        CheckPauseState();
	}

    protected virtual void FixedUpdate()
    {
        CheckPauseState();
    }

    private bool _wasPaused = false;
    protected bool IsPaused
    {
        get
        {
            return gameState.IsPaused;
        }
    }

    protected virtual bool CheckPauseState()
    {
        bool _isPaused = IsPaused;
        if (_wasPaused != _isPaused)
        {
            if (_isPaused)
            {
                //entering pause state
                EnterPause();
            } else
            {
                //leaving pause state
                ExitPause();
            }
        }
        _wasPaused = _isPaused;
        return _isPaused;
    }

    protected abstract void EnterPause();
    protected abstract void ExitPause();
}
