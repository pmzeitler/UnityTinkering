using UnityEngine;
using System.Collections;

public class WindowMessage : BaseUIMessage
{
    private string textToDisplay;
    private int messageDuration;

    public string TextToDisplay
    {
        get
        {
            return this.textToDisplay;
        }
        private set
        {
            this.textToDisplay = value;
        }
    }

    public int MessageDuration
    {
        get
        {
            return this.messageDuration;
        }
        private set
        {
            this.messageDuration = value;
        }
    }

    public WindowMessage(GameObject originObject, string textToDisplayIn, int messageDurationIn) : base(originObject)
    {
        this.TextToDisplay = textToDisplayIn;
        this.MessageDuration = messageDurationIn;
    }

}
