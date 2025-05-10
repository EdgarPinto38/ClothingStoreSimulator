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

    public void PurchaseSkin()
    {
        if (!isPurchased)
        {
            isPurchased = true;
            purchasedSkins.Add(new SkinData(skinName, skinImage));
            purchaseButton.interactable = false;
            Debug.Log($"Skin {skinName} comprada y agregada al inventario.");

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
    }

    private int GetSkinIndex(string skinName)
    {
        switch (skinName.ToLower())
        {
            case "cabeza blanca": return 0;
            case "cabeza amarilla": return 1;
            case "cabeza negra": return 2;
            default:
                Debug.LogError($"⚠️ Error: '{skinName}' no es una cabeza válida.");
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
                Debug.LogError($"⚠️ Error: '{skinName}' no es un cuerpo válido.");
                return -1;
        }
    }

    public class SkinData
    {
        public string name { get; private set; }
        public Sprite image { get; private set; }

        public SkinData(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }
    }
}

