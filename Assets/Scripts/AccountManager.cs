using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    public static Account currentAccount;
    public static bool loggedIn;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Database.InitializeDatabase();
    }
}