using UnityEngine;
using System.Collections;


class MsgUISmallSpeechBubble : BaseUIMessage
{
    public string WindowMessage { get; private set; }
    public int MessageDuration { get; private set; }

    public MsgUISmallSpeechBubble(GameObject gameObject, string windowMessage, int timeToDisplay) : base (gameObject)
    {
        WindowMessage = windowMessage;
        MessageDuration = timeToDisplay;



    }



}

