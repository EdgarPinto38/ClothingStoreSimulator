using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // Movimiento izquierda/derecha
        movement.y = Input.GetAxisRaw("Vertical");   // Movimiento arriba/abajo
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public bool IsPlayerIdle()
    {
        return movement == Vector2.zero; // Retorna verdadero si el jugador no se está moviendo
    }

}