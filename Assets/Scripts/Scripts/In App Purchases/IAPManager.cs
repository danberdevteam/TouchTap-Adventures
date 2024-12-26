using System.Collections;
using System.Collections.Generic;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;
    private static Product test_product = null;
    [SerializeField] private TextMeshProUGUI restoreMessageText;

    public const string DASHBOARD = "dashboard";
    public const string LEADERBOARD = "leaderboard";
    public static string SUB1 = "subscription1";

    private static TMP_Text myText;

    private bool return_complete = true;

    private Button dashboardButton;
    private Button leaderboardButton;
    private Button catBuyButton;
    public delegate void PurchaseSuccessCallback();
    public static event PurchaseSuccessCallback OnDashboardPurchaseSuccess;
    public static event PurchaseSuccessCallback OnLeaderboardPurchaseSuccess;
    public static event PurchaseSuccessCallback OnCatPurchaseSuccess;

    public string catIDnumber;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            InitializePurchasing();
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void Start()
    {

        // If we haven't set up the Unity Purchasing reference
        if (storeController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    ConfigurationBuilder builder;
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(DASHBOARD, ProductType.NonConsumable);
        builder.AddProduct(LEADERBOARD, ProductType.NonConsumable);

        for (int i = 0; i < 11; i++)
        {
            builder.AddProduct("skin" + i, ProductType.NonConsumable);
            print("skin" + i);
        }

        // builder.AddProduct(SUB1, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    // public void AddToBuilder(string buildId)
    // {
    //     builder.AddProduct(buildId, ProductType.NonConsumable);
    // }


    private bool IsInitialized()
    {

        return storeController != null && storeExtensionProvider != null;

    }

    public void BuyDashboard(Button lockedButton)
    {
        dashboardButton = lockedButton; // Store the reference to the button

        ParentalGate.Instance.ShowParentalGate(() =>
    {
        BuyProductID(DASHBOARD); // Proceed with purchase if access is granted
    });
    }

    public void BuyLeaderboard(Button lockedButton)
    {
        print("Buy leaderboard function called");
        leaderboardButton = lockedButton; // Store the reference to the button

        ParentalGate.Instance.ShowParentalGate(() =>
    {
        BuyProductID(LEADERBOARD); // Proceed with purchase if access is granted
    });
    }

    public void BuyCharacter(Button lockedButton, string catIDNum)
    {
        catBuyButton = lockedButton;
        catIDnumber = catIDNum;

        // Reset the parental gate
        ParentalGate.Instance.ResetParentalGate();

        ParentalGate.Instance.ShowParentalGate(() =>
    {
        BuyProductID(catIDNum); // Proceed with purchase if access is granted
    });
    }

    public void UnlockButton(Button buttonToUnLock)
    {

        buttonToUnLock.GetComponent<CharacterBuyButton>().removePrice();
        buttonToUnLock.onClick.RemoveAllListeners();
    }
    public void UnlockButtonSkinIAP(Button buttonToUnLock)
    {
        print("skin unlock called" + buttonToUnLock.GetComponentsInChildren<Image>().Length);
        buttonToUnLock.GetComponent<CharacterBuyButton>().removePrice();
        // buttonToUnLock.GetComponentsInChildren<TMP_Text>()[0].gameObject.SetActive(false);
        buttonToUnLock.onClick.RemoveAllListeners();
    }

    // public void BuyNoAds()
    // {
    //     BuyProductID(NO_ADS);
    // }

    public void CompletePurchase()
    {
        if (test_product == null)
            MyDebug("Cannot complete purchase, product not initialized.");
        else
        {
            storeController.ConfirmPendingPurchase(test_product);
            MyDebug("Completed purchase with " + test_product.transactionID.ToString());
        }

    }

    public void ToggleComplete()
    {
        return_complete = !return_complete;
        MyDebug("Complete = " + return_complete.ToString());

    }


    bool BuyProductID(string productId)
    {
        print("Product to be bought " + productId);
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);
            print("product ID : " + product.transactionID);

            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                storeController.InitiatePurchase(product);
                return true;
            }
            else
            {
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                return false;
            }
        }
        else
        {
            MyDebug("BuyProductID FAIL. Not initialized.");
            return false;
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        storeController = controller;
        storeExtensionProvider = extensions;

        CheckPurchasedItems();
    }

    private void CheckPurchasedItems()
    {
        if (storeController == null) return;

        foreach (var product in storeController.products.all)
        {
            if (product.hasReceipt)
            {
                Debug.Log($"Product already purchased: {product.definition.id}");
                // Save purchase status locally
                PlayerPrefs.SetInt(product.definition.id, 1); // 1 = Purchased
            }
            else
            {
                Debug.Log($"Product not purchased: {product.definition.id}");
                PlayerPrefs.SetInt(product.definition.id, 0); // 0 = Not purchased
            }
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product purchasedProduct = args.purchasedProduct;
        string purchasedProductId = purchasedProduct.definition.id;

        print(purchasedProductId + catIDnumber + "Cats number");

        if (purchasedProductId == DASHBOARD)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            SavePurchaseToPlayFab(DASHBOARD);
            DisplayProductMetadata(purchasedProduct);
            if (dashboardButton != null)
            {
                UnlockButton(dashboardButton);
            }
            OnDashboardPurchaseSuccess?.Invoke();
        }
        else if (purchasedProductId == LEADERBOARD)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            SavePurchaseToPlayFab(LEADERBOARD);
            DisplayProductMetadata(purchasedProduct);
            if (leaderboardButton != null)
            {
                UnlockButton(leaderboardButton);
            }
            OnLeaderboardPurchaseSuccess?.Invoke();
        }
        else if (purchasedProductId == catIDnumber)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            SavePurchaseToPlayFab(catIDnumber);
            DisplayProductMetadata(purchasedProduct);
            if (catBuyButton != null)
            {
                UnlockButtonSkinIAP(catBuyButton);
            }
            OnCatPurchaseSuccess?.Invoke();
        }
        else
        {
            Debug.Log($"Unknown product purchased: {purchasedProductId}");
        }

        return PurchaseProcessingResult.Complete;
    }

    private void DisplayProductMetadata(Product product)
    {
        if (product != null)
        {
            Debug.Log($"Product Title: {product.metadata.localizedTitle}");
            Debug.Log($"Product Description: {product.metadata.localizedDescription}");
            Debug.Log($"Product Price: {product.metadata.localizedPriceString}");
            Debug.Log($"Currency Code: {product.metadata.isoCurrencyCode}");
            Debug.Log($"Transaction ID: {product.transactionID}");
        }
        else
        {
            Debug.Log("Product metadata not available.");
        }
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void MyDebug(string debug)
    {

        Debug.Log(debug);
        // myText.text += "\r\n" + debug;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void RestorePurchases()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.isEditor)
        {
            if (storeExtensionProvider == null && !Application.isEditor)
            {
                SaveRestoreMessage("RestorePurchases failed: Unity IAP not initialized.");
                Debug.LogError("RestorePurchases failed: Unity IAP not initialized.");
                return;
            }

            // Check if there are previous purchases
            bool hasPurchases = false;
            foreach (var product in storeController.products.all)
            {
                if (product.hasReceipt)
                {
                    hasPurchases = true;
                    break;
                }
            }

            if (!hasPurchases)
            {
                string warning = "No previous purchases found. Restore not needed.";
                SaveRestoreMessage(warning);
                DisplayRestoreMessage(restoreMessageText);
                Debug.LogWarning(warning);
                return;
            }

            Debug.Log("Restore purchases started...");
            SaveRestoreMessage("Restore purchases in progress...");

            if (Application.isEditor)
            {
                // Simulate successful restore in the editor
                StartCoroutine(SimulateEditorRestore());
                return;
            }

            storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((success, error) =>
            {
                if (success)
                {
                    Debug.Log("Restore Purchases completed successfully.");
                    SaveRestoreMessage("Restore Purchases completed successfully.");
                    DisplayRestoreMessage(restoreMessageText);

                    // Sync restored purchases with the current logged-in user
                    foreach (var product in storeController.products.all)
                    {
                        if (product.hasReceipt)
                        {
                            ValidateAndRestoreProduct(product);
                        }
                        else
                        {
                            Debug.Log($"Product not purchased or no receipt: {product.definition.id}");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Restore Purchases failed: {error}");
                    SaveRestoreMessage($"Restore Purchases failed: {error}");
                    DisplayRestoreMessage(restoreMessageText);
                }
            });
        }
        else
        {
            string warning = "Restore Purchases is not available on this platform.";
            SaveRestoreMessage(warning);
            DisplayRestoreMessage(restoreMessageText);
            Debug.LogWarning(warning);
        }
    }


    private void ValidateAndRestoreProduct(Product product)
    {
        string productId = product.definition.id;

        // Retrieve the current logged-in user's data
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(),
        result =>
        {
            if (result.Data != null && result.Data.ContainsKey(productId))
            {
                Debug.Log($"Restoring product for the current user: {productId}");
                SavePurchaseToPlayFab(productId); // Sync with PlayFab or local storage
            }
            else
            {
                Debug.LogWarning($"Product {productId} is not associated with the current logged-in user. Skipping restoration.");
            }
        },
        error =>
        {
            Debug.LogError($"Failed to validate user data for product {productId}: {error.ErrorMessage}");
        });
    }


    // Simulate Restore Purchases in the Unity Editor
    private IEnumerator SimulateEditorRestore()
    {
        yield return new WaitForSeconds(2); // Simulate processing delay
        Debug.Log("Simulated Restore Purchases completed successfully.");
        SaveRestoreMessage("Simulated Restore Purchases completed successfully.");
        DisplayRestoreMessage(restoreMessageText);
    }


    // Save the restore message in PlayerPrefs
    private void SaveRestoreMessage(string message)
    {
        PlayerPrefs.SetString("RestoreMessage", message);
        PlayerPrefs.Save();
    }

    // Retrieve and display the restore message
    public void DisplayRestoreMessage(TextMeshProUGUI restoreMessageText)
    {
        if (PlayerPrefs.HasKey("RestoreMessage"))
        {
            string message = PlayerPrefs.GetString("RestoreMessage");
            restoreMessageText.text = message;
            Debug.Log("Displayed Restore Message: " + message);
        }
        else
        {
            restoreMessageText.text = "No restore messages available.";
            Debug.Log("No restore messages found.");
        }
    }

    private void SavePurchaseToPlayFab(string productId)
    {
        var data = new Dictionary<string, string>
    {
        { productId, "purchased" }
    };

        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = data
        },
        result => Debug.Log($"Successfully saved purchase state to PlayFab for {productId}."),
        error => Debug.LogError($"Failed to save purchase state to PlayFab: {error.ErrorMessage}"));
    }

    public void ClearRestoreMessage()
    {
        if (restoreMessageText != null)
        {
            restoreMessageText.text = string.Empty;
            Debug.Log("Restore message cleared.");
        }
    }

}
