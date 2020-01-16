using System;
using System.Collections.Generic;
using TMPro;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public Transform player1SpawnLocation;
    public Transform player2SpawnLocation;
    public Image characterPreviewImage1;
    public TextMeshProUGUI characterReadyText1;
    public Image characterPreviewImage2;
    public TextMeshProUGUI characterReadyText2;
    public Transform characterIconsObject;
    public Transform player1FlagsObject;
    public Transform player2FlagsObject;

    public AssetReferenceSprite player1Flag;
    public AssetReferenceSprite player2Flag;

    private async void Start()
    {
        var characters1 = new List<GameObject>();
        var characters2 = new List<GameObject>();
        var icons1 = new List<Sprite>();
        var icons2 = new List<Sprite>();

        foreach (var equippedAsset in AccountManager.currentAccount.equippedAssets)
        {
            var assetDetails1 = Database.GetIndexedAssetDetails(1, equippedAsset.Value);
            var assetDetails2 = Database.GetIndexedAssetDetails(2, equippedAsset.Value);

            var iconHandle1 = new UniTask<IReadOnlyList<Sprite>>(() => GameAssetManager.LoadAddressableSprites(assetDetails1.iconTag));
            var prefabHandle1 = new UniTask<IReadOnlyList<GameObject>>(() => GameAssetManager.LoadAddressableGameObjects(assetDetails1.assetTag));
            var iconHandle2 = new UniTask<IReadOnlyList<Sprite>>(() => GameAssetManager.LoadAddressableSprites(assetDetails2.iconTag));
            var prefabHandle2 = new UniTask<IReadOnlyList<GameObject>>(() => GameAssetManager.LoadAddressableGameObjects(assetDetails2.assetTag));

            var (prefab1, prefab2, sprite1, sprite2) = await UniTask.WhenAll(prefabHandle1, prefabHandle2, iconHandle1, iconHandle2);
            icons1.Add(sprite1[0]);
            icons2.Add(sprite2[0]);
            characters1.Add(prefab1[0]);
            characters2.Add(prefab2[0]);
        }

        var images = characterIconsObject.GetComponentsInChildren<Image>();
        for (var i = 0; i < icons1.Count; i++)
        {
            images[i].sprite = icons1[i];
        }

        LoadFlagSprites(player1Flag, player1FlagsObject, characters1, icons1, player1SpawnLocation, characterPreviewImage1, characterReadyText1, 0);
        LoadFlagSprites(player2Flag, player2FlagsObject, characters2, icons2, player2SpawnLocation, characterPreviewImage2, characterReadyText2, 1);
        
        PlayerInputDeviceManager.ReassignDevices();
    }

    private static void LoadFlagSprites(AssetReferenceSprite playerFlagReference, Component buttonParent, IReadOnlyList<GameObject> characters, IReadOnlyList<Sprite> characterIcons, Transform spawnLocation, Image characterPreviewImage,
        TextMeshProUGUI characterReadyText, int playerIndex)
    {
        void OnSpriteLoaded(AsyncOperationHandle<Sprite> asyncOperationHandle)
        {
            SetFlagBehavior(buttonParent, characters, characterIcons, spawnLocation, characterPreviewImage, characterReadyText, asyncOperationHandle.Result, playerIndex);
        }

        playerFlagReference.LoadAssetAsync().Completed += OnSpriteLoaded;
    }

    private static void SetFlagBehavior(Component buttonParent, IReadOnlyList<GameObject> characters, IReadOnlyList<Sprite> characterIcons, Transform spawnLocation, Image characterPreviewImage,
        TextMeshProUGUI characterReadyText, Sprite playerFlag, int playerIndex)
    {
        var flagButtons = buttonParent.GetComponentsInChildren<FlagButton>();

        var index = 0;
        for (; index < characters.Count; index++)
        {
            var characterIndex = index;
            var character = characters[characterIndex];
            var flagButton = flagButtons[characterIndex];
            flagButton.image.sprite = playerFlag;

            void OnFlagSelected(bool selected)
            {
                if (selected)
                {
                    characterPreviewImage.sprite = characterIcons[characterIndex];
                }
            }

            flagButton.onValueChanged.AddListener(OnFlagSelected);
            
            if (characterPreviewImage.sprite == false)
            {
                characterPreviewImage.sprite = characterIcons[characterIndex];
            }

            void OnFlagClick()
            {
                CharacterSelectionManager.main.ConfirmSelection(character, new InstantiationParameters(spawnLocation.position, spawnLocation.rotation, null), playerIndex);
                characterReadyText.gameObject.SetActive(true);
                //characterReadyText.text = $"{character.name} Ready";
                for (var i = 0; i < flagButtons.Length; i++)
                {
                    flagButtons[i].interactable = false;
                }
            }

            flagButton.onClick.AddListener(OnFlagClick);
        }

        for (; index < flagButtons.Length; index++)
        {
            flagButtons[index].interactable = false;
        }
    }
}