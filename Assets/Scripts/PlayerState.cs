using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Vector2 lastDirection = Vector2.down; // Dirección del jugador

    public void SaveDirection(Vector2 direction)
    {
        lastDirection = direction; // Guarda la última dirección
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection; // Retorna la dirección guardada
    }
}