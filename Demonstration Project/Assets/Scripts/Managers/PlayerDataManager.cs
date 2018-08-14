using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : ScriptableObject
{

    private static PlayerDataManager _instance;

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
