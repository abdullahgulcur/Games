using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    int y_axis;

    public float speedY;

    Rigidbody2D r2d;

    float stopLowerY;

    GameObject elevator;

    public float stopHigherY;

    void Start () {


        y_axis = 0;
        elevator = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        r2d = elevator.GetComponent<Rigidbody2D>();


        stopLowerY = elevator.transform.position.y;

    }

    void FixedUpdate () {
		
        if(y_axis == 1)
        {
            if (elevator.transform.localPosition.y < stopHigherY)
                r2d.velocity = new Vector2(0, speedY);
            else
                r2d.velocity = new Vector2(0f, 0f);
            
        }else if(y_axis == -1)
        {
            if(elevator.transform.position.y > stopLowerY)
                r2d.velocity = new Vector2(0, -speedY);
            else
                r2d.velocity = new Vector2(0f, 0f);
        }

	}

    public void Set_Y_Axis(int x)
    {
        y_axis = x;
    }
}
