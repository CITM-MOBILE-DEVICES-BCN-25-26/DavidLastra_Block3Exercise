using System.Collections.Generic;
using CleanRefactor.Application;
using CleanRefactor.Domain;
using CleanRefactor.Presentation;
using UnityEngine;

namespace CleanRefactor.Composition
{
    /// <summary>
    /// COMPOSITION ROOT (manual dependency injection).
    ///
    /// This is the ONLY place where the concrete classes of every layer are
    /// constructed and wired together. No DI framework (Zenject / VContainer) is
    /// needed — a manual bootstrap is enough.
    ///
    /// It is also the ONLY place allowed to read Unity [SerializeField] values
    /// and turn them into the plain-C# ShopConfig. This satisfies the rule:
    /// "the domain/application logic should not depend on Unity serialization."
    ///
    /// Dependency direction (all arrows point INWARDS, towards the domain):
    ///   Bootstrap -> View -> Presenter -> UseCases -> Domain
    ///                                  \-> Repository (Infrastructure)
    /// </summary>
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
            // 1. Build the plain-C# configuration from the serialized fields.
            ShopConfig config = new ShopConfig(
                bomb:        new ShopItemConfig(bombCost, bombMaxUses),
                shield:      new ShopItemConfig(shieldCost, shieldMaxUses),
                doubleCoins: new ShopItemConfig(doubleCoinsCost, maxUses: 1,
                                                requiredLevel: doubleCoinsRequiredLevel));

            // 2. Infrastructure: the concrete persistence implementation.
            //    Using a JSON file on disk. Thanks to the IPlayerRepository
            //    abstraction this could be swapped for PlayerPrefs, a cloud
            //    save, etc. without touching domain/application code.
            var repository = new Infrastructure.JsonFilePlayerRepository(
                defaultCoins, defaultLevel);

            // 2b. Debug: optionally wipe saved data so the shop starts fresh.
            //     Reset() goes through the IPlayerRepository abstraction, so the
            //     Bootstrap never touches PlayerPrefs directly.
            if (resetToDefaultOnStart)
            {
                repository.Reset();
                Debug.Log("[ShopBootstrap] Debug reset: shop data restored to defaults.");
            }

            // 3. Domain items + catalog (Open/Closed: register new items here).
            var catalog = new ShopCatalog(new List<IShopItem>
            {
                new BombItem(config.Bomb),
                new ShieldItem(config.Shield),
                new DoubleCoinsItem(config.DoubleCoins)
            });

            // 4. Application use cases.
            var purchaseItem  = new PurchaseItemUseCase(catalog, repository);
            var getShopStatus = new GetShopStatusUseCase(catalog, repository);

            // 5. Presentation.
            var feedback  = new PurchaseFeedbackPresenter();
            var presenter = new ShopPresenter(
                shopView, purchaseItem, getShopStatus, feedback, purchaseAudio);

            // 6. Inject the presenter into the passive View.
            shopView.Bind(presenter);
        }
    }
}
