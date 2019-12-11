using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemsCategory : MonoBehaviour
{
    public Transform childContainer;
    public GameObject storeItemRowPrefab;
    public GameObject storeItemPrefab;

    private async void Awake()
    {
        foreach (var characterName in GameAssetManager.main.characterNames)
        {
            var storeItemRow = Instantiate(storeItemRowPrefab, childContainer).transform;
            var toggleGroup = storeItemRow.GetComponent<ToggleGroup>();
            
            var handle = GameAssetManager.main.LoadAllSpecificCharacterIcons(characterName);
            await handle;
            
            GameAssetManager.main.equippedSkins.Add(characterName, handle.Result[0].name);

            for (var i = 0; i < handle.Result.Count; i++)
            {
                var icon = handle.Result[i];
                var storeItem = Instantiate(storeItemPrefab, storeItemRow).GetComponent<StoreItem>();
                storeItem.itemImage.sprite = icon;
                storeItem.itemNameText.text = icon.name;
                storeItem.equipToggle.group = toggleGroup;
                storeItem.equipToggle.onValueChanged.AddListener((value) =>
                {
                    if(value)
                    {
                        GameAssetManager.main.equippedSkins[characterName] = icon.name;
                    }
                    storeItem.equipped = value;
                });
                
                if (i == 0)
                {
                    storeItem.bought = true;
                    storeItem.equipToggle.isOn = true;
                }
            }
        }
    }
}
