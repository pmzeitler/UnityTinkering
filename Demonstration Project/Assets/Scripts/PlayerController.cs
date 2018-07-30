using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //private float velocity = 10;

    private bool useSticks = false;

    //This is the maximum velocity to permit the player to move in.
    private float velocityMax = 10.0f;

    //This is the amount by which to reduce the velocity by per frame.
    private float velocityDecay = 1.0f;

    //This is how much the velocity delta increases each frame that movement persists in a direction.
    private float velocityAccel = 1.5f;

    //Dead zone for movement. If an axis's absolute value is less than this, it will be treated as not moving.
    private float axisDeadZone = 0.15f;

    private Vector2 currentAcceleration = new Vector2(0.0f, 0.0f);

    // Use this for initialization
    void Start ()
    {
    
    }
    
    // Update is called once per frame
    void FixedUpdate ()
    {
        // Cache the inputs.
        float h = 0.0f;
        float v = 0.0f;

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

        bool movingHorizontal = ((h >= axisDeadZone) || (h <= (0.0f - axisDeadZone)));
        bool movingVertical = ((v >= axisDeadZone) || (v <= (0.0f - axisDeadZone)));

        Vector2 newVelocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, this.GetComponent<Rigidbody2D>().velocity.y);

        handleAccelDecel(movingHorizontal, h, ref newVelocity.x, ref currentAcceleration.x);
        handleAccelDecel(movingVertical, v, ref newVelocity.y, ref currentAcceleration.y);

        this.GetComponent<Rigidbody2D>().velocity = newVelocity;

        /**
        this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (velocity * h, this.GetComponent<Rigidbody2D> ().velocity.y);

        if (v != 0) {
            this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (this.GetComponent<Rigidbody2D> ().velocity.x, velocity * v);
        }
        **/
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
