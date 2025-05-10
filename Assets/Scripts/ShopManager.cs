using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private List<SkinPurchase> allSkinButtons = new List<SkinPurchase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Recolectar todos los botones al inicio
        CollectAllButtons();
    }

    public void CollectAllButtons()
    {
        allSkinButtons.Clear();
        SkinPurchase[] buttons = FindObjectsOfType<SkinPurchase>(true); // Busca incluso objetos inactivos
        foreach (var button in buttons)
        {
            allSkinButtons.Add(button);
            // Forzar verificación inicial
            button.CheckIfPurchased();
        }
        Debug.Log($"🛒 {allSkinButtons.Count} botones de compra registrados.");
    }

    public void RefreshAllButtons()
    {
        // Si la lista está vacía, recolectar botones primero
        if (allSkinButtons.Count == 0)
        {
            CollectAllButtons();
        }

        foreach (var button in allSkinButtons)
        {
            if (button != null)
            {
                button.CheckIfPurchased();
            }
        }
        Debug.Log("🔄 Todos los botones de la tienda actualizados.");
    }

    // Método para buscar un botón específico por nombre
    public SkinPurchase FindButtonBySkinName(string skinName)
    {
        string normalizedName = skinName.ToLower().Trim();

        // Si la lista está vacía, recolectar botones primero
        if (allSkinButtons.Count == 0)
        {
            CollectAllButtons();
        }

        foreach (var button in allSkinButtons)
        {
            if (button != null && button.skinName.ToLower().Trim() == normalizedName)
            {
                return button;
            }
        }

        // Si no se encuentra, intentar buscar nuevamente en la escena
        SkinPurchase[] allButtons = FindObjectsOfType<SkinPurchase>(true);
        foreach (var button in allButtons)
        {
            if (button.skinName.ToLower().Trim() == normalizedName)
            {
                return button;
            }
        }

        return null;
    }

    // Método para reactivar un botón específico por nombre
    public void ReactivateButton(string skinName)
    {
        SkinPurchase button = FindButtonBySkinName(skinName);
        if (button != null)
        {
            button.ReactivateSkin();
            Debug.Log($"✅ Botón reactivado desde ShopManager: {skinName}");
        }
        else
        {
            Debug.LogWarning($"⚠️ No se pudo encontrar el botón para reactivar: {skinName}");
        }
    }
}