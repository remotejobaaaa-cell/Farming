using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class PurchaserManager : MonoBehaviour, IDetailedStoreListener
{
    const string k_Environment = "production";
    private IStoreController m_StoreController; // The Unity Purchasing system.


    public static PurchaserManager instance = null;

    public static string noAdsProduct = "noads";
    public static string unlockAllLevelsProduct = "unlockalllevels";
    public static string unlockAllFarmersProduct = "unlockallfarmers";
    public static string unlockAllGameProduct = "unlockallgame";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        Initialize(OnSuccess, OnError);
    }
    void Start()
    {
        // Check if Unity Gaming Services initialized successfully
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            var text =
                "Error: Unity Gaming Services not initialized.\n" +
                "To initialize Unity Gaming Services, open the file \"GameServicesManager.cs\" " +
                "and ensure the Initialize method is called in the \"Awake\" method.";
            //informationText.text = text;
            Debug.LogError(text);
        }

        // Initialize Unity Purchasing
        InitializePurchasing();
    }
    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);
            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }

    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        // informationText.text = text;
        Debug.Log(text);
    }
    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        // informationText.text = text;
        Debug.LogError(text);
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        builder.AddProduct(noAdsProduct, ProductType.NonConsumable);
        builder.AddProduct(unlockAllFarmersProduct, ProductType.NonConsumable);
        builder.AddProduct(unlockAllLevelsProduct, ProductType.NonConsumable);
        builder.AddProduct(unlockAllGameProduct, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void PurchaseNoAds()
    {
        m_StoreController.InitiatePurchase(noAdsProduct);
    }

    public void PurchaseAllLevels()
    {
        m_StoreController.InitiatePurchase(unlockAllLevelsProduct);
    }

    public void PurchasAllFarmer()
    {
        m_StoreController.InitiatePurchase(unlockAllFarmersProduct);
    }

    public void PurchasAllGame()
    {
        m_StoreController.InitiatePurchase(unlockAllGameProduct);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        MainMenuManager menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {  //Add the purchased product to the players inventory
            if (product.definition.id == noAdsProduct)
            {
#if UNITY_EDITOR
                Debug.Log("No Ads Purchased successfully");
#endif
                menuManager.playerData.noAds = 3;
                menuManager.topBarMenu.Initilize();
                PlayerDataController.instance.Save();
            }
            else if (product.definition.id == unlockAllFarmersProduct)
            {
#if UNITY_EDITOR
                Debug.Log("All Farmers Purchased successfully");
#endif
                menuManager.playerData.unlockAllFarmers = 3;
                menuManager.ownVehicleMenu.InitializeFarmer();
                PlayerDataController.instance.Save();
            }
            else if (product.definition.id == unlockAllLevelsProduct)
            {
#if UNITY_EDITOR
                Debug.Log("All Levels Purchased successfully");
#endif
                menuManager.playerData.unlockAllLevels = 3;
                menuManager.levelSelectionMenuManager.Initialize();
                PlayerDataController.instance.Save();
            }
            else if (product.definition.id == unlockAllGameProduct)
            {
#if UNITY_EDITOR
                Debug.Log("All Game Purchased successfully");
#endif
                menuManager.playerData.unlockAllGame = 3;
                menuManager.playerData.noAds = 3;
                menuManager.topBarMenu.Initilize();

                menuManager.playerData.unlockAllLevels = 3;
                menuManager.levelSelectionMenuManager.Initialize();

                menuManager.playerData.unlockAllFarmers = 3;
                menuManager.ownVehicleMenu.InitializeFarmer();
                PlayerDataController.instance.Save();
            }
        }
        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }

}
