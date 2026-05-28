using CleanRefactor.Domain;

namespace CleanRefactor.Presentation
{
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
        public string BuildHoverMessage(ShopItemType item, PurchaseStatus status)
        {
            string itemName = ItemName(item);

            switch (status)
            {
                case PurchaseStatus.Purchased: 
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
