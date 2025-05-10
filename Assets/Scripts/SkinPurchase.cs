using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkinPurchase : MonoBehaviour
{
    public Button purchaseButton;
    public bool isPurchased = false;
    public string skinName;
    public Sprite skinImage;

    public static List<SkinData> purchasedSkins = new List<SkinData>();

    void Start()
    {
        CheckIfPurchased();
    }

    void OnEnable()
    {
        // Asegurarse de que el estado del botón sea correcto al activarse
        CheckIfPurchased();
    }

    public void PurchaseSkin()
    {
        if (!isPurchased)
        {
            bool isHeadOwned = IsHeadSkinOwned(skinName);
            bool isBodyOwned = IsBodySkinOwned(skinName);

            if (!isHeadOwned && !isBodyOwned)
            {
                Debug.Log($"💰 Comprando skin: {skinName}");
                isPurchased = true;
                purchasedSkins.Add(new SkinData(skinName, skinImage));
                purchaseButton.interactable = false;

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

        Debug.Log($"🔁 Botón reactivado: {skinName}");
    }

    public void ForceUpdateButton()
    {
        CheckIfPurchased();
        Debug.Log($"🔄 Botón actualizado: {skinName}");
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

        public SkinData(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }
    }
}