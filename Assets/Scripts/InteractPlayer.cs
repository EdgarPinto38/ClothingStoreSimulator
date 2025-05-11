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

    public GameObject gamePanel;
    public GameObject pausePanel;

    void Start()
    {
        playerRaycast = FindObjectOfType<PlayerRaycast>();
        // Ocultar el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


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
            PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
            player.ForceLookUp();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Mostrar el cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
            player.ForceLookUp();
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
        }

        if (isPlayerIdle && Input.GetKeyDown(KeyCode.E) && detectedObject != null && detectedObject.CompareTag("Sell"))
        {
            
            // Abrir el panel de venta
            if (skinSellPanel != null)
            {
                skinSellPanel.OpenSellPanel();
                PlayerRaycast player = FindObjectOfType<PlayerRaycast>();
                player.ForceLookUp();
            }
            else
            {
                Debug.LogError("El SellPanel no está asignado en el inspector.");
            }
        }
    }

    void OpenStore()
    {
        // Mostrar el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        storePanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Tienda abierta: Movimiento bloqueado completamente.");
    }

    public void NormalTime()
    {
        // Ocultar el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    
}