using System;
using TMPro;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    public static StorePanel main;
    private static Wallet _currentWallet;
    public TextMeshProUGUI balanceField;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    private void Start()
    {
        _currentWallet = AccountManager.currentAccount.wallet;
        UpdateBalance();
    }

    public void UpdateBalance()
    {
        balanceField.text = $"{_currentWallet.virtualBalance.ToString()} {_currentWallet.currency}";
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}