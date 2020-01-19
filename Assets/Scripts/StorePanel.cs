using System;
using TMPro;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    private static Wallet _currentWallet;
    public TextMeshProUGUI balanceField;

    private void Start()
    {
        _currentWallet = AccountManager.currentAccount.wallet;
        
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