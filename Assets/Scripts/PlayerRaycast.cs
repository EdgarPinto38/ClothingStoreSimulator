using UnityEngine;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    public float rayDistance = 2f; 
    public LayerMask interactableLayer; 
    public TextMeshProUGUI objectNameText;

    private Vector2 lastDirection = Vector2.down; // Dirección inicial del raycast

    void Update()
    {
        // Obtiene la dirección del movimiento del jugador
        if (Input.GetAxisRaw("Horizontal") > 0) lastDirection = Vector2.right;
        else if (Input.GetAxisRaw("Horizontal") < 0) lastDirection = Vector2.left;
        else if (Input.GetAxisRaw("Vertical") > 0) lastDirection = Vector2.up;
        else if (Input.GetAxisRaw("Vertical") < 0) lastDirection = Vector2.down;

        // Dispara el Raycast en la dirección guardada
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastDirection, rayDistance, interactableLayer);

        if (hit.collider != null)
        {
            objectNameText.text = hit.collider.gameObject.name; // Muestra el nombre del objeto
            objectNameText.color = Color.yellow; 
        }
        else
        {
            objectNameText.text = ""; // Borra el texto si no hay un objeto interactivo
        }

        // Debug: Muestra el raycast en la vista de escena
        Debug.DrawRay(transform.position, lastDirection * rayDistance, Color.red);
    }
}