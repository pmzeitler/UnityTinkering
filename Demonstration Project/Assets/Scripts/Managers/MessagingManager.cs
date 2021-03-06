﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagingManager : ScriptableObject, IAcceptsMessages<BaseMessage> {

    private static MessagingManager _instance = null;

    public static MessagingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<MessagingManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public void AcceptMessage(BaseMessage messageIn)
    {
        //Debug.Log("Received " + messageIn.GetType().Name + " message " + messageIn.UUID.ToString() + "; preparing to route");
        if (messageIn is BaseUIMessage)
        {
            UIManager.Instance.AcceptMessage((BaseUIMessage)messageIn);
        }
        else if  (messageIn is BaseGameStateMessage)
        {
            GameStateManager.Instance.AcceptMessage((BaseGameStateMessage)messageIn);
        }
        else if (messageIn is BasePlayerMessage)
        {
            PlayerDataManager.Instance.AcceptMessage((BasePlayerMessage)messageIn);
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("MessagingManager created");
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
