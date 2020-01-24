using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    Vector3 startPos;

    public float speed;

    float length;

    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

	void FixedUpdate () {

        transform.Translate(Time.deltaTime * speed, 0,0);
        
        if(transform.position.x - startPos.x > length)
        {
            transform.position = startPos;
        }
    }

    public void SetSpeed(float x)
    {
        speed = x;
    }
}
