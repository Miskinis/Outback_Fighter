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
    
    public string[] character1AssetLabels = {"Character", "Player 1"};
    public string[] character1IconAssetLabels = {"Character Icon", "Player 1"};
    
    public string[] character2AssetLabels = {"Character", "Player 2"};
    public string[] character2IconAssetLabels = {"Character Icon", "Player 2"};

    private UniTask<IReadOnlyList<GameObject>> _characters1Array;
    private UniTask<IReadOnlyList<GameObject>> _characters2Array;
    private UniTask<IReadOnlyList<Sprite>> _characterIcon1Array;
    private UniTask<IReadOnlyList<Sprite>> _characterIcon2Array;
    
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    private static async UniTask<IReadOnlyList<GameObject>> LoadCharacters(string[] key)
    {
        var handle = await Addressables.LoadAssetsAsync<GameObject>(new List<object>(key), null, Addressables.MergeMode.Intersection).Task;
        return handle.ToList();
    }

    private static async UniTask<IReadOnlyList<Sprite>> LoadCharacterIcons(string[] key)
    {
        var handle = await Addressables.LoadAssetsAsync<Sprite>(new List<object>(key), null, Addressables.MergeMode.Intersection).Task;
        return handle.ToList();
    }

    public async UniTask<Tuple<IReadOnlyList<GameObject>, IReadOnlyList<GameObject>, IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>> LoadAllAsync()
    {
        _characters1Array = new UniTask<IReadOnlyList<GameObject>>(() => LoadCharacters(character1AssetLabels));
        _characters2Array = new UniTask<IReadOnlyList<GameObject>>(() => LoadCharacters(character2AssetLabels));
        _characterIcon1Array = new UniTask<IReadOnlyList<Sprite>>(() => LoadCharacterIcons(character1IconAssetLabels));
        _characterIcon2Array = new UniTask<IReadOnlyList<Sprite>>(() => LoadCharacterIcons(character2IconAssetLabels));
        
        var (characters1, characters2, icons1, icons2) = await UniTask.WhenAll(_characters1Array, _characters2Array, _characterIcon1Array, _characterIcon2Array);
        return new Tuple<IReadOnlyList<GameObject>, IReadOnlyList<GameObject>, IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>(characters1, characters2, icons1, icons2);
    }
}