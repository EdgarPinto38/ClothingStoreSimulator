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

        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player != null)
        {
            player.ChangeHeadSkin(0); // Cabeza blanca
            player.ChangeBodySkin(0); // Cuerpo blanco
        }

        if (SkinPurchase.purchasedSkins.Count == 0)
        {
            // Las skins iniciales son gratuitas (precio 0)
            SkinPurchase.purchasedSkins.Add(new SkinPurchase.SkinData("cabeza blanca", whiteHeadSprite, 0));
            SkinPurchase.purchasedSkins.Add(new SkinPurchase.SkinData("cuerpo blanco", whiteBodySprite, 0));
        }
    }

    void Update()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        bool isPlayerIdle = playerMovement != null && playerMovement.IsPlayerIdle();

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

    public void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();

        foreach (SkinPurchase.SkinData skin in SkinPurchase.purchasedSkins)
        {
            GameObject newButton = Instantiate(skinButtonPrefab, inventoryContent);

            // Configurar imagen
            Transform imageTransform = newButton.transform.Find("SkinImage");
            if (imageTransform != null)
            {
                imageTransform.GetComponent<Image>().sprite = skin.image;
            }

            // Configurar nombre
            Transform textTransform = newButton.transform.Find("SkinName");
            if (textTransform != null)
            {
                textTransform.GetComponent<TextMeshProUGUI>().text = skin.name;
            }

            // Configurar indicador de equipado
            Transform equippedTransform = newButton.transform.Find("EquippedStatus");
            if (equippedTransform != null && equippedTransform.GetComponent<TextMeshProUGUI>() != null)
            {
                bool isHeadEquipped = player.IsHeadSkinEquipped(skin.name);
                bool isBodyEquipped = player.IsBodySkinEquipped(skin.name);
                bool isEquipped = isHeadEquipped || isBodyEquipped;

                equippedTransform.GetComponent<TextMeshProUGUI>().text = isEquipped ? "Equipado" : "";
                equippedTransform.gameObject.SetActive(isEquipped);
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null && player != null)
            {
                bool isHeadEquipped = player.IsHeadSkinEquipped(skin.name);
                bool isBodyEquipped = player.IsBodySkinEquipped(skin.name);
                bool isEquipped = isHeadEquipped || isBodyEquipped;

                buttonComponent.interactable = !isEquipped;

                buttonComponent.onClick.AddListener(() => SelectSkin(skin.name));
            }
        }
    }

    public void CloseInventory()
    {
        Time.timeScale = 1;
        inventoryPanel.SetActive(false);
    }

    public void SelectSkin(string skinName)
    {
        PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
        if (player != null)
        {
            int skinIndex = GetSkinIndex(skinName);
            string skinNameLower = skinName.ToLower().Trim();

            if (skinNameLower.Contains("cabeza"))
            {
                player.ChangeHeadSkin(skinIndex);
            }

            if (skinNameLower.Contains("cuerpo") ||
                skinNameLower == "azul" ||
                skinNameLower == "rojo" ||
                skinNameLower == "rosa" ||
                skinNameLower == "verde")
            {
                player.ChangeBodySkin(skinIndex);
            }

            UpdateInventoryUI();
        }
    }

    public void ForceRefreshInventory()
    {
        Debug.Log("🔄 Forzando actualización del inventario...");
        UpdateInventoryUI();
    }

    private int GetSkinIndex(string skinName)
    {
        Dictionary<string, int> skinMapping = new Dictionary<string, int>
        {
            { "cabeza blanca", 0 }, { "cabeza amarilla", 1 }, { "cabeza negra", 2 },
            { "cuerpo blanco", 0 }, { "cuerpo amarillo", 1 }, { "cuerpo negro", 2 },
            { "azul", 3 }, { "rojo", 4 }, { "rosa", 5 }, { "verde", 6 }
        };

        string key = skinName.ToLower().Trim();

        if (skinMapping.ContainsKey(key)) return skinMapping[key];

        string[] colors = { "azul", "rojo", "rosa", "verde" };
        for (int i = 0; i < colors.Length; i++)
        {
            if (key == colors[i]) return i + 3;
        }

        Debug.LogError($"❌ Skin no encontrada: {skinName}");
        return -1;
    }
}