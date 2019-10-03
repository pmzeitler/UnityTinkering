﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : BaseControllerObject, IQueuesAndProcessesMessages<BasePlayerMessage>
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

    private List<BasePlayerMessage> messageQueue;

    // Use this for initialization
    void Start()
    {
        this.facingDirection = Direction.NORTH;
        this.messageQueue = new List<BasePlayerMessage>();
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
        //TODO: remove this CheckPaused 
        CheckPaused();

        base.FixedUpdate();

        if (!IsPaused)
        {
            InputMappingManager.Instance.CheckUserInput();

            processMessages(messageQueue);
        }

    }

    private void CheckPaused()
    {
        if (Input.GetKeyDown(KeyCode.P) && !pauseKeyDown)
        {
            pauseKeyDown = true;
            gameState.IsPaused = !gameState.IsPaused;
        }

        if (Input.GetKeyUp(KeyCode.P) && pauseKeyDown)
        {
            pauseKeyDown = false;
        }
    }

    private void SpawnInteractorCheck()
    {
        Vector3 instantiateLocation = this.GetComponent<Transform>().position;
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

    public void AcceptMessage(BasePlayerMessage messageIn)
    {
        this.queueMessage(messageIn);
    }

    protected override void EnterPause()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    protected override void ExitPause()
    {
        gameObject.GetComponent<Animator>().enabled = true;
    }

    public void queueMessage(BasePlayerMessage messageIn)
    {
        messageQueue.Add(messageIn);
    }

    public void processMessages(ICollection<BasePlayerMessage> messages)
    {
        if (messages.Count < 1)
        {
            return;
        }
        int messagesProcessed = 0;
        List<BasePlayerMessage> clearThese = new List<BasePlayerMessage>();
        foreach (BasePlayerMessage messageIn in messages)
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
            }
            else
            {
                messageCleared = false;
            }

            if (messageCleared)
            {
                clearThese.Add(messageIn);
            }
        }
        //Debug.Log("PlayerController processed " + messagesProcessed + " of " + messages.Count + " msgs this frame; " + clearThese.Count + " to clear");

        foreach (BasePlayerMessage messageOut in clearThese)
        {
            messages.Remove(messageOut);
        }
        /*
        if (messages.Count > 0)
        {
            Debug.Log("Messages cleared; PlayerController queue still has " + messages.Count + " messages");
            foreach (BasePlayerMessage messageIn in messages)
            {
                Debug.Log("Message held: " + messageIn.GetType().Name);
            }
        }
        */
    }
}
