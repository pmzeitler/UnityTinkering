using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : ScriptableObject, IAcceptsMessages<BaseMessage>
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
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }

    public void AcceptMessage(BaseMessage messageIn)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
