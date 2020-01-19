using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemsCategory : MonoBehaviour
{
    public Transform childContainer;
    public GameObject storeItemRowPrefab;
    public GameObject storeItemPrefab;

    private async void Start()
    {
        foreach (var skin in Database.main.skins)
        {
            var storeItemRow = Instantiate(storeItemRowPrefab, childContainer).transform;
            var toggleGroup = storeItemRow.GetComponent<ToggleGroup>();

            foreach (var assetDetails in skin.Value)
            {
                var assetDetails1 = Database.GetIndexedAssetDetails(1, assetDetails);
                var iconHandle1 = await new UniTask<IReadOnlyList<Sprite>>(() => GameAssetManager.LoadAddressableSprites(assetDetails1.iconTag));
                
                for (var i = 0; i < iconHandle1.Count; i++)
                {
                    var storeItem = Instantiate(storeItemPrefab, storeItemRow).GetComponent<StoreItem>();
                    storeItem.itemImage.sprite = iconHandle1[i];
                    storeItem.itemNameText.text = assetDetails.displayName;
                    storeItem.equipToggle.group = toggleGroup;

                    
                    var currentCharacterName = skin.Key;
                    var currentAssetDetails = assetDetails;

                    void OnItemBuy()
                    {
                        if (AccountManager.currentAccount.wallet.virtualBalance >= assetDetails.price)
                        {
                            if (Database.main.TryBuyAsset(AccountManager.currentAccount, assetDetails))
                            {
                                storeItem.bought = true;
                            }
                        }
                    }
                    storeItem.buyButton.onClick.AddListener(OnItemBuy);
                    storeItem.GetComponentInChildren<TextMeshProUGUI>().text = $"{assetDetails.price.ToString()} {Database.currency}";
                    
                    void OnEquip(bool value)
                    {
                        if (value)
                        {
                            Database.SaveEquippedChanges( AccountManager.currentAccount, currentCharacterName, currentAssetDetails);
                        }

                        storeItem.equipped = value;
                    }
                    storeItem.equipToggle.onValueChanged.AddListener(OnEquip);
                    
                
                    if (Database.main.IsEquipped(AccountManager.currentAccount, assetDetails))
                    {
                        storeItem.bought = true;
                        storeItem.equipToggle.isOn = true;
                    }
                }
            }
        }
    }
}
