using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : ScriptableObject, IAcceptsMessages<BasePlayerMessage>
{

    private static PlayerDataManager _instance;

    private PlayerController _playerController;

    public static PlayerDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<PlayerDataManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public Direction MovingDirection { get; set; }
    public Direction FacingDirection { get; set; }

    public PlayerController PlayerController { set
        {
            _playerController = value;
        }
    }

    public void AcceptMessage(BasePlayerMessage messageIn)
    {
        if(messageIn is BasePlayerControlMessage)
        {
            this._playerController.AcceptMessage((BasePlayerControlMessage)messageIn);
        } 
    }

    // Use this for initialization
    void Awake () {
		if (_instance == null)
        {
            _instance = this;
            Debug.Log("PlayerDataManager created");
            this.MovingDirection = Direction.NORTH;
            this.FacingDirection = Direction.NORTH;
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
