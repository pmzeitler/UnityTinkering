using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : ScriptableObject, IAcceptsMessages<BaseUIMessage> {

    public static UIManager _instance;

    private GameObject UICanvas;
    private GameObject ConSenseInteractionIcon;

    private int _interactionZoneCount = 0;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<UIManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("UIManager created");
            UICanvas = FindByName("UICanvas", SceneManager.GetActiveScene().GetRootGameObjects());
            ConSenseInteractionIcon = UICanvas.transform.Find("ContextSensitiveIndicator").gameObject;
            if (UICanvas == null)
            {
                Debug.Log("Could not find a UICanvas to draw to");
            }
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }

    protected GameObject FindByName(string nameIn, GameObject[] objectList)
    {
        GameObject retval = null;

        foreach (var gameObject in objectList)
        {
            if (gameObject.name == nameIn)
            {
                retval = gameObject;
                break;
            }
        }

        return retval;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void processWindowMessage(WindowMessage messageIn)
    {
        GameObject newWindow = (GameObject)Instantiate(Resources.Load("UI Prefabs/BaseDialogBox"), UICanvas.transform);
        BaseAutoDestructComponent badc = newWindow.GetComponent<BaseAutoDestructComponent>();
        if (badc != null)
        {
            badc.TimeToDisplay = messageIn.MessageDuration;
        }
        GameObject textObj = newWindow.transform.Find("MainDialogText").gameObject;
        Text textComponent = textObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = messageIn.TextToDisplay;
        }
    }


    public void processContextSensitiveToggleMessage(MsgUiConSenseInAdjust messageIn)
    {

        if (messageIn.Increase)
        {
            _interactionZoneCount++;
        } else
        {
            _interactionZoneCount--;
        }

        if (_interactionZoneCount > 0)
        {
            ConSenseInteractionIcon.SetActive(true);
        } else
        {
            _interactionZoneCount = 0;
            ConSenseInteractionIcon.SetActive(false);
        }
    }

    public void AcceptMessage(BaseUIMessage messageIn)
    {
        Debug.Log("UI Received " + messageIn.GetType().Name + " message " + messageIn.UUID.ToString() + "; preparing to render");

        if (messageIn is WindowMessage)
        {
            processWindowMessage((WindowMessage)messageIn);
        } else if (messageIn is MsgUiConSenseInAdjust)
        {
            processContextSensitiveToggleMessage((MsgUiConSenseInAdjust)messageIn);
        }
    }
}
