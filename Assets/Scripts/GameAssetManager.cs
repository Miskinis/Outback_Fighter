using System;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;

[DefaultExecutionOrder(-100)]
public class GameAssetManager : MonoBehaviour
{
    public static GameAssetManager main;

    private UniTask<IReadOnlyList<GameObject>> _charactersDefaultPrefabArray1;
    private UniTask<IReadOnlyList<GameObject>> _charactersDefaultPrefabArray2;
    private UniTask<IReadOnlyList<Sprite>> _characterDefaultIconArray1;
    private UniTask<IReadOnlyList<Sprite>> _characterDefaultIconArray2;

    private async void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    public static async UniTask<IReadOnlyList<GameObject>> LoadAddressableGameObjects(string[] key)
    {
        var handle = await Addressables.LoadAssetsAsync<GameObject>(new List<object>(key), null, Addressables.MergeMode.Intersection).Task;
        return handle.ToList();
    }
    
    public static async UniTask<IReadOnlyList<GameObject>> LoadAddressableGameObjects(string key)
    {
        var handle = await Addressables.LoadAssetsAsync<GameObject>(key, null).Task;
        return handle.ToList();
    }

    public static async UniTask<IReadOnlyList<Sprite>> LoadAddressableSprites(string[] key)
    {
        var handle = await Addressables.LoadAssetsAsync<Sprite>(new List<object>(key), null, Addressables.MergeMode.Intersection).Task;
        return handle.ToList();
    }
    
    public static async UniTask<IReadOnlyList<Sprite>> LoadAddressableSprites(string key)
    {
        var handle = await Addressables.LoadAssetsAsync<Sprite>(key, null).Task;
        return handle.ToList();
    }
}