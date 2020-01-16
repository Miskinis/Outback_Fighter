using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public Image itemImage;
    public Button buyButton;
    public Toggle equipToggle;

    private bool _bought;
    public bool bought
    {
        get { return _bought; }
        set
        {
            if(value)
            {
                buyButton.gameObject.SetActive(false);
                equipToggle.gameObject.SetActive(true);
            }
            _bought = value;
        }
    }

    private bool _equipped;

    public bool equipped
    {
        get { return _equipped; }
        set
        {
            var colors = equipToggle.colors;
            if (value)
            {
                colors.normalColor = Color.green;
                equipToggle.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
            }
            else
            {
                colors.normalColor = Color.white;
                equipToggle.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
            equipToggle.colors = colors;
            _equipped = value;
        }
    }

    private void Awake()
    {
        if (equipped)
        {
            equipToggle.isOn = true;
        }
    }
}
