using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeColliderController : MonoBehaviour {

    public Text textBoxObj;

    private int countdown = 0;
    private const int COUNTDOWN_START = 90;
    private bool clearBox = false;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
        if (countdown > 0)
        {
            countdown--;
        }
        if ((countdown <= 0) && clearBox)
        {
            textBoxObj.text = "";
            clearBox = false;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractionCircle")
        {
            textBoxObj.text = "You have hit the eye!!";
            countdown = COUNTDOWN_START;
            clearBox = true;
        } 
    }
}
