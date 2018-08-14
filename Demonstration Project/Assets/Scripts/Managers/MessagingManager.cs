using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagingManager : ScriptableObject {

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
