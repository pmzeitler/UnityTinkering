using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : ScriptableObject, IAcceptsMessages<BaseGameStateMessage>
{

    private static GameStateManager _instance = null;

    public GameState GameState { get; private set; }

    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance <GameStateManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public bool IsPaused { get; set; }



    // Use this for initialization
    void Awake() {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("GameStateManager created");
            this.IsPaused = false;
            this.GameState = GameState.IN_GAMEPLAY;

            if (SystemDataManager.Instance == null)
            {
                Debug.Log("Something has gone horribly wrong while instantiating SystemDataManager");
            }
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }

    private void togglePause()
    {
        IsPaused = !IsPaused;
    }

    public void AcceptMessage(BaseGameStateMessage messageIn)
    {
        if (messageIn is MsgGaStTogglePause)
        {
            togglePause();
        }
        else
        {
            Debug.Log("GameStateManager Received " + messageIn.GetType().Name + " message, but no handler is established");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
