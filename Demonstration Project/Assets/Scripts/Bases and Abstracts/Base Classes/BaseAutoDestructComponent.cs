using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAutoDestructComponent : BaseControllerObject
{

    private int _timeMax;

    public int TimeToDisplay
    {
        get
        {
            return _timeMax;
        }
        set
        {
            countdown = value;
            _timeMax = value;
        }
    }

    protected int countdown = 0;

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!IsPaused)
        {
            if (countdown > 0)
            {
                countdown--;
            }
            if (countdown <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void EnterPause()
    {
        //throw new System.NotImplementedException();
    }

    protected override void ExitPause()
    {
        //throw new System.NotImplementedException();
    }
}
