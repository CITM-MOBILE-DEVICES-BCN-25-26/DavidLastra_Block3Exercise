using System;
using System.IO;
using CleanRefactor.Domain;
using UnityEngine;

namespace CleanRefactor.Infrastructure
{
    public sealed class JsonFilePlayerRepository : IPlayerRepository
    {
       
        [Serializable]
        private class PlayerData
        {
            public int coins;
            public int playerLevel;
            public int bombUses;
            public int shieldUses;
            public bool hasDoubleCoins;
        }

        private readonly string _filePath;
        private readonly int _defaultCoins;
        private readonly int _defaultLevel;

        public JsonFilePlayerRepository(
            int defaultCoins = 500,
            int defaultLevel = 1,
            string fileName = "player_save.json")
        {
            _defaultCoins = defaultCoins;
            _defaultLevel = defaultLevel;
            _filePath = Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
        }

        public PlayerState Load()
        {
            if (!File.Exists(_filePath))
            {
                return new PlayerState(_defaultCoins, _defaultLevel, 0, 0, false);
            }
                
            try
            {
                string json = File.ReadAllText(_filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                if (data == null)
                {
                    return new PlayerState(_defaultCoins, _defaultLevel, 0, 0, false);
                }

                return new PlayerState(
                    data.coins,
                    data.playerLevel,
                    data.bombUses,
                    data.shieldUses,
                    data.hasDoubleCoins);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[JsonFilePlayerRepository] Could not read save, " +
                                 "using defaults. " + e.Message);
                return new PlayerState(_defaultCoins, _defaultLevel, 0, 0, false);
            }
        }

        public void Save(PlayerState player)
        {
            var data = new PlayerData
            {
                coins          = player.Coins,
                playerLevel    = player.PlayerLevel,
                bombUses       = player.BombUses,
                shieldUses     = player.ShieldUses,
                hasDoubleCoins = player.HasDoubleCoins
            };

            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(_filePath, json);
        }

        public void Reset()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }     
        }
    }
}
