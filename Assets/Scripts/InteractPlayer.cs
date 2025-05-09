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

    void Start()
    {
        playerRaycast = FindObjectOfType<PlayerRaycast>();
    }

    void Update()
    {
        if (playerRaycast == null) return;

        Vector2 rayDirection = playerRaycast.GetLastDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, interactableLayer);

        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

        if (hit.collider != null)
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

        if (Input.GetKeyDown(KeyCode.E) && detectedObject != null && detectedObject.CompareTag("Store"))
        {
            OpenStore();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStore();
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