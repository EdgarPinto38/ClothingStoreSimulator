using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SkinSellPanel : MonoBehaviour
{
    public Transform itemsContainer;
    public GameObject itemPrefab;

    [Header("Configuración de venta")]
    [Tooltip("Porcentaje del precio original que se recupera al vender (0.5 = 50%)")]
    [Range(0.1f, 1.0f)]
    public float sellValueMultiplier = 0.5f;

    public void OpenSellPanel()
    {
        gameObject.SetActive(true);
        LoadSellableSkins();
    }

    public void LoadSellableSkins()
    {
        // Limpiar los items existentes
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player == null)
        {
            Debug.LogError("❌ No se encontró el PlayerRaycast");
            return;
        }

        // Cargar solo las skins que no están equipadas
        foreach (var skin in SkinPurchase.purchasedSkins)
        {
            string skinName = skin.name;
            bool isHeadEquipped = player.IsHeadSkinEquipped(skinName);
            bool isBodyEquipped = player.IsBodySkinEquipped(skinName);

            if (!isHeadEquipped && !isBodyEquipped)
            {
                GameObject newItem = Instantiate(itemPrefab, itemsContainer);

                // Configurar nombre y sprite
                TextMeshProUGUI nameText = newItem.transform.Find("SkinName")?.GetComponent<TextMeshProUGUI>();
                Image imageComponent = newItem.transform.Find("SkinImage")?.GetComponent<Image>();

                if (nameText != null) nameText.text = skinName;
                if (imageComponent != null && skin.image != null) imageComponent.sprite = skin.image;

                // Configurar precio de venta
                TextMeshProUGUI priceText = newItem.transform.Find("SellPrice")?.GetComponent<TextMeshProUGUI>();
                if (priceText != null)
                {
                    int sellValue = Mathf.RoundToInt(skin.price * sellValueMultiplier);
                    priceText.text = sellValue.ToString() + " 🪙";
                }

                // Configurar botón de venta
                Button button = newItem.GetComponent<Button>();
                if (button != null)
                {
                    // Mantener una referencia al objeto skin actual para evitar problemas de closure
                    SkinPurchase.SkinData currentSkin = skin;
                    button.onClick.AddListener(() => SellSkin(currentSkin));
                }
            }
        }
    }

    private void SellSkin(SkinPurchase.SkinData skinData)
    {
        if (skinData == null)
        {
            Debug.LogError("❌ Intentando vender una skin nula");
            return;
        }

        // Calcular el valor de venta
        int sellValue = 100;

        Debug.Log($"🗑️ Vendiendo skin: {skinData.name} por {sellValue} monedas");

        // Añadir monedas al jugador
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(sellValue);
        }

        // Eliminar del inventario
        SkinPurchase.purchasedSkins.Remove(skinData);

        // Reactivar botón en la tienda
        ReactivateShopButton(skinData.name);

        // Refrescar la interfaz
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        if (inventory != null)
        {
            inventory.ForceRefreshInventory();
        }

        // Recargar el panel de venta
        LoadSellableSkins();
    }

    private void ReactivateShopButton(string skinName)
    {
        Debug.Log($"🔄 Intentando reactivar botón para: {skinName}");

        // Obtener todos los botones de compra en la escena
        SkinPurchase[] allPurchaseButtons = FindObjectsOfType<SkinPurchase>(true); // El true busca incluso objetos inactivos
        bool buttonFound = false;

        // Normalizar el nombre para comparación
        string normalizedName = skinName.ToLower().Trim();

        foreach (var purchaseButton in allPurchaseButtons)
        {
            string buttonSkinName = purchaseButton.skinName.ToLower().Trim();

            if (buttonSkinName == normalizedName)
            {
                buttonFound = true;
                purchaseButton.isPurchased = false;
                purchaseButton.purchaseButton.interactable = true;
                purchaseButton.UpdatePriceDisplay(); // Actualizar el texto del precio
                Debug.Log($"✅ Botón reactivado: {purchaseButton.skinName}");
            }
        }

        if (!buttonFound)
        {
            Debug.LogWarning($"⚠️ No se encontró ningún botón con el nombre: {skinName}");
        }
    }

    public void CloseSellPanel()
    {
        gameObject.SetActive(false);
    }
}