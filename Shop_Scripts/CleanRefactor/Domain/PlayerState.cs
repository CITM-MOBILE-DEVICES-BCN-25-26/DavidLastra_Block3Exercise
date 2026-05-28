namespace CleanRefactor.Domain
{
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
