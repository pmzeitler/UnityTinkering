using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : ScriptableObject, IAcceptsMessages<BasePlayerMessage>
{

    private static PlayerDataManager _instance;

    private PlayerController _playerController;

    //TODO replace UnityEngine.Object with the executable action object type
    private Dictionary<SetActionSlot, UnityEngine.Object> ActionSlots;

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
        } else if (messageIn is MsgPlayerExecuteSetAction)
        {
            this.HandleExecuteActionSlotMessage((MsgPlayerExecuteSetAction)messageIn);
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


            ActionSlots = new Dictionary<SetActionSlot, UnityEngine.Object>();
            foreach (SetActionSlot slotName in Enum.GetValues(typeof(SetActionSlot)))
            {
                ActionSlots[slotName] = null;
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
	
    private void HandleExecuteActionSlotMessage(MsgPlayerExecuteSetAction messageIn)
    {
        //TODO actually execute the actions
        Debug.Log("Received " + messageIn.GetType().Name + " message for Slot:" + messageIn.ActionSlot);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
