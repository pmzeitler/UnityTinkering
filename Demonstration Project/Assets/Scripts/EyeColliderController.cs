using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeColliderController : MonoBehaviour {

    public Text textBoxObj;

    private const int COUNTDOWN_START = 90;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractionCircle")
        {
            MessagingManager.Instance.AcceptMessage(new WindowMessage(gameObject, "You have hit the eye!!", COUNTDOWN_START));
        } 
    }
}
