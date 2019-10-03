using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMappingManager : ScriptableObject {

    private static InputMappingManager _instance;

    private Dictionary<GameState, Dictionary<KeyCode, object>> MappingStructure;

    public static InputMappingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<InputMappingManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    // Use this for initialization
    void Awake () {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("InputMappingManager created");

            this.MappingStructure = new Dictionary<GameState, Dictionary<KeyCode, object>>();
            foreach (GameState state in Enum.GetValues(typeof(GameState)) )
            {
                this.MappingStructure[state] = new Dictionary<KeyCode, object>();
            }

            LoadDefaultKeyMapping();
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }

    private void LoadDefaultKeyMapping()
    {
        MappingStructure[GameState.IN_GAMEPLAY][KeyCode.Space] = "AttemptingToSpawnInteractor";
    }

    public void CheckUserInput()
    {
        Dictionary<KeyCode, object> currentModeMappings = MappingStructure[GameStateManager.Instance.GameState];

        foreach(KeyValuePair<KeyCode, object> checkMe in currentModeMappings)
        {
            bool actDown = Input.GetKeyDown(checkMe.Key);
            bool actHeld = Input.GetKey(checkMe.Key);
            bool actUp = Input.GetKeyUp(checkMe.Key);

            if (actHeld)
            {
                if(actDown)
                {
                    Debug.Log("STARTING ACTION " + checkMe.Value.ToString());
                } else
                {
                    Debug.Log("HOLDING ACTION " + checkMe.Value.ToString());
                }
            } else if (actUp)
            {
                Debug.Log("ENDING ACTION " + checkMe.Value.ToString());
            }
        }




    }
}
