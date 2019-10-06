using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : BaseControllerObject, IQueuesAndProcessesMessages<BasePlayerControlMessage>
{
    public Transform interactionCircle;
    //private float velocity = 10;

    private bool useSticks = true;

    //This is the maximum velocity to permit the player to move in.
    public float velocityMax = 500.0f;

    //This is the amount by which to reduce the velocity by per frame.
    private float velocityDecay = 10.0f;

    //This is how much the velocity delta increases each frame that movement persists in a direction.
    private float velocityAccel = 15.0f;

    //Dead zone for movement. If an axis's absolute value is less than this, it will be treated as not moving.
    private const float AXIS_DEAD_ZONE = 0.15f;

    private const float DIAGONAL_RATIO = 0.75f;

    private Vector2 currentAcceleration = new Vector2(0.0f, 0.0f);

    private Animator animator;

    //private bool canSpawnInteractor = true;
    private bool pauseKeyDown = false;

    public Direction facingDirection { get; private set; }

    private Dictionary<ControlStep, List<BasePlayerControlMessage>> messageQueues;

    // Use this for initialization
    void Start()
    {
        this.facingDirection = Direction.NORTH;
        this.messageQueues = new Dictionary<ControlStep, List<BasePlayerControlMessage>>();
        messageQueues[ControlStep.FIRST] = new List<BasePlayerControlMessage>();
        messageQueues[ControlStep.GENERAL] = new List<BasePlayerControlMessage>();
        messageQueues[ControlStep.LAST] = new List<BasePlayerControlMessage>();

    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        //Debug.Log("PlayerData instance is " + playerData);
        playerData.PlayerController = this;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        //TODO: move this to an InputController object
        InputMappingManager.Instance.CheckUserInput();

        processMessages(messageQueues[ControlStep.FIRST]);
        processMessages(messageQueues[ControlStep.GENERAL]);
        processMessages(messageQueues[ControlStep.LAST]);
    }



    private void SpawnInteractorCheck()
    {
        Vector3 instantiateLocation = gameObject.transform.position;
        switch (playerData.FacingDirection)
        {
            case Direction.NORTH:
                instantiateLocation.y += this.GetComponent<Collider2D>().bounds.size.y;
                break;
            case Direction.SOUTH:
                instantiateLocation.y -= this.GetComponent<Collider2D>().bounds.size.y;
                break;
            case Direction.EAST:
                instantiateLocation.x += this.GetComponent<Collider2D>().bounds.size.x;
                break;
            case Direction.WEST:
                instantiateLocation.x -= this.GetComponent<Collider2D>().bounds.size.x;
                break;
            case Direction.NORTHEAST:
                instantiateLocation.y += (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x += (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.NORTHWEST:
                instantiateLocation.y += (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x -= (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.SOUTHEAST:
                instantiateLocation.y -= (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x += (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.SOUTHWEST:
                instantiateLocation.y -= (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x -= (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;

        }
        Instantiate(interactionCircle, instantiateLocation, Quaternion.identity);

    }

    private void AdjustContextCheckCircle()
    {
        Vector3 instantiateLocation = gameObject.transform.position;
        switch (playerData.FacingDirection)
        {
            case Direction.NORTH:
                instantiateLocation.y += this.GetComponent<Collider2D>().bounds.size.y;
                break;
            case Direction.SOUTH:
                instantiateLocation.y -= this.GetComponent<Collider2D>().bounds.size.y;
                break;
            case Direction.EAST:
                instantiateLocation.x += this.GetComponent<Collider2D>().bounds.size.x;
                break;
            case Direction.WEST:
                instantiateLocation.x -= this.GetComponent<Collider2D>().bounds.size.x;
                break;
            case Direction.NORTHEAST:
                instantiateLocation.y += (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x += (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.NORTHWEST:
                instantiateLocation.y += (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x -= (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.SOUTHEAST:
                instantiateLocation.y -= (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x += (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;
            case Direction.SOUTHWEST:
                instantiateLocation.y -= (this.GetComponent<Collider2D>().bounds.size.y * DIAGONAL_RATIO);
                instantiateLocation.x -= (this.GetComponent<Collider2D>().bounds.size.x * DIAGONAL_RATIO);
                break;

        }
        //Debug.Log("ContextCheckZone is being adjusted");
        gameObject.transform.Find("ContextCheckZone").position = instantiateLocation;
    }

    private void AdjustFacing(float h, float v, bool movingHorizontal, bool movingVertical)
    {
        string triggerName = "Moving_";

        if (movingHorizontal && !movingVertical)
        {
            if (h > 0)
            {
                triggerName += "E";
            }
            else
            {
                triggerName += "W";
            }
        }
        else if (!movingHorizontal && movingVertical)
        {
            if (v > 0)
            {
                triggerName += "N";
            }
            else
            {
                triggerName += "S";
            }
        }
        else if (movingHorizontal && movingVertical)
        {
            if (v > 0)
            {
                triggerName += "N";
            }
            else
            {
                triggerName += "S";
            }

            if (h > 0)
            {
                triggerName += "E";
            }
            else
            {
                triggerName += "W";
            }
        }
        else
        {
            triggerName = null;
        }

        if ((triggerName != null) && (triggerName != "Moving_"))
        {
            animator.SetTrigger(triggerName);
            switch (triggerName)
            {
                case "Moving_N":
                    facingDirection = Direction.NORTH;
                    break;
                case "Moving_NE":
                    facingDirection = Direction.NORTHEAST;
                    break;
                case "Moving_E":
                    facingDirection = Direction.EAST;
                    break;
                case "Moving_SE":
                    facingDirection = Direction.SOUTHEAST;
                    break;
                case "Moving_S":
                    facingDirection = Direction.SOUTH;
                    break;
                case "Moving_SW":
                    facingDirection = Direction.SOUTHWEST;
                    break;
                case "Moving_W":
                    facingDirection = Direction.WEST;
                    break;
                case "Moving_NW":
                    facingDirection = Direction.NORTHWEST;
                    break;
                default:
                    //do nothing;
                    break;
            }
        }
        playerData.MovingDirection = facingDirection;
        playerData.FacingDirection = facingDirection;
        AdjustContextCheckCircle();
    }

    private void forceFacing(Direction? faceThis)
    {
        if(faceThis == null)
        {
            return;
        }
        string animationTarget = "Moving_";
        switch (faceThis)
        {
            case Direction.EAST:
                animationTarget += "E";
                break;
            case Direction.NORTHEAST:
                animationTarget += "NE";
                break;
            case Direction.SOUTHEAST:
                animationTarget += "SE";
                break;
            case Direction.NORTH:
                animationTarget += "N";
                break;
            case Direction.SOUTH:
                animationTarget += "S";
                break;
            case Direction.WEST:
                animationTarget += "W";
                break;
            case Direction.NORTHWEST:
                animationTarget += "NW";
                break;
            case Direction.SOUTHWEST:
                animationTarget += "SW";
                break;
        }
        animator.SetTrigger(animationTarget);
        playerData.FacingDirection = (Direction)faceThis;
        playerData.MovingDirection = (Direction)faceThis;
        AdjustContextCheckCircle();

    }

    private bool CheckIfAllSame(params bool[] values)
    {
        bool retval = true;
        bool checkAgainst = values[0];
        foreach (bool val in values)
        {
            if (checkAgainst ^ val)
            {
                retval = false;
                break;
            }
        }

        return retval;
    }

    private Direction? DetermineFacingDirection(Vector2 targetPosition, bool faceAway = false)
    {
        Direction? retval = null;

        Vector2 myPosition = gameObject.transform.position;

        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        if (myPosition.x > targetPosition.x)
        {
            west = true;
        } else if (myPosition.x < targetPosition.x)
        {
            east = true;
        }

        if (myPosition.y > targetPosition.y)
        {
            south = true;
        }
        else if (myPosition.y < targetPosition.y)
        {
            north = true;
        }

        //Debug.Log("Discovered directions: N-" + north + " S-" + south + " E-" + east + " W-" + west);

        if (faceAway)
        {
            north = !north;
            south = !south;
            east = !east;
            west = !west;
        }

        //if they're all the same, the target is right on top of me & no facing should change
        if (!CheckIfAllSame(north, south, east, west))
        {
            if (north && !south)
            {
                if (east && !west)
                {
                    retval = Direction.NORTHEAST;
                } else  if (west && !east)
                {
                    retval = Direction.NORTHWEST;
                } else
                {
                    retval = Direction.NORTH;
                }
            } else if (south && !north)
            {
                if (east && !west)
                {
                    retval = Direction.SOUTHEAST;
                }
                else if (west && !east)
                {
                    retval = Direction.SOUTHWEST;
                }
                else
                {
                    retval = Direction.SOUTH;
                }
            } else if (east && !west)
            {
                retval = Direction.EAST;
            }
            else if (west && !east)
            {
                retval = Direction.WEST;
            }
        } else
        {
           // Debug.Log("All identical directions: N-" + north + " S-" + south + " E-" + east + " W-" + west);
        }

        return retval;
    }



    private void handleAccelDecel(bool moving, float direction, ref float currentVelocity, ref float acceleration)
    {
        if (!moving)
        {
            acceleration = 0.0f;
            if (currentVelocity > 0)
            {
                currentVelocity -= velocityDecay;
                if (currentVelocity < 0.0f)
                {
                    currentVelocity = 0.0f;
                }
            }
            else
            {
                currentVelocity += velocityDecay;
                if (currentVelocity > 0.0f)
                {
                    currentVelocity = 0.0f;
                }
            }
        }
        else
        {
            float intendedVelocityMax = velocityMax * Mathf.Abs(direction);
            if (direction > 0)
            {
                if ((currentVelocity + acceleration + velocityAccel) < intendedVelocityMax)
                {
                    acceleration += velocityAccel;
                    currentVelocity += acceleration;
                }
                else
                {
                    currentVelocity = intendedVelocityMax;
                }
            }
            else
            {
                if (Mathf.Abs(currentVelocity - (acceleration + velocityAccel)) < intendedVelocityMax)
                {
                    acceleration += velocityAccel;
                    currentVelocity -= acceleration;
                }
                else
                {
                    currentVelocity = (0.0f - intendedVelocityMax);
                }
            }
        }
    }

    private void HandleMovement(Vector2 movement, bool movingHorizontal, bool movingVertical)
    {
        AdjustFacing(movement.x, movement.y, movingHorizontal, movingVertical);

        Vector2 newVelocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, this.GetComponent<Rigidbody2D>().velocity.y);

        handleAccelDecel(movingHorizontal, movement.x, ref newVelocity.x, ref currentAcceleration.x);
        handleAccelDecel(movingVertical, movement.y, ref newVelocity.y, ref currentAcceleration.y);

        this.GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    private void HandleFacingRequest(MsgPlayerFacingRequest messageIn)
    {
        //Debug.Log("Attempting to face object at " + messageIn.OriginObject.transform.position + "; player position is " + gameObject.transform.position);
        Direction? faceThis = DetermineFacingDirection(messageIn.OriginObject.transform.position, messageIn.FaceAway);
        //Debug.Log("Facing direction should be " + faceThis);
        forceFacing(faceThis);
    }

    public void AcceptMessage(BasePlayerControlMessage messageIn)
    {
        if (!IsPaused)
        {
            this.queueMessage(messageIn);
        }
    }

    protected override void EnterPause()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    protected override void ExitPause()
    {
        gameObject.GetComponent<Animator>().enabled = true;
    }

    public void queueMessage(BasePlayerControlMessage messageIn)
    {
        messageQueues[messageIn.ControlStep].Add(messageIn);
    }

    public void processMessages(ICollection<BasePlayerControlMessage> messages)
    {
        if (messages.Count < 1)
        {
            return;
        }
        int messagesProcessed = 0;
        List<BasePlayerControlMessage> clearThese = new List<BasePlayerControlMessage>();
        foreach (BasePlayerControlMessage messageIn in messages)
        {
            bool messageCleared = true;
            if (messageIn is MsgPlayerSpawnInteraction)
            {
                SpawnInteractorCheck();
                messagesProcessed++;
            } else if (messageIn is MsgPlayerMovementRequest)
            {
                HandleMovement(((MsgPlayerMovementRequest)messageIn).StickPosition, ((MsgPlayerMovementRequest)messageIn).MovingHorizontal, ((MsgPlayerMovementRequest)messageIn).MovingVertical);

                messagesProcessed++;
            } else if (messageIn is MsgPlayerFacingRequest)
            {
                //TODO: This is temporarily disabled
                //HandleFacingRequest((MsgPlayerFacingRequest)messageIn);
                messagesProcessed++;
            }
            else
            {
                Debug.Log("PlayerController received " + messageIn.GetType().Name + " message during " + messageIn.ControlStep + " step, but no handler is established");
                messageCleared = false;
            }

            if (messageCleared)
            {
                clearThese.Add(messageIn);
            }
        }
        //Debug.Log("PlayerController processed " + messagesProcessed + " of " + messages.Count + " msgs this frame; " + clearThese.Count + " to clear");

        foreach (BasePlayerControlMessage messageOut in clearThese)
        {
            messages.Remove(messageOut);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            if (collision.gameObject.tag.StartsWith("NPC"))
            {
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(collision.gameObject, true));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPaused)
        {
            if (collision.gameObject.tag.StartsWith("NPC"))
            {
                messenger.AcceptMessage(new MsgUiConSenseInAdjust(collision.gameObject, false));
            }
        }
    }






}
