using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // back olanlar uzak olanlar
    public GameObject air;

    public GameObject forest_layer_0;
    public GameObject forest_layer_1;
    public GameObject forest_layer_2;
    public GameObject forest_layer_3;
    public GameObject forest_layer_4;
    
    public GameObject tree_front;
    public GameObject bush_front;

    public GameObject clouds;
    public GameObject sun;
    
    float interpVelocity;
    public float smooth;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    Vector3 forest_startPos;
    Vector3 tree_front_startPos;
    Vector3 bush_front_startPos;
    Vector3 clouds_startPos;
    Vector3 sun_startPos;

    Vector3 forest_layer_0_startPos;
    Vector3 forest_layer_1_startPos;
    Vector3 forest_layer_2_startPos;
    Vector3 forest_layer_3_startPos;
    Vector3 forest_layer_4_startPos;

    void Start()
    {
        targetPos = transform.position;
        
        tree_front_startPos = tree_front.transform.position;
        bush_front_startPos = bush_front.transform.position;
        clouds_startPos = clouds.transform.position;
        sun_startPos = sun.transform.position;

        forest_layer_0_startPos = forest_layer_0.transform.position;
        forest_layer_1_startPos = forest_layer_1.transform.position;
        forest_layer_2_startPos = forest_layer_2.transform.position;
        forest_layer_3_startPos = forest_layer_3.transform.position;
        forest_layer_4_startPos = forest_layer_4.transform.position;
    }
    
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);
            
            interpVelocity = targetDirection.magnitude * smooth;

           // Debug.Log(targetDirection.x.ToString());
            /*
            if (targetDirection.x > 0)
                offset.x = 0.15f;
            else if (targetDirection.x < 0)
                offset.x = -0.15f;
            else
                offset.x = 0f;
                */
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            targetPos.y = offset.y;

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 1f);


            tree_front.transform.SetPositionAndRotation(new Vector2((tree_front_startPos - this.transform.position / 2f).x, tree_front_startPos.y), Quaternion.identity);
            bush_front.transform.SetPositionAndRotation(new Vector2((bush_front_startPos - this.transform.position / 2f).x, bush_front_startPos.y), Quaternion.identity);
            clouds.transform.SetPositionAndRotation(new Vector2((clouds_startPos + this.transform.position / 1.1f).x, clouds_startPos.y), Quaternion.identity);

            //forest layers
            forest_layer_0.transform.SetPositionAndRotation(new Vector2((forest_layer_0_startPos + this.transform.position / 1.2f).x, forest_layer_0_startPos.y), Quaternion.identity);
            forest_layer_1.transform.SetPositionAndRotation(new Vector2((forest_layer_1_startPos + this.transform.position / 1.5f).x, forest_layer_1_startPos.y), Quaternion.identity);
            forest_layer_2.transform.SetPositionAndRotation(new Vector2((forest_layer_2_startPos + this.transform.position / 2f).x, forest_layer_2_startPos.y), Quaternion.identity);
            forest_layer_3.transform.SetPositionAndRotation(new Vector2((forest_layer_3_startPos + this.transform.position / 2.5f).x, forest_layer_3_startPos.y), Quaternion.identity);
            forest_layer_4.transform.SetPositionAndRotation(new Vector2((forest_layer_4_startPos + this.transform.position / 3f).x, forest_layer_4_startPos.y), Quaternion.identity);

            air.transform.SetPositionAndRotation(new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            sun.transform.SetPositionAndRotation(new Vector2((sun_startPos + this.transform.position).x, sun_startPos.y), Quaternion.identity);
            
        }
    }


}
