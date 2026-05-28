using CleanRefactor.Domain;
using UnityEngine;

namespace CleanRefactor.Infrastructure
{
    /// <summary>
    /// Infrastructure implementation of IPlayerRepository backed by Unity's
    /// PlayerPrefs.
    ///
    /// This is the ONLY class in the whole solution allowed to touch
    /// PlayerPrefs. The domain and application layers depend on the
    /// IPlayerRepository abstraction, never on this concrete type.
    ///
    /// Because the dependency is inverted, this file could be deleted and
    /// replaced by a JsonFilePlayerRepository or a CloudPlayerRepository with
    /// zero changes to the business code.
    /// </summary>
    public sealed class PlayerPrefsPlayerRepository : IPlayerRepository
    {
        // Keys are centralised as constants to avoid the magic strings that
        // were scattered all over the original ShopManager.
        private const string CoinsKey          = "Coins";
        private const string PlayerLevelKey    = "PlayerLevel";
        private const string BombUsesKey       = "BombUses";
        private const string ShieldUsesKey     = "ShieldUses";
        private const string HasDoubleCoinsKey = "HasDoubleCoins";

        private readonly int _defaultCoins;
        private readonly int _defaultLevel;

        public PlayerPrefsPlayerRepository(int defaultCoins = 500, int defaultLevel = 1)
        {
            _defaultCoins = defaultCoins;
            _defaultLevel = defaultLevel;
        }

        public PlayerState Load()
        {
            return new PlayerState(
                coins:          PlayerPrefs.GetInt(CoinsKey, _defaultCoins),
                playerLevel:    PlayerPrefs.GetInt(PlayerLevelKey, _defaultLevel),
                bombUses:       PlayerPrefs.GetInt(BombUsesKey, 0),
                shieldUses:     PlayerPrefs.GetInt(ShieldUsesKey, 0),
                hasDoubleCoins: PlayerPrefs.GetInt(HasDoubleCoinsKey, 0) == 1);
        }

        public void Save(PlayerState player)
        {
            PlayerPrefs.SetInt(CoinsKey,          player.Coins);
            PlayerPrefs.SetInt(PlayerLevelKey,    player.PlayerLevel);
            PlayerPrefs.SetInt(BombUsesKey,       player.BombUses);
            PlayerPrefs.SetInt(ShieldUsesKey,     player.ShieldUses);
            PlayerPrefs.SetInt(HasDoubleCoinsKey, player.HasDoubleCoins ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void Reset()
        {
            // Only the shop's own keys are removed (NOT PlayerPrefs.DeleteAll),
            // so unrelated game data stored by other systems is preserved.
            PlayerPrefs.DeleteKey(CoinsKey);
            PlayerPrefs.DeleteKey(PlayerLevelKey);
            PlayerPrefs.DeleteKey(BombUsesKey);
            PlayerPrefs.DeleteKey(ShieldUsesKey);
            PlayerPrefs.DeleteKey(HasDoubleCoinsKey);
            PlayerPrefs.Save();
        }
    }
}
