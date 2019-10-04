using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMappingManager : ScriptableObject {

    private static InputMappingManager _instance;

    private Dictionary<GameState, Dictionary<KeyCode, BaseAction>> MappingStructure;
    private Dictionary<GameState, Dictionary<Direction, KeyCode>> DirectionalKeysMappings;
    private bool useSticks = false;
    private const float AXIS_DEAD_ZONE = 0.15f;

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

            this.MappingStructure = new Dictionary<GameState, Dictionary<KeyCode, BaseAction>>();
            this.DirectionalKeysMappings = new Dictionary<GameState, Dictionary<Direction, KeyCode>>();
            foreach (GameState state in Enum.GetValues(typeof(GameState)) )
            {
                this.MappingStructure[state] = new Dictionary<KeyCode, BaseAction>();
                this.DirectionalKeysMappings[state] = new Dictionary<Direction, KeyCode>();
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
        MappingStructure[GameState.IN_GAMEPLAY][KeyCode.Space] = new SpawnInteractorAction();
        MappingStructure[GameState.IN_GAMEPLAY][KeyCode.JoystickButton0] = MappingStructure[GameState.IN_GAMEPLAY][KeyCode.Space];

        DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.NORTH] = KeyCode.UpArrow;
        DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.SOUTH] = KeyCode.DownArrow;
        DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.WEST] = KeyCode.LeftArrow;
        DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.EAST] = KeyCode.RightArrow;

        foreach (GameState gameState in Enum.GetValues(typeof(GameState)) )
        {
            MappingStructure[gameState][KeyCode.P] = new TogglePauseAction();
            MappingStructure[gameState][KeyCode.JoystickButton8] = MappingStructure[gameState][KeyCode.P];
        }


    }

    private void DetermineMovement(ref float h, ref float v, ref bool mh, ref bool mv)
    {
        if (useSticks)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey(DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.WEST]))
            {
                h = -1.0f;
            }
            else if (Input.GetKey(DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.EAST]))
            {
                h = 1.0f;
            }

            if (Input.GetKey(DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.SOUTH]))
            {
                v = -1.0f;
            }
            else if (Input.GetKey(DirectionalKeysMappings[GameState.IN_GAMEPLAY][Direction.NORTH]))
            {
                v = 1.0f;
            }
        }

        if (Mathf.Abs(h) <= AXIS_DEAD_ZONE)
        {
            h = 0.0f;
            mh = false;
        } else
        {
            mh = true;
        }
        if (Mathf.Abs(v) <= AXIS_DEAD_ZONE)
        {
            v = 0.0f;
            mv = false;
        } else
        {
            mv = true;
        }
    }

    public void CheckUserInput()
    {
        float h = 0.0f;
        float v = 0.0f;
        bool mh = false;
        bool mv = false;

        DetermineMovement(ref h, ref v, ref mh, ref mv);

        MessagingManager.Instance.AcceptMessage(new MsgPlayerMovementRequest(new Vector2(h, v), mh, mv));

        Dictionary<KeyCode, BaseAction> currentModeMappings = MappingStructure[GameStateManager.Instance.GameState];

        foreach(KeyValuePair<KeyCode, BaseAction> checkMe in currentModeMappings)
        {
            /*
             * if (DirectionalKeysMappings[GameStateManager.Instance.GameState].ContainsValue(checkMe.Key) )
            {
                continue;
            }
            */

            bool actDown = Input.GetKeyDown(checkMe.Key);
            bool actHeld = Input.GetKey(checkMe.Key);
            bool actUp = Input.GetKeyUp(checkMe.Key);

            if (actHeld)
            {
                if(actDown)
                {
                    Debug.Log("STARTING ACTION " + checkMe.Value.ToString());
                    checkMe.Value.StartAction();
                } else
                {
                    Debug.Log("HOLDING ACTION " + checkMe.Value.ToString());
                    checkMe.Value.HeldAction();
                }
            } else if (actUp)
            {
                Debug.Log("ENDING ACTION " + checkMe.Value.ToString());
                checkMe.Value.EndAction();
            }
        }




    }
}
