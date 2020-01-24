using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsController : MonoBehaviour {

    public float blowSpeed = 2.85f;
    public GameObject player;
    public GameObject body;
    public float distance = 4f;
    public GameObject characterDistance;

    float speedBoost = 8f;
    Rigidbody2D rb;
    Animator anim;
    float speed;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    bool isAttacked;
    float timer;
    int time;
    bool died;
    int dir;

    HitAudioSource has;
    SwingAudioSource sas;

    public GameObject weapon;

    bool checkAllDied = true;

    void Start () {

        sas = GetComponent<SwingAudioSource>();
        has = GetComponent<HitAudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate()
    {

        if (GameController.Instance.playerDead)
        {
            anim.SetInteger("State", 0);

            if (rb != null)
                StopMoving();
        }
        else

        if (!died)
        {
            SetDirection();
            SetTimer();
            SetAnimation();

            if (dir == -1)
                speed *= speedBoost * 1.5f;
            else
                speed *= speedBoost * 1.5f;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
            else if (speed != 0)
                MoveHorizontal(speed);
            else
                StopMoving();


        }
        else
        {
            if (checkAllDied)
            {
                if (GameController.Instance.atBossRoom)
                {
                    if (!GameController.Instance.gameFinished)
                        if (GameController.Instance.current_level == GameController.Instance.player_level)
                        {
                            GameController.Instance.levelPassed = true;
                            GameController.Instance.castlePassed = true;
                            GameController.Instance.player_level++;
                        }

                    player.transform.parent.GetComponent<PlayerMobileCtrl>().FadeinInTime(5f);
                    GameController.Instance.player_total_money += GameController.Instance.tempMoney;
                    GameController.Instance.skillpoints_left += 12;
                    GameController.Instance.tempMoney = 0;
                }

                checkAllDied = false;
            }

            Destroy(GetComponent<Rigidbody2D>());
            RemoveCollidersRecursively();
            anim.SetInteger("State", 4);

            if ((transform.position - player.transform.position).magnitude > 30f)
            {
                Destroy(gameObject);
            }

            if ((transform.position - player.transform.position).magnitude > 30f)
            {
                Destroy(gameObject);
            }
        }

    }

    void SetAnimation()
    {

        if (isAttacked)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                anim.SetInteger("State", 5);                            // sok olsun
        }
        else
        {
            if (speed != 0)
            {
                anim.SetInteger("State", 1); // yuru
            }
            else if (speed == 0)
            {
                if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance &&
                    Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance * 1.25)
                {
                    if (time % 2 == 0)
                    {
                        if (Utility.GetRandomNumber(0, 2) == 0)
                            anim.SetInteger("State", 3);
                        else
                            anim.SetInteger("State", 2);

                        //rb.AddForce(speed * new Vector2(5f, 0f));
                    }
                    else
                    {
                        anim.SetInteger("State", 0);
                    }
                }
                else
                {
                    anim.SetInteger("State", 0);
                }

            }

        }

    }

    void MoveHorizontal(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        
        if (speed < 0)
        {
            if (dir == 1) // adama dogru yuruyosa
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                //anim.SetFloat("Speed", 1);
                anim.SetInteger("State", 6);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                
               // anim.SetFloat("Speed", -1.25f);
            }

        }
        else if (speed > 0)
        {
            if (dir == 1) // adama dogru yuruyosa
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                //anim.SetFloat("Speed", 1);
                anim.SetInteger("State", 6);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                
                // anim.SetFloat("Speed", -1.25f);
            }
        }


    }

    public void WhipSFX()
    {
        sas.RandomizeSfxWhip(whip0, whip1, whip2, whip3);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void SetDirection()
    {
        
        if (CanMove() == 0)
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else if (CanMove() == 2)
            speed = 0;
        else
        {
            speed = -Utility.EnemyDirection(player.transform, this.transform);
        }

        /*
        if (CanMove() == 0)
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else if (CanMove() == 1)
            speed = -Utility.EnemyDirection(player.transform, this.transform);
            */
    }

    int CanMove()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < 20 // adama dogru yurur
            && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance * 1.25f && time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack0") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            dir = -1;
            return 0;
        }

        else if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < distance// geri yurur
            &&
            time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack0") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            dir = 1;
            return 1;
        }
        else // durur
            return 2;

    }

    public void SetWeaponActive()
    {
        weapon.GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        weapon.GetComponent<Collider2D>().enabled = false;
    }

    void SetTimer()
    {
        timer += Time.deltaTime;
        time = (int)timer;
    }

    public void SetIsAttacked(bool b)
    {
        isAttacked = b;
    }

    public void SetIsDead(bool b)
    {
        died = b;
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    private void RemoveCollidersRecursively()
    {
        var allColliders = GetComponentsInChildren<Collider2D>();

        foreach (var childCollider in allColliders) Destroy(childCollider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cliff"))
        {
            GameController.Instance.totalEnemy--;
            StartCoroutine(CallDestroyEnemy(3f));
        }

        if (other.gameObject.CompareTag("Extinction"))
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x - 25f, transform.position.y, transform.position.z), Quaternion.identity);
        }

        if (other.gameObject.CompareTag("Extinction0"))
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x + 25f, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }

    IEnumerator CallDestroyEnemy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
