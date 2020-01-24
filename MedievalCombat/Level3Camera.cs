using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Camera : MonoBehaviour {

    public GameObject mountainLayer;

    public GameObject forest_layer_0;
    public GameObject forest_layer_1;
    public GameObject forest_layer_2;

    public GameObject hill_layer;

    public GameObject fog_layer;
    public GameObject moonLayer;
    public GameObject starsLayer;
    public GameObject airLayer;

    public GameObject tree_front;

    float interpVelocity;
    public float smooth;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    Vector2 fog_layer_startPos;
    Vector2 moonLayer_startPos;
    Vector2 starsLayer_startPos;
    Vector2 airLayer_startPos;

    Vector2 hill_layer_startPos;

    Vector3 mountain_startPos;

    Vector3 tree_front_startPos;

    Vector3 forest_layer_0_startPos;
    Vector3 forest_layer_1_startPos;
    Vector3 forest_layer_2_startPos;


    void Start () {

        targetPos = transform.position;

        mountain_startPos = mountainLayer.transform.position;

        tree_front_startPos = tree_front.transform.position;
        hill_layer_startPos = hill_layer.transform.position;
        forest_layer_0_startPos = forest_layer_0.transform.position;
        forest_layer_1_startPos = forest_layer_1.transform.position;
        forest_layer_2_startPos = forest_layer_2.transform.position;

        fog_layer_startPos = fog_layer.transform.position;
        moonLayer_startPos = moonLayer.transform.position;
        starsLayer_startPos = starsLayer.transform.position;
        airLayer_startPos = airLayer.transform.position;

    }


    void FixedUpdate() {
        
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * smooth;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            targetPos.y = offset.y;

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 1f);
            
            tree_front.transform.SetPositionAndRotation(new Vector3((tree_front_startPos - this.transform.position / 2f).x, tree_front_startPos.y, tree_front_startPos.z), Quaternion.identity);

            mountainLayer.transform.SetPositionAndRotation(new Vector3((mountain_startPos + this.transform.position / 1.1f).x, mountain_startPos.y, mountain_startPos.z), Quaternion.identity);
            
            fog_layer.transform.SetPositionAndRotation(new Vector2(fog_layer_startPos.x + transform.position.x, fog_layer_startPos.y), Quaternion.identity);
            starsLayer.transform.SetPositionAndRotation(new Vector2(starsLayer_startPos.x +transform.position.x, starsLayer_startPos.y), Quaternion.identity);
            moonLayer.transform.SetPositionAndRotation(new Vector2(moonLayer_startPos.x + transform.position.x, moonLayer_startPos.y), Quaternion.identity);
            airLayer.transform.SetPositionAndRotation(new Vector2(airLayer_startPos.x + transform.position.x, airLayer_startPos.y), Quaternion.identity);

            hill_layer.transform.SetPositionAndRotation(new Vector2((hill_layer_startPos.x + (this.transform.position / 1.8f).x), hill_layer_startPos.y), Quaternion.identity);

            //forest layers
            forest_layer_0.transform.SetPositionAndRotation(new Vector3((forest_layer_0_startPos + this.transform.position / 1.15f).x, forest_layer_0_startPos.y, forest_layer_0_startPos.z), Quaternion.identity);
            forest_layer_1.transform.SetPositionAndRotation(new Vector3((forest_layer_1_startPos + this.transform.position / 1.25f).x, forest_layer_1_startPos.y, forest_layer_1_startPos.z), Quaternion.identity);
            forest_layer_2.transform.SetPositionAndRotation(new Vector3((forest_layer_2_startPos + this.transform.position / 1.4f).x, forest_layer_2_startPos.y, forest_layer_2_startPos.z), Quaternion.identity);

        }

    }
}
