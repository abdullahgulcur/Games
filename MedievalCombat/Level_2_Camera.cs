using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_2_Camera : MonoBehaviour {

    public GameObject forest_layer_0;
    public GameObject forest_layer_1;
    public GameObject forest_layer_2;
    public GameObject forest_layer_3;
    public GameObject forest_layer_4;

   // public GameObject tree_back;
    public GameObject tree_front;
    public GameObject bush_front;

    float interpVelocity;
    public float smooth;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    Vector3 forest_startPos;
    //Vector3 tree_back_startPos;
    Vector3 tree_front_startPos;
    Vector3 bush_front_startPos;
    Vector3 bush_back_startPos;
    Vector3 sun_startPos;

    Vector3 forest_layer_0_startPos;
    Vector3 forest_layer_1_startPos;
    Vector3 forest_layer_2_startPos;
    Vector3 forest_layer_3_startPos;
    Vector3 forest_layer_4_startPos;


    void Start () {

        targetPos = transform.position;

      //  tree_back_startPos = tree_back.transform.position;
        tree_front_startPos = tree_front.transform.position;
        bush_front_startPos = bush_front.transform.position;

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

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            targetPos.y = offset.y;

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 1f);
            
          //  tree_back.transform.SetPositionAndRotation(new Vector3((tree_back_startPos - this.transform.position / 20f).x, tree_back_startPos.y, tree_back_startPos.z), Quaternion.identity);
            tree_front.transform.SetPositionAndRotation(new Vector3((tree_front_startPos - this.transform.position / 2f).x, tree_front_startPos.y, tree_front_startPos.z), Quaternion.identity);
            bush_front.transform.SetPositionAndRotation(new Vector3((bush_front_startPos - this.transform.position / 1.5f).x, bush_front_startPos.y, bush_front_startPos.z), Quaternion.identity);

            //forest layers
            forest_layer_0.transform.SetPositionAndRotation(new Vector3((forest_layer_0_startPos + this.transform.position / 1.15f).x, forest_layer_0_startPos.y, forest_layer_0_startPos.z), Quaternion.identity);
            forest_layer_1.transform.SetPositionAndRotation(new Vector3((forest_layer_1_startPos + this.transform.position / 1.25f).x, forest_layer_1_startPos.y, forest_layer_1_startPos.z), Quaternion.identity);
            forest_layer_2.transform.SetPositionAndRotation(new Vector3((forest_layer_2_startPos + this.transform.position / 1.4f).x, forest_layer_2_startPos.y, forest_layer_2_startPos.z), Quaternion.identity);
            forest_layer_3.transform.SetPositionAndRotation(new Vector3((forest_layer_3_startPos + this.transform.position / 2f).x, forest_layer_3_startPos.y, forest_layer_3_startPos.z), Quaternion.identity);
            forest_layer_4.transform.SetPositionAndRotation(new Vector3((forest_layer_4_startPos + this.transform.position / 3f).x, forest_layer_4_startPos.y, forest_layer_4_startPos.z), Quaternion.identity);
            
        }
    }

}
