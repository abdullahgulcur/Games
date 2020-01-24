using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InterfaceCtrl : MonoBehaviour {

	
    public void PressArena()
    {
        SceneManager.LoadScene("Arena");
    }

    public void PressBlackSmith()
    {
        SceneManager.LoadScene("BlackSmith");
    }

    public void PressWeaponry()
    {
        SceneManager.LoadScene("Weaponry");
    }

    public void PressGrocery()
    {
        SceneManager.LoadScene("Grocery");
    }

    public void PressCharacterMenu()
    {
        SceneManager.LoadScene("CharacterMenu");
    }

}
