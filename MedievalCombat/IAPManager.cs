using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance { set; get; }

    private static IStoreController m_StoreController; 
    private static IExtensionProvider m_StoreExtensionProvider; 
    
    public static string money_250 = "money250";
    public static string money_500 = "money500";
    public static string money_1000 = "money1000";
    public static string money_2000 = "money2000";

    public static string openMap_1 = "openmap1";
    public static string openMap_2 = "openmap2";
    public static string openMap_3 = "openmap3";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(money_250, ProductType.Consumable);
        builder.AddProduct(money_500, ProductType.Consumable);
        builder.AddProduct(money_1000, ProductType.Consumable);
        builder.AddProduct(money_2000, ProductType.Consumable);

        builder.AddProduct(openMap_1, ProductType.Consumable);
        builder.AddProduct(openMap_2, ProductType.Consumable);
        builder.AddProduct(openMap_3, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void Buy_250_Money()
    {
        BuyProductID(money_250);
    }

    public void Buy_500_Money()
    {
        BuyProductID(money_500);
    }

    public void Buy_1000_Money()
    {
        BuyProductID(money_1000);
    }

    public void Buy_2000_Money()
    {
        BuyProductID(money_2000);
    }
    
    public void BuyMap_1()
    {
        BuyProductID(openMap_1);
    }

    public void BuyMap_2()
    {
        BuyProductID(openMap_2);
    }

    public void BuyMap_3()
    {
        BuyProductID(openMap_3);
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        if (String.Equals(args.purchasedProduct.definition.id, money_250, StringComparison.Ordinal))
        {
            GameController.Instance.player_total_money += 250;
            SaveSystem.SavePlayer();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, money_500, StringComparison.Ordinal))
        {
            GameController.Instance.player_total_money += 500;
            SaveSystem.SavePlayer();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, money_1000, StringComparison.Ordinal))
        {
            GameController.Instance.player_total_money += 1000;
            SaveSystem.SavePlayer();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, money_2000, StringComparison.Ordinal))
        {
            GameController.Instance.player_total_money += 2000;
            SaveSystem.SavePlayer();
        }
        
        else if (String.Equals(args.purchasedProduct.definition.id, openMap_1, StringComparison.Ordinal))
        {
            GameController.Instance.OpenableMapIndex = 1;
            SaveSystem.SavePlayer();
            GameController.Instance.restartScene = true;
        }

        else if (String.Equals(args.purchasedProduct.definition.id, openMap_2, StringComparison.Ordinal))
        {
            GameController.Instance.OpenableMapIndex = 2;
            SaveSystem.SavePlayer();
            GameController.Instance.restartScene = true;
        }

        else if (String.Equals(args.purchasedProduct.definition.id, openMap_3, StringComparison.Ordinal))
        {
            GameController.Instance.OpenableMapIndex = 3;
            SaveSystem.SavePlayer();
            GameController.Instance.restartScene = true;
        }

        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}