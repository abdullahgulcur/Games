using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public AudioClip emptyPage;
    public AudioClip selectEffect;

    int selectedLevelIndex;

    public Button[] lvlBtn;

    public Button playBtn;

    public InterfaceSoundManager ism;

    public GameObject fadeSprite;

    public Text levelTxt;

    int clickedSkipCount = 0;

    public GameObject hints;
    public GameObject interfaceSound;

    //public GameObject adManager;

    string currentState = "skip";

    void Start()
    {
        if (!GameController.Instance.firstGameOpen)
            hints.SetActive(false);

        //SetLevelBtnColor();
    }

    public void SetHintsInactive()
    {
        if (!GameController.Instance.firstGameOpen)
            hints.SetActive(false);
    }

    public void SetLevelBtnColor()
    {
        int playerLevel = GameController.Instance.player_level;

        for (int i = 0; i < lvlBtn.Length; i++)
        {
            if (i < playerLevel - 1) // gectigi levelse
            {
                lvlBtn[i].transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                lvlBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                lvlBtn[i].interactable = true;
            }
            else if (i == playerLevel - 1) // geldigi levelse
            {
                lvlBtn[i].transform.localScale = new Vector3(0.40f, 0.40f, 1f);
                lvlBtn[i].GetComponent<Image>().color = new Color(0.76f, 0.16f, 0.16f, 1f);
                lvlBtn[i].interactable = true;
                AddFade(lvlBtn[i].gameObject);
            }
            else // daha ulasmamissa
            {
                lvlBtn[i].transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                lvlBtn[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                lvlBtn[i].interactable = false;
            }
                
        }
    }

    void AddFade(GameObject gameObj)
    {
        StartCoroutine(FadeInOut(1f, -1, gameObj));
    }

    IEnumerator FadeInOut(float val, int dir, GameObject obj)
    {
        yield return new WaitForSeconds(0.1f / 15);
        
        if (val < 0)
            dir *= -1;
        else if (val > 1)
            dir *= -1;

        if (dir == -1)
        {
            Color color = new Color(1, val - 0.01f, val - 0.01f);
            obj.GetComponent<Image>().color = color;
            StartCoroutine(FadeInOut(val - 0.01f, dir, obj));
        }
        else if (dir == 1)
        {
            Color color = new Color(1, val + 0.01f, val + 0.01f);
            obj.GetComponent<Image>().color = color;
            StartCoroutine(FadeInOut(val + 0.01f, dir, obj));
        }
    }

    void SetLevelText(int x)
    {
        levelTxt.text = "Level " + (x + 1).ToString();
    }

    public void Skip()
    {
        if (clickedSkipCount == 0)
        {
            currentState = "level";
            hints.transform.GetChild(0).gameObject.SetActive(false);
            hints.transform.GetChild(1).gameObject.SetActive(true);
            hints.transform.GetChild(10).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(4).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(3).GetComponent<Text>().text = "Your starting point. There are different 50 levels with different difficulties. To start, you can click sword image.";
            hints.transform.localPosition = new Vector3(-188, 93, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }
    }

    public void TickEmpty()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        levelTxt.gameObject.SetActive(false);
        SetLevelBtnColor();
        playBtn.image.color = new Color(1f, 1f, 1f, 0.33f);
        selectedLevelIndex = -1;
    }

    public void SetLevel(int x)
    {
        if (GameController.Instance.firstGameOpen)
        {
            if (!(x == 0 && currentState.Equals("level")))
                return;

            currentState = "play";
            ism.Select(selectEffect);
            hints.transform.GetChild(10).gameObject.SetActive(false); // triangle
            hints.transform.GetChild(9).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(3).GetComponent<Text>().text = "When you click that button, there is no come back... God keep you safe, warrior!";
            hints.transform.localPosition = new Vector3(-182, -166, 0);
        }

        if (lvlBtn[x].interactable)
        {
            ism.Select(selectEffect);
            SetLevelText(x);
            levelTxt.gameObject.SetActive(true);
            SetLevelBtnColor();
            lvlBtn[x].transform.localScale = new Vector3(0.55f, 0.55f, 1f);

            if(x != GameController.Instance.player_level - 1)
                lvlBtn[x].GetComponent<Image>().color = new Color(1f, 0.38f, 0f);


            selectedLevelIndex = x;
            playBtn.image.color = new Color(1f, 1f, 1f, 0.6f);
        }
        
    }

    public void OpenLevel()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("play"))
                return;

        if (selectedLevelIndex != -1)
            fadeSprite.GetComponent<Animator>().SetInteger("State", 2);

        ism.Select(selectEffect);

        if (selectedLevelIndex != -1)
            interfaceSound.GetComponent<MusicLoop>().StopPlayingMusic();

        StartCoroutine(CallOpenLevellately());

        
    }

    IEnumerator CallOpenLevellately()
    {
        yield return new WaitForSeconds(0.5f);

        GameController.Instance.totalEnemy = 0;
        GameController.Instance.foundKey = false;
        Utility.StartTimers();
        GameController.Instance.current_level = selectedLevelIndex + 1;

        if (selectedLevelIndex != -1)
        {
            LevelMusicController.lmc.StartPLayingMusicLately(1f);
        }

        switch (selectedLevelIndex)
        {
            case -1:
                ism.PlaySingle(emptyPage);
                break;
            case 0:
                SceneManager.LoadScene("l_7");
                break;
            case 1:
                SceneManager.LoadScene("l_1");
                break;
            case 2:
                SceneManager.LoadScene("l_5");
                break;
            case 3:
                SceneManager.LoadScene("l_2");
                break;
            case 4:
                SceneManager.LoadScene("Castle1");
                GameController.Instance.isInCastle = true;
                break;
            case 5:
                SceneManager.LoadScene("l_3");
                break;
            case 6:
                SceneManager.LoadScene("l_2");
                break;
            case 7:
                SceneManager.LoadScene("l_6");
                break;
            case 8:
                SceneManager.LoadScene("Castle2");
                GameController.Instance.isInCastle = true;
                break;
            case 9:
                SceneManager.LoadScene("l_1");
                break;
            case 10:
                SceneManager.LoadScene("l_2");
                break;
            case 11:
                SceneManager.LoadScene("l_2");
                break;
            case 12:
                SceneManager.LoadScene("l_4");
                break;
            case 13:
                SceneManager.LoadScene("l_4");
                break;
            case 14:
                SceneManager.LoadScene("Castle3");
                GameController.Instance.isInCastle = true;
                break;
            case 15:
                SceneManager.LoadScene("l_7");
                break;
            case 16:
                SceneManager.LoadScene("l_1");
                break;
            case 17:
                SceneManager.LoadScene("l_3");
                break;
            case 18:
                SceneManager.LoadScene("l_6");
                break;
            case 19:
                SceneManager.LoadScene("l_2");
                break;
            case 20:
                SceneManager.LoadScene("Castle4");
                GameController.Instance.isInCastle = true;
                break;
            case 21:
                SceneManager.LoadScene("l_4");
                break;
            case 22:
                SceneManager.LoadScene("l_5");
                break;
            case 23:
                SceneManager.LoadScene("l_1");
                break;
            case 24:
                SceneManager.LoadScene("l_5");
                break;
            case 25:
                SceneManager.LoadScene("Castle5");
                GameController.Instance.isInCastle = true;
                break;
            case 26:
                SceneManager.LoadScene("l_3");
                break;
            case 27:
                SceneManager.LoadScene("l_3");
                break;
            case 28:
                SceneManager.LoadScene("l_5");
                break;
            case 29:
                SceneManager.LoadScene("Castle6");
                GameController.Instance.isInCastle = true;
                break;
            case 30:
                SceneManager.LoadScene("l_7");
                break;
            case 31:
                SceneManager.LoadScene("l_1");
                break;
            case 32:
                SceneManager.LoadScene("l_4");
                break;
            case 33:
                SceneManager.LoadScene("l_6");
                break;
            case 34:
                SceneManager.LoadScene("Castle7");
                GameController.Instance.isInCastle = true;
                break;
            case 35:
                SceneManager.LoadScene("l_3");
                break;
            case 36:
                SceneManager.LoadScene("l_2");
                break;
            case 37:
                SceneManager.LoadScene("l_1");
                break;
            case 38:
                SceneManager.LoadScene("l_4");
                break;
            case 39:
                SceneManager.LoadScene("l_3");
                break;
            case 40:
                SceneManager.LoadScene("Castle8");
                GameController.Instance.isInCastle = true;
                break;
            case 41:
                SceneManager.LoadScene("l_5");
                break;
            case 42:
                SceneManager.LoadScene("l_2");
                break;
            case 43:
                SceneManager.LoadScene("l_6");
                break;
            case 44:
                SceneManager.LoadScene("Castle9");
                GameController.Instance.isInCastle = true;
                break;
            case 45:
                SceneManager.LoadScene("l_7");
                break;
            case 46:
                SceneManager.LoadScene("l_1");
                break;
            case 47:
                SceneManager.LoadScene("l_5");
                break;
            case 48:
                SceneManager.LoadScene("l_4");
                break;
            case 49:
                SceneManager.LoadScene("Castle10");
                GameController.Instance.isInCastle = true;
                break;
                /*
              .
              .
              .
              .
              .
              .    
                */

        }
    }

}
