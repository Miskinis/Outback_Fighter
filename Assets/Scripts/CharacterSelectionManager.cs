using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public string[] characterLabels = {"Character", "Player 1"};
    public string[] characterIconLabels = {"Character Icon", "Player 1"};

    private void Awake()
    {
        var buttons = GetComponentsInChildren<Button>();
        
        var loadCharactersAsync = Addressables.LoadAssetsAsync<GameObject>(new List<object>(characterLabels), null, Addressables.MergeMode.Intersection);

        void OnLoadCharactersAsyncOnCompleted(AsyncOperationHandle<IList<GameObject>> characterHandle)
        {
            var loadIconsAsync = Addressables.LoadAssetsAsync<Sprite>(new List<object>(characterIconLabels), null, Addressables.MergeMode.Intersection);

            void OnLoadIconsAsyncOnCompleted(AsyncOperationHandle<IList<Sprite>> iconsHandle)
            {
                for (int i = 0; i < iconsHandle.Result.Count; i++)
                {
                    var icon   = iconsHandle.Result[i];
                    var button = buttons[i];
                    button.image.sprite = icon;
                    int characterIndex = i;

                    void ButtonOnClick()
                    {
                        Addressables.InstantiateAsync(characterHandle.Result[characterIndex]);
                    }

                    button.onClick.AddListener(ButtonOnClick);
                }
            }

            loadIconsAsync.Completed += OnLoadIconsAsyncOnCompleted;
        }

        loadCharactersAsync.Completed += OnLoadCharactersAsyncOnCompleted;
    }
}
