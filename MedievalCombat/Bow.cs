using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    Animator anim;

    public GameObject arrow0; 
    public GameObject arrow1; // embered

    public float range;

    public float maxRange; // oyuncuya hangi mesafeden sonra sikacagini belirtir

    GameObject player;

    public GameObject[] bows;

    float rot_z;

    public float rot_offset;
    Vector3 direction;

    bool foundPlayer = false;

    int bowIndex;

    int ember;

    void Start () {
        Destroy(transform.GetChild(0).gameObject);
        anim = GetComponent<Animator>();
        SetBow();
        SetEmberLogic();

    }
	
	void Update () {

        if (!foundPlayer)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 30f);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("AchillesBody"))
                {
                    player = colliders[i].gameObject;
                    foundPlayer = true;
                }
            }

        }

        if (foundPlayer)
        {
            if (GameController.Instance.playerDead)
            {
                anim.SetInteger("State", 0);
            }
            else
            {
                direction = player.transform.position - transform.position;

                direction.Normalize();
                rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180 + rot_offset);// - 90

                if(Vector3.Distance(transform.position, player.transform.position) < maxRange)
                    anim.SetInteger("State", 1);
                else
                    anim.SetInteger("State", 0);
            }
        }

    }

    void SetEmberLogic()
    {
        if (bowIndex > 3)
            if (Utility.GetRandomNumber(0, 2) == 0)
                ember = 1;

    }

    void SetBow()
    {
        bowIndex = Utility.GetBowLogic(bows, GameController.Instance.player_level);
        
        bows[bowIndex].SetActive(true);
    }

    public void BowThrow()
    {
        if(ember == 0)
        {
            GameObject arrow = Instantiate(arrow0, transform.position, Quaternion.Euler(0f, 0f, rot_z - 90 + rot_offset)) as GameObject;
            arrow.GetComponent<Arrow>().SetDamageAmount(Utility.GetArrowDamageAmount(bowIndex, 0));
            arrow.GetComponent<Arrow>().SetVector(direction);
        }
        else
        {
            GameObject arrow = Instantiate(arrow1, transform.position, Quaternion.Euler(0f, 0f, rot_z - 90 + rot_offset)) as GameObject;
            arrow.GetComponent<Arrow>().SetDamageAmount(Utility.GetArrowDamageAmount(bowIndex, 1));
            arrow.GetComponent<Arrow>().SetVector(direction);
        }
        
    }


}
