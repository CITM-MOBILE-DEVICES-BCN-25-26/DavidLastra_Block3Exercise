using System;
using System.IO;
using CleanRefactor.Domain;
using UnityEngine;

namespace CleanRefactor.Infrastructure
{
    /// <summary>
    /// Infrastructure implementation of IPlayerRepository that persists the
    /// player state to a JSON file on disk.
    ///
    /// This is the ONLY class (together with the alternative PlayerPrefs one)
    /// that knows HOW data is stored. The domain and application layers depend
    /// only on the IPlayerRepository abstraction, never on this concrete type
    /// nor on Unity's file/JSON APIs.
    ///
    /// The file lives in Application.persistentDataPath, the platform-safe
    /// writable folder Unity provides on every target (PC, Android, iOS...).
    ///
    /// Swapping PlayerPrefs for this class required ZERO changes to the
    /// domain/application code — that is exactly the point of the abstraction.
    /// </summary>
    public sealed class JsonFilePlayerRepository : IPlayerRepository
    {
        /// <summary>
        /// Serializable transfer object. Unity's JsonUtility only serializes
        /// public fields (not C# properties with private setters), so we map the
        /// PlayerState to/from this plain struct of fields.
        /// </summary>
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

        /// <param name="fileName">Name of the save file (relative to persistentDataPath).</param>
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
            // No save yet -> return the configured defaults.
            if (!File.Exists(_filePath))
                return new PlayerState(_defaultCoins, _defaultLevel, 0, 0, false);

            try
            {
                string json = File.ReadAllText(_filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                // Corrupt or empty file -> fall back to defaults.
                if (data == null)
                    return new PlayerState(_defaultCoins, _defaultLevel, 0, 0, false);

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
            // Deletes the save file so the next Load() returns the defaults.
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
