using System.Collections.Generic;
using CleanRefactor.Application;
using CleanRefactor.Domain;
using CleanRefactor.Presentation;
using UnityEngine;

namespace CleanRefactor.Composition
{
    public sealed class ShopBootstrap : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private ShopView shopView;
        [SerializeField] private UnityPurchaseAudio purchaseAudio;

        [Header("Item Costs")]
        [SerializeField] private int bombCost = 100;
        [SerializeField] private int shieldCost = 150;
        [SerializeField] private int doubleCoinsCost = 300;

        [Header("Item Limits")]
        [SerializeField] private int bombMaxUses = 3;
        [SerializeField] private int shieldMaxUses = 2;
        [SerializeField] private int doubleCoinsRequiredLevel = 5;

        [Header("Player Defaults (first run)")]
        [SerializeField] private int defaultCoins = 500;
        [SerializeField] private int defaultLevel = 1;

        [Header("Debug")]
        [Tooltip("When enabled, all saved shop data is wiped on Play so the " +
                 "shop starts from the default values above. For testing only.")]
        [SerializeField] private bool resetToDefaultOnStart = false;

        private void Awake()
        {
            ShopConfig config = new ShopConfig(
                bomb:        new ShopItemConfig(bombCost, bombMaxUses),
                shield:      new ShopItemConfig(shieldCost, shieldMaxUses),
                doubleCoins: new ShopItemConfig(doubleCoinsCost, maxUses: 1,
                                                requiredLevel: doubleCoinsRequiredLevel));

            var repository = new Infrastructure.JsonFilePlayerRepository(
                defaultCoins, defaultLevel);


            if (resetToDefaultOnStart)
            {
                repository.Reset();
                Debug.Log("[ShopBootstrap] Debug reset: shop data restored to defaults.");
            }

            var catalog = new ShopCatalog(new List<IShopItem>
            {
                new BombItem(config.Bomb),
                new ShieldItem(config.Shield),
                new DoubleCoinsItem(config.DoubleCoins)
            });

            var purchaseItem  = new PurchaseItemUseCase(catalog, repository);
            var getShopStatus = new GetShopStatusUseCase(catalog, repository);

            var feedback  = new PurchaseFeedbackPresenter();
            var presenter = new ShopPresenter(
                shopView, purchaseItem, getShopStatus, feedback, purchaseAudio);

            shopView.Bind(presenter);
        }
    }
}
