using System;
using UnityEngine;

public class PressPlayerBehavior : MonoBehaviour
{
    public static PressPlayerBehavior main;

    private void Awake()
    {
        main = this;

        if (AccountManager.loggedIn)
        {
            OnPressPlayer();
        }
    }

    public GameObject[] objectsToClose;
    public GameObject[] objectsToOpen;

    public void OnPressPlayer()
    {
        foreach (var panel in objectsToClose)
        {
            panel.SetActive(false);
        }

        foreach (var panel in objectsToOpen)
        {
            panel.SetActive(true);
        }
    }
}