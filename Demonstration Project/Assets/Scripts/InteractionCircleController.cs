﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionCircleController : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update () {
		
	}

}
