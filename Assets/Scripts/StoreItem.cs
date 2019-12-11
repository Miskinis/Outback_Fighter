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
            colors.normalColor = value ? Color.green : Color.white;
            equipToggle.colors = colors;
            _equipped = value;
        }
    }

    private void Awake()
    {
        //equipToggle.group = GetComponentInParent<ToggleGroup>();

        if (bought == false)
        {
            buyButton.onClick.AddListener(OnItemBuy);
        }

        if (equipped)
        {
            equipToggle.isOn = true;
        }
    }

    private void OnItemBuy()
    {
        bought = true;
    }
}
