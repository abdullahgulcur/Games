using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {

    float length, startpos;
    public GameObject camera;
    public float parallaxEffect;

    public float offsetX;

    private bool hitShore;

    void Start () {

        hitShore = false;
        startpos = transform.position.x;// + offsetX;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!hitShore)
        {
            float temp = camera.transform.position.x * (1 - parallaxEffect);
            float dist = camera.transform.position.x * parallaxEffect;

            transform.position = new Vector2(startpos + dist + offsetX, transform.position.y);

            if (temp > startpos + length)
                startpos += length;
            else if (temp < startpos - length)
                startpos -= length;
        }

        

	}

    public void SetHitShore()
    {
        hitShore = true;
    }
}
