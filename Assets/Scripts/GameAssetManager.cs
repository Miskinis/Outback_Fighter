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

    public string[] characterNames = new[] {"Woody", "Scarycrow", "Swordy"};
    
    public string player1Tag = "Player 1";
    public string player2Tag = "Player 2";
    public string characterTag = "Character";
    public string iconTag = "Icon";
    public string skinTag = "Skin";
    public string defaultTag = "default";

    private UniTask<IReadOnlyList<GameObject>> _charactersDefaultPrefabArray1;
    private UniTask<IReadOnlyList<GameObject>> _charactersDefaultPrefabArray2;
    private UniTask<IReadOnlyList<Sprite>> _characterDefaultIconArray1;
    private UniTask<IReadOnlyList<Sprite>> _characterDefaultIconArray2;

    public Dictionary<string, string> equippedSkins = new Dictionary<string, string>();
    
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

    public async  UniTask<IReadOnlyList<Sprite>> LoadDefaultCharacter1Icons()
    {
        _characterDefaultIconArray1 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player1Tag, iconTag, defaultTag}));
        return await _characterDefaultIconArray1;
    }
    
    public async  UniTask<IReadOnlyList<Sprite>> LoadAllCharacter1Icons()
    {
        _characterDefaultIconArray1 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player1Tag, iconTag}));
        return await _characterDefaultIconArray1;
    }
    
    public async  UniTask<IReadOnlyList<Sprite>> LoadAllSpecificCharacterIcons(string characterName)
    {
        return await new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player1Tag, iconTag, characterName}));
    }
    /*public async UniTask<Tuple<IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>> LoadAllDefaultCharacterIcons()
    {
        _characterDefaultIconArray1 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player1Tag, iconTag, defaultTag}));
        _characterDefaultIconArray2 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player2Tag, iconTag, defaultTag}));
        
        var (icons1, icons2) = await UniTask.WhenAll(_characterDefaultIconArray1, _characterDefaultIconArray2);
        return new Tuple<IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>(icons1, icons2);
    }*/
    
    public async UniTask<Tuple<IReadOnlyList<GameObject>, IReadOnlyList<GameObject>, IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>> LoadAllDefaultCharactersAndIcons()
    {
        _charactersDefaultPrefabArray1 = new UniTask<IReadOnlyList<GameObject>>(() => LoadAddressableGameObjects(new []{player1Tag, characterTag, defaultTag}));
        _charactersDefaultPrefabArray2 = new UniTask<IReadOnlyList<GameObject>>(() => LoadAddressableGameObjects(new []{player2Tag, characterTag, defaultTag}));
        _characterDefaultIconArray1 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player1Tag, iconTag, defaultTag}));
        _characterDefaultIconArray2 = new UniTask<IReadOnlyList<Sprite>>(() => LoadAddressableSprites(new []{player2Tag, iconTag, defaultTag}));
        
        var (characters1, characters2, icons1, icons2) = await UniTask.WhenAll(_charactersDefaultPrefabArray1, _charactersDefaultPrefabArray2, _characterDefaultIconArray1, _characterDefaultIconArray2);
        return new Tuple<IReadOnlyList<GameObject>, IReadOnlyList<GameObject>, IReadOnlyList<Sprite>, IReadOnlyList<Sprite>>(characters1, characters2, icons1, icons2);
    }
}