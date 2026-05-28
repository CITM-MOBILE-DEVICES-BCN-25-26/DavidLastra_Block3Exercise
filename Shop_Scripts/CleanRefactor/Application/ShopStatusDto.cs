using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    /// <summary>
    /// DTO describing the current shop state for the UI.
    ///
    /// Requirement #5: "The UI must not calculate these rules directly."
    /// The application layer computes whether each item can be bought AND the
    /// semantic reason (PurchaseStatus) for each one, so the View can both
    /// enable/disable buttons and explain WHY an item is not buyable on hover.
    ///
    /// The View never sees the domain model: it only receives these booleans
    /// and semantic enums and lets the PurchaseFeedbackPresenter turn the enum
    /// into displayable text.
    /// </summary>
    public sealed class ShopStatusDto
    {
        public int Coins { get; }

        public bool CanBuyBomb { get; }
        public bool CanBuyShield { get; }
        public bool CanBuyDoubleCoins { get; }

        // Semantic reason for each item (Purchased == can be bought).
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
