using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Camera : MonoBehaviour {

    float interpVelocity;
    public float smooth;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    public GameObject back;

    public GameObject clouds;
    public Vector2 cloudsOffset;
    Vector3 clouds_startPos;

    public GameObject bush_back;

    public GameObject forest_layer_0;
    public GameObject forest_layer_1;
    public GameObject forest_layer_2;

    public GameObject sea_mountain_layer_0;
    Vector3 sea_mountain_startPos;

    Vector3 forest_layer_0_startPos;
    Vector3 forest_layer_1_startPos;
    Vector3 forest_layer_2_startPos;

    Vector3 tree_back_startPos;

    Vector3 bush_back_startPos;


    public GameObject tree_back;

    public GameObject hints;
    public GameObject eventSystem;

    void Start()
    {
        targetPos = transform.position;

        clouds_startPos = clouds.transform.position;

        sea_mountain_startPos = sea_mountain_layer_0.transform.position;

        forest_layer_0_startPos = forest_layer_0.transform.position;
        forest_layer_1_startPos = forest_layer_1.transform.position;
        forest_layer_2_startPos = forest_layer_2.transform.position;

        bush_back_startPos = bush_back.transform.position;

        tree_back_startPos = tree_back.transform.position;
        
        if (GameController.Instance.firstGameOpen)
        {
            eventSystem.SetActive(false);
            StartCoroutine(ShowHints());
        }
    }

    IEnumerator ShowHints()
    {
        Text hintsText = hints.transform.GetChild(1).GetComponent<Text>();

        yield return new WaitForSeconds(1.3f);
        hintsText.text = "You have to kill all of enemy in that level.";
        hints.SetActive(true);
        yield return new WaitForSeconds(2f);
        hintsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        hintsText.text = "Each level has more than 10 enemies with different kind of equipments.";
        hintsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        hintsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        hintsText.text = "If you leave before killing each enemy, you cannot earn any money...";
        hintsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        hintsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        hintsText.text = "Good luck, warrior !";
        hintsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        hints.SetActive(false);

        eventSystem.SetActive(true);
        GameController.Instance.firstGameOpen = false;
        SaveSystem.SavePlayer();
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

            clouds.transform.SetPositionAndRotation(new Vector2((clouds_startPos + this.transform.position / 1.1f).x, transform.position.y + cloudsOffset.y + clouds_startPos.y), Quaternion.identity);
            back.transform.SetPositionAndRotation(new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            forest_layer_0.transform.SetPositionAndRotation(new Vector2((forest_layer_0_startPos + this.transform.position / 1.2f).x, forest_layer_0_startPos.y), Quaternion.identity);
            forest_layer_1.transform.SetPositionAndRotation(new Vector2((forest_layer_1_startPos + this.transform.position / 1.5f).x, forest_layer_1_startPos.y), Quaternion.identity);
            forest_layer_2.transform.SetPositionAndRotation(new Vector2((forest_layer_2_startPos + this.transform.position / 2f).x, forest_layer_2_startPos.y), Quaternion.identity);

            sea_mountain_layer_0.transform.SetPositionAndRotation(new Vector3((sea_mountain_startPos + this.transform.position / 1.05f).x, transform.position.y / 1.5f, sea_mountain_startPos.z), Quaternion.identity);

            tree_back.transform.SetPositionAndRotation(new Vector2((tree_back_startPos + this.transform.position / 3f).x, tree_back_startPos.y), Quaternion.identity);

            bush_back.transform.SetPositionAndRotation(new Vector3((bush_back_startPos + this.transform.position / 4f).x, bush_back_startPos.y, bush_back_startPos.z), Quaternion.identity);

        }

    }
}
