using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherController : MonoBehaviour { // archer spawn positiona gore hareket edemiyor bazen
    
    Animator anim;
    Rigidbody2D rb;

    //public AudioClip insarr0;
    public AudioClip throwArr0;
    public AudioClip throwArr1;
    public AudioClip throwArr2;

    public GameObject player;

    public GameObject moneyPocket;

    public GameObject characterDistance;

    public GameObject arrowPrefab;
    public GameObject arrowEmberedPrefab;
    public GameObject leftHand;

    public GameObject body;
    public GameObject head;

    public float StopDistance;

    public float speed;
    float speedBoost = 0;
    public float blowSpeed;

    float speedX;
    float speedY;

    float archerSpeed;
    float distance;

    bool isAttacked = false;
    int attackState;

    float timer;
    int time;

    bool died = false;

    bool movable = false;

    bool weaponChangable = true;
    bool isArcher = true;

    EnemySkill es;
    ArcherArmour aa;

    HitAudioSource has;
    SwingAudioSource sas;

    public AudioClip walking0;

    public AudioClip fall0;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    public GameObject archer;

    bool moneyInstantiated;

    int ember;

    bool checkAllDied = true;

    void Start () {
        
        sas = GetComponent<SwingAudioSource>();
        has = GetComponent<HitAudioSource>();

        aa = GetComponent<ArcherArmour>();
        es = GetComponent<EnemySkill>();

        SetEmberLogic();

        StartCoroutine(SetSpeedBoost());

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(SetAttackDistance());
        StartCoroutine(SetPersonSize());

        speedX = speed;
        speedY = 0.57f * speed;
        
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    void HeadAnimations()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            anim.SetInteger("HeadState", 0);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
        {
            anim.SetInteger("HeadState", 2);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SwitchWeapon") || anim.GetCurrentAnimatorStateInfo(0).IsName("Throwing"))
        {
            anim.SetInteger("HeadState", 1);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
        {
            anim.SetInteger("HeadState", 4);
        }


    }

    void FixedUpdate () {

        if (GameController.Instance.playerDead)
        {
            anim.SetInteger("HeadState", 0);
            anim.SetInteger("State", 0);

            if (rb != null)
                StopMoving();
        }
        else

        if (!died)
        {
            SetTimer();
            SetAnimation();
            SetDirection();
            HeadAnimations();

            if (!isArcher && movable)
            {
                archerSpeed *= speedBoost;
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                    rb.velocity = new Vector2(Utility.EnemyDirection(this.transform, player.transform) * blowSpeed, rb.velocity.y);
                else if (archerSpeed != 0)
                    MoveHorizontal(archerSpeed);
                else
                    StopMoving();
            }

            if ((anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
                && Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance - 2f)
            {
                this.rb.velocity = new Vector2(Utility.EnemyDirection(player.transform, this.transform) * 2f, rb.velocity.y);
            }
            
        }
        else
        {
            if (checkAllDied)
            {
                GameController.Instance.totalEnemy--;

                if (Utility.CheckAllDied())
                {
                    if (!GameController.Instance.gameFinished)
                        if (GameController.Instance.current_level == GameController.Instance.player_level && !GameController.Instance.isInCastle)
                        {
                            GameController.Instance.levelPassed = true;
                            GameController.Instance.player_level++;
                        }

                    if (!GameController.Instance.isInCastle)
                        player.transform.parent.GetComponent<PlayerMobileCtrl>().FadeinInTime(5f);

                    GameController.Instance.player_total_money += GameController.Instance.tempMoney;
                    GameController.Instance.tempMoney = 0;
                }

                checkAllDied = false;
            }

            Destroy(GetComponent<Rigidbody2D>());
            anim.SetInteger("State", 10);
            anim.SetInteger("HeadState", 3);
            RemoveCollidersRecursively();

            if (!moneyInstantiated)
                StartCoroutine(MoneyPocket());

            if ((transform.position - player.transform.position).magnitude > 30f)
            {
                Destroy(gameObject);
            }
        }


    }

    IEnumerator MoneyPocket()
    {
        moneyInstantiated = true;
        yield return new WaitForSeconds(0.5f);
        GameObject money = Instantiate(moneyPocket) as GameObject;
        money.transform.parent = this.gameObject.transform;
        money.transform.localPosition = new Vector2(-13f, -7f);
        money.transform.parent = null;
    }

    IEnumerator SetSpeedBoost()
    {
        yield return new WaitForSeconds(0.001f);
        speedBoost = es.GetEnemyAgility() / 10f + 4f;
    }

    public void FallSoundFX()
    {
        this.GetComponent<MovementAudioSource>().PlaySingle(fall0);
    }

    IEnumerator SetPersonSize()
    {
        yield return new WaitForSeconds(0.001f);

        for (int i = 0; i < es.GetEnemyPower(); i++)
            this.transform.localScale += new Vector3(0.0005f, 0.0005f, 0f);
    }

    IEnumerator SetAttackDistance()
    {
        yield return new WaitForSeconds(0.001f);
        
        distance = 5f + ((float)aa.GetSword() / 12) ;
    }
    /*
    public void InsertArrow()
    {
        sas.PlaySingle(insarr0);
    }*/

    void SetAnimation()
    {
        if(Utility.AtSameYPosition(player.transform, transform))
        {
            if (isArcher)
            {
                if (Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) > StopDistance)
                {
                    anim.SetInteger("State", 0);
                }

                else if (Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) < StopDistance &&
                    Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) > 10f)
                {
                    anim.SetInteger("State", 1);
                    rb.velocity = new Vector2(0f, 0f);
                }
                else
                {
                    if (weaponChangable)
                    {
                        weaponChangable = false;
                        anim.SetInteger("State", 2);
                        StartCoroutine(SetIsArcher(false));
                        StartCoroutine(CallSetMovable());
                        StartCoroutine(ChangeWeapon());
                    }
                }

            }
            else// if(movable)
            {
                if (isAttacked)
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
                        anim.SetInteger("State", 5);
                }
                else
                {
                    if (archerSpeed != 0)
                    {
                        anim.SetInteger("State", 6);
                    }
                    else if (archerSpeed == 0)
                    {
                        if (Mathf.Abs(Utility.GetDistanceBetween(transform, player.transform)) < distance)
                        {
                            if (time % 2 == 0)
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
                        else
                        {
                            anim.SetInteger("State", 0);
                        }

                    }

                }
            }
        }
        else
        {
            anim.SetInteger("State", 0);
        }

        
        


    }

    IEnumerator SetIsArcher(bool b)
    {
        yield return new WaitForSeconds(0.3f);
        isArcher = b;
    }

    public void SetWeaponActive()
    {
        aa.GetWeapon().GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        aa.GetWeapon().GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator CallSetMovable()
    {
        yield return new WaitForSeconds(0.16f);
        StartCoroutine(SetMovable());
    }
    IEnumerator ChangeWeapon()
    {
        yield return new WaitForSeconds(0.32f);
        aa.ChangeArcherWeapon();
    }

    public IEnumerator SetMovable() // animation da cagriliyo, silah degistirdikten sonra hareket etsin diye
    {
        yield return new WaitForSeconds(0.5f);
        movable = true;
    }

    void MoveHorizontal(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (speed < 0)
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        else if (speed > 0)
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
    }

    void SetDirection()
    {
        if(Utility.EnemyDirection(player.transform, this.transform) == 1)
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        else
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);

        if (CanMove() && Utility.AtSameYPosition(player.transform, transform))
            archerSpeed = Utility.EnemyDirection(player.transform, this.transform);
        else
            archerSpeed = 0;
    }

    bool CanMove()
    {
        if (Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) < StopDistance &&
            Mathf.Abs(Utility.GetDistanceBetween(this.transform, player.transform)) > distance && time % 2 == 0 &&
            !(anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack")))
            return true;
        else
            return false;
    }

    public void WhipSFX()
    {
        has.RandomizeSfxWhip(whip0, whip1, whip2, whip3);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
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

    void SetEmberLogic()
    {
        if (aa.GetBow() > 3)
            if (Utility.GetRandomNumber(0, 2) == 0)
                ember = 1;

    }

    public void ThrowArrow()
    {
        if (ember == 0)
        {
            GameObject arrow = Instantiate(arrowPrefab, leftHand.transform.position, Quaternion.Euler(0, archer.transform.eulerAngles.y, 118)) as GameObject;
            arrow.GetComponent<ArrowCtrl>().SetDamageAmount(Utility.GetArrowDamageAmount(aa.GetBow(), ember));
            Rigidbody2D rb2d = arrow.GetComponent<Rigidbody2D>();

            if (this.transform.eulerAngles.y == 180)
                rb2d.velocity = new Vector2(-speedX, speedY);
            else
                rb2d.velocity = new Vector2(speedX, speedY);

            has.RandomizeSfx(throwArr0, throwArr1, throwArr2);
        }
        else
        {
            GameObject arrow = Instantiate(arrowEmberedPrefab, leftHand.transform.position, Quaternion.Euler(0, archer.transform.eulerAngles.y, 118)) as GameObject;
            arrow.GetComponent<ArrowCtrl>().SetDamageAmount(Utility.GetArrowDamageAmount(aa.GetBow(), ember));
            Rigidbody2D rb2d = arrow.GetComponent<Rigidbody2D>();

            if (this.transform.eulerAngles.y == 180)
                rb2d.velocity = new Vector2(-speedX, speedY);
            else
                rb2d.velocity = new Vector2(speedX, speedY);

            has.RandomizeSfx(throwArr0, throwArr1, throwArr2);
        }
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
        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().OpenDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().CloseDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
        }
    }

    IEnumerator CallDestroyEnemy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

}
