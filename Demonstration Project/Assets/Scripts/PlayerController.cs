using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform interactionCircle;
    //private float velocity = 10;

    private bool useSticks = false;

    //This is the maximum velocity to permit the player to move in.
    private float velocityMax = 10.0f;

    //This is the amount by which to reduce the velocity by per frame.
    private float velocityDecay = 1.0f;

    //This is how much the velocity delta increases each frame that movement persists in a direction.
    private float velocityAccel = 1.5f;

    //Dead zone for movement. If an axis's absolute value is less than this, it will be treated as not moving.
    private const float AXIS_DEAD_ZONE = 0.15f;

    private const float DIAGONAL_RATIO = 0.75f;

    private Vector2 currentAcceleration = new Vector2(0.0f, 0.0f);

    private Animator animator;

    private bool canSpawnInteractor = true;

    public Direction facingDirection { get; private set; }

    // Use this for initialization
    void Start ()
    {
        this.facingDirection = Direction.NORTH;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // Cache the inputs.
        float h = 0.0f;
        float v = 0.0f;

        DetermineMovement(ref h, ref v);

        bool movingHorizontal = ((h >= AXIS_DEAD_ZONE) || (h <= (0.0f - AXIS_DEAD_ZONE)));
        bool movingVertical = ((v >= AXIS_DEAD_ZONE) || (v <= (0.0f - AXIS_DEAD_ZONE)));

        AdjustFacing(h, v, movingHorizontal, movingVertical);

        Vector2 newVelocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, this.GetComponent<Rigidbody2D>().velocity.y);

        handleAccelDecel(movingHorizontal, h, ref newVelocity.x, ref currentAcceleration.x);
        handleAccelDecel(movingVertical, v, ref newVelocity.y, ref currentAcceleration.y);

        this.GetComponent<Rigidbody2D>().velocity = newVelocity;

        if (Input.GetKeyDown(KeyCode.Space) && canSpawnInteractor)
        {
            Vector3 instantiateLocation = this.GetComponent<Transform>().position;
            switch (facingDirection)
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
            canSpawnInteractor = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            canSpawnInteractor = true;
        }
    }

    private void DetermineMovement(ref float h, ref float v)
    {
        if (useSticks)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                h = -1.0f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                h = 1.0f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                v = -1.0f;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                v = 1.0f;
            }
        }
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
    }

    private void handleAccelDecel(bool moving, float direction, ref float currentVelocity, ref float acceleration)
    {
        if (!moving)
        {
            acceleration = 0.0f;
            if (direction > 0)
            {
                currentVelocity -= velocityDecay;
                if(currentVelocity < 0.0f)
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
            if (direction > 0)
            {
                if ((currentVelocity + acceleration + velocityAccel) < velocityMax)
                {
                    acceleration += velocityAccel;
                    currentVelocity += acceleration;
                }
                else
                {
                    currentVelocity = velocityMax;
                }
            }
            else
            {
                if ((currentVelocity - (acceleration + velocityAccel)) > velocityMax)
                {
                    acceleration += velocityAccel;
                    currentVelocity -= acceleration;
                }
                else
                {
                    currentVelocity = (0.0f - velocityMax);
                }
            }
        }
    }
}
