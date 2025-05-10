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
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject); // Limpia el inventario antes de actualizarlo
        }

        foreach (SkinPurchase.SkinData skin in SkinPurchase.purchasedSkins)
        {
            GameObject newButton = Instantiate(skinButtonPrefab, inventoryContent);

            Transform imageTransform = newButton.transform.Find("SkinImage");
            if (imageTransform != null)
            {
                imageTransform.GetComponent<Image>().sprite = skin.image;
            }
            else
            {
                Debug.LogError("SkinImage no encontrado en el prefab.");
            }

            Transform textTransform = newButton.transform.Find("SkinName");
            if (textTransform != null)
            {
                textTransform.GetComponent<TextMeshProUGUI>().text = skin.name;
            }
            else
            {
                Debug.LogError("SkinName no encontrado en el prefab.");
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