using UnityEngine;
using TMPro;

public class InteractPlayer : MonoBehaviour
{
    public float rayDistance = 2f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI objectNameText;
    public GameObject storePanel;

    private GameObject detectedObject;
    private PlayerRaycast playerRaycast;
    public SkinSellPanel skinSellPanel;

    void Start()
    {
        playerRaycast = FindObjectOfType<PlayerRaycast>();
    }

    void Update()
    {
        if (playerRaycast == null) return;

        // Verificar si el jugador está quieto
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        bool isPlayerIdle = playerMovement != null && playerMovement.IsPlayerIdle();

        Vector2 rayDirection = playerRaycast.GetLastDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, interactableLayer);

        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

        if (hit.collider != null && isPlayerIdle)
        {
            detectedObject = hit.collider.gameObject;
            objectNameText.text = detectedObject.name;
            objectNameText.color = Color.yellow;
        }
        else
        {
            detectedObject = null;
            objectNameText.text = "";
        }

        // Solo permitir interacción si el jugador está quieto
        if (isPlayerIdle && Input.GetKeyDown(KeyCode.E) && detectedObject != null && detectedObject.CompareTag("Store"))
        {
            OpenStore();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStore();
        }

        if (isPlayerIdle && Input.GetKeyDown(KeyCode.E) && detectedObject != null && detectedObject.CompareTag("Sell"))
        {
            // Abrir el panel de venta
            if (skinSellPanel != null)
            {
                skinSellPanel.OpenSellPanel(); // Llama al método del panel
            }
            else
            {
                Debug.LogError("El SellPanel no está asignado en el inspector.");
            }
        }
    }

    void OpenStore()
    {
        storePanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Tienda abierta: Movimiento bloqueado completamente.");
    }

    void CloseStore()
    {
        storePanel.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Tienda cerrada: Movimiento restaurado.");
    }
}