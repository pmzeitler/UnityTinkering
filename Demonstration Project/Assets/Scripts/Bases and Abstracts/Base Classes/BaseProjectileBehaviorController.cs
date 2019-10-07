using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileBehaviorController : BaseControllerObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public string OriginTag = "";


    protected override void EnterPause()
    {
        //this is an awful hack but oh well
        gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>().enabled = false;
    }

    protected override void ExitPause()
    {
        gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>().enabled = true;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        gameObject.transform.position = new Vector2(gameObject.transform.position.x + 0.35f, gameObject.transform.position.y);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == OriginTag)
        {
            //objects are not affected by their own projectiles
            return;
        }

        if (collision.gameObject.name == "WallsLayer")
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Collided with " + collision.gameObject.name + " of tag " + collision.gameObject.tag + ", but no handler found");
        }
        
    }



}
