using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour {

    Animator anim;

    public AudioClip changePage;
    public AudioClip emptyPage;

    public GameObject mapCanvas;
    public GameObject characterMenuCanvas;
    public GameObject paper;
    public GameObject player;

    public GameObject[] maps;
    public GameObject[] mapBtns;

    public GameObject mapStuff;

    public GameObject[] patterns;
    public GameObject slots;

    GameObject envanter;
    GameObject scrollBar;
    GameObject greenBoxParent;
    GameObject diffSlots;

    public InterfaceSoundManager ism;

    public GameObject levelController;

    public Button playBtn;

    int index = 0;

    public GameObject fadeSprite;

    public GameObject eventSystem;

    public Image locker;

    public Text purchaseText;
    public Button unlockBtn;

    

    void Start() {
        anim = GetComponent<Animator>();

        envanter = paper.transform.GetChild(6).gameObject;
        scrollBar = paper.transform.GetChild(8).gameObject;
        greenBoxParent = paper.transform.GetChild(5).gameObject;
        diffSlots = paper.transform.GetChild(4).gameObject;
    }

	
    public void ReturnMainMenu()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ism.ChangePage(changePage);
        eventSystem.SetActive(false);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        StartCoroutine(ResetFade());

        index = 0;
    }

    IEnumerator ResetFade()
    {
        
        yield return new WaitForSeconds(0.5f);
        mapCanvas.gameObject.SetActive(false);
        characterMenuCanvas.gameObject.SetActive(true);
        paper.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        ResetMaps();
        ResetMapBtns();
        mapStuff.gameObject.SetActive(false);

        slots.gameObject.SetActive(true);
        patterns[0].gameObject.SetActive(false);
        patterns[1].gameObject.SetActive(true);

        envanter.gameObject.SetActive(false);
        scrollBar.gameObject.SetActive(false);
        greenBoxParent.gameObject.SetActive(false);
        diffSlots.gameObject.SetActive(false);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(0.5f);

        locker.gameObject.SetActive(false);
        purchaseText.gameObject.SetActive(false);
        eventSystem.SetActive(true);
        playBtn.gameObject.SetActive(true);
        unlockBtn.gameObject.SetActive(false);
    }

    void ResetMaps()
    {
        foreach (GameObject map in maps)
            map.gameObject.SetActive(false);
        
    }

    void ResetMapBtns()
    {
        foreach (GameObject mapbtn in mapBtns)
            mapbtn.gameObject.SetActive(false);

    }

    public void OpenLevel(int x)
    {
        StartCoroutine(OpenLevelLate(x));
    }

    

    public void MapRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetMaps();
        ResetMapBtns();

        if (index < 3)
        {
            index++;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        maps[index].gameObject.SetActive(true);

        levelController.GetComponent<LevelController>().SetLevelBtnColor();
        levelController.GetComponent<LevelController>().TickEmpty();

        if (index <= GameController.Instance.OpenableMapIndex)
        {
            unlockBtn.gameObject.SetActive(false);
            purchaseText.gameObject.SetActive(false);
            locker.gameObject.SetActive(false);
            mapBtns[index].gameObject.SetActive(true);
            playBtn.gameObject.SetActive(true);
        }
        else if(index == GameController.Instance.OpenableMapIndex + 1 && GameController.Instance.player_level == index * 15 + 1) // ve level uyusuyorsa
        {
            unlockBtn.gameObject.SetActive(true);
            purchaseText.gameObject.SetActive(true);
            //purchaseText.text = "Map is closed. Achieve levels with " + GetPriceOfMap(index).ToString() + " $.";
            locker.gameObject.SetActive(true);
            playBtn.gameObject.SetActive(false);
        }
        else
        {
            unlockBtn.gameObject.SetActive(false);
            purchaseText.gameObject.SetActive(false);
            locker.gameObject.SetActive(true);
            playBtn.gameObject.SetActive(false);
        }

        

        playBtn.image.color = new Color(1f, 1f, 1f, 0.36f);
    }

    float GetPriceOfMap(int index)
    {
        return 1.99f;
    }
    
    void Update()
    {
        if (GameController.Instance.restartScene)
        {
            StartCoroutine(ResetMap1());
            GameController.Instance.restartScene = false;
        }
    }

    IEnumerator ResetMap1()
    {
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        unlockBtn.gameObject.SetActive(false);
        purchaseText.gameObject.SetActive(false);
        locker.gameObject.SetActive(false);
        mapBtns[index].gameObject.SetActive(true);
        playBtn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
    }

    public void Unlock()
    {
        switch (index)
        {
            case 1:
                //IAPManager.Instance.BuyMap_1();
                GameController.Instance.OpenableMapIndex = 1;
                SaveSystem.SavePlayer();
                GameController.Instance.restartScene = true;
                break;
            case 2:
                //IAPManager.Instance.BuyMap_2();
                GameController.Instance.OpenableMapIndex = 2;
                SaveSystem.SavePlayer();
                GameController.Instance.restartScene = true;
                break;
            case 3:
                //IAPManager.Instance.BuyMap_3();
                GameController.Instance.OpenableMapIndex = 3;
                SaveSystem.SavePlayer();
                GameController.Instance.restartScene = true;
                break;

        }
    }

    public void MapLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetMaps();
        ResetMapBtns();

        if (index > 0)
        {
            index--;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        levelController.GetComponent<LevelController>().SetLevelBtnColor();
        levelController.GetComponent<LevelController>().TickEmpty();

        if (index <= GameController.Instance.OpenableMapIndex)
        {
            unlockBtn.gameObject.SetActive(false);
            purchaseText.gameObject.SetActive(false);
            locker.gameObject.SetActive(false);
            mapBtns[index].gameObject.SetActive(true);
            playBtn.gameObject.SetActive(true);
        }
        else if (index == GameController.Instance.OpenableMapIndex + 1 && GameController.Instance.player_level == index * 15 + 1)
        {
            unlockBtn.gameObject.SetActive(true);
            purchaseText.gameObject.SetActive(true);
            //purchaseText.text = "Map is closed. Achieve levels with " + GetPriceOfMap(index).ToString() + " $.";
            locker.gameObject.SetActive(true);
            playBtn.gameObject.SetActive(false);
        }
        else
        {
            unlockBtn.gameObject.SetActive(false);
            purchaseText.gameObject.SetActive(false);
            locker.gameObject.SetActive(true);
            playBtn.gameObject.SetActive(false);
        }

        maps[index].gameObject.SetActive(true);

        

        playBtn.image.color = new Color(1f, 1f, 1f, 0.36f);
    }

    IEnumerator OpenLevelLate(int x)
    {
        GameController.Instance.playerDead = false;
        switch (x)
        {
            case 0:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Level1");
                break;
            case 1:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Arena");
                break;
            case 2:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Level2");
                break;
            case 3:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Castle");
                break;
            case 4:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Level3");
                break;
            case 5:
                anim.SetTrigger("Fade");
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("NewLevel");
                break;

        }
    }


}
