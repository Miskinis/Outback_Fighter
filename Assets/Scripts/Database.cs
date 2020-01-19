using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Database
{
    public static Database main;
    public List<Account> accounts;
    public Dictionary<string, List<AssetDetails>> skins;
    public static string currency = "Gems";

    public static void InitializeDatabase()
    {
        if (TryGetDatabase(out main) == false)
        {
            main = CreateDatabase();
            SaveData();
        }
    }

    public bool TryBuyAsset(Account account, AssetDetails assetDetails)
    {
        if (account.wallet.virtualBalance >= assetDetails.price && account.ownedAssets.Contains(assetDetails) == false)
        {
            account.ownedAssets.Add(assetDetails);
            account.wallet.virtualBalance -= assetDetails.price;
            SaveData();
            return true;
        }

        return false;
    }

    public static void SaveWalletChanges(Account account, Wallet wallet)
    {
        account.wallet = wallet;
        SaveData();
    }
    
    public static void SaveEquippedChanges(Account account, string characterName, AssetDetails assetDetails)
    {
        account.equippedAssets[characterName] = assetDetails;
        SaveData();
    }

    public static AssetDetails GetIndexedAssetDetails(int playerIndex, AssetDetails assetDetails)
    {
        var indexString = $" {playerIndex.ToString()}";
        var modifiedAssetDetails = new AssetDetails(assetDetails);
        modifiedAssetDetails.iconTag += indexString;
        modifiedAssetDetails.assetTag += indexString;
        return modifiedAssetDetails;
    }
    
    private static void SaveData()
    {
        /*try
        {
            var path = $"{Application.streamingAssetsPath}/database.json";

            using (StreamWriter sw = new StreamWriter(path)) 
            {
                sw.Write(JsonUtility.ToJson(main));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Debug.LogError($"Failed to write database to file because {e}");
        }*/
    }

    private static bool TryGetDatabase(out Database database)
    {
        try
        {
            var path = $"{Application.streamingAssetsPath}/database.json";
            
            using (StreamReader sr = new StreamReader(path)) 
            {
                database = JsonUtility.FromJson<Database>(sr.ReadToEnd());
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read database from file");
            database = null;
            return false;
        }
    }
    
    public bool TryGetAccount(string username, string password, out Account returnAccount)
    {
        foreach (var account in accounts)
        {
            if (account.username == username && account.password == password)
            {
                returnAccount = account;
                return true;
            }
        }

        returnAccount = null;
        return false;
    }

    public bool IsOwned(AssetDetails assetDetails)
    {
        foreach (var skin in skins)
        {
            if (skin.Value.Contains(assetDetails))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool IsEquipped(Account account, AssetDetails assetDetails)
    {
        return account.equippedAssets.ContainsValue(assetDetails);
    }

    public bool IsOwned(string characterName, AssetDetails assetDetails)
    {
        return skins[characterName].Contains(assetDetails);
    }

    private static Database CreateDatabase()
    {
        var database = new Database();
        
        var skins = new Dictionary<string, List<AssetDetails>>
        {
            {
                "Woody",
                new List<AssetDetails>
                {
                    new AssetDetails("Woody Default", "Woody Default Icon", "Woody Default"),
                    new AssetDetails("Technowood", "Technowood Icon", "Technowood", 1000)
                }
            },
            {
                "Scarycrow", 
                new List<AssetDetails>
                {
                    new AssetDetails("Scarycrow Default", "Scarycrow Default Icon", "Scarycrow Default"),
                    new AssetDetails("Meloncrow", "Meloncrow Icon", "Meloncrow", 500)
                }
            },
            {
                "Swordy", 
                new List<AssetDetails>
                {
                    new AssetDetails("Swordy Default", "Swordy Default Icon", "Swordy Default"),
                    new AssetDetails("Edgelord", "Edgelord Icon", "Edgelord", 500)
                }
            }
        };

        database.accounts = new List<Account>
        {
            new Account
            (
                "admin",
                "admin",
                new Wallet
                (
                    2000,
                    "Gems",
                    "1111222233334444",
                    "123",
                    DateTime.Today.AddYears(1)
                ),
                new List<AssetDetails>
                {
                    skins["Woody"][0],
                    skins["Scarycrow"][0],
                    skins["Swordy"][0]
                },
                new Dictionary<string, AssetDetails>
                {
                    {"Woody", skins["Woody"][0]},
                    {"Scarycrow", skins["Scarycrow"][0]},
                    {"Swordy", skins["Swordy"][0]}
                }
            )
        };

        database.skins = skins;

        return database;
    }
}

[Serializable]
public class Account
{
    public string username;
    public string password;

    public Wallet wallet;
    public List<AssetDetails> ownedAssets;
    public Dictionary<string, AssetDetails> equippedAssets;

    public Account(string username, string password, Wallet wallet, List<AssetDetails> ownedAssets, Dictionary<string, AssetDetails> equippedAssets)
    {
        this.username = username;
        this.password = password;
        this.wallet = wallet;
        this.ownedAssets = ownedAssets;
        this.equippedAssets = equippedAssets;
    }
}

[Serializable]
public class Wallet
{
    public int virtualBalance;
    public string currency;
    public string cardNumbers;
    public string csv;
    public DateTime expDate;

    public Wallet(int virtualBalance, string currency, string cardNumbers, string csv, DateTime expDate)
    {
        this.virtualBalance = virtualBalance;
        this.currency = currency;
        this.cardNumbers = cardNumbers;
        this.csv = csv;
        this.expDate = expDate;
    }
}

[Serializable]
public class AssetDetails
{
    public Guid guid;
    public string displayName;
    public string iconTag;
    public string assetTag;
    public int price;

    public AssetDetails(string displayName, string iconTag, string assetTag, int price = 0)
    {
        guid = Guid.NewGuid();
        this.displayName = displayName;
        this.iconTag = iconTag;
        this.assetTag = assetTag;
        this.price = price;
    }

    public AssetDetails(AssetDetails assetDetails)
    {
        guid = assetDetails.guid;
        displayName = assetDetails.displayName;
        iconTag = assetDetails.iconTag;
        assetTag = assetDetails.assetTag;
        price = assetDetails.price;
    }
}