using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Configuración de monedas")]
    public int coins = 100; // Este valor se respeta desde el Inspector
    public int maxCoins = 9999;

    [Header("Configuración inicial opcional")]
    [Tooltip("Valor por defecto si no se ha configurado manualmente en el Inspector")]
    public int defaultStartingCoins = 100;

    // Evento para notificar cambios en las monedas
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Opcional: Si coins es 0, usa el valor por defecto
            if (coins == 0 && defaultStartingCoins > 0)
            {
                coins = defaultStartingCoins;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        NotifyCoinsChanged();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        coins = Mathf.Min(coins + amount, maxCoins);
        NotifyCoinsChanged();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;

        if (coins >= amount)
        {
            coins -= amount;
            NotifyCoinsChanged();
            return true;
        }
        else
        {
            Debug.Log($"🔴 No tienes suficientes monedas. Tienes: {coins}, necesitas: {amount}");
            return false;
        }
    }

    // Notifica a todos los oyentes sobre el cambio en las monedas
    private void NotifyCoinsChanged()
    {
        OnCoinsChanged?.Invoke(coins);
    }

    // Método para resetear las monedas (útil para pruebas o nueva partida)
    public void ResetCoins(int amount = 100)
    {
        coins = amount;
        NotifyCoinsChanged();
        Debug.Log($"🔄 Monedas reseteadas a: {coins}");
    }
}