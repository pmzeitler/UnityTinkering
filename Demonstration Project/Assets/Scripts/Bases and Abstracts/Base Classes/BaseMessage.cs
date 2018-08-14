using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMessage {

    private GameObject originObject;
    private Guid uuid;

    public GameObject OriginObject
    {
        get
        {
            return this.originObject;
        }

        private set
        {
            this.originObject = value;
        }
    }

    public Guid UUID
    {
        get
        {
            return this.uuid;
        }

        private set
        {
            this.uuid = value;
        }
    }

    protected BaseMessage(GameObject originObjectIn)
    {
        this.OriginObject = originObjectIn;
        this.UUID = Guid.NewGuid();
    }

}
