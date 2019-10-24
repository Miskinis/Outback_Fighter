using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class CharacterSelectionMenu : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject readyPanel;
    public string[] characterAssetLabels = {"Character", "Player 1"};
    public string[] characterIconAssetLabels = {"Character Icon", "Player 1"};

    private void Awake()
    {
        readyPanel.SetActive(false);

        var loadCharactersAsync = Addressables.LoadAssetsAsync<GameObject>(new List<object>(characterAssetLabels), null, Addressables.MergeMode.Intersection);

        void OnLoadCharactersAsyncOnCompleted(AsyncOperationHandle<IList<GameObject>> characterHandle)
        {
            var loadIconsAsync = Addressables.LoadAssetsAsync<Sprite>(new List<object>(characterIconAssetLabels), null, Addressables.MergeMode.Intersection);

            void OnLoadIconsAsyncOnCompleted(AsyncOperationHandle<IList<Sprite>> iconsHandle)
            {
                var buttons = GetComponentsInChildren<Button>();
                int index = 0;
                for (; index < iconsHandle.Result.Count; index++)
                {
                    var icon   = iconsHandle.Result[index];
                    var button = buttons[index];
                    button.image.sprite = icon;
                    int characterIndex = index;

                    void ButtonOnClick()
                    {
                        CharacterSelectionManager.main.ConfirmSelection(characterHandle.Result[characterIndex], new InstantiationParameters(spawnLocation.position, spawnLocation.rotation, null));
                        readyPanel.SetActive(true);
                        gameObject.SetActive(false);
                    }

                    button.onClick.AddListener(ButtonOnClick);
                }

                for (; index < buttons.Length; index++)
                {
                    buttons[index].interactable = false;
                }
            }

            loadIconsAsync.Completed += OnLoadIconsAsyncOnCompleted;
        }

        loadCharactersAsync.Completed += OnLoadCharactersAsyncOnCompleted;
    }
}
