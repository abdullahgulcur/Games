using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpan : MonoBehaviour {

    private bool foundPlayer = false;
    private GameObject player;

    ArcherController ac;

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
            Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 50f, transform.position.y + 5f),
                new Vector2(transform.position.x + 50f, transform.position.y - 5f));

            int lenght = colliders.Length;

            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].gameObject.CompareTag("AchillesBody"))
                {
                    player = colliders[i].gameObject;
                    foundPlayer = true;
                    this.transform.GetChild(0).gameObject.SetActive(true);
                    ac = transform.GetChild(0).GetComponent<ArcherController>();
                    ac.SetPlayer(player);
                    this.transform.GetChild(0).gameObject.transform.parent = null;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
