using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMobileCtrl : MonoBehaviour {
    
    /// <summary>
    /// Burada transform ile nasil buttonlara erisebilecegimizi gosteriyor
    /// </summary>

    Player playerCtrl;

    public GameObject popupMsg;

    public Button leftBtn;
    public Button rigfhtBtn;
    public Button jumpBtn;
    public Button attackBtn;
    public Button defenseBtn;
    Button optionsBtn;
    Button quitBtn;

    public GameObject mobileUI;

    public Button[] allButtons;
    Vector3[] startScales;

    Color pressedBtnColor = new Color(1f, 0.557f, 0f, 0.53f);
    Color releasedBtnColor = new Color(1f, 1f, 1f, 0.53f);

    bool showBloodChecked = true;
    bool soundsChecked = true;
    bool musicChecked = true;

    void Start()
    {
        try
        {
            quitBtn = mobileUI.transform.GetChild(0).GetChild(10).GetComponent<Button>(); // butona erismek
            optionsBtn = mobileUI.transform.GetChild(0).GetChild(9).GetComponent<Button>();
        }
        catch
        {
            Debug.Log("find me");
        }


        playerCtrl = GetComponent<Player>();
        InitStartScales();
    }

    void InitStartScales()
    {
        startScales = new Vector3[allButtons.Length];

        for (int i = 0; i < allButtons.Length; i++)
        {
            try
            {
                startScales[i] = allButtons[i].transform.localScale;
            }
            catch
            {
                Debug.Log("find me");
            }
            
        }
    }

    public void ShowInterface()
    {
        if (quitBtn.interactable)
        {
            popupMsg.gameObject.SetActive(true);
            leftBtn.interactable = false;
            rigfhtBtn.interactable = false;
            jumpBtn.interactable = false;
            playerCtrl.SetJmpBtnInteractable(false);
            attackBtn.interactable = false;
            defenseBtn.interactable = false;
            optionsBtn.interactable = false;
        }
        
    }

    public void OptionsQuit()
    {
        mobileUI.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);
        leftBtn.interactable = true;
        rigfhtBtn.interactable = true;
        jumpBtn.interactable = true;
        playerCtrl.SetJmpBtnInteractable(true);
        attackBtn.interactable = true;
        defenseBtn.interactable = true;
        quitBtn.interactable = true;
    }

    public void ShowBlood()
    {

    }

    public void Sounds()
    {

    }

    public void Music()
    {

    }

    public void YesBtn()
    {
        LevelMusicController.lmc.StopPlayingMusic();
        GameController.Instance.tempMoney = 0;
        mobileUI.GetComponent<Animator>().SetInteger("State", 1);
        StartCoroutine(CallOpenLevellately());
    }

    public void NoBtn()
    {
        popupMsg.gameObject.SetActive(false);
        leftBtn.interactable = true;
        rigfhtBtn.interactable = true;
        jumpBtn.interactable = true;
        playerCtrl.SetJmpBtnInteractable(true);
        attackBtn.interactable = true;
        defenseBtn.interactable = true;
        optionsBtn.interactable = true;
    }
    
    IEnumerator CallOpenLevellately()
    {
        SaveSystem.SavePlayer();
        yield return new WaitForSeconds(0.5f);
        GameController.Instance.isInCastle = false;
        AdManager.Instance.ShowInterstitial();
        SceneManager.LoadScene("Interface");
    }

    public void FadeinInTime(float time)
    {
        StartCoroutine(FadeInWhenPlayerDeadLately(time));
    }

    public void FadeinInTimeToBoss(float time)
    {
        StartCoroutine(FadeInWhenPlayerDeadLately1(time));
    }

    IEnumerator FadeInWhenPlayerDeadLately(float time)
    {
        yield return new WaitForSeconds(time);
        LevelMusicController.lmc.StopPlayingMusic();
        mobileUI.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        SaveSystem.SavePlayer();
        GameController.Instance.isInCastle = false;
        AdManager.Instance.ShowInterstitial();
        SceneManager.LoadScene("Interface");
    }

    IEnumerator FadeInWhenPlayerDeadLately1(float time)
    {
        yield return new WaitForSeconds(time);
        LevelMusicController.lmc.StopPlayingMusic();
        mobileUI.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        AdManager.Instance.ShowInterstitial();
        SceneManager.LoadScene("BossRoom");
    }
    
    public void ShowOptions()
    {
        if (optionsBtn.interactable)
        {
            mobileUI.transform.GetChild(0).GetChild(12).gameObject.SetActive(true);
            leftBtn.interactable = false;
            rigfhtBtn.interactable = false;
            jumpBtn.interactable = false;
            playerCtrl.SetJmpBtnInteractable(false);
            attackBtn.interactable = false;
            defenseBtn.interactable = false;
            quitBtn.interactable = false;
        }
            
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MobileLeftClicked();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MobileRigthClicked();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MobileJumpClicked();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            MobileAttackClicked();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            MobileLeftUnClicked();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            MobileRigthUnClicked();
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            MobileAttackUnClicked();
        }
    }
    
    public void SetUIUnInteractable()
    {
        foreach (Button bt in allButtons)
            bt.interactable = false;
    }
   
    public void MobileRigthClicked()
    {
        if (rigfhtBtn.interactable)
        {
            playerCtrl.Mobile_RightClicked();
            rigfhtBtn.image.color = pressedBtnColor;
        }
    }

    public void MobileRigthUnClicked()
    {
        playerCtrl.Mobile_RightUnClicked();
        rigfhtBtn.image.color = releasedBtnColor;
    }

    public void MobileLeftClicked()
    {
        if (leftBtn.interactable)
        {
            playerCtrl.Mobile_LeftClicked();
            leftBtn.image.color = pressedBtnColor;
        }
    }

    public void MobileLeftUnClicked()//
    {
        if (!GameController.Instance.playerDead)
        {
            playerCtrl.Mobile_LeftUnClicked();
            leftBtn.image.color = releasedBtnColor;
        }
        
    }

    public void MobileAttackClicked()
    {
        if (attackBtn.interactable)
        {
            playerCtrl.Mobile_AttackClicked();
            attackBtn.image.color = pressedBtnColor;
            defenseBtn.interactable = false;
        }
    }

    public void MobileAttackUnClicked()//
    {
        if (!GameController.Instance.playerDead)
        {
            playerCtrl.Mobile_AttackUnClicked();
            attackBtn.image.color = releasedBtnColor;
            defenseBtn.interactable = true;
        }

        
    }

    public void MobileJumpClicked()
    {
        if (jumpBtn.interactable)
        {
            playerCtrl.Mobile_JumpClicked();
            jumpBtn.image.color = pressedBtnColor;
        }
    }

    public void MobileJumpUnClicked()//
    {
        if (!GameController.Instance.playerDead)
        {
            jumpBtn.image.color = releasedBtnColor;

        }
    }

    public void MobileDefenseClicked()
    {
        if (defenseBtn.interactable)
        {
            StartCoroutine(playerCtrl.Mobile_DefenseClicked());
            defenseBtn.image.color = pressedBtnColor;
            attackBtn.interactable = false;
        }
    }

    public void MobileDefenseUnClicked()//
    {
        if (!GameController.Instance.playerDead)
        {
            playerCtrl.Mobile_DefenseUnClicked();
            defenseBtn.image.color = releasedBtnColor;
            attackBtn.interactable = true;
        }
        
    }

    public void IncreaseSizeOfButton(int index)
    {
        if (allButtons[index].interactable)
            allButtons[index].transform.localScale = startScales[index] * 1.25f;
    }

    public void DecreaseSizeOfButton(int index)
    {
        allButtons[index].transform.localScale = startScales[index];
    }

}
