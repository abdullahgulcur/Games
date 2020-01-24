using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCastle : MonoBehaviour {

    float interpVelocity;
    public float smooth;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    public GameObject back;


    void Start () {
        targetPos = transform.position;
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * smooth;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

           // targetPos.y = offset.y;

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 1f);


            back.transform.SetPositionAndRotation(new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

        }

    }
}
