using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletPanel : MonoBehaviour
{
    private static Wallet _currentWallet;
    
    public TMP_InputField cardNumberField;
    public TMP_InputField csvField;
    public TMP_InputField expDateField;
    public TextMeshProUGUI balanceField;
    public TextMeshProUGUI currencyField;

    public Button editButton;

    private void Start()
    {
        _currentWallet = AccountManager.currentAccount.wallet;

        editButton.onClick.AddListener(() =>
        {
            var textField = editButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textField.text == "Edit")
            {
                cardNumberField.readOnly = false;
                csvField.readOnly = false;
                expDateField.readOnly = false;
                textField.text = "Save";
            }
            else
            {
                cardNumberField.readOnly = true;
                csvField.readOnly = true;
                expDateField.readOnly = true;
                textField.text = "Edit";

                _currentWallet.cardNumbers = cardNumberField.text;
                _currentWallet.csv = csvField.text;
                _currentWallet.expDate = DateTime.Parse(expDateField.text);
                
                Database.SaveWalletChanges(AccountManager.currentAccount, _currentWallet);
            }
        });

        cardNumberField.text = _currentWallet.cardNumbers;
        csvField.text = _currentWallet.csv;
        expDateField.text = _currentWallet.expDate.ToShortDateString();
        balanceField.text = _currentWallet.virtualBalance.ToString();
        currencyField.text = _currentWallet.currency;
    }
}
