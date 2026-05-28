namespace CleanRefactor.Domain
{
    /// <summary>
    /// Pure domain entity. Represents everything the shop needs to know about
    /// the player. It has NO dependency on Unity (no MonoBehaviour, no PlayerPrefs).
    ///
    /// Mutations are exposed as explicit, intention-revealing methods instead of
    /// public setters, so business rules cannot be bypassed by accident.
    /// </summary>
    public sealed class PlayerState
    {
        public int Coins { get; private set; }
        public int PlayerLevel { get; private set; }
        public int BombUses { get; private set; }
        public int ShieldUses { get; private set; }
        public bool HasDoubleCoins { get; private set; }

        public PlayerState(int coins, int playerLevel, int bombUses, int shieldUses, bool hasDoubleCoins)
        {
            Coins = coins;
            PlayerLevel = playerLevel;
            BombUses = bombUses;
            ShieldUses = shieldUses;
            HasDoubleCoins = hasDoubleCoins;
        }

        /// <summary>Subtracts the given cost from the player's coins.</summary>
        public void SpendCoins(int amount)
        {
            if (amount < 0) return;
            Coins -= amount;
        }

        public void AddBombUse()    => BombUses++;
        public void AddShieldUse()  => ShieldUses++;
        public void GrantDoubleCoins() => HasDoubleCoins = true;
    }
}
