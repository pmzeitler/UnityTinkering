using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public static GameStateManager _instance = null;

    public bool IsPaused { get; set; }

    // Use this for initialization
    void Awake() {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("GameStateManager created");
            this.IsPaused = false;
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
