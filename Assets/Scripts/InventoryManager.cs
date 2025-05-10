using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject skinButtonPrefab;
    public Transform inventoryContent;

    public Sprite whiteHeadSprite;
    public Sprite whiteBodySprite;

    void Start()
    {
        inventoryPanel.SetActive(false);

        // 🔥 Forzar skins iniciales equipadas
        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player != null)
        {
            player.ChangeHeadSkin(0); // Cabeza blanca
            player.ChangeBodySkin(0); // Cuerpo blanco
        }

        Debug.Log("✔️ Skins iniciales: Cabeza blanca y Cuerpo blanco equipados.");

        // Agregar skins predeterminadas desde 'SkinPurchase'
        if (SkinPurchase.purchasedSkins.Count == 0)
        {
            SkinPurchase.purchasedSkins.Add(new SkinPurchase.SkinData("cabeza blanca", whiteHeadSprite));
            SkinPurchase.purchasedSkins.Add(new SkinPurchase.SkinData("cuerpo blanco", whiteBodySprite));
            Debug.Log("✅ Se agregaron skins predeterminadas al inventario.");
        }
    }

    void Update()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        bool isPlayerIdle = playerMovement != null && playerMovement.IsPlayerIdle();

        // Solo abrir el inventario si el jugador está quieto
        if (isPlayerIdle && Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            Time.timeScale = 0;
        }
    }

    void ToggleInventory()
    {
        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        player.ForceLookUp();
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject); // 🔥 Limpiar botones antes de actualizarlos
        }

        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();

        foreach (SkinPurchase.SkinData skin in SkinPurchase.purchasedSkins)
        {
            GameObject newButton = Instantiate(skinButtonPrefab, inventoryContent);

            Transform imageTransform = newButton.transform.Find("SkinImage");
            if (imageTransform != null)
            {
                imageTransform.GetComponent<Image>().sprite = skin.image;
            }

            Transform textTransform = newButton.transform.Find("SkinName");
            if (textTransform != null)
            {
                textTransform.GetComponent<TextMeshProUGUI>().text = skin.name;
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null && player != null)
            {
                // Verificar si esta skin está actualmente equipada
                bool isHeadEquipped = player.IsHeadSkinEquipped(skin.name);
                bool isBodyEquipped = player.IsBodySkinEquipped(skin.name);
                bool isEquipped = isHeadEquipped || isBodyEquipped;

                // Solo deshabilitar el botón si la skin está equipada actualmente
                buttonComponent.interactable = !isEquipped;

                if (isEquipped)
                {
                    Debug.Log($"Skin equipada: {skin.name} - Botón deshabilitado");
                }
                else
                {
                    Debug.Log($"Skin disponible: {skin.name} - Botón habilitado");
                }

                buttonComponent.onClick.AddListener(() => SelectSkin(skin.name));
            }
        }
    }

    public void CloseInventory()
    {
        Time.timeScale = 1; // Restablecer el tiempo de juego
        inventoryPanel.SetActive(false); // Cerrar el inventario
        Debug.Log("Inventario cerrado");

        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player != null)
        {
            Debug.Log("Ejecutando ForceLookUp() para orientar al jugador hacia arriba...");
            player.ForceLookUp();
        }
    }

    public void SelectSkin(string skinName)
    {
        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player != null)
        {
            int skinIndex = GetSkinIndex(skinName); // Obtener índice de la skin
            string skinNameLower = skinName.ToLower().Trim();

            // Determinar si es una skin de cabeza, cuerpo o ambos (colores especiales)
            bool isColorOnly = skinNameLower == "azul" || skinNameLower == "rojo" ||
                              skinNameLower == "rosa" || skinNameLower == "verde";

            if (skinNameLower.Contains("cabeza") || isColorOnly)
            {
                player.ChangeHeadSkin(skinIndex);
                Debug.Log($"Skin de cabeza cambiada a: {skinName} ({skinIndex})");
            }

            if (skinNameLower.Contains("cuerpo") || isColorOnly)
            {
                player.ChangeBodySkin(skinIndex);
                Debug.Log($"Skin de cuerpo cambiada a: {skinName} ({skinIndex})");
            }

            UpdateInventoryUI(); // Refrescar el inventario para activar/desactivar los botones
        }
    }

    private int GetSkinIndex(string skinName)
    {
        Dictionary<string, int> skinMapping = new Dictionary<string, int>
        {
            { "cabeza blanca", 0 },
            { "cabeza amarilla", 1 },
            { "cabeza negra", 2 },
            { "cuerpo blanco", 0 },
            { "cuerpo amarillo", 1 },
            { "cuerpo negro", 2 },
            { "azul", 3 },
            { "rojo", 4 },
            { "rosa", 5 },
            { "verde", 6 }
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