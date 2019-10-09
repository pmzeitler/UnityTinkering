using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    void DoInteract(GameObject playerObject);

    String ShortName { get; set; }

}

