using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private Animator ac;
    private Rigidbody rb;
    public bool isGrounded;
    
    void Start()
    {
        ac = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            ac.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            ac.SetInteger("State", 0);

        }

        if (Input.GetKey(KeyCode.L))
        {
            ac.SetInteger("State", 2);

        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            ac.SetInteger("State", 0);

        }


        GroundCheck();
    }
    
    public void Move(Vector3 dir)
    {

    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 0.05f;
        Vector3 dir = new Vector3(0, -1f);

        Debug.DrawRay(transform.position, dir, Color.green);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
