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
        Debug.Log($"Verificando si la skin ya está comprada: {skinName}");

        bool isHeadSkin = skinName.ToLower().Contains("cabeza");
        bool isBodySkin = skinName.ToLower().Contains("cuerpo");

        if (isHeadSkin)
        {
            Debug.Log($"Es una skin de cabeza: {skinName}");
            if (IsHeadSkinOwned(skinName))
            {
                purchaseButton.interactable = false;
                Debug.Log($"Botón deshabilitado: El jugador ya tiene la cabeza {skinName}");
            }
        }

        if (isBodySkin)
        {
            Debug.Log($"Es una skin de cuerpo: {skinName}");
            if (IsBodySkinOwned(skinName))
            {
                purchaseButton.interactable = false;
                Debug.Log($"Botón deshabilitado: El jugador ya tiene el cuerpo {skinName}");
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
                isPurchased = true;
                purchasedSkins.Add(new SkinData(skinName, skinImage));
                purchaseButton.interactable = false;
                Debug.Log($"Skin {skinName} comprada y agregada al inventario.");

                PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
                if (player != null)
                {
                    int skinIndex = GetSkinIndex(skinName);
                    if (skinIndex >= 0)
                    {
                        if (skinName.ToLower().Contains("cabeza"))
                        {
                            Debug.Log($"Aplicando skin comprada a la cabeza: {skinName}");
                            player.ChangeHeadSkin(skinIndex);
                        }
                        else if (skinName.ToLower().Contains("cuerpo"))
                        {
                            Debug.Log($"Aplicando skin comprada al cuerpo: {skinName}");
                            player.ChangeBodySkin(skinIndex);
                        }
                    }
                }

                // Actualizar el inventario si está abierto
                InventoryManager inventory = FindObjectOfType<InventoryManager>();
                if (inventory != null && inventory.inventoryPanel.activeSelf)
                {
                    inventory.SelectSkin(skinName);
                }
            }
            else
            {
                Debug.Log($"Compra bloqueada: El jugador ya tiene {skinName}");
                purchaseButton.interactable = false;
            }
        }
    }

    private bool IsHeadSkinOwned(string skinName)
    {
        string normalizedName = skinName.ToLower().Trim();
        return purchasedSkins.Exists(skin => skin.name.ToLower().Trim() == normalizedName && skin.name.ToLower().Contains("cabeza"));
    }

    private bool IsBodySkinOwned(string skinName)
    {
        string normalizedName = skinName.ToLower().Trim();
        return purchasedSkins.Exists(skin => skin.name.ToLower().Trim() == normalizedName && skin.name.ToLower().Contains("cuerpo"));
    }

    private int GetSkinIndex(string skinName)
    {
        Dictionary<string, int> skinMapping = new Dictionary<string, int>
        {
            { "cabeza blanca", 0 }, { "cabeza amarilla", 1 }, { "cabeza negra", 2 },
            { "cuerpo blanco", 0 }, { "cuerpo amarillo", 1 }, { "cuerpo negro", 2 },
            { "azul", 3 }, { "rojo", 4 }, { "rosa", 5 }, { "verde", 6 }
        };

        string normalizedName = skinName.ToLower().Trim();

        // Comprobar directamente el nombre normalizado
        if (skinMapping.ContainsKey(normalizedName))
        {
            return skinMapping[normalizedName];
        }

        // Si no se encuentra, verificar si es solo un color (sin prefijo "cuerpo" o "cabeza")
        string[] colorNames = { "azul", "rojo", "rosa", "verde" };
        if (System.Array.IndexOf(colorNames, normalizedName) >= 0)
        {
            // Para estos colores, el índice es su posición + 3
            return System.Array.IndexOf(colorNames, normalizedName) + 3;
        }

        Debug.LogError($"Skin no encontrada: {skinName}");
        return -1;
    }

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