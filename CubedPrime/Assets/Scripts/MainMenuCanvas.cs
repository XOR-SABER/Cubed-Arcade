using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject mainMenu;

    void ActivateCanvas()
    {
        mainMenu.SetActive(true);
    }
}
