using CleanRefactor.Domain;

namespace CleanRefactor.Presentation
{
    /// <summary>
    /// Translates a semantic PurchaseStatus into a human-readable UI message.
    ///
    /// This is the ONLY place where final UI text such as "Bomb purchased!" or
    /// "Not enough coins" exists. The application layer returns semantic enums
    /// (requirement #6); turning them into localised/displayable strings is a
    /// pure presentation concern.
    ///
    /// Single Responsibility: status -> text. Nothing else.
    /// </summary>
    public sealed class PurchaseFeedbackPresenter
    {
        public string BuildMessage(ShopItemType item, PurchaseStatus status)
        {
            string itemName = ItemName(item);

            switch (status)
            {
                case PurchaseStatus.Purchased:
                    return itemName + " purchased!";
                case PurchaseStatus.NotEnoughCoins:
                    return "Not enough coins for " + itemName;
                case PurchaseStatus.MaxUsesReached:
                    return itemName + " already at max uses";
                case PurchaseStatus.RequiredLevelNotReached:
                    return "Level too low for " + itemName;
                case PurchaseStatus.AlreadyOwned:
                    return itemName + " already owned";
                default:
                    return "Unknown result";
            }
        }

        /// <summary>
        /// Message shown while the pointer hovers an item. If the item can be
        /// bought it invites the action; otherwise it explains the blocking
        /// reason, reusing the same semantic statuses as BuildMessage.
        /// </summary>
        public string BuildHoverMessage(ShopItemType item, PurchaseStatus status)
        {
            string itemName = ItemName(item);

            switch (status)
            {
                case PurchaseStatus.Purchased: // here "Purchased" means "buyable"
                    return "Click to buy " + itemName;
                case PurchaseStatus.NotEnoughCoins:
                    return "Can't buy " + itemName + ": not enough coins";
                case PurchaseStatus.MaxUsesReached:
                    return "Can't buy " + itemName + ": max uses purchased";
                case PurchaseStatus.RequiredLevelNotReached:
                    return "Can't buy " + itemName + ": level too low";
                case PurchaseStatus.AlreadyOwned:
                    return "Can't buy " + itemName + ": already owned";
                default:
                    return itemName;
            }
        }

        private static string ItemName(ShopItemType item)
        {
            switch (item)
            {
                case ShopItemType.Bomb:        return "Bomb";
                case ShopItemType.Shield:      return "Shield";
                case ShopItemType.DoubleCoins: return "Double Coins";
                default:                       return item.ToString();
            }
        }
    }
}
