using UnityEngine;
using System.Collections;


class MsgUISmallSpeechBubble : BaseUIMessage
{
    public string WindowMessage { get; private set; }
    public int MessageDuration { get; private set; }
    public Vector2 BubbleOffset { get; set; }

    public MsgUISmallSpeechBubble(GameObject gameObject, string windowMessage, int timeToDisplay, Vector2 bubbleOffset) : base (gameObject)
    {
        WindowMessage = windowMessage;
        MessageDuration = timeToDisplay;
        BubbleOffset = bubbleOffset;


    }



}

