using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkinPurchase : MonoBehaviour
{
    public Button purchaseButton;
    public bool isPurchased = false;
    public string skinName;
    public Sprite skinImage;
    public int price = 100; // Precio predeterminado
    public TextMeshProUGUI priceText; // Texto que muestra el precio

    public static List<SkinData> purchasedSkins = new List<SkinData>();

    void Start()
    {
        CheckIfPurchased();
        UpdatePriceDisplay();
    }

    void OnEnable()
    {
        // Asegurarse de que el estado del botón sea correcto al activarse
        CheckIfPurchased();
        UpdatePriceDisplay();
    }

    public void UpdatePriceDisplay()
    {
        if (priceText != null)
        {
            if (isPurchased)
            {
                priceText.text = "COMPRADO";
            }
            else
            {
                priceText.text = price.ToString() + " 🪙";
            }
        }
    }

    public void PurchaseSkin()
    {
        if (!isPurchased)
        {
            bool isHeadOwned = IsHeadSkinOwned(skinName);
            bool isBodyOwned = IsBodySkinOwned(skinName);

            if (!isHeadOwned && !isBodyOwned)
            {
                // Verificar si tiene suficientes monedas
                if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCoins(price))
                {
                    Debug.Log($"💰 Comprando skin: {skinName} por {price} monedas");
                    isPurchased = true;
                    purchasedSkins.Add(new SkinData(skinName, skinImage, price));
                    purchaseButton.interactable = false;
                    UpdatePriceDisplay();

                    PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
                    if (player != null)
                    {
                        int skinIndex = GetSkinIndex(skinName);
                        if (skinIndex >= 0)
                        {
                            if (skinName.ToLower().Contains("cabeza"))
                            {
                                player.ChangeHeadSkin(skinIndex);
                            }
                            else if (skinName.ToLower().Contains("cuerpo") ||
                                     skinName.ToLower() == "azul" ||
                                     skinName.ToLower() == "rojo" ||
                                     skinName.ToLower() == "rosa" ||
                                     skinName.ToLower() == "verde")
                            {
                                player.ChangeBodySkin(skinIndex);
                            }
                        }
                    }

                    // Actualizar el inventario si está abierto
                    InventoryManager inventory = FindObjectOfType<InventoryManager>();
                    if (inventory != null)
                    {
                        inventory.ForceRefreshInventory();
                    }
                }
                else
                {
                    // Mostrar mensaje de error - no hay suficientes monedas
                    Debug.Log($"❌ No tienes suficientes monedas para comprar {skinName}. Necesitas: {price}");
                }
            }
        }
    }

    public void CheckIfPurchased()
    {
        bool isSkinOwned = IsHeadSkinOwned(skinName) || IsBodySkinOwned(skinName);
        isPurchased = isSkinOwned;

        if (purchaseButton != null)
        {
            purchaseButton.interactable = !isSkinOwned;
        }

        UpdatePriceDisplay();
    }

    public bool IsHeadSkinOwned(string skinName)
    {
        string normalizedName = skinName.ToLower().Trim();
        return purchasedSkins.Exists(s => s.name.ToLower().Trim() == normalizedName &&
                                         s.name.ToLower().Contains("cabeza"));
    }

    public bool IsBodySkinOwned(string skinName)
    {
        string normalizedName = skinName.ToLower().Trim();
        return purchasedSkins.Exists(s => s.name.ToLower().Trim() == normalizedName &&
                                         (s.name.ToLower().Contains("cuerpo") ||
                                          s.name.ToLower() == "azul" ||
                                          s.name.ToLower() == "rojo" ||
                                          s.name.ToLower() == "rosa" ||
                                          s.name.ToLower() == "verde"));
    }

    public void ReactivateSkin()
    {
        isPurchased = false;

        if (purchaseButton != null)
        {
            purchaseButton.interactable = true;
        }

        UpdatePriceDisplay();
        Debug.Log($"🔁 Botón reactivado: {skinName}");
    }

    public int GetSkinIndex(string skinName)
    {
        Dictionary<string, int> mapping = new Dictionary<string, int>
        {
            {"cabeza blanca", 0}, {"cabeza amarilla", 1}, {"cabeza negra", 2},
            {"cuerpo blanco", 0}, {"cuerpo amarillo", 1}, {"cuerpo negro", 2},
            {"azul", 3}, {"rojo", 4}, {"rosa", 5}, {"verde", 6}
        };

        string key = skinName.ToLower().Trim();

        if (mapping.ContainsKey(key)) return mapping[key];

        string[] colors = { "azul", "rojo", "rosa", "verde" };
        for (int i = 0; i < colors.Length; i++)
        {
            if (key == colors[i]) return i + 3;
        }

        Debug.LogError($"❌ Skin no encontrada: {skinName}");
        return -1;
    }

    [System.Serializable]
    public class SkinData
    {
        public string name;
        public Sprite image;
        public int price;

        public SkinData(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
            this.price = 50; // Precio predeterminado
        }

        public SkinData(string name, Sprite image, int price)
        {
            this.name = name;
            this.image = image;
            this.price = price;
        }
    }
}