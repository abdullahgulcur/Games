using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedBossCtrl : MonoBehaviour {

    public GameObject player;
    public GameObject weapon;
    public GameObject cloak;
    public GameObject characterDistance;

    Animator anim;
    Rigidbody2D rb;

    public float blowSpeed = 3f;
    public float stopDistance = 18f;
    public float attackDistance = 8f;
    public float defenseAttackDistance = 4.5f;
    public float speedMultiplier;

    bool isAttacked;
    bool canAttack;
    bool died;

    int speed;
    int attackState;

    float timer;
    int time;

    bool canAttack2;

    bool foundPlayer = false;

    bool checkAllDied = true;
    bool isAttacking;

    void Start()
    {

        died = false;
        isAttacked = false;
        canAttack = false;
        canAttack2 = true;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        

        if (Utility.AtSameYPosition(player.transform, transform))
        {
            if (GameController.Instance.playerDead)
            {
                anim.SetInteger("State", 0);

                if (rb != null)
                    StopMoving();
            }
            else if (!died)
            {
                SetIsAttacking();
                SetTimer();
                SetAnimation();
                SetDirection();
                SetAnimSpeed();

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                    rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
                else if (speed != 0)
                    MoveHorizontal(speed);
                else
                    StopMoving();

                if (isAttacking && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > defenseAttackDistance - 0.25f)
                {
                    this.rb.velocity = new Vector2(Utility.EnemyDirection(player.transform, this.transform) * 2f, rb.velocity.y);
                }

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

                Destroy(cloak);
                RemoveCollidersRecursively();
                Destroy(GetComponent<Rigidbody2D>());
                anim.SetInteger("State", 5);
            }
        }
        else
        {
            anim.SetInteger("State", 0);
        }




    }

    void SetAnimSpeed()
    {
        if (isAttacking)
            anim.speed = Utility.GetRandomNumber(10, 15) / 10f;
        else
            anim.speed = 1;
    }

    void SetIsAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_0"))
            isAttacking = true;
        else
            isAttacking = false;
    }


    void SetDirection()
    {
        if (CanMove() && Utility.AtSameYPosition(player.transform, transform))
            speed = Utility.EnemyDirection(player.transform, this.transform);
        else
            speed = 0;
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void MoveHorizontal(float speed)
    {
        rb.velocity = new Vector2(speed * speedMultiplier, rb.velocity.y);

        if (speed < 0)
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        else if (speed > 0)
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
    }

    bool CanMove()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < stopDistance &&
            defenseAttackDistance < Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_0") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1")))
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    void SetAnimation()
    {

        if (isAttacked)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                anim.SetInteger("State", 2);
        }
        else
        {
            if (speed != 0)
            {
                anim.SetInteger("State", 1);
            }
            else if (speed == 0)
            {
                if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < defenseAttackDistance)
                {
                    if (Utility.GetRandomNumber(0, 2) == 0)
                        anim.SetInteger("State", 3);
                    else
                        anim.SetInteger("State", 4);
                }
                else
                {
                    anim.SetInteger("State", 0);
                }

            }

        }

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

    public void SetWeaponActive()
    {
        weapon.GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        weapon.GetComponent<Collider2D>().enabled = false;
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
}
