using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject shadow;

    public GameObject body;
    public GameObject[] shields;

    public AudioClip coinTake;
    public AudioClip doorClosed;

    public AudioClip whip0;
    public AudioClip whip1;
    public AudioClip whip2;
    public AudioClip whip3;

    public AudioClip jump0;
    public AudioClip fall0;

    public GameObject characterDistance;

    Rigidbody2D rb;
    Animator anim;

    int attackState;
    bool pressedInside;
    public bool isJumping = false;
    int backDirection;

    public Button jmpBtn;
    public Button dfsBtn;
    public Button attackBtn;

    bool isAttacked = false;
    bool isAttacking = false;
    bool canRotate;

    private bool pressed_A;
    bool pressed_D;
    bool pressed_Attack;
    bool pressed_Jump;

    bool died = false;

    bool clickedSomething;

    private float playerS;
    public bool isGrounded = true;
    public float jumpSpeed;

    private float playerSpeed;
    public float blowSpeed;

    public float startAgility = 4f;
    public float gravityScale = 1.9f;

    bool flyShocked = false;
    bool jumpPrepration = false;

    PlayerState ps;
    Elevator elevator;

    bool pressed_Defense = false;

    bool touchBoat = false;

    bool doorClosedOnlyOnce = true;

    //bool whipped;

    MovementAudioSource mas;
    HitAudioSource has;
    SwingAudioSource sas;

    int groundState; // 1: ground 2: stairs

    private bool jmpBtnInteractable = true;

    bool canJumpAgain = false;
    bool mainDoorOpened = false;

    bool diedOnce = false;

    void Start()
    {
        sas = GetComponent<SwingAudioSource>();
        has = GetComponent<HitAudioSource>();
        mas = GetComponent<MovementAudioSource>();
        ps = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (GameController.Instance.player_shield != -1)
            shields[GameController.Instance.player_shield].GetComponent<Collider2D>().enabled = false;
        else
        {
            try
            {
                dfsBtn.interactable = false;

            }
            catch
            {

            }

        }

        playerSpeed = startAgility + GameController.Instance.player_agility / 10f;

        // StartCoroutine(SetPersonSize());

        SetPlayerSpeed(0);

    }

    void HeadAnimations()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.SetInteger("HeadState", 0);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FlyShock") || anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
        {
            anim.SetInteger("HeadState", 2);
        }

        if (isAttacking)
        {
            anim.SetInteger("HeadState", 4);
        }


    }

    void FixedUpdate()
    {



        if (!died)
        {
            if (flyShocked)
                rb.gravityScale = 3;
            else
                rb.gravityScale = gravityScale;

            SetIsAttacking();
            SetAttackState();
            HeadAnimations();
            SetCanMove();
            rb.velocity = new Vector2(playerS, rb.velocity.y);

            if (isAttacking || isJumping)
                jmpBtn.interactable = false;
            else if (jmpBtnInteractable)
            {
                try
                {
                    jmpBtn.interactable = true;
                }
                catch
                {
                    Debug.Log("find me");

                }
            }

            if (isGrounded)
            {
                shadow.gameObject.SetActive(true);
            }
            else
            {
                shadow.gameObject.SetActive(false);
                if (!isJumping)
                {
                    anim.SetInteger("State", 5);
                }
            }


            Translations();

            if (!isJumping)
            {
                if (isAttacked)
                {
                    if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyShock") || anim.GetCurrentAnimatorStateInfo(0).IsName("Shock")))
                    {
                        anim.SetInteger("State", 7);
                    }

                }
                else if (!pressed_A && !pressed_D && !pressed_Attack && !pressed_Jump)
                {
                    anim.SetInteger("State", 0);
                    if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyShock") ||
                        anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                        anim.GetCurrentAnimatorStateInfo(0).IsName("Shock")))
                    {

                        SetPlayerSpeed(0);
                    }
                }
            }

            if (isJumping)
            {
                if (isAttacked)
                {
                    anim.SetInteger("State", 9);
                    SetPlayerSpeed(blowSpeed * backDirection);
                }

            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("FlyShock"))
                flyShocked = true;
            else
                flyShocked = false;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shock"))
            {
                SetPlayerSpeed(blowSpeed * backDirection);
            }

        }
        else
        {
            if (!diedOnce)
            {
                GameController.Instance.playerDead = true;
                GetComponent<PlayerMobileCtrl>().SetUIUnInteractable();
                GameController.Instance.tempMoney = 0;
                GetComponent<PlayerMobileCtrl>().FadeinInTime(3f);
                Destroy(GetComponent<Rigidbody2D>());
                anim.SetLayerWeight(anim.GetLayerIndex("Layer1"), 0f);
                anim.SetInteger("HeadState", 3);
                anim.SetInteger("State", 10);
                jmpBtn.interactable = false;
                diedOnce = true;
            }
            
        }
        
    }

    void Translations()
    {

        if (pressed_D)
        {
            if (!isJumping)
            {
                if (!isAttacking)
                {
                    anim.SetInteger("State", 1);
                    SetPlayerSpeed(playerSpeed);
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                }
            }
            else if (!flyShocked && !anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                SetPlayerSpeed(playerSpeed);
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") && isGrounded)
            {
                anim.SetInteger("State", 1);
            }


        }

        if (pressed_A)
        {
            if (!isJumping)
            {
                if (!isAttacking)
                {
                    anim.SetInteger("State", 1);
                    transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                    SetPlayerSpeed(-playerSpeed);
                }
            }
            else if (!flyShocked && !anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                SetPlayerSpeed(-playerSpeed);
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") && isGrounded)
            {
                anim.SetInteger("State", 1);
            }

        }

        if (pressed_Attack)
        {
            if (!isJumping)
            {
                if (pressed_A)
                {
                    anim.SetInteger("State", 2);
                    SetPlayerSpeed(-playerSpeed);
                }
                else if (pressed_D)
                {
                    anim.SetInteger("State", 2);
                    SetPlayerSpeed(playerSpeed);
                }
                else
                {
                    anim.SetInteger("State", attackState);
                }

            }
            else
                anim.SetInteger("State", 6);
        }

    }

    public void Mobile_RightClicked()
    {
        clickedSomething = true;
        pressed_D = true;
    }

    public void Mobile_LeftClicked()
    {
        clickedSomething = true;
        pressed_A = true;
    }

    public void Mobile_RightUnClicked()
    {
        clickedSomething = false;
        pressed_D = false;

        if (!isJumping)
        {
            anim.SetInteger("State", 0);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack"))
                SetPlayerSpeed(0);
        }
        else if (!flyShocked)
            SetPlayerSpeed(0);

    }

    public void Mobile_LeftUnClicked()
    {
        clickedSomething = false;
        pressed_A = false;

        if (!isJumping)
        {
            anim.SetInteger("State", 0);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack"))
                SetPlayerSpeed(0);

        }
        else if (!flyShocked)
            SetPlayerSpeed(0);

    }


    public void Mobile_AttackClicked()
    {
        clickedSomething = true;
        pressed_Attack = true;

    }

    public void Mobile_AttackUnClicked()
    {
        pressed_Attack = false;

        clickedSomething = false;

        if (!isJumping)
        {
            if (pressed_A)
            {
                clickedSomething = true;
                anim.SetInteger("State", 1);
            }
            else if (pressed_D)
            {
                clickedSomething = true;
                anim.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
            }

        }

    }

    public IEnumerator Mobile_DefenseClicked()
    {
        if (dfsBtn.IsInteractable())
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Layer1"), 1f);
            anim.SetTrigger("Defense");

            yield return new WaitForSeconds(0.14f);

            if (GameController.Instance.player_shield != -1)
                shields[GameController.Instance.player_shield].GetComponent<Collider2D>().enabled = true;
        }

    }

    public void Mobile_DefenseUnClicked()
    {
        if (GameController.Instance.player_shield != -1)
            shields[GameController.Instance.player_shield].GetComponent<Collider2D>().enabled = false;

        anim.SetLayerWeight(anim.GetLayerIndex("Layer0"), 1f);
        anim.SetTrigger("DefenseReverse");
    }

    public void Mobile_JumpClicked()
    {

        if (jmpBtn.IsInteractable())
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shock") && !isAttacking)
            {
                StartCoroutine(Jump1());
                isJumping = true;
            }

        }

    }

    public void SetWeaponActive()
    {
        ps.GetWeapon().GetComponent<Collider2D>().enabled = true;
    }

    public void SetWeaponInactive()
    {
        ps.GetWeapon().GetComponent<Collider2D>().enabled = false;
    }


    public void WhipSFX()
    {
        sas.RandomizeSfxWhip(whip0, whip1, whip2, whip3);
    }

    IEnumerator Walk()
    {

        yield return new WaitForSeconds(0.4f);
        SetPlayerSpeed(0);
        yield return null;
    }

    IEnumerator SetPersonSize()
    {
        yield return new WaitForSeconds(0.001f);

        for (int i = 0; i < GameController.Instance.player_power; i++)
            this.transform.localScale += new Vector3(0.001f, 0.001f, 0f);
    }

    IEnumerator Jump1()
    {

        if (isGrounded && canJumpAgain)
        {

            anim.SetInteger("State", 5);
            jumpPrepration = true;
            yield return new WaitForSeconds(0.2f);
            jumpPrepration = false;
            mas.PlaySingle(jump0);
            isGrounded = false;

            if (groundState == 1)
            {
                rb.AddForce(new Vector2(0, jumpSpeed));
                canJumpAgain = false;
            }
                
            else if (groundState == 2)
            {
                rb.AddForce(new Vector2(0, jumpSpeed * 1.5f));// 
                canJumpAgain = false;
            }
                

        }
    }

    void SetIsAttacking()
    {
        isAttacking = (anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"));
    }

    void SetPlayerSpeed(float x)
    {
        if (!touchBoat)
            playerS = x;
        else
            playerS = x + 10;
    }

    public void SetIsDead(bool b)
    {
        died = b;
    }

    void SetAttackState()
    {
        attackState = (int)(Random.Range(3, 5)); // 3 veya 4 uretir
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ground") && !jumpPrepration && other.enabled)//rb.velocity.y <= 1f)
        {
            if (!died)
            {
                groundState = 1;

                if (jmpBtnInteractable)
                    jmpBtn.interactable = true;

                isJumping = false;
                isGrounded = true;
                canJumpAgain = true;

                if (pressed_A)
                {

                    transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                    SetPlayerSpeed(-playerSpeed);
                    anim.SetInteger("State", 1);

                }
                if (pressed_D)
                {
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                    SetPlayerSpeed(playerSpeed);
                    anim.SetInteger("State", 1);
                }

                if (!clickedSomething)
                    anim.SetInteger("State", 0);
            }
        }

        if (other.gameObject.CompareTag("Stairs") && !jumpPrepration && other.enabled)//rb.velocity.y <= 1f)
        {
            groundState = 2;

            if (jmpBtnInteractable)
                jmpBtn.interactable = true;

            isJumping = false;
            isGrounded = true;
            canJumpAgain = true;

            if (pressed_A)
            {
                anim.SetInteger("State", 1);
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                SetPlayerSpeed(-playerSpeed);
            }
            if (pressed_D)
            {
                anim.SetInteger("State", 1);
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                SetPlayerSpeed(playerSpeed);
            }

            if (!clickedSomething)
                anim.SetInteger("State", 0);
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Ceiling"))
        {
            other.transform.parent.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        }

        if (other.gameObject.CompareTag("Cliff"))
        {
            GetComponent<PlayerMobileCtrl>().FadeinInTime(0f);
        }

        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().OpenDoor();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            GameController.Instance.foundKey = true;
            GetComponent<FoleyAudioSource>().CoinTake(coinTake);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Keys"))
        {
            GetComponent<FoleyAudioSource>().CoinTake(coinTake);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
            else
            {
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().ShowText();

                if (doorClosedOnlyOnce)
                {
                    //GetComponent<FoleyAudioSource>().DoorClosed(doorClosed);
                    doorClosedOnlyOnce = false;
                }

            }
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().OpenDoor();
            else
            {
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().ShowText();

                if (doorClosedOnlyOnce)
                {
                    //GetComponent<FoleyAudioSource>().DoorClosed(doorClosed);
                    doorClosedOnlyOnce = false;
                }

            }
        }

        if (other.gameObject.CompareTag("MainDoor"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
            {
                if (!mainDoorOpened)
                {
                    other.transform.GetChild(0).GetComponent<MainDoor>().OpenDoor();
                    mainDoorOpened = true;
                    

                    if(!GameController.Instance.gameFinished)
                        GetComponent<PlayerMobileCtrl>().FadeinInTimeToBoss(5f); // go to boss
                    else
                        GetComponent<PlayerMobileCtrl>().FadeinInTime(5f); // returns interface
                }
                
            }
                
            else
            {
                other.transform.GetChild(0).GetComponent<MainDoor>().ShowText();

                if (doorClosedOnlyOnce)
                {
                    //GetComponent<FoleyAudioSource>().DoorClosed(doorClosed);
                    doorClosedOnlyOnce = false;
                }

            }
        }

        if (other.gameObject.CompareTag("Elevator"))
        {
            Elevator elevator = other.gameObject.transform.parent.GetComponent<Elevator>();
            elevator.Set_Y_Axis(1);
        }

        if (other.gameObject.CompareTag("Coffin"))
        {
            other.transform.GetChild(1).gameObject.SetActive(false);
            other.GetComponent<Coffin>().BurstCover();
            other.transform.GetChild(3).GetComponent<CursedController>().SetPlayer(gameObject);
        }

        if (other.gameObject.CompareTag("Gallows"))
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("Boat"))
        {
            touchBoat = true;
        }

        if (other.gameObject.CompareTag("MoneyPocket"))
        {
            GetComponent<FoleyAudioSource>().CoinTake(coinTake);

            if (GameController.Instance.current_level == GameController.Instance.player_level)
                GameController.Instance.tempMoney += Utility.CalculatePocketMoneyAmount(GameController.Instance.player_level);
            else if(GameController.Instance.current_level <= GameController.Instance.player_level)
                GameController.Instance.tempMoney += Utility.CalculatePocketMoneyAmount(GameController.Instance.player_level) / 4;

            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Gold"))
        {
            GetComponent<FoleyAudioSource>().CoinTake(coinTake);

            if (GameController.Instance.current_level == GameController.Instance.player_level)
                GameController.Instance.tempMoney += Utility.CalculateTreasureAmount(GameController.Instance.player_level);
            else if (GameController.Instance.current_level <= GameController.Instance.player_level)
                GameController.Instance.tempMoney += Utility.CalculateTreasureAmount(GameController.Instance.player_level) / 4;

            Destroy(other.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ceiling"))
        {
            other.transform.parent.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        }

        if (other.gameObject.CompareTag("Door"))
        {
            other.transform.GetChild(0).GetComponent<CastleInnerDoor>().CloseDoor();
        }

        if (other.gameObject.CompareTag("LockedDoor"))
        {
            if (GameController.Instance.foundKey)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
            else
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().NotToShowText();

            doorClosedOnlyOnce = true;
        }

        if (other.gameObject.CompareTag("LockedDoor1"))
        {
            if (!other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().CloseDoor();
            else
                other.transform.GetChild(0).GetComponent<CastleLockedDoor>().NotToShowText();

            doorClosedOnlyOnce = true;
        }

        if (other.gameObject.CompareTag("MainDoor"))
        {
            if (other.gameObject.transform.GetChild(1).gameObject.active)
                other.transform.GetChild(0).GetComponent<MainDoor>().NotToShowText();

            doorClosedOnlyOnce = true;
        }

        if (other.gameObject.CompareTag("Elevator"))
        {
            Elevator elevator = other.gameObject.transform.parent.GetComponent<Elevator>();
            elevator.Set_Y_Axis(-1);
        }
        if (other.gameObject.CompareTag("Boat"))
        {
            touchBoat = false;
        }

    }

    public void SetJmpBtnInteractable(bool b)
    {
        jmpBtnInteractable = b;
    }

    public void FallSoundFX()
    {
        mas.PlaySingle(fall0);
    }

    public void SetIsAttacked(bool b)
    {
        isAttacked = b;
    }

    public void BackDirection(int x)
    {
        backDirection = x;
    }

    void SetCanMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") || // bunlari yaparken hareket edemesin
            anim.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            SetPlayerSpeed(0);
    }

    public Animator GetAnim()
    {
        return anim;
    }

}
