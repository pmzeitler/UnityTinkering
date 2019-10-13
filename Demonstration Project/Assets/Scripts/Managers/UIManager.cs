using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : ScriptableObject, IAcceptsMessages<BaseUIMessage> {

    public static UIManager _instance;

    private GameObject UICanvas;
    private GameObject ConSenseInteractionIcon;

    private const string CAMERA_NAME = "Main Camera";

    private Camera camera = null;

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
            GameObject cameraObj = FindByName(CAMERA_NAME, SceneManager.GetActiveScene().GetRootGameObjects());
            if (cameraObj != null)
            {
                cameraObj.TryGetComponent<Camera>(out camera);
            } else
            {
                Debug.Log("Could not find a " + CAMERA_NAME + " object to search in");
            }
            if (camera == null)
            {
                Debug.Log("Could not get camera component from cameraObj");
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

    public Camera Camera { get
        {
            return camera;
        }
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


    private void processContextSensitiveToggleMessage(MsgUiConSenseInAdjust messageIn)
    {

        if (messageIn.Activate)
        {
            ConSenseInteractionIcon.SetActive(true);
            //MessagingManager.Instance.AcceptMessage(new MsgPlayerFacingRequest(messageIn.OriginObject));

        } else
        {
            ConSenseInteractionIcon.SetActive(false);
        }
    }

    public void AcceptMessage(BaseUIMessage messageIn)
    {
        

        if (messageIn is WindowMessage)
        {
            processWindowMessage((WindowMessage)messageIn);
        } else if (messageIn is MsgUiConSenseInAdjust)
        {
            processContextSensitiveToggleMessage((MsgUiConSenseInAdjust)messageIn);
        }
        else if (messageIn is MsgUISmallSpeechBubble)
        {
            ProcessSpeechBubbleMessage((MsgUISmallSpeechBubble)messageIn);
        } else
        {
            Debug.Log("UIManager Received " + messageIn.GetType().Name + " message, but no handler is established");
        }
    }

    private void ProcessSpeechBubbleMessage(MsgUISmallSpeechBubble messageIn)
    {
        GameObject newWindow = (GameObject)Instantiate(Resources.Load("UI Prefabs/SpeechBubblePrefab"), UICanvas.transform);
        BaseAutoDestructSpeechBubble badc = newWindow.GetComponent<BaseAutoDestructSpeechBubble>();
        if (badc != null)
        {
            badc.TimeToDisplay = messageIn.MessageDuration;
            badc.TrackObject = messageIn.OriginObject;
            badc.offset = messageIn.BubbleOffset;
        }
        GameObject textObj = newWindow.transform.Find("SpeechBubbleText").gameObject;
        Text textComponent = textObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = messageIn.WindowMessage;
        }

        Vector2 screenPos = AdjustPositionForUIDisplay(messageIn.OriginObject.transform.position);

        badc.transform.localPosition = screenPos + badc.offset;


    }


    public Vector2 AdjustPositionForUIDisplay(Vector2 basePosition)
    {
        Vector2 viewPos = camera.WorldToViewportPoint(basePosition);
        viewPos.x -= 0.5f;
        viewPos.y -= 0.5f;
        Vector2 retval = new Vector2(camera.pixelWidth * viewPos.x, camera.pixelHeight * viewPos.y);
        return retval;
    }





}
