using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int coins = 0;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        coins += amount;
        UpdateCoinDisplay();
        Debug.Log($"🟡 Ganaste {amount} monedas. Total: {coins}");
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCoinDisplay();
            Debug.Log($"🔵 Gastaste {amount} monedas. Restante: {coins}");
            return true;
        }
        else
        {
            Debug.Log($"🔴 No tienes suficientes monedas. Tienes: {coins}, necesitas: {amount}");
            return false;
        }
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = "Monedas: " + coins.ToString();
        }
        else
        {
            Debug.LogWarning("⚠️ El campo 'coinText' no está asignado.");
        }
    }
}