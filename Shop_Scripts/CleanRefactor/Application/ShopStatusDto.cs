using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    public sealed class ShopStatusDto
    {
        public int Coins { get; }

        public bool CanBuyBomb { get; }
        public bool CanBuyShield { get; }
        public bool CanBuyDoubleCoins { get; }
        public PurchaseStatus BombStatus { get; }
        public PurchaseStatus ShieldStatus { get; }
        public PurchaseStatus DoubleCoinsStatus { get; }

        public ShopStatusDto(
            int coins,
            PurchaseStatus bombStatus,
            PurchaseStatus shieldStatus,
            PurchaseStatus doubleCoinsStatus)
        {
            Coins = coins;
            BombStatus = bombStatus;
            ShieldStatus = shieldStatus;
            DoubleCoinsStatus = doubleCoinsStatus;

            CanBuyBomb        = bombStatus == PurchaseStatus.Purchased;
            CanBuyShield      = shieldStatus == PurchaseStatus.Purchased;
            CanBuyDoubleCoins = doubleCoinsStatus == PurchaseStatus.Purchased;
        }
    }
}
