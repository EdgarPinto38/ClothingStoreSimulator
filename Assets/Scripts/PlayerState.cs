using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Vector2 lastDirection = Vector2.down; // Direcci�n del jugador

    public void SaveDirection(Vector2 direction)
    {
        lastDirection = direction; // Guarda la �ltima direcci�n
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection; // Retorna la direcci�n guardada
    }
}