using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {
    
    public float speedX;
    public GameObject waves;

    int offset = 3;

    void FixedUpdate()
    {
        if (transform.position.x < 62)
        {
            transform.Translate(Time.deltaTime * speedX, 0, 0);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
            GetComponent<Animator>().SetInteger("State", 1);
            transform.GetChild(3).parent = null;
            Destroy(GetComponent<Boat>());

            waves.transform.GetChild(0).GetComponent<Wave>().SetSpeed(10f / offset);
            waves.transform.GetChild(1).GetComponent<Wave>().SetSpeed(7f / offset);
            waves.transform.GetChild(2).GetComponent<Wave>().SetSpeed(6f / offset);
            waves.transform.GetChild(3).GetComponent<Wave>().SetSpeed(5f / offset);
            waves.transform.GetChild(4).GetComponent<Wave>().SetSpeed(3f / offset);
        }

    }
    
}
