using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public enum IAP_ID { no_ads, diamond_100, diamond_300, diamond_500, diamond_1k1, diamond_2k5, diamond_5k, diamond_10k }

namespace DarkcupGames
{
    public class ShopIAPManager : MonoBehaviour
    {
        public static ShopIAPManager Instance;
        public static MyIAPManager iap;
        public Transform clickedButton;
        [SerializeField] private TextMeshProUGUI diamondTxt;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (iap == null)
            {
                Init();
            }
        }

        private void OnEnable()
        {
            var userData = GameSystem.userdata;
            diamondTxt.text = userData.diamond.ToString();
        }

        public void Init()
        {
            iap = new MyIAPManager();
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            var ids = (IAP_ID[])Enum.GetValues(typeof(IAP_ID));
            foreach ( var id in ids)
            {
                if (id == IAP_ID.no_ads) builder.AddProduct(id.ToString(), ProductType.NonConsumable);
                else builder.AddProduct(id.ToString(), ProductType.Consumable);
            }
            UnityPurchasing.Initialize(iap, builder);
        }

        public bool IsInitDone()
        {
            if (iap == null) return false;
            if (iap.initSuccess == false) return false;
            if (iap.prices == null) return false;
            return true;
        }

        public void BuyProduct(string productId, Action onComplete)
        {
            MyIAPManager.currentBuySKU = productId;
            iap.OnPurchaseClicked(productId, onComplete);
        }

        public void OnBuyComlete(string sku)
        {
            if (GameSystem.userdata.boughtItems == null)
            {
                GameSystem.userdata.boughtItems.Add(sku);
                GameSystem.SaveUserDataToLocal();
            }
        }

        public void BuyNoAdsPackage()
        {
            if (IsInitDone() == false)
            {
                return;
            }
            string id = IAP_ID.no_ads.ToString();

            if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
            bool boughNoAds = GameSystem.userdata.boughtItems.Contains(id);
            if (boughNoAds) return;
            iap.OnPurchaseClicked(id, () =>
            {
                if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
                if (GameSystem.userdata.boughtItems.Contains(id) == false)
                {
                    GameSystem.userdata.boughtItems.Add(id);
                    GameSystem.SaveUserDataToLocal();
                }
                string currentScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentScene);
            });
        }

        public void BuyDiamond(IAP_ID id)
        {
            if (IsInitDone() == false)
            {
                return;
            }
            var amount = id switch
            {
                IAP_ID.diamond_100 =>  100,
                IAP_ID.diamond_300 =>  300,
                IAP_ID.diamond_500 =>  500,
                IAP_ID.diamond_1k1 =>  1100,
                IAP_ID.diamond_2k5 =>  2500,
                IAP_ID.diamond_5k =>  5000,
                IAP_ID.diamond_10k =>  10000,
                _=> 0
            };

            if(amount == 0)
            {
                Debug.LogError("Wrong id");
                return;
            }

            iap.OnPurchaseClicked(id.ToString(), () =>
            {
                GameSystem.userdata.diamond += amount;
                GameSystem.SaveUserDataToLocal();
                UIManager.Instance.SpawnEffectReward(clickedButton);
            });
        }
    }
}