using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    public AudioClip shieldBrokeWood;

    public ParticleSystem burst0;
    public ParticleSystem burst1;
    public ParticleSystem burst2;

    public Vector2 pos0;
    public Vector2 pos1;
    public Vector2 pos2;

    bool broke = false;

    void Start()
    {
        GameController.Instance.totalEnemy++;
    }

    public void BurstCover()
    {
        if (!broke)
        {
            Vector2 pos_0 = new Vector2(transform.position.x + pos0.x, transform.position.y + pos0.y);
            Vector2 pos_1 = new Vector2(transform.position.x + pos1.x, transform.position.y + pos1.y);
            Vector2 pos_2 = new Vector2(transform.position.x + pos2.x, transform.position.y + pos2.y);

            Instantiate(burst0, pos_0, Quaternion.identity);
            Instantiate(burst1, pos_1, Quaternion.identity);
            Instantiate(burst2, pos_2, Quaternion.identity);

            GetComponent<HitAudioSource>().ShieldBrokeWood(shieldBrokeWood);

            Collider2D[] c = GetComponents<Collider2D>();
            c[0].enabled = false;
            c[1].enabled = false;

            transform.GetChild(3).gameObject.SetActive(true);

            broke = true;
        }

    }

}