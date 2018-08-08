using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPCMovement : MonoBehaviour {

    private const int COUNTDOWN_MAX = 90;
    private int countdown = 0;
    private Direction direction = Direction.EAST;

    public float SpeedPerFrame = 0.05f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = gameObject.transform.position;
        countdown--;
        switch (direction)
        {
            case Direction.EAST:
                newPosition.x += SpeedPerFrame;
                if (countdown <= 0)
                {
                    direction = Direction.SOUTH;
                }
                break;
            case Direction.WEST:
                newPosition.x -= SpeedPerFrame;
                if (countdown <= 0)
                {
                    direction = Direction.NORTH;
                }
                break;
            case Direction.NORTH:
                newPosition.y += SpeedPerFrame;
                if (countdown <= 0)
                {
                    direction = Direction.EAST;
                }
                break;
            case Direction.SOUTH:
                newPosition.y -= SpeedPerFrame;
                if (countdown <= 0)
                {
                    direction = Direction.WEST;
                }
                break;
            default:
                //do nothing;
                break;
        }
        if (countdown <= 0)
        {
            countdown = COUNTDOWN_MAX;
        }
        gameObject.transform.position = newPosition;
	}
}
