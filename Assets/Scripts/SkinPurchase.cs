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
            Debug.Log($"Ejecutando IsBodySkinOwned({skinName})...");
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
            // 🔥 Verifica si la skin ya está en el inventario antes de comprarla
            bool isHeadOwned = IsHeadSkinOwned(skinName);
            bool isBodyOwned = IsBodySkinOwned(skinName);

            if (!isHeadOwned && !isBodyOwned)  
            {
                isPurchased = true;
                purchasedSkins.Add(new SkinData(skinName, skinImage));
                purchaseButton.interactable = false;
                Debug.Log($"✅ Skin {skinName} comprada y agregada al inventario.");

                PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
                if (player != null)
                {
                    int headSkinIndex = GetSkinIndex(skinName);
                    int bodySkinIndex = GetBodySkinIndex(skinName);

                    if (headSkinIndex >= 0)
                    {
                        Debug.Log($"🔥 Aplicando skin comprada a la cabeza: {skinName}");
                        player.ChangeHeadSkin(headSkinIndex);
                    }
                    else if (bodySkinIndex >= 0)
                    {
                        Debug.Log($"🔥 Aplicando skin comprada al cuerpo: {skinName}");
                        player.ChangeBodySkin(bodySkinIndex);
                    }
                }
            }
            else
            {
                Debug.Log($"🚫 Compra bloqueada: El jugador ya tiene {skinName}");
                purchaseButton.interactable = false; // 🔥 Asegurar que el botón se deshabilite si ya tiene la skin
            }
        }
    }

    // 🔥 Método para verificar si la skin ya está en el inventario
    private bool IsHeadSkinOwned(string skinName)
    {
        foreach (SkinData skin in purchasedSkins)
        {
            if (skin.name.ToLower().Contains("cabeza") && skin.name.ToLower().Trim() == skinName.ToLower().Trim())
            {
                Debug.Log($"🚫 El jugador ya tiene esta cabeza: {skinName}");
                return true;
            }
        }
        return false;
    }

    private bool IsBodySkinOwned(string skinName)
    {
        Debug.Log($"🔍 Buscando skin de cuerpo en el inventario: {skinName}");

        foreach (SkinData skin in purchasedSkins)
        {
            Debug.Log($"🧐 Comparando con: {skin.name}");

            if (skin.name.ToLower().Contains("cuerpo") && skin.name.ToLower().Trim() == skinName.ToLower().Trim())
            {
                Debug.Log($"🚫 El jugador ya tiene este cuerpo: {skinName}");
                return true;
            }
        }

        Debug.Log($"❌ No se encontró la skin de cuerpo en el inventario: {skinName}");
        return false;
    }

    private int GetSkinIndex(string skinName)
    {
        switch (skinName.ToLower())
        {
            case "cabeza blanca": return 0;
            case "cabeza amarilla": return 1;
            case "cabeza negra": return 2;
            default:
               
                return -1;
        }
    }

    private int GetBodySkinIndex(string skinName)
    {
        switch (skinName.ToLower())
        {
            case "cuerpo blanco": return 0;
            case "cuerpo amarillo": return 1;
            case "cuerpo negro": return 2;
            case "cuerpo azul": return 3;
            case "cuerpo rojo": return 4;
            case "cuerpo rosa": return 5;
            case "cuerpo verde": return 6;
            default:
               
                return -1;
        }
    }

    public class SkinData
    {
        public string name;
        public Sprite image;

        // 🔥 Agregar un constructor que acepte 'name' y 'image'
        public SkinData(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }
    }
}

