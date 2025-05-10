using UnityEngine;
using TMPro;

public class SimpleCoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinText1,coinText2,coinText3,coinText4;

    void Start()
    {
        // Actualiza el display al inicio
        UpdateCoinDisplay();

        // Suscribe al evento de cambio de monedas
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
        }
    }

    void OnDestroy()
    {
        // Elimina la suscripción al evento
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= OnCoinsChanged;
        }
    }

    void OnCoinsChanged(int amount)
    {
        // Simplemente actualiza el texto cuando cambia la cantidad de monedas
        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        if (coinText1 != null && CurrencyManager.Instance != null)
        {
            coinText1.text ="$" + CurrencyManager.Instance.coins.ToString();
            coinText2.text = "$" + CurrencyManager.Instance.coins.ToString();
            coinText3.text = "$" + CurrencyManager.Instance.coins.ToString();
            coinText4.text = "$" + CurrencyManager.Instance.coins.ToString();
        }
    }
}