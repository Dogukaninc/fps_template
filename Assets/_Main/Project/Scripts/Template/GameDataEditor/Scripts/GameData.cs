using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Main.Project.Scripts.Template.GameDataEditor.Scripts
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        // HELPERS
        public SavedLevelData SavedLevelData => savedLevelDataList[currentLevelIndex];

        public SavedLevelData GetSavedLevelData(int lvl)
        {
            var result = savedLevelDataList.FirstOrDefault(x => x.lvl == lvl);
            if (result != null) return result;

            var newData = new SavedLevelData() { lvl = lvl };
            savedLevelDataList.Add(newData);
            if (lvl == 0) newData.IsOpen = true;
            return newData;
        }

        // DATA
        public CurrencyData currencyData;

        public List<SavedLevelData> savedLevelDataList = new();
        public int currentLevelIndex;
        public int currentStageIndex;

        public SavedUpgradeData[] savedPlayerUpgradeDataList;
        public SavedTutorialData savedTutorialData;
        public List<SavedBreakableObjectData> savedBreakableObjectDataList = new();
        public SavedAxeData savedAxeData;
        public SavedMotorData savedMotorData;
        //public UpgradedPlayerData upgradedPlayerData;

        // INTERSTITIAL
        public int requiredSessionForAd;
        public int completedSessionForAd;

        public bool CanShowInterstitial() => true; //completedSessionForAd >= requiredSessionForAd;

        // public void AddCurrency(ItemType itemType, int amount)
        // {
        //     currencyData.Add(itemType, amount);
        //     //Events.OnCurrencyEarned?.Invoke(itemType, amount);
        //
        //     Save();
        // }
        //
        // public bool TryGetCurrency(ItemType itemType, int requestedAmount)
        // {
        //     var canGet = currencyData.TryToSpend(itemType, requestedAmount);
        //     if (canGet)
        //     {
        //         //Events.OnCurrencySpent?.Invoke(itemType, requestedAmount);
        //         Save();
        //     }
        //
        //     return canGet;
        // }

        public bool TryGetCoin(int requested)
        {
            /*if (currencyData.Coin < requested) return false;

            currencyData.Coin -= requested;
            Debug.LogWarning(requested + " coin spent!");
            Events.OnCoinSpent?.Invoke();
            StaticHelper.Instance.SaveGameData();*/
            return true;
        }

        public void AddCoin(int earning)
        {
            /*currencyData.Coin += earning;
            Events.OnCoinEarned?.Invoke();
            StaticHelper.Instance.SaveGameData();*/
        }

        public bool TryGetGem(int requested)
        {
            /*if (currencyData.Gem < requested) return false;

            currencyData.Gem -= requested;
            Debug.LogWarning(requested + " gem spent!");
            Events.OnGemSpent?.Invoke();
            StaticHelper.Instance.SaveGameData();*/
            return true;
        }

        public void AddGem(int earning)
        {
            /*currencyData.Gem += earning;
            //Events.OnGemCollected?.Invoke(earning);
            StaticHelper.Instance.SaveGameData();*/
        }

        public void Save() => SaveManager.SaveData(this);
        public void Load() => SaveManager.LoadData(this);
    }

    [Serializable]
    public class CurrencyData
    {
        public int Coin;
        public int Gem;

        [SerializeField] private List<CurrencyEntry> currencyList = new();
        /*
        public void Add(ItemType itemType, int amount)
        {
            var entry = currencyList.Find(e => e.itemType == itemType);

            if (entry != null)
            {
                entry.amount += amount;

                Events.OnCurrencyUpdated?.Invoke(itemType, entry.amount);
            }
            else
            {
                currencyList.Add(new CurrencyEntry { itemType = itemType, amount = amount });
            }
        }

        public bool TryToSpend(ItemType itemType, int requestedAmount)
        {
            var entry = currencyList.Find(e => e.itemType == itemType);

            if (entry == null || entry.amount < requestedAmount) return false;

            entry.amount -= requestedAmount;
            Events.OnCurrencyUpdated?.Invoke(itemType, entry.amount);
            return true;
        }

        public bool CanSpend(ItemType itemType, int requestedAmount)
        {
            var entry = currencyList.Find(e => e.itemType == itemType);

            return entry != null && entry.amount >= requestedAmount;
        }

        public int GetAmount(ItemType itemType)
        {
            var entry = currencyList.Find(e => e.itemType == itemType);
            return entry?.amount ?? 0;
        }
        */
    }

    [Serializable]
    public class CurrencyEntry
    {
        // public ItemType itemType;
        // public int amount;
    }

    /*[System.Serializable]
    public class CurrencyData
    {
        public int amount;

        // METHODS
        public bool TryGet(int requested)
        {
            if (amount < requested) return false;

            amount -= requested;
            //Events.OnCoinSpent?.Invoke();
            StaticHelper.Instance.SaveGameData();
            return true;
        }

        public void Add(int earning)
        {
            amount += earning;
            Events.OnCoinEarned?.Invoke();
            StaticHelper.Instance.SaveGameData();
        }
    }*/

    [Serializable]
    public class SavedLevelData
    {
        // HELPERS
        //public SavedStageData SavedStageData => savedStageDataList[currentStageIndex];

        // DATA
        //public SavedStageData[] savedStageDataList;
        public int currentStageIndex;

        public int lvl;

        public bool IsOpen;

        //public float MaxPlayTime;
        public float MaxClearedEnemyPercentage;
    }

    /*[System.Serializable]
    public class SavedStageData
    {
        //public int[] xpList;
    }*/

    // TODO: name eklenebilir okunurluk icin
    [Serializable]
    public class SavedUpgradeData
    {
        //public int index;
        //public UpgradeType upgradeType;
        public int level;
        //public bool isMax;
    }
    
    [Serializable]
    public class SavedTutorialData
    {
        public enum TutorialInfo
        {
            Ready,
            Shown,
            Completed
        }
        
        public int lastIndex;
        public TutorialInfo tutorialInfo;
        public bool completed;
    }

    [Serializable]
    public class SavedBreakableObjectData
    {
        public string id;
        public bool isBroken;

        public SavedBreakableObjectData(string id)
        {
            this.id = id;
        }
    }

    [Serializable]
    public class SavedAxeData
    {
        public int level;
        public AxeUpgradeStatus axeUpgradeStatus;
        public float leftSecondsForClaim;
    }

    [Serializable]
    public class SavedMotorData
    {
        //public int 
        public SavedMotorPartData[] motorPartDataList;
    }

    [Serializable]
    public class SavedMotorPartData
    {
        // public ItemData itemData;
        public bool isCollected;
        public bool isFixed;
    }

    public enum AxeUpgradeStatus
    {
        ReadyToBeUpgraded,
        Upgrading,
        ReadyToBeClaimed
    }
}