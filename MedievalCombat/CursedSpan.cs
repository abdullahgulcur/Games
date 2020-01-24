using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedSpan : MonoBehaviour {

    private bool foundPlayer = false;
    private GameObject player;

    CursedController ic;

    void Start()
    {
        GameController.Instance.totalEnemy++;
    }

    void Update()
    {

        if (Time.frameCount % 30 == 0)
        {
            FindPlayer();
        }

    }

    void FindPlayer()
    {
        if (!foundPlayer)
        {
            Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 20f, transform.position.y + 5f),
                new Vector2(transform.position.x + 20f, transform.position.y - 5f));

            int lenght = colliders.Length;

            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].gameObject.CompareTag("AchillesBody"))
                {
                    player = colliders[i].gameObject;
                    foundPlayer = true;
                    this.transform.GetChild(0).gameObject.SetActive(true);
                    ic = transform.GetChild(0).GetComponent<CursedController>();
                    ic.SetPlayer(player);
                    this.transform.GetChild(0).gameObject.transform.parent = null;
                    Destroy(this.gameObject);
                }
            }
        }
    }



}
