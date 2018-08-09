using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour {

    public static PlayerDataManager _instance;

    
    
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
