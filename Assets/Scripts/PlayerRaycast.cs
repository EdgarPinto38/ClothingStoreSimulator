using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public Animator headAnimator;
    public Animator bodyAnimator;

    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;

    private enum SkinColor { White, Yellow, Black, Azul, Rojo, Rosa, Verde }
    private SkinColor currentHeadSkin = SkinColor.White;
    private SkinColor currentBodySkin = SkinColor.White;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            isMoving = true;

            if (moveX > 0)
            {
                lastDirection = Vector2.right;
                headRenderer.flipX = false;
                bodyRenderer.flipX = false;
                SetWalkAnimation();
            }
            else if (moveX < 0)
            {
                lastDirection = Vector2.left;
                headRenderer.flipX = true;
                bodyRenderer.flipX = true;
                SetWalkAnimation();
            }
            else if (moveY > 0)
            {
                lastDirection = Vector2.up;
                SetWalkAnimation();
            }
            else if (moveY < 0)
            {
                lastDirection = Vector2.down;
                SetWalkAnimation();
            }
        }
        else
        {
            isMoving = false;
            PlayIdleAnimation();
        }
    }

    public void SetWalkAnimation()
    {
        ResetTriggers();
        ActivateHeadTrigger("WalkRight", "WalkUp", "WalkDown");
        ActivateBodyTrigger("WalkRight", "WalkUp", "WalkDown");
    }

    public void PlayIdleAnimation()
    {
        ResetTriggers();
        ActivateHeadTrigger("IdleRight", "IdleUp", "IdleDown");
        ActivateBodyTrigger("IdleRight", "IdleUp", "IdleDown");
    }

    void ActivateHeadTrigger(string rightTrigger, string upTrigger, string downTrigger)
    {
        string headState = GetStateTrigger(rightTrigger, upTrigger, downTrigger, currentHeadSkin);
        if (!string.IsNullOrEmpty(headState))
        {
            headAnimator.SetTrigger(headState);
        }
    }

    void ActivateBodyTrigger(string rightTrigger, string upTrigger, string downTrigger)
    {
        string bodyState = GetStateTrigger(rightTrigger, upTrigger, downTrigger, currentBodySkin);
        if (!string.IsNullOrEmpty(bodyState))
        {
            bodyAnimator.SetTrigger(bodyState);
        }
    }

    string GetStateTrigger(string rightTrigger, string upTrigger, string downTrigger, SkinColor skin)
    {
        if (lastDirection == Vector2.right || lastDirection == Vector2.left)
        {
            return $"{rightTrigger}_{skin}";
        }
        else if (lastDirection == Vector2.up)
        {
            return $"{upTrigger}_{skin}";
        }
        else
        {
            return $"{downTrigger}_{skin}";
        }
    }

    public void ResetTriggers()
    {
        foreach (SkinColor color in System.Enum.GetValues(typeof(SkinColor)))
        {
            headAnimator.ResetTrigger($"WalkRight_{color}");
            headAnimator.ResetTrigger($"WalkUp_{color}");
            headAnimator.ResetTrigger($"WalkDown_{color}");
            headAnimator.ResetTrigger($"IdleRight_{color}");
            headAnimator.ResetTrigger($"IdleUp_{color}");
            headAnimator.ResetTrigger($"IdleDown_{color}");

            bodyAnimator.ResetTrigger($"WalkRight_{color}");
            bodyAnimator.ResetTrigger($"WalkUp_{color}");
            bodyAnimator.ResetTrigger($"WalkDown_{color}");
            bodyAnimator.ResetTrigger($"IdleRight_{color}");
            bodyAnimator.ResetTrigger($"IdleUp_{color}");
            bodyAnimator.ResetTrigger($"IdleDown_{color}");
        }

        Debug.Log("Triggers reseteados correctamente.");
    }

    public void ChangeHeadSkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < System.Enum.GetValues(typeof(SkinColor)).Length)
        {
            currentHeadSkin = (SkinColor)skinIndex; // 🔥 Actualizar la skin de cabeza equipada
            ResetTriggers();
            PlayIdleAnimation();

            Debug.Log($"✔️ Cabeza equipada: {currentHeadSkin}");
        }
    }

    public void ChangeBodySkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < System.Enum.GetValues(typeof(SkinColor)).Length)
        {
            currentBodySkin = (SkinColor)skinIndex; // 🔥 Actualizar la skin de cuerpo equipada
            ResetTriggers();
            PlayIdleAnimation();

            Debug.Log($"✔️ Cuerpo equipado: {currentBodySkin}");
        }
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }

    public void ForceLookUp()
    {
        lastDirection = Vector2.up;
        ResetTriggers();
        headAnimator.SetTrigger($"IdleUp_{currentHeadSkin}");
        bodyAnimator.SetTrigger($"IdleUp_{currentBodySkin}");
        Debug.Log($"Estado IdleUp aplicado con cabeza {currentHeadSkin} y cuerpo {currentBodySkin}");
    }

    public bool IsHeadSkinEquipped(string skinName)
    {
        // Convertir el nombre de la skin al formato que coincide con el enum
        string skinNameLower = skinName.ToLower().Trim();

        if (!skinNameLower.Contains("cabeza"))
        {
            return false;
        }

        // Extraer solo el color de la skin
        string colorName;
        if (skinNameLower.Contains("cabeza "))
        {
            colorName = skinNameLower.Replace("cabeza ", "");
        }
        else
        {
            colorName = skinNameLower; // Para colores como "azul", "rosa", etc.
        }

        // Mapeamos los nombres en español a los valores del enum
        Dictionary<string, SkinColor> colorMapping = new Dictionary<string, SkinColor>
        {
            { "blanca", SkinColor.White },
            { "amarilla", SkinColor.Yellow },
            { "negra", SkinColor.Black },
            { "azul", SkinColor.Azul },
            { "rojo", SkinColor.Rojo },
            { "rosa", SkinColor.Rosa },
            { "verde", SkinColor.Verde }
        };

        // Verificar si el color existe en el mapping y comparar con la skin actual
        if (colorMapping.TryGetValue(colorName, out SkinColor mappedColor))
        {
            bool isEquipped = currentHeadSkin == mappedColor;
            Debug.Log($"🔍 Comparando cabeza equipada ({currentHeadSkin}) con {mappedColor} - Resultado: {isEquipped}");
            return isEquipped;
        }

        Debug.Log($"⚠️ Color no encontrado en el mapping para cabeza: {colorName}");
        return false;
    }

    public bool IsBodySkinEquipped(string skinName)
    {
        // Convertir el nombre de la skin al formato que coincide con el enum
        string skinNameLower = skinName.ToLower().Trim();

        // Si la skin no tiene "cuerpo" en el nombre y no es un color general, no es una skin de cuerpo
        if (!skinNameLower.Contains("cuerpo") &&
            !(skinNameLower == "azul" || skinNameLower == "rojo" ||
              skinNameLower == "rosa" || skinNameLower == "verde"))
        {
            return false;
        }

        // Extraer solo el color de la skin
        string colorName;
        if (skinNameLower.Contains("cuerpo "))
        {
            colorName = skinNameLower.Replace("cuerpo ", "");
        }
        else
        {
            colorName = skinNameLower; // Para colores como "azul", "rosa", etc.
        }

        // Mapeamos los nombres en español a los valores del enum
        Dictionary<string, SkinColor> colorMapping = new Dictionary<string, SkinColor>
        {
            { "blanco", SkinColor.White },
            { "amarillo", SkinColor.Yellow },
            { "negro", SkinColor.Black },
            { "azul", SkinColor.Azul },
            { "rojo", SkinColor.Rojo },
            { "rosa", SkinColor.Rosa },
            { "verde", SkinColor.Verde }
        };

        // Verificar si el color existe en el mapping y comparar con la skin actual
        if (colorMapping.TryGetValue(colorName, out SkinColor mappedColor))
        {
            bool isEquipped = currentBodySkin == mappedColor;
            Debug.Log($"🔍 Comparando cuerpo equipado ({currentBodySkin}) con {mappedColor} - Resultado: {isEquipped}");
            return isEquipped;
        }

        Debug.Log($"⚠️ Color no encontrado en el mapping para cuerpo: {colorName}");
        return false;
    }

    private string ConvertToSkinName(string type, string skinName)
    {
        return $"{type} {skinName}".ToLower().Trim();
    }
}