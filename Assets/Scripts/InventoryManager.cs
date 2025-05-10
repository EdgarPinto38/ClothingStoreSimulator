using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject skinButtonPrefab;
    public Transform inventoryContent;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
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
}